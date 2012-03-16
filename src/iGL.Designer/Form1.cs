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
using iGL.Engine.Math;

namespace iGL.Designer
{
    public partial class Form1 : Form
    {          
        private EditorGame _game;      
        private DateTime _lastRender;
        private DateTime _lastTick;

        private Dictionary<Type, Type> _gameObjectDialogTypes;
       
        private System.Drawing.Point _lastMousePosition;
            
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

            renderTimer.Start();
            tickTimer.Start();

            openTKControl.OnObjectAdded += new EventHandler<OpenTKControl.ObjectAddedEvent>(openTKControl_OnObjectAdded);
            openTKControl.OnSelectObject += new EventHandler<OpenTKControl.SelectObjectEvent>(openTKControl_OnSelectObject);
            openTKControl.OnObjectRemoved += new EventHandler<OpenTKControl.ObjectRemovedEvent>(openTKControl_OnObjectRemoved);
        }

        void openTKControl_OnObjectRemoved(object sender, OpenTKControl.ObjectRemovedEvent e)
        {
            UpdateSceneTree();
        }

        void openTKControl_OnSelectObject(object sender, OpenTKControl.SelectObjectEvent e)
        {
            SelectObject(e.SelectedObject);
        }

        void openTKControl_OnObjectAdded(object sender, OpenTKControl.ObjectAddedEvent e)
        {
            UpdateSceneTree();
        }                     
      
        private void tickTimer_Tick(object sender, EventArgs e)
        {
            float timePassed = (float)(DateTime.UtcNow - _lastTick).TotalSeconds;
            if (timePassed < (1.0f / 100.0f)) return;

            openTKControl.Tick(timePassed);            

            _lastTick = DateTime.UtcNow;
        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            openTKControl.Render();
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

            openTKControl.EditOperation = iGL.Designer.OpenTKControl.EditOperationType.MOVE;

            /* select proper node */
            SelectNode(sceneTree.Nodes[0], obj);            
        }

        private void SelectNode(TreeNode node, GameObject obj)
        {
            if (node == null) return;

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

       

        private void UpdateSceneTree()
        {
            sceneTree.Nodes.Clear();

            var sceneNode = sceneTree.Nodes.Add("Scene");
            sceneNode.Tag = openTKControl.Game.Scene;

            foreach (var gameObject in openTKControl.Game.Scene.GameObjects.Where(g => !g.Designer))
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

                control.Scene = openTKControl.Game.Scene;

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
            openTKControl.EditOperation = OpenTKControl.EditOperationType.SCALE;
        }

        private void toolRotate_Click(object sender, EventArgs e)
        {
            openTKControl.EditOperation = OpenTKControl.EditOperationType.ROTATE;
        }

        private void toolTranslate_Click(object sender, EventArgs e)
        {
            openTKControl.EditOperation = OpenTKControl.EditOperationType.MOVE;
        }                      
    }
}
