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
using System.Collections;
using System.IO;

namespace iGL.Designer
{
    public partial class MainForm : Form
    {
        private DateTime _lastRender;
        private DateTime _lastTick;
        private string _currentFilename;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _lastRender = DateTime.UtcNow;
            _lastTick = DateTime.UtcNow;

            LoadGameObjectTree();

            renderTimer.Start();
            tickTimer.Start();

            openTKControl.OnObjectAdded += new EventHandler<OpenTKControl.ObjectAddedEvent>(openTKControl_OnObjectAdded);
            openTKControl.OnSelectObject += new EventHandler<OpenTKControl.SelectObjectEvent>(openTKControl_OnSelectObject);
            openTKControl.OnObjectRemoved += new EventHandler<OpenTKControl.ObjectRemovedEvent>(openTKControl_OnObjectRemoved);

            UpdateSceneTree();
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

            var gameObjectsNode = _gameObjectsTree.Nodes.Add("Objects");

            foreach (var gameObject in EngineAssets.Instance.GameObjects)
            {
                var node = gameObjectsNode.Nodes.Add(gameObject.Name);
                node.Tag = gameObject;
            }

            var gameComponentsNode = _gameObjectsTree.Nodes.Add("Components");

            foreach (var gameComponent in EngineAssets.Instance.Components)
            {
                var node = gameComponentsNode.Nodes.Add(gameComponent.Name);
                node.Tag = gameComponent;
            }

            gameObjectsNode.ExpandAll();
            gameComponentsNode.ExpandAll();
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
                openTKControl.SetOperation(iGL.Designer.OpenTKControl.OperationType.MOVE);
            }
            else
            {
                toolStripStatusLabel.Text = "Ready";
            }

            /* select proper node */
            if (sceneTree.Nodes.Count > 0)
            {
                SelectNode(sceneTree.Nodes[0], obj);
            }
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

            foreach (var child in gameObject.Children.Where(g => !g.Designer))
            {
                AddSceneNode(newNode, child);
            }
        }

        private void sceneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tabControl1.SelectedTab = propertiesTab;
         
            foreach (var control in flowLayoutPanel1.Controls)
            {
                if (control is GameObjectDlg) ((GameObjectDlg)control).Unload();
            }

            flowLayoutPanel1.Controls.Clear();

            if (e.Node.Tag is Scene)
            {
                var scene = e.Node.Tag as Scene;

                var control = new SceneControlDlg();

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

                obj.OnComponentAdded += gameObject_OnComponentAdded;
                obj.OnComponentRemoved += gameObject_OnComponentRemoved;
               
                var control = new GameObjectDlg();
                control.GameObject = obj;

                flowLayoutPanel1.Controls.Add(control);
          
                foreach (var component in obj.Components)
                {
                    var componentPanel = new ComponentPanel();
                    componentPanel.LoadComponent(component);

                    flowLayoutPanel1.Controls.Add(componentPanel);
                }
            }

        }

        private void toolScale_Click(object sender, EventArgs e)
        {
            openTKControl.SetOperation(OpenTKControl.OperationType.SCALE);
        }

        private void toolRotate_Click(object sender, EventArgs e)
        {
            openTKControl.SetOperation(OpenTKControl.OperationType.ROTATE);
        }

        private void toolTranslate_Click(object sender, EventArgs e)
        {
            openTKControl.SetOperation(OpenTKControl.OperationType.MOVE);
        }

        private void toolPan_Click(object sender, EventArgs e)
        {
            openTKControl.SetOperation(OpenTKControl.OperationType.PANVIEW);
        }

        private void toolPointer_Click(object sender, EventArgs e)
        {
            openTKControl.SetOperation(OpenTKControl.OperationType.NONE);
        }

        private void sceneTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (sceneTree.SelectedNode != null && sceneTree.SelectedNode.Tag is GameObject)
            {
                var gameObject = sceneTree.SelectedNode.Tag as GameObject;
                gameObject.OnComponentAdded -= gameObject_OnComponentAdded;
                gameObject.OnComponentRemoved -= gameObject_OnComponentRemoved;
            }
        }

        void gameObject_OnComponentRemoved(object sender, Engine.Events.ComponentRemovedEvent e)
        {
            Control controlToRemove = null;
            foreach (var control in flowLayoutPanel1.Controls)
            {
                if (control is ComponentPanel && ((ComponentPanel)control).GameComponent == e.Component)
                {
                    controlToRemove = control as Control;
                    break;
                }
            }

            flowLayoutPanel1.Controls.Remove(controlToRemove);
        }

        void gameObject_OnComponentAdded(object sender, Engine.Events.ComponentAddedEvent e)
        {
            var componentPanel = new ComponentPanel();
            componentPanel.LoadComponent(e.Component);

            flowLayoutPanel1.Controls.Add(componentPanel);
        }

        private void toolPlay_Click(object sender, EventArgs e)
        {
            /* copy scene and play */

            openTKControl.Play();
        }

        private void toolStop_Click(object sender, EventArgs e)
        {
            openTKControl.Stop();
        }

        private void toolPause_Click(object sender, EventArgs e)
        {
            openTKControl.Pause();
        }

        private void toolStripSnap_Click(object sender, EventArgs e)
        {
            openTKControl.Snap = toolStripSnap.Checked;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStabelize_Click(object sender, EventArgs e)
        {
            openTKControl.PreStabilizePhysics = toolStripStabilize.Checked;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentFilename == null) saveToolStripMenuItem1_Click(sender, e);

            Save(_currentFilename);
        }

        private void Save(string filename)
        {
            using (FileStream f = new FileStream(filename, FileMode.Create))
            {
                var json = EditorGame.Instance().SaveSceneToJson();
                System.Text.UTF8Encoding encoding = new UTF8Encoding();
                var bytes = encoding.GetBytes(json);

                f.Write(bytes, 0, bytes.Length);

                f.Flush();
                f.Close();

                _currentFilename = saveFileDialog.FileName;
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream f = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    var bytes = new byte[f.Length];
                    f.Read(bytes, 0, (int)f.Length);
                    
                    var chars = new char[f.Length];

                    var decoder = System.Text.UTF8Encoding.UTF8.GetDecoder();
                    decoder.GetChars(bytes, 0, bytes.Length, chars, 0);

                    var json = new string(chars);

                    openTKControl.LoadScene(json);

                    _currentFilename = openFileDialog.FileName;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openTKControl.LoadScene(null);
            sceneTree.Nodes.Clear();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Save(saveFileDialog.FileName);
            }
        }

       
    }
}
