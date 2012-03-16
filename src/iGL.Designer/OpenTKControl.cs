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
        
        public EditOperationType EditOperation { get; set; }

        public EditorGame Game { get; private set; }

        public enum EditAxisType
        {
            XAXIS,
            YAXIS,
            ZAXIS,
            ALL
        }

        public enum EditOperationType
        {
            MOVE,
            ROTATE,
            SCALE
        }

        private bool _glLoaded = false;
        private GameObject _selectedObject;
        private Gizmo _selectionGizmo;
        private Scene _scene;

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
                var count = _scene.GameObjects.Count(o => o.GetType() == (Type)node.Tag);

                instance.Name = ((Type)node.Tag).Name + count.ToString();

                if (!(instance is Camera))
                {
                    Vector4 nearPlane, farPlane;
                    var point = PointToClient(new System.Drawing.Point(e.X, e.Y));
                    _scene.ScreenPointToWorld(new Engine.Math.Point(point.X, point.Y), out nearPlane, out farPlane);

                    var position = new Vector3(nearPlane);

                    var direction = new Vector3(farPlane - nearPlane);
                    direction.Normalize();
                    direction = Vector3.Multiply(direction, 10);

                    instance.Position = position + direction;
                }

                _scene.AddGameObject(instance);

                if (_scene.CurrentCamera == null && instance.Components.Any(c => c is CameraComponent))
                {
                    _scene.SetCurrentCamera(instance);
                }

                if (_objectAddedEvent != null)
                {
                    _objectAddedEvent(this, new ObjectAddedEvent() { AddedObject = instance });
                }                

                /* hook up mouse events */

                instance.OnMouseIn += (a, b) => Cursor = Cursors.Cross;
                instance.OnMouseOut += (a, b) => Cursor = Cursors.Arrow;
                instance.OnMouseDown += (a, b) =>
                {
                    _selectedObject = a as GameObject;
                    UpdateGizmo();

                    if (_selectObjectEvent != null)
                    {
                        _selectObjectEvent(this, new SelectObjectEvent() { SelectedObject = a as GameObject });
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
            Game.MouseButton(Engine.Events.MouseButton.Button1, false, e.X, e.Y);
        }

        void OpenTKControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left || 
                e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Game.MouseButton(Engine.Events.MouseButton.Button1, true, e.X, e.Y);
                if (_scene.LastMouseDownTarget == null)
                {
                    _selectedObject = null;
                    if (_selectObjectEvent != null)
                    {
                        _selectObjectEvent(this, new SelectObjectEvent());
                    }
                }             
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

            _scene = new Scene();

            _scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(_scene_OnMouseMove);

            /* origin gizmo */
            var gizmo = new Gizmo(30.0f);
            gizmo.UniformSphere.Visible = false;

            _scene.AddGameObject(gizmo);

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

            _scene.AddGameObject(_selectionGizmo);

            Game.SetScene(_scene);

            var size = ClientSize;

            Game.Resize(size.Width, size.Height);
            Game.Load();

            _scene.AmbientColor = new Vector4(1, 1, 1, 1);
        }

        void OpenTKControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        public void Tick(float timeElapsed)
        {
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
            if (_selectedObject == null) return;

            /* perform edit operation */
            /* size of directional vector gives an indication of mouse movement magnitude */

            //var distance = (e.NearPlane - _selectedObject.Position).Length;
            var distance = 1.0f;

            if (_scene.CurrentCamera.Properties is PerspectiveProperties)
            {
                /* in a projection matrix, calculate the near plane / object plane ratio */
                var camProps = _scene.CurrentCamera.Properties as PerspectiveProperties;

                var direction = _selectedObject.Position - _scene.CurrentCamera.GameObject.Position;
                var directionNorm = direction;
                directionNorm.Normalize();

                var center = _scene.CurrentCamera.Target - _scene.CurrentCamera.GameObject.Position;
                center.Normalize();

                var objectDistance = Vector3.Dot(center, directionNorm) * direction.Length;

                distance = objectDistance;
            }

            var vector = EditOperation == EditOperationType.MOVE ? _selectedObject.Position :
                        (EditOperation == EditOperationType.ROTATE ? _selectedObject.Rotation :
                        (EditOperation == EditOperationType.SCALE ? _selectedObject.Scale : new Vector3()));

            if (EditAxis.HasValue)
            {
                if (EditAxis.Value == EditAxisType.XAXIS)
                {
                    var change = new Vector3(e.DirectionOnNearPlane.X * distance, 0, 0);
                    vector += change;
                }
                else if (EditAxis.Value == EditAxisType.YAXIS)
                {
                    var change = new Vector3(0, e.DirectionOnNearPlane.Y * distance, 0);
                    vector += change;
                }
                else if (EditAxis.Value == EditAxisType.ZAXIS)
                {
                    var change = new Vector3(0, 0, e.DirectionOnNearPlane.Y * distance);
                    vector += change;
                }
                else if (EditAxis.Value == EditAxisType.ALL)
                {
                    var change = new Vector3(e.DirectionOnNearPlane.X * distance, e.DirectionOnNearPlane.Y * distance, e.DirectionOnNearPlane.Z * distance);
                    vector += change;
                }
            }

            if (EditOperation == EditOperationType.MOVE) _selectedObject.Position = vector;
            if (EditOperation == EditOperationType.ROTATE) _selectedObject.Rotation = vector;
            if (EditOperation == EditOperationType.SCALE) _selectedObject.Scale = vector;

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
    }
}
