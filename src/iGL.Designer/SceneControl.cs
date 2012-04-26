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
        private Scene _scene;

        public SceneControl()
        {
            InitializeComponent();
        }

        public void Load(Scene scene)
        {
            _scene = scene;
            scene.OnObjectAdded += new EventHandler<Engine.Events.GameObjectAddedEvent>(scene_OnObjectAdded);

            LoadSceneTree();
            LoadResourceTree();
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
            var newNode = sceneNode.Nodes.Add(gameObject.Name);
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
