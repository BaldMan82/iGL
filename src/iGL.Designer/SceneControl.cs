using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Engine.Resources;

namespace iGL.Designer
{
    public partial class SceneControl : UserControl
    {
        public class NodeSelected : EventArgs
        {
            public object NodeValue { get; set; }
        }

        public class NodeUnSelected : EventArgs
        {
            public object NodeValue { get; set; }
        }

        internal event EventHandler<NodeSelected> _nodeSelectedEvent;
        internal event EventHandler<NodeUnSelected> _nodeUnSelectedEvent;

        private Scene _scene;

        public SceneControl()
        {
            InitializeComponent();
            sceneTree.AfterSelect += new TreeViewEventHandler(sceneTree_AfterSelect);
            sceneTree.BeforeSelect += new TreeViewCancelEventHandler(sceneTree_BeforeSelect);
        }

        void sceneTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_nodeUnSelectedEvent != null) _nodeUnSelectedEvent(this, new NodeUnSelected() { NodeValue = e.Node.Tag });
        }

        void sceneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_nodeSelectedEvent != null) _nodeSelectedEvent(this, new NodeSelected() { NodeValue = e.Node.Tag });
        }

        public event EventHandler<NodeSelected> OnNodeSelected
        {
            add
            {
                _nodeSelectedEvent += value;
            }
            remove
            {
                _nodeSelectedEvent -= value;
            }
        }

        public event EventHandler<NodeUnSelected> OnNodeUnSelected
        {
            add
            {
                _nodeUnSelectedEvent += value;
            }
            remove
            {
                _nodeUnSelectedEvent -= value;
            }
        }

        public void LoadScene(Scene scene)
        {
            _scene = scene;
            scene.OnObjectAdded += new EventHandler<Engine.Events.GameObjectAddedEvent>(scene_OnObjectAdded);

            RefreshScene();
        }

        public void RefreshScene()
        {
            if (_scene == null) return;

            LoadSceneTree();
            LoadResourceTree();
        }

        public void Clear()
        {
            sceneTree.Nodes.Clear();
            resourceTree.Nodes.Clear();
        }

        public void SelectNodeWithValue(object value)
        {
            if (sceneTree.Nodes.Count > 0)
            {
                SelectNode(sceneTree.Nodes[0], value);
            }
        }

        private void SelectNode(TreeNode node, object obj)
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

        private void LoadResourceTree()
        {
            resourceTree.Nodes.Clear();

            var textureNode = resourceTree.Nodes.Add("Textures");

            foreach (var resource in _scene.Resources.Where(r => r is Texture))
            {
                textureNode.Nodes.Add(resource.Name);
            }

            var fontNode = resourceTree.Nodes.Add("Fonts");

            foreach (var resource in _scene.Resources.Where(r => r is iGL.Engine.Resources.Font))
            {
                fontNode.Nodes.Add(resource.Name);
            }

            var meshNode = resourceTree.Nodes.Add("Meshes");

            foreach (var resource in _scene.Resources.Where(r => r is iGL.Engine.Resources.ColladaMesh))
            {
                meshNode.Nodes.Add(resource.Name);
            }
        }

        private void LoadSceneTree()
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

        private void AddSceneNode(TreeNode sceneNode, GameObject gameObject)
        {
            var objName = string.IsNullOrEmpty(gameObject.Name) ? string.Format("[{0}] Unnamed", gameObject.GetType().ToString()) : gameObject.Name;
            var newNode = sceneNode.Nodes.Add(objName);
            newNode.Tag = gameObject;

            foreach (var child in gameObject.Children.Where(g => !g.Designer))
            {
                AddSceneNode(newNode, child);
            }
        }

        void scene_OnObjectAdded(object sender, Engine.Events.GameObjectAddedEvent e)
        {
            
        }

        private void toolStripAdd_Click(object sender, EventArgs e)
        {
            AddResourceDlg dlg = new AddResourceDlg();
            if (dlg.ShowDialog() == DialogResult.OK)
            {                
                _scene.AddResource(dlg.Resource);

                LoadResourceTree();
            }
        }

        private void toolStripAddFont_Click(object sender, EventArgs e)
        {

        }
    }
}
