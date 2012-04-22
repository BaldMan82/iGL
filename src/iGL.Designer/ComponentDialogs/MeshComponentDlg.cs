using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;

namespace iGL.Designer.ComponentDialogs
{
    [GameObjectDialog(typeof(MeshComponent))]
    public partial class MeshComponentDlg : ComponentControl
    {
        public MeshComponentDlg()
        {
            InitializeComponent();
        }

        private void MeshComponentDlg_Load(object sender, EventArgs e)
        {
            var meshComponent = this.Component as MeshComponent;        
            materialDlg.Material = meshComponent.Material;

            comboTexture.Items.Add(string.Empty);

            foreach (var texture in EditorGame.Instance().Scene.Resources.Where(r => r is Texture))
            {
                comboTexture.Items.Add(texture.Name);
            }

            comboTexture.SelectedItem = meshComponent.Material.TextureName;
        }

        public override void UpdateComponent()
        {
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

            ((MeshComponent)meshComponent).RefreshTexture();
        }

        private void comboTexture_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComponent();
        }
    }
}
