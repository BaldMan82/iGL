using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using System.Threading;
using System.Reflection;
using iGL.Engine;
using iGL.Designer.Code;

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

        private void glControl1_Load(object sender, EventArgs e)
        {
            _glLoaded = true;
            _game = new EditorGame(new WinGL());
            _scene = new Scene();
            _game.SetScene(_scene);

            var size = glControl1.ClientSize;

            _game.Resize(size.Width, size.Height);
            _game.Load();

            renderTimer.Start();
            tickTimer.Start();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();  
        }

        private void Render()
        {
            if (!_glLoaded) return;
          
            _game.Render();

            glControl1.SwapBuffers();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            GLControl control = sender as GLControl;            
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            _game.MouseMove(e.X, e.Y);
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            _game.MouseButton(Engine.Events.MouseButton.Button1, true, e.X, e.Y);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            _game.MouseButton(Engine.Events.MouseButton.Button1, false, e.X, e.Y);
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

                UpdateSceneTree();

                /* hook up mouse events */

                instance.OnMouseIn += (a, b) => glControl1.Cursor = Cursors.Cross;
                instance.OnMouseOut += (a, b) => glControl1.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(((Type)node.Tag).Name + ": " + ex.Message);
            }
        }

        private void UpdateSceneTree()
        {
            sceneTree.Nodes.Clear();

            var sceneNode = sceneTree.Nodes.Add("Scene");
            sceneNode.Tag = _scene;

            foreach (var gameObject in _scene.GameObjects)
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
    }
}
