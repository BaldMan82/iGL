using System;
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
using System.IO;

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

        public class SceneLoadedEvent : EventArgs
        {
            public Scene Scene { get; set; }
        }

        public EditAxisType? EditAxis { get; set; }

        public OperationType Operation { get; private set; }

        public EditorGame Game { get; private set; }

        public bool Snap { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float SnapValue { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float SnapValueRotation { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PreStabilizePhysics { get; set; }

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
            PANVIEW,
            DROPPING
        }

        private bool _glLoaded = false;
        private GameObject _selectedObject;
        private Gizmo _selectionGizmo;
        private Scene _workingScene;
        private bool _isPaused;
        private bool _isPlaying;
        private Vector3 _unsnappedOperationVector;
        
        private event EventHandler<SelectObjectEvent> _selectObjectEvent;
        private event EventHandler<ObjectAddedEvent> _objectAddedEvent;
        private event EventHandler<ObjectRemovedEvent> _objectRemovedEvent;
        private event EventHandler<SceneLoadedEvent> _sceneLoadedEvent;

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

            SnapValue = 0.25f;
            SnapValueRotation = (float)(Math.PI / 8.0);
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

        public event EventHandler<SceneLoadedEvent> OnSceneLoaded
        {
            add
            {
                _sceneLoadedEvent += value;
            }

            remove
            {
                _sceneLoadedEvent -= value;
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

                    var pos = position + direction;
                    SnapTo(ref pos);

                    instance.Position = pos;
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
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(((Type)node.Tag).Name + ": " + ex.Message);
            }
        }

        private void BindEvents(GameObject instance)
        {
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
            Game.MouseButton(e.Button.ToMouseButton(), true, e.X, e.Y);

            if (!EditorGame.InDesignMode) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle && _workingScene.LastNearPlaneMousePosition.HasValue)
            {
                var lastNear = new Vector3(_workingScene.LastNearPlaneMousePosition.Value);

                var lookAt = _workingScene.CurrentCamera.Target - lastNear;
                lookAt.Normalize();

                /* look around function */
                _workingScene.CurrentCamera.Target = lastNear + lookAt * 10.0f;
            }

           
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

            if (_selectedObject != null)
            {               
                if (Operation == OperationType.MOVE) _unsnappedOperationVector = _selectedObject.Position;
                if (Operation == OperationType.ROTATE) _unsnappedOperationVector = _selectedObject.Rotation;
                if (Operation == OperationType.SCALE) _unsnappedOperationVector = _selectedObject.Scale;
            }

            UpdateMenuStatus();
            
        }        

        void OpenTKControl_MouseMove(object sender, MouseEventArgs e)
        {
            Game.MouseMove(e.X, e.Y);    
        }

        void OpenTKControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl control = sender as OpenTK.GLControl;
        }

        void OpenTKControl_Load(object sender, EventArgs e)
        {
            _glLoaded = true;
            Game = EditorGame.Instance();
            iGL.Engine.Game.InDesignMode = true;

            Game.Resize(Size.Width, Size.Height);

            LoadScene(null);

            var size = ClientSize;

            Game.Resize(size.Width, size.Height);
                      
        }

        void OpenTKControl_MouseWheel(object sender, MouseEventArgs e)
        {
            Game.MouseZoom(e.Delta);

            if (!EditorGame.InDesignMode) return;

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

        void OpenTKControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        public void LoadScene(string xml)
        {
            ClearSelection();

            var physics = new DesignPhysics();
            //physics.OnCollision += new EventHandler<EventArgs>(physics_OnCollision);
            _workingScene = new Scene(physics);

            _workingScene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(_scene_OnMouseMove);
                     
            Game.SetScene(_workingScene);

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

            _workingScene.OnObjectAdded += _workingScene_OnObjectAdded;

            if (!string.IsNullOrEmpty(xml))
            {
                Game.LoadScene(xml);
            }
            else
            {
                /* new scene, create a camera */
                var camera = new PerspectiveCamera() { Name = "PerspectiveCamera" };
                _workingScene.AddGameObject(camera);

                _workingScene.SetCurrentCamera(camera);

                Game.LoadScene();
             }
            
            Game.Load();

            if (_sceneLoadedEvent != null) _sceneLoadedEvent(this, new SceneLoadedEvent() { Scene = Game.Scene });
        }

        void _workingScene_OnObjectAdded(object sender, GameObjectAddedEvent e)
        {
            BindEvents(e.GameObject);

            if (_objectAddedEvent != null)
            {
                _objectAddedEvent(this, new ObjectAddedEvent() { AddedObject = sender as GameObject });
            }
        }       

        void physics_OnCollision(object sender, EventArgs e)
        {
            EditAxis = null;
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
            
            if (timeElapsed > 0.01f) timeElapsed = 0.01f;

            Game.Tick(timeElapsed, _isPlaying || Operation == OperationType.DROPPING);
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
            if (Operation == OperationType.DROPPING) return;

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
         
            var distance = 1.0f;

            if (_workingScene.CurrentCamera is PerspectiveCameraComponent)
            {
                /* in a perspective camera, calculate the near plane / object plane ratio */
           
                var direction = _selectedObject.Position - _workingScene.CurrentCamera.GameObject.Position;
                var directionNorm = direction;
                directionNorm.Normalize();

                var center = _workingScene.CurrentCamera.Target - _workingScene.CurrentCamera.GameObject.Position;
                center.Normalize();

                var objectDistance = Vector3.Dot(center, directionNorm) * direction.Length;

                distance = objectDistance;
            }

            var vector = Operation == OperationType.MOVE ? _unsnappedOperationVector :
                        (Operation == OperationType.ROTATE ? _unsnappedOperationVector :
                        (Operation == OperationType.SCALE ? _unsnappedOperationVector : new Vector3()));

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
            _unsnappedOperationVector = vector;           

            if (change.Length == 0) return;

            if (Operation == OperationType.MOVE)
            {
                SnapTo(ref vector);
                _selectedObject.Position = vector;               
            }

            if (Operation == OperationType.ROTATE)
            {
                SnapToRotation(ref vector);
                _selectedObject.Rotation = vector;
            }
            if (Operation == OperationType.SCALE)
            {
                SnapTo(ref vector);
                _selectedObject.Scale = vector;
            }
        }

        private void UpdateGizmo()
        {
            if (_selectionGizmo == null) return;

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
                dropMenuItem.Enabled = _selectedObject.Components.Any(c => c is RigidBodyComponent);
                cloneMenuItem.Enabled = true;
            }
            else
            {
                dropMenuItem.Enabled = false;
                deleteMenuItem.Enabled = false;
                cloneMenuItem.Enabled = false;
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

            //var bytes = Game.SaveScene();

            var scene = new Scene(new Physics2d());

            //foreach (var gameObject in _workingScene.GameObjects.Where(o => !o.Designer))
            //{
            //    var clone = gameObject.Clone();
            //    if (clone.Components.Count() != gameObject.Components.Count())
            //    {
            //        throw new Exception("Component creation mode error: not counting equal component count after clone.");
            //    }

            //    scene.AddGameObject(clone);
            //}

            iGL.Engine.Game.InDesignMode = false;

            var xml = Game.SaveScene();

            Game.SetScene(scene);           
            Game.LoadScene(xml);

            Game.Load();

           // ((Physics2d)scene.Physics).SleepAll();

            //if (_workingScene.CurrentCamera != null)
            //{
            //    scene.SetCurrentCamera(scene.GameObjects.Single(g => g.Id == _workingScene.CurrentCamera.GameObject.Id));
            //}

            //if (_workingScene.CurrentLight != null)
            //{
            //    scene.SetCurrentLight(scene.GameObjects.Single(g => g.Id == _workingScene.CurrentLight.GameObject.Id));
            //}

            _isPlaying = true;

            if (PreStabilizePhysics)
            {
                /* stabilize physics */

                int iterations = 0;
                var physics2 = scene.Physics as Physics2d;

                scene.Physics.Step(0.001f);

                while (!physics2.CheckAllSleeping() && ++iterations < 10000)
                {
                    scene.Physics.Step(0.001f);
                }

                if (physics2.CheckAllSleeping())
                {
                    MessageBox.Show("Stable");
                }
            }
        }
        
        public void Pause()
        {
            _isPaused = true;
        }

        public void Stop()
        {
            _isPlaying = false;
            _isPaused = false;

            iGL.Engine.Game.InDesignMode = true;

            Game.SetScene(_workingScene);
        }

        private void dropMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedObject != null)
            {
                Operation = OperationType.DROPPING;
                _selectedObject.OnSleep += _selectedObject_OnSleep;
            }
        }

        void _selectedObject_OnSleep(object sender, SleepEvent e)
        {           
            MessageBox.Show("Sleeping");
            ((GameObject)sender).OnSleep -= _selectedObject_OnSleep;
        }

        private void SnapTo(ref Vector3 vector)
        {
            if (!Snap) return;

            vector.X = (float)((int)(vector.X / SnapValue)) * SnapValue;
            vector.Y = (float)((int)(vector.Y / SnapValue)) * SnapValue;
            vector.Z = (float)((int)(vector.Z / SnapValue)) * SnapValue;
        }

        private void SnapToRotation(ref Vector3 vector)
        {
            if (!Snap) return;

            vector.X = (float)((int)(vector.X / SnapValueRotation)) * SnapValueRotation;
            vector.Y = (float)((int)(vector.Y / SnapValueRotation)) * SnapValueRotation;
            vector.Z = (float)((int)(vector.Z / SnapValueRotation)) * SnapValueRotation;
        }

        private void cloneMenuItem_Click(object sender, EventArgs e)
        {
            var clone = _selectedObject.Clone();
            var count = _workingScene.GameObjects.Count(o => o.GetType() == _selectedObject.GetType());

            clone.Name = _selectedObject.GetType().Name + count.ToString();

            _workingScene.AddGameObject(clone);         
        }
    }
}
