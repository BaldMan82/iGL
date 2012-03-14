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

namespace iGL.Designer.ComponentDialogs
{
    public partial class MaterialDlg : UserControl
    {
        private Material _material;

        public Material Material
        {
            get { return _material; }
            set
            {
                _material = value;
                ShowColors();
            }
        }

        public MaterialDlg()
        {
            InitializeComponent();
        }

        private void btnAmbient_Click(object sender, EventArgs e)
        {
            colorDialog.Color = _material.Ambient.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _material.Ambient = colorDialog.Color.ToVectorColor();
                _material.Ambient = new Vector4(_material.Ambient.X, _material.Ambient.Y, _material.Ambient.Z, txtAmbientAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void btnDiffuse_Click(object sender, EventArgs e)
        {
            colorDialog.Color = Material.Diffuse.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Material.Diffuse = colorDialog.Color.ToVectorColor();
                Material.Diffuse = new Vector4(Material.Diffuse.X, Material.Diffuse.Y, Material.Diffuse.Z, txtDiffuseAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void btnSpecular_Click(object sender, EventArgs e)
        {
            colorDialog.Color = Material.Specular.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Material.Specular = colorDialog.Color.ToVectorColor();
                Material.Specular = new Vector4(Material.Specular.X, Material.Specular.Y, Material.Specular.Z, txtSpecularAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void MaterialDlg_Load(object sender, EventArgs e)
        {
            if (Material == null) Material = new Material();

            ShowColors();
        }

        private void ShowColors()
        {
            pnlAmbient.BackColor = Material.Ambient.ToSystemColor();
            pnlDiffuse.BackColor = Material.Diffuse.ToSystemColor();
            pnlSpecular.BackColor = Material.Specular.ToSystemColor();

            txtAmbientAlpha.Text = Material.Ambient.W.ToInvariant();
            txtDiffuseAlpha.Text = Material.Diffuse.W.ToInvariant();
            txtSpecularAlpha.Text = Material.Specular.W.ToInvariant();
        }
    }
}
