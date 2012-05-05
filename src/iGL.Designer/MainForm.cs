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

            openTKControl.OnObjectAdded += new EventHandler<OpenTKControl.ObjectAddedEvent>(openTKControl_OnObjectAdded);
            openTKControl.OnSelectObject += new EventHandler<OpenTKControl.SelectObjectEvent>(openTKControl_OnSelectObject);
            openTKControl.OnObjectRemoved += new EventHandler<OpenTKControl.ObjectRemovedEvent>(openTKControl_OnObjectRemoved);
            openTKControl.OnSceneLoaded += new EventHandler<OpenTKControl.SceneLoadedEvent>(openTKControl_OnSceneLoaded);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _lastRender = DateTime.UtcNow;
            _lastTick = DateTime.UtcNow;

            LoadGameObjectTree();

            renderTimer.Start();
            tickTimer.Start();                  

            sceneControl.OnNodeSelected += new EventHandler<SceneControl.NodeSelected>(sceneControl_OnNodeSelected);
            sceneControl.OnNodeUnSelected += new EventHandler<SceneControl.NodeUnSelected>(sceneControl_OnNodeUnSelected);
        }

        void sceneControl_OnNodeUnSelected(object sender, SceneControl.NodeUnSelected e)
        {
            if (e.NodeValue is GameObject)
            {
                var obj = e.NodeValue as GameObject;

                obj.OnComponentAdded -= gameObject_OnComponentAdded;
                obj.OnComponentRemoved -= gameObject_OnComponentRemoved;
            }
        }

        void sceneControl_OnNodeSelected(object sender, SceneControl.NodeSelected e)
        {
            tabControl1.SelectedTab = propertiesTab;

            foreach (var control in flowLayoutPanel1.Controls)
            {
                if (control is GameObjectDlg) ((GameObjectDlg)control).Unload();
            }

            flowLayoutPanel1.Controls.Clear();

            if (e.NodeValue is Scene)
            {
                var scene = e.NodeValue as Scene;

                var control = new SceneControlDlg();

                control.Scene = openTKControl.Game.Scene;            
                flowLayoutPanel1.Controls.Add(control);
            }
            else
            {
                var obj = e.NodeValue as GameObject;

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

        void openTKControl_OnSceneLoaded(object sender, OpenTKControl.SceneLoadedEvent e)
        {
            sceneControl.LoadScene(e.Scene);
        }

        void openTKControl_OnObjectRemoved(object sender, OpenTKControl.ObjectRemovedEvent e)
        {
            sceneControl.RefreshScene();
        }

        void openTKControl_OnSelectObject(object sender, OpenTKControl.SelectObjectEvent e)
        {
            SelectObject(e.SelectedObject);
        }

        void openTKControl_OnObjectAdded(object sender, OpenTKControl.ObjectAddedEvent e)
        {
            sceneControl.RefreshScene();
        }

        private void tickTimer_Tick(object sender, EventArgs e)
        {
            float timePassed = (float)(DateTime.UtcNow - _lastTick).TotalSeconds;
            //if (timePassed < (1.0f / 100.0f)) return;

            

            openTKControl.Tick(timePassed*2);
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

            sceneControl.SelectNodeWithValue(obj);
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
            if (string.IsNullOrEmpty(_currentFilename))
            {
                saveToolStripMenuItem1_Click(sender, e);
            }
            else
            {
                Save(_currentFilename);
            }
        }

        private void Save(string filename)
        {
            if (openTKControl.IsPlaying)
            {
                MessageBox.Show("Cannot save while playing.");
                return;
            }

            using (FileStream f = new FileStream(filename, FileMode.Create))
            {
                var xml = EditorGame.Instance().SaveScene();

                var encoder = new System.Text.UTF8Encoding();
                var bytes = encoder.GetBytes(xml);

                f.Write(bytes, 0, bytes.Length);

                _currentFilename = saveFileDialog.FileName;
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderTimer.Stop();
            tickTimer.Stop();         

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream f = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    var bytes = new byte[f.Length];
                    f.Read(bytes, 0, bytes.Length);

                    var encoder = new System.Text.UTF8Encoding();
                    
                    openTKControl.LoadScene(encoder.GetString(bytes));

                    _currentFilename = openFileDialog.FileName;
                    sceneControl.SelectNodeWithValue(openTKControl.WorkingScene);
                }
            }

            openTKControl.Tick(0.01f);

            renderTimer.Start();
            tickTimer.Start();         
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sceneControl.Clear();
            openTKControl.LoadScene(null);
            sceneControl.SelectNodeWithValue(openTKControl.WorkingScene);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Save(saveFileDialog.FileName);
            }
        }

        private void textureConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var converter = new TextureConverter();
            converter.ShowDialog();
        }

        private void sceneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

       
    }
}
