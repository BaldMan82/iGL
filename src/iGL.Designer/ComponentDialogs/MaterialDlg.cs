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
        private bool _internalUpdate;

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
            Controls.HookAllChangeEvents(() => UpdateColor());
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
                _material.Diffuse = colorDialog.Color.ToVectorColor();
                _material.Diffuse = new Vector4(_material.Diffuse.X, _material.Diffuse.Y, _material.Diffuse.Z, txtDiffuseAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void btnSpecular_Click(object sender, EventArgs e)
        {
            colorDialog.Color = Material.Specular.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _material.Specular = colorDialog.Color.ToVectorColor();
                _material.Specular = new Vector4(_material.Specular.X, _material.Specular.Y, _material.Specular.Z, txtSpecularAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void UpdateColor()
        {
            if (_internalUpdate) return;

            /* alpha text box changed */
            _material.Ambient = new Vector4(_material.Ambient.X, _material.Ambient.Y, _material.Ambient.Z, txtAmbientAlpha.TextToFloat());
            _material.Diffuse = new Vector4(_material.Diffuse.X, _material.Diffuse.Y, _material.Diffuse.Z, txtDiffuseAlpha.TextToFloat());
            _material.Specular = new Vector4(_material.Specular.X, _material.Specular.Y, _material.Specular.Z, txtSpecularAlpha.TextToFloat());
            
            _material.TextureRepeatX = cbTilingRepeatX.Checked;
            _material.TextureRepeatY = cbTilingRepeatY.Checked;

            _material.TextureTilingX = txtTilingX.TextToFloat();
            _material.TextureTilingY = txtTilingY.TextToFloat();
        }

        private void MaterialDlg_Load(object sender, EventArgs e)
        {
            if (_material == null) _material = new Material();

            ShowColors();          
        }

        private void ShowColors()
        {
            _internalUpdate = true;

            pnlAmbient.BackColor = _material.Ambient.ToSystemColor();
            pnlDiffuse.BackColor = _material.Diffuse.ToSystemColor();
            pnlSpecular.BackColor = _material.Specular.ToSystemColor();

            txtAmbientAlpha.Text = _material.Ambient.W.ToInvariantText();
            txtDiffuseAlpha.Text = _material.Diffuse.W.ToInvariantText();
            txtSpecularAlpha.Text = _material.Specular.W.ToInvariantText();

            cbTilingRepeatX.Checked = _material.TextureRepeatX;
            cbTilingRepeatY.Checked = _material.TextureRepeatY;

            txtTilingX.Text = _material.TextureTilingX.ToInvariantText();
            txtTilingY.Text = _material.TextureTilingY.ToInvariantText();

            _internalUpdate = false;
        }
    }
}
