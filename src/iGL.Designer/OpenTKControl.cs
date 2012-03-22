﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Engine.Math;
using iGL.Engine.Events;
using System.Diagnostics;

namespace iGL.Designer
{
    public partial class OpenTKControl : OpenTK.GLControl
    {
        public class SelectObjectEvent : EventArgs
        {
            public GameObject SelectedObject { get; set; }
        }

        public class ObjectAddedEvent : EventArgs
        {
            public GameObject AddedObject { get; set; }
        }

        public class ObjectRemovedEvent : EventArgs
        {
            public GameObject RemovedObject { get; set; }
        }

        public EditAxisType? EditAxis { get; set; }

        public OperationType Operation { get; private set; }

        public EditorGame Game { get; private set; }      

        public enum EditAxisType
        {
            XAXIS,
            YAXIS,
            ZAXIS,
            ALL
        }

        public enum OperationType
        {
            NONE,
            MOVE,
            ROTATE,
            SCALE,
            PANVIEW
        }

        private bool _glLoaded = false;
        private GameObject _selectedObject;
        private Gizmo _selectionGizmo;
        private Scene _workingScene;
        private bool _isPaused;
        private bool _isPlaying;

        private event EventHandler<SelectObjectEvent> _selectObjectEvent;
        private event EventHandler<ObjectAddedEvent> _objectAddedEvent;
        private event EventHandler<ObjectRemovedEvent> _objectRemovedEvent;
        
        public OpenTKControl()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                Paint += new PaintEventHandler(OpenTKControl_Paint);
                Load += new EventHandler(OpenTKControl_Load);
                Resize += new EventHandler(OpenTKControl_Resize);
                MouseMove += new MouseEventHandler(OpenTKControl_MouseMove);
                MouseDown += new MouseEventHandler(OpenTKControl_MouseDown);
                MouseUp += new MouseEventHandler(OpenTKControl_MouseUp);
                DragEnter += new DragEventHandler(OpenTKControl_DragEnter);
                DragDrop += new DragEventHandler(OpenTKControl_DragDrop);
                MouseWheel += new MouseEventHandler(OpenTKControl_MouseWheel);
            }
        }

        void OpenTKControl_MouseWheel(object sender, MouseEventArgs e)
        {
            /* zoom camera */

            if (_workingScene.CurrentCamera != null)
            {
                var lookAt = _workingScene.CurrentCamera.Target - _workingScene.CurrentCamera.GameObject.Position;
                lookAt.Normalize();
                lookAt *= -(e.Delta / 200.0f);

                _workingScene.CurrentCamera.Target -= lookAt;
                _workingScene.CurrentCamera.GameObject.Position -= lookAt;
            }
        }

        protected new bool DesignMode
        {
            get
            {
                if (base.DesignMode)
                    return true;

                return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }
        public event EventHandler<SelectObjectEvent> OnSelectObject
        {
            add
            {
                _selectObjectEvent += value;
            }
            remove
            {
                _selectObjectEvent -= value;
            }
        }

        public event EventHandler<ObjectAddedEvent> OnObjectAdded
        {
            add
            {
                _objectAddedEvent += value;
            }
            remove
            {
                _objectAddedEvent -= value;
            }
        }

        public event EventHandler<ObjectRemovedEvent> OnObjectRemoved
        {
            add
            {
                _objectRemovedEvent += value;
            }
            remove
            {
                _objectRemovedEvent -= value;
            }
        }

        void OpenTKControl_DragDrop(object sender, DragEventArgs e)
        {
            var node = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (node == null) return;
            try
            {
                var instance = Activator.CreateInstance(node.Tag as Type) as GameObject;
                var count = _workingScene.GameObjects.Count(o => o.GetType() == (Type)node.Tag);

                instance.Name = ((Type)node.Tag).Name + count.ToString();

                if (!(instance.Components.Any(c => c is CameraComponent)))
                {
                    Vector4 nearPlane, farPlane;
                    var point = PointToClient(new System.Drawing.Point(e.X, e.Y));
                    _workingScene.ScreenPointToWorld(new Engine.Math.Point(point.X, point.Y), out nearPlane, out farPlane);

                    var position = new Vector3(nearPlane);

                    var direction = new Vector3(farPlane - nearPlane);
                    direction.Normalize();
                    direction = Vector3.Multiply(direction, 10);

                    instance.Position = position + direction;
                }

                _workingScene.AddGameObject(instance);

                if (_workingScene.CurrentCamera == null && instance.Components.Any(c => c is CameraComponent))
                {
                    _workingScene.SetCurrentCamera(instance);
                }

                if (_workingScene.CurrentLight == null && instance.Components.Any(c => c is LightComponent))
                {
                    _workingScene.SetCurrentLight(instance);
                }

                if (_objectAddedEvent != null)
                {
                    _objectAddedEvent(this, new ObjectAddedEvent() { AddedObject = instance });
                }

                /* hook up mouse events */

                instance.OnMouseIn += (a, b) =>
                {
                    if (Operation == OperationType.PANVIEW) return;

                    Cursor = Cursors.Cross;
                };

                instance.OnMouseOut += (a, b) =>
                {
                    if (Operation == OperationType.PANVIEW) return;

                    Cursor = Cursors.Arrow;
                };

                instance.OnMouseDown += (a, b) =>
                {
                    if (Operation == OperationType.PANVIEW || 
                        (b.Button != MouseButton.Button1 && b.Button != MouseButton.Button2)) return;

                    _selectedObject = (a as GameObject).Root;

                    var rigidBody = _selectedObject.Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

                    ((DesignPhysics)_workingScene.Physics).SelectedObject = _selectedObject;
                    
                    UpdateGizmo();

                    if (_selectObjectEvent != null)
                    {
                        _selectObjectEvent(this, new SelectObjectEvent() { SelectedObject = _selectedObject });
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(((Type)node.Tag).Name + ": " + ex.Message);
            }
        }

        void OpenTKControl_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        void OpenTKControl_MouseUp(object sender, MouseEventArgs e)
        {            
            Game.MouseButton(e.Button.ToMouseButton(), false, e.X, e.Y);
        }

        void OpenTKControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && _workingScene.LastNearPlaneMousePosition.HasValue)
            {
                var lastNear = new Vector3(_workingScene.LastNearPlaneMousePosition.Value);

                var lookAt = _workingScene.CurrentCamera.Target - lastNear;
                lookAt.Normalize();

                /* look around function */
                _workingScene.CurrentCamera.Target = lastNear + lookAt * 10.0f;
            }

            Game.MouseButton(e.Button.ToMouseButton(), true, e.X, e.Y);
            if (_workingScene.LastMouseDownTarget == null)
            {
                ClearSelection();
            }
            
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                var point = new System.Drawing.Point(e.X, e.Y);
                point = PointToScreen(point);
                contextMenu.Show(point.X, point.Y);
            }

            UpdateMenuStatus();
            
        }        

        void OpenTKControl_MouseMove(object sender, MouseEventArgs e)
        {
            Game.MouseMove(e.X, e.Y);

            //_lastMousePosition = e.Location;
        }

        void OpenTKControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl control = sender as OpenTK.GLControl;
        }

        void OpenTKControl_Load(object sender, EventArgs e)
        {
            _glLoaded = true;
            Game = EditorGame.Instance();
            Game.Resize(Size.Width, Size.Height);

            var physics = new DesignPhysics();
            physics.OnCollision += new EventHandler<EventArgs>(physics_OnCollision);
            _workingScene = new Scene(physics);
            
            _workingScene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(_scene_OnMouseMove);

            /* origin gizmo */
            var gizmo = new Gizmo() { ArrowLength = 30.0f, ShowUniformSphere = false };            

            _workingScene.AddGameObject(gizmo);

            _selectionGizmo = new Gizmo();

            _selectionGizmo.Visible = false;
            _selectionGizmo.Enabled = false;

            _selectionGizmo.XDirectionArrow.OnMouseDown += (a, b) => EditAxis = EditAxisType.XAXIS;
            _selectionGizmo.XDirectionArrow.OnMouseUp += (a, b) => EditAxis = null;
            _selectionGizmo.XDirectionArrow.OnMouseIn += (a, b) => Cursor = Cursors.Hand;
            _selectionGizmo.XDirectionArrow.OnMouseOut += (a, b) => Cursor = Cursors.Arrow;

            _selectionGizmo.YDirectionArrow.OnMouseDown += (a, b) => EditAxis = EditAxisType.YAXIS;
            _selectionGizmo.YDirectionArrow.OnMouseUp += (a, b) => EditAxis = null;
            _selectionGizmo.YDirectionArrow.OnMouseIn += (a, b) => Cursor = Cursors.Hand;
            _selectionGizmo.YDirectionArrow.OnMouseOut += (a, b) => Cursor = Cursors.Arrow;

            _selectionGizmo.ZDirectionArrow.OnMouseDown += (a, b) => EditAxis = EditAxisType.ZAXIS;
            _selectionGizmo.ZDirectionArrow.OnMouseUp += (a, b) => EditAxis = null;
            _selectionGizmo.ZDirectionArrow.OnMouseIn += (a, b) => Cursor = Cursors.Hand;
            _selectionGizmo.ZDirectionArrow.OnMouseOut += (a, b) => Cursor = Cursors.Arrow;

            _selectionGizmo.UniformSphere.OnMouseDown += (a, b) => EditAxis = EditAxisType.ALL;
            _selectionGizmo.UniformSphere.OnMouseUp += (a, b) => EditAxis = null;
            _selectionGizmo.UniformSphere.OnMouseIn += (a, b) => Cursor = Cursors.Hand;
            _selectionGizmo.UniformSphere.OnMouseOut += (a, b) => Cursor = Cursors.Arrow;

            _workingScene.AddGameObject(_selectionGizmo);

            Game.SetScene(_workingScene);

            var size = ClientSize;

            Game.Resize(size.Width, size.Height);
            Game.Load();

            _workingScene.AmbientColor = new Vector4(1, 1, 1, 1);
        }

        void physics_OnCollision(object sender, EventArgs e)
        {
            EditAxis = null;
        }

        void OpenTKControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void ClearSelection()
        {
            _selectedObject = null;
            if (_selectObjectEvent != null)
            {
                _selectObjectEvent(this, new SelectObjectEvent());
            }

            UpdateGizmo();
        }

        public void Tick(float timeElapsed)
        {
            if (_isPaused && _isPlaying) return;

            Game.Tick(timeElapsed);
        }

        public void Render()
        {
            UpdateGizmo();

            if (!_glLoaded) return;

            EditorGame.Instance().Render();

            SwapBuffers();
        }

        void _scene_OnMouseMove(object sender, Engine.Events.MouseMoveEvent e)
        {
            if (_workingScene.MouseButtonState[MouseButton.ButtonMiddle])
            {               
                _workingScene.CurrentCamera.GameObject.Position -= e.DirectionOnNearPlane * 10.0f;
                return;
            }

            if (Operation == OperationType.PANVIEW && _workingScene.MouseButtonState[MouseButton.Button1])
            {
                if (_workingScene.CurrentCamera != null)
                {
                    _workingScene.CurrentCamera.GameObject.Position -= e.DirectionOnNearPlane * 5.0f;
                    _workingScene.CurrentCamera.Target -= e.DirectionOnNearPlane * 5.0f;
                }

                return;
            }

            if (_selectedObject == null) return;

            /* perform edit operation */
            /* size of directional vector gives an indication of mouse movement magnitude */

            //var distance = (e.NearPlane - _selectedObject.Position).Length;
            var distance = 1.0f;

            if (_workingScene.CurrentCamera.Properties is PerspectiveProperties)
            {
                /* in a projection matrix, calculate the near plane / object plane ratio */
                var camProps = _workingScene.CurrentCamera.Properties as PerspectiveProperties;

                var direction = _selectedObject.Position - _workingScene.CurrentCamera.GameObject.Position;
                var directionNorm = direction;
                directionNorm.Normalize();

                var center = _workingScene.CurrentCamera.Target - _workingScene.CurrentCamera.GameObject.Position;
                center.Normalize();

                var objectDistance = Vector3.Dot(center, directionNorm) * direction.Length;

                distance = objectDistance;
            }

            var vector = Operation == OperationType.MOVE ? _selectedObject.Position :
                        (Operation == OperationType.ROTATE ? _selectedObject.Rotation :
                        (Operation == OperationType.SCALE ? _selectedObject.Scale : new Vector3()));

            Vector3 change = new Vector3(0);

            if (EditAxis.HasValue)
            {                
                if (EditAxis.Value == EditAxisType.XAXIS)
                {
                    change = new Vector3(e.DirectionOnNearPlane.X * distance, 0, 0);
                    
                }
                else if (EditAxis.Value == EditAxisType.YAXIS)
                {
                    change = new Vector3(0, e.DirectionOnNearPlane.Y * distance, 0);                   
                }
                else if (EditAxis.Value == EditAxisType.ZAXIS)
                {
                    change = e.DirectionOnNearPlane * distance;
                    change.X = 0;
                    change.Y = 0;
                }
                else if (EditAxis.Value == EditAxisType.ALL)
                {
                    change = new Vector3(e.DirectionOnNearPlane.X * distance, e.DirectionOnNearPlane.Y * distance, e.DirectionOnNearPlane.Z * distance);
                }                             
            }

            vector += change;

            if (Operation == OperationType.MOVE)
            {
                var length = change.Length;
                float iterations = (length / 0.01f);
                var unitVec = change / iterations;
                var totalVec = new Vector3(0);

                bool hadCollision = false;

                while (totalVec.Length < change.Length && !hadCollision)
                {
                    _selectedObject.Position += unitVec;
                    _workingScene.Physics.Step(1.0f / 1000.0f);

                    hadCollision = ((DesignPhysics)_workingScene.Physics).HadCollision;

                    totalVec += unitVec;
                }               
            }

            if (Operation == OperationType.ROTATE) _selectedObject.Rotation = vector;
            if (Operation == OperationType.SCALE) _selectedObject.Scale = vector;

        }

        private void UpdateGizmo()
        {
            if (_selectedObject == null)
            {
                _selectionGizmo.Visible = false;
                _selectionGizmo.Enabled = false;
            }
            else
            {
                _selectionGizmo.Visible = true;
                _selectionGizmo.Enabled = true;

                _selectionGizmo.Position = _selectedObject.Position;
            }
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            /* delete selected object */
            if (_selectedObject == null) return;

            Game.Scene.UnloadObject(_selectedObject);
            if (_objectRemovedEvent != null) _objectRemovedEvent(this, new ObjectRemovedEvent() { RemovedObject = _selectedObject });

            _selectedObject = null;
        }

        private void UpdateMenuStatus()
        {
            if (_selectedObject != null)
            {
                deleteMenuItem.Enabled = true;
            }
            else
            {
                deleteMenuItem.Enabled = false;
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            UpdateMenuStatus();
        }

        public void SetOperation(OperationType operation)
        {
            Operation = operation;

            if (Operation == OperationType.PANVIEW)
            {
                ClearSelection();

                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void Play()
        {
            if (_isPlaying && _isPaused)
            {
                _isPaused = false;
                return;
            }

            if (_isPlaying) return;

            var scene = new Scene(new Physics());

            foreach (var gameObject in _workingScene.GameObjects.Where(o => !o.Designer))
            {
                var clone = gameObject.Clone();
                if (clone.Components.Count() != gameObject.Components.Count())
                {
                    throw new Exception("Component creation mode error: not counting equal component count after clone.");
                }

                scene.AddGameObject(clone);
            }

            Game.SetScene(scene);
            Game.LoadScene();

            if (_workingScene.CurrentCamera != null)
            {
                scene.SetCurrentCamera(scene.GameObjects.Single(g => g.Id == _workingScene.CurrentCamera.GameObject.Id));
            }

            if (_workingScene.CurrentLight != null)
            {
                scene.SetCurrentLight(scene.GameObjects.Single(g => g.Id == _workingScene.CurrentLight.GameObject.Id));
            }

            _isPlaying = true;
        }
        
        public void Pause()
        {
            _isPaused = true;
        }


        public void Stop()
        {
            _isPlaying = false;
            _isPaused = false;
            Game.SetScene(_workingScene);
        }
    }
}
