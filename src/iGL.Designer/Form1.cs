using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using iGL.Engine;
using iGL.Designer.Code;
using iGL.Engine.Math;

namespace iGL.Designer
{
    public partial class Form1 : Form
    {
        private bool _glLoaded = false;      
        private EditorGame _game;
        private Scene _scene;
      
        private DateTime _lastRender;
        private DateTime _lastTick;

        private Dictionary<Type, Type> _gameObjectDialogTypes;

        private GameObject _selectedObject;
        private Gizmo _selectionGizmo;
        private System.Drawing.Point _lastMousePosition;

        private enum EditAxis
        {
            XAXIS,
            YAXIS,
            ZAXIS,
            ALL
        }

        private enum EditOperation
        {
            MOVE,
            ROTATE,
            SCALE
        }

        private EditAxis? _editAxis;
        private EditOperation _editOperation;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _lastRender = DateTime.UtcNow;
            _lastTick = DateTime.UtcNow;

            LoadGameObjectTree();

            /* find all dialog types */
            var asm = Assembly.GetExecutingAssembly();

            var gameObjectDialogs = asm.GetTypes().Where(t => t.GetCustomAttributes(false).Any(o => o.GetType() == typeof(GameObjectDialogAttribute))).ToList();
            _gameObjectDialogTypes = new Dictionary<Type, Type>();

            foreach (var gameObjectDlg in gameObjectDialogs)
            {
                var attribute = gameObjectDlg.GetCustomAttributes(false).First(o => o.GetType() == typeof(GameObjectDialogAttribute)) as GameObjectDialogAttribute;
                _gameObjectDialogTypes.Add(attribute.GameObjectType, gameObjectDlg);

            }
        }      
       
        void _scene_OnMouseMove(object sender, Engine.Events.MouseMoveEvent e)
        {
            if (_selectedObject == null) return;

            /* perform edit operation */
            /* size of directional vector gives an indication of mouse movement magnitude */

            var distance = (e.NearPlane - _selectedObject.Position).Length;

            var vector = _editOperation == EditOperation.MOVE ? _selectedObject.Position :
                        (_editOperation == EditOperation.ROTATE ? _selectedObject.Rotation :
                        (_editOperation == EditOperation.SCALE ? _selectedObject.Scale : new Vector3()));          

            if (_editAxis.HasValue)
            {
                if (_editAxis.Value == EditAxis.XAXIS)
                {
                    var change = new Vector3(e.DirectionOnNearPlane.X * distance, 0, 0);
                    //change = Vector3.Transform(change, rotation);

                    vector += change;
                }
                else if (_editAxis.Value == EditAxis.YAXIS)
                {
                    var change = new Vector3(0, e.DirectionOnNearPlane.Y * distance, 0);
                    //change = Vector3.Transform(change, rotation);
                    vector += change;
                }
                else if (_editAxis.Value == EditAxis.ZAXIS)
                {
                    var change = new Vector3(0, 0,  e.DirectionOnNearPlane.Z * distance);
                    //change = Vector3.Transform(change, rotation);
                    vector += change;
                }
                else if (_editAxis.Value == EditAxis.ALL)
                {
                    var change = new Vector3(e.DirectionOnNearPlane.X * distance, e.DirectionOnNearPlane.Y * distance, e.DirectionOnNearPlane.Z * distance);
                    vector += change;
                }
            }

            if (_editOperation == EditOperation.MOVE) _selectedObject.Position = vector;
            if (_editOperation == EditOperation.ROTATE) _selectedObject.Rotation = vector;          
            if (_editOperation == EditOperation.SCALE) _selectedObject.Scale = vector;
        }             

        private void Render()
        {
            if (!_glLoaded) return;
          
            _game.Render();

            glControl1.SwapBuffers();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            _glLoaded = true;
            _game = new EditorGame(new WinGL());
            _scene = new Scene();

            _scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(_scene_OnMouseMove);

            /* origin gizmo */
            var gizmo = new Gizmo(30.0f);
            gizmo.UniformSphere.Visible = false;

            _scene.AddGameObject(gizmo);

            _selectionGizmo = new Gizmo();

            _selectionGizmo.Visible = false;
            _selectionGizmo.Enabled = false;

            _selectionGizmo.XDirectionArrow.OnMouseDown += (a, b) => _editAxis = EditAxis.XAXIS;
            _selectionGizmo.XDirectionArrow.OnMouseUp += (a, b) => _editAxis = null;
            _selectionGizmo.XDirectionArrow.OnMouseIn += (a, b) => glControl1.Cursor = Cursors.Hand;
            _selectionGizmo.XDirectionArrow.OnMouseOut += (a, b) => glControl1.Cursor = Cursors.Arrow;

            _selectionGizmo.YDirectionArrow.OnMouseDown += (a, b) => _editAxis = EditAxis.YAXIS;
            _selectionGizmo.YDirectionArrow.OnMouseUp += (a, b) => _editAxis = null;
            _selectionGizmo.YDirectionArrow.OnMouseIn += (a, b) => glControl1.Cursor = Cursors.Hand;
            _selectionGizmo.YDirectionArrow.OnMouseOut += (a, b) => glControl1.Cursor = Cursors.Arrow;

            _selectionGizmo.ZDirectionArrow.OnMouseDown += (a, b) => _editAxis = EditAxis.ZAXIS;
            _selectionGizmo.ZDirectionArrow.OnMouseUp += (a, b) => _editAxis = null;
            _selectionGizmo.ZDirectionArrow.OnMouseIn += (a, b) => glControl1.Cursor = Cursors.Hand;
            _selectionGizmo.ZDirectionArrow.OnMouseOut += (a, b) => glControl1.Cursor = Cursors.Arrow;

            _selectionGizmo.UniformSphere.OnMouseDown += (a, b) => _editAxis = EditAxis.ALL;
            _selectionGizmo.UniformSphere.OnMouseUp += (a, b) => _editAxis = null;
            _selectionGizmo.UniformSphere.OnMouseIn += (a, b) => glControl1.Cursor = Cursors.Hand;
            _selectionGizmo.UniformSphere.OnMouseOut += (a, b) => glControl1.Cursor = Cursors.Arrow;

            _scene.AddGameObject(_selectionGizmo);

            _game.SetScene(_scene);

            var size = glControl1.ClientSize;

            _game.Resize(size.Width, size.Height);
            _game.Load();

            renderTimer.Start();
            tickTimer.Start();

            _scene.AmbientColor = new Vector4(1, 1, 1, 1);
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl control = sender as OpenTK.GLControl;            
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {            
            _game.MouseMove(e.X, e.Y);
            _lastMousePosition = e.Location;
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            _game.MouseButton(Engine.Events.MouseButton.Button1, true, e.X, e.Y);
            if (_scene.LastMouseDownTarget == null)
            {
                SelectObject(null);
            }
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            _game.MouseButton(Engine.Events.MouseButton.Button1, false, e.X, e.Y);
        }

        private void glControl1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void glControl1_DragDrop(object sender, DragEventArgs e)
        {
            var node = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (node == null) return;
            try
            {
                var instance = Activator.CreateInstance(node.Tag as Type) as GameObject;
                var count = _scene.GameObjects.Count(o => o.GetType() == (Type)node.Tag);

                instance.Name = ((Type)node.Tag).Name + count.ToString();

                _scene.AddGameObject(instance);

                if (_scene.CurrentCamera == null && instance.Components.Any(c => c is CameraComponent))
                {
                    _scene.SetCurrentCamera(instance);
                }

                UpdateSceneTree();

                /* hook up mouse events */

                instance.OnMouseIn += (a, b) => glControl1.Cursor = Cursors.Cross;
                instance.OnMouseOut += (a, b) => glControl1.Cursor = Cursors.Arrow;
                instance.OnMouseDown += (a, b) =>
                {
                    SelectObject(a as GameObject);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(((Type)node.Tag).Name + ": " + ex.Message);
            }
        }

        private void tickTimer_Tick(object sender, EventArgs e)
        {
            float timePassed = (float)(DateTime.UtcNow - _lastTick).TotalSeconds;
            if (timePassed < (1.0f / 100.0f)) return;

            _game.Tick(timePassed);

            _lastTick = DateTime.UtcNow;
        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            UpdateGizmo();

            Render();
        }

        private void LoadGameObjectTree()
        {
            var engineAssembly = Assembly.GetAssembly(typeof(Game));
            var testGame = Assembly.GetAssembly(typeof(TestGame.TestGame));

            var gameObjects = GetGameObjectTypes(engineAssembly);
            gameObjects.AddRange(GetGameObjectTypes(testGame));

            var gameObjectsNode = _gameObjectsTree.Nodes.Add("Objects");

            foreach (var gameObject in gameObjects)
            {
                var node = gameObjectsNode.Nodes.Add(gameObject.Name);
                node.Tag = gameObject;
            }

            var gameComponents = GetGameComponentTypes(engineAssembly);
            gameComponents.AddRange(GetGameComponentTypes(testGame));

            var gameComponentsNode = _gameObjectsTree.Nodes.Add("Components");

            foreach (var gameComponent in gameComponents)
            {
                var node = gameComponentsNode.Nodes.Add(gameComponent.Name);
                node.Tag = gameComponent;
            }

            gameObjectsNode.ExpandAll();
            gameComponentsNode.ExpandAll();
        }

        private List<Type> GetGameObjectTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => HasGameObjectType(t) && !t.IsAbstract).ToList();
        }

        private bool HasGameObjectType(Type type)
        {
            if (type.IsAbstract) return false;

            if (type.BaseType == typeof(GameObject)) return true;
            if (type.BaseType == typeof(Object)) return false;

            return HasGameObjectType(type.BaseType);
        }

        private List<Type> GetGameComponentTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => HasGameComponentType(t) && !t.IsAbstract).ToList();
        }

        private bool HasGameComponentType(Type type)
        {
            if (type.BaseType == null) return false;

            if (type.BaseType == typeof(GameComponent)) return true;
            if (type.BaseType == typeof(Object)) return false;

            return HasGameComponentType(type.BaseType);
        }

        private void _gameObjectsTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
    
        private void SelectObject(GameObject obj)
        {
            if (obj != null)
            {
                toolStripStatusLabel.Text = string.Format("Selected: {0}", obj.Name == string.Empty ? "[Unnamed]" : obj.Name);
            }
            else
            {
                toolStripStatusLabel.Text = "Ready";
            }

            _selectedObject = obj;

            _editOperation = EditOperation.MOVE;

            /* select proper node */
            SelectNode(sceneTree.TopNode, obj);            
        }

        private void SelectNode(TreeNode node, GameObject obj)
        {
            if (node.Tag == obj || obj == null && node.Tag is Scene)
            {
                sceneTree.SelectedNode = node;
                node.BackColor = Color.Silver;
            }
            else
            {
                node.BackColor = Color.White;
            }

            foreach (var childNode in node.Nodes)
            {
                SelectNode(childNode as TreeNode, obj);
            }          
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

        private void UpdateSceneTree()
        {
            sceneTree.Nodes.Clear();

            var sceneNode = sceneTree.Nodes.Add("Scene");
            sceneNode.Tag = _scene;

            foreach (var gameObject in _scene.GameObjects.Where(g => !g.Designer))
            {
                AddSceneNode(sceneNode, gameObject);
            }

            sceneNode.ExpandAll();
        }

        private void AddSceneNode(TreeNode node, GameObject gameObject)
        {
            var newNode = node.Nodes.Add(gameObject.Name);
            newNode.Tag = gameObject;

            foreach (var child in gameObject.Children)
            {
                AddSceneNode(newNode, child);
            }
        }

        private void sceneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tabControl1.SelectedTab = propertiesTab;

            flowLayoutPanel1.Controls.Clear();

            if (e.Node.Tag is Scene)
            {
                var scene = e.Node.Tag as Scene;

                var baseControl = _gameObjectDialogTypes[typeof(Scene)];
                var control = Activator.CreateInstance(baseControl) as SceneControlDlg;

                control.Scene = _scene;

                var label = new Label();
                label.Width = control.Width;
                label.BackColor = Color.Silver;
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Text = "Scene Properties";

                flowLayoutPanel1.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(control);
            }
            else
            {
                var obj = e.Node.Tag as GameObject;

                var baseControl = _gameObjectDialogTypes[typeof(GameObject)];
                var control = Activator.CreateInstance(baseControl) as BaseObjectControl;

                control.GameObject = obj;

                var label = new Label();
                label.Width = control.Width;
                label.BackColor = Color.Silver;
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Text = "Base Properties";

                flowLayoutPanel1.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(control);

                foreach (var component in obj.Components)
                {
                    var componentControl = Activator.CreateInstance(_gameObjectDialogTypes[component.GetType()]) as ComponentControl;
                    componentControl.Component = component;

                    var componentLabel = new Label();
                    componentLabel.Width = control.Width;
                    componentLabel.BackColor = Color.Silver;
                    componentLabel.BorderStyle = BorderStyle.FixedSingle;
                    componentLabel.Text = component.GetType().Name;

                    flowLayoutPanel1.Controls.Add(componentLabel);
                    flowLayoutPanel1.Controls.Add(componentControl);
                }
            }

        }

        private void toolScale_Click(object sender, EventArgs e)
        {
            _editOperation = EditOperation.SCALE;
        }

        private void toolRotate_Click(object sender, EventArgs e)
        {
            _editOperation = EditOperation.ROTATE;
        }

        private void toolTranslate_Click(object sender, EventArgs e)
        {
            _editOperation = EditOperation.MOVE;
        }                      
    }
}
