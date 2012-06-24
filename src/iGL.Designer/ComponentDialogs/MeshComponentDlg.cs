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

namespace iGL.Designer.ComponentDialogs
{
    [GameObjectDialog(typeof(MeshComponent))]
    public partial class MeshComponentDlg : ComponentControl
    {
        private bool internalUpdate = false;

        public MeshComponentDlg()
        {
            InitializeComponent();
        }

        private void MeshComponentDlg_Load(object sender, EventArgs e)
        {
            internalUpdate = true;

            var meshComponent = this.Component as MeshComponent;
            materialDlg.Material = meshComponent.Material;

            comboTexture.Items.Add(string.Empty);
            comboNormalMap.Items.Add(string.Empty);

            foreach (var texture in EditorGame.Instance().Scene.Resources.Where(r => r is Texture))
            {
                comboTexture.Items.Add(texture.Name);
                comboNormalMap.Items.Add(texture.Name);
            }

            foreach (var meshResource in EditorGame.Instance().Scene.Resources.Where(r => r is ColladaMesh))
            {
                comboMeshResource.Items.Add(meshResource.Name);
            }

            comboTexture.SelectedItem = meshComponent.Material.TextureName;
            comboNormalMap.SelectedItem = meshComponent.Material.NormalTextureName;
            comboMeshResource.SelectedItem = meshComponent.MeshResourceName;

            internalUpdate = false;
        }

        public override void UpdateComponent()
        {
            if (internalUpdate) return;

            var meshComponent = this.Component as MeshComponent;

            string item = comboTexture.SelectedItem as string;
            if (!string.IsNullOrEmpty(item))
            {
                meshComponent.Material.TextureName = item;
            }
            else
            {
                meshComponent.Material.TextureName = null;
            }

            item = comboNormalMap.SelectedItem as string;
            if (!string.IsNullOrEmpty(item))
            {
                meshComponent.Material.NormalTextureName = item;
            }
            else
            {
                meshComponent.Material.NormalTextureName = null;
            }

            meshComponent.RefreshTexture();

            /* reload mesh render component to incorporate tiling */
            var renderComponent = meshComponent.GameObject.Components.FirstOrDefault(c => c is MeshRenderComponent) as MeshRenderComponent;
            if (renderComponent != null) renderComponent.Reload();
        }

        private void comboTexture_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComponent();
        }       

        private void comboMeshResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var meshComponent = this.Component as MeshComponent;
            if (meshComponent.MeshResourceName == comboMeshResource.SelectedItem as string) return;

            meshComponent.MeshResourceName = comboMeshResource.SelectedItem as string;
            meshComponent.Reload();

            /* reload render component if any */
            var renderComponent = this.Component.GameObject.Components.FirstOrDefault(c => c is MeshRenderComponent) as MeshRenderComponent;
            if (renderComponent != null) renderComponent.Reload();
        }

        private void comboNormalMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComponent();
        }
    }
}
