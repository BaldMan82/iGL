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
    public partial class PointLightDlg : UserControl
    {
        private PointLight _pointLight;

        public PointLight PointLight
        {
            get { return _pointLight; }
            set
            {
                _pointLight = value;
                ShowColors();
            }
        }

        public PointLightDlg()
        {
            InitializeComponent();
        }

        private void btnAmbient_Click(object sender, EventArgs e)
        {
            colorDialog.Color = _pointLight.Ambient.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _pointLight.Ambient = colorDialog.Color.ToVectorColor();
                _pointLight.Ambient = new Vector4(_pointLight.Ambient.X, _pointLight.Ambient.Y, _pointLight.Ambient.Z, txtAmbientAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void btnDiffuse_Click(object sender, EventArgs e)
        {
            colorDialog.Color = _pointLight.Diffuse.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _pointLight.Diffuse = colorDialog.Color.ToVectorColor();
                _pointLight.Diffuse = new Vector4(_pointLight.Diffuse.X, _pointLight.Diffuse.Y, _pointLight.Diffuse.Z, txtDiffuseAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void btnSpecular_Click(object sender, EventArgs e)
        {
            colorDialog.Color = _pointLight.Specular.ToSystemColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _pointLight.Specular = colorDialog.Color.ToVectorColor();
                _pointLight.Specular = new Vector4(_pointLight.Specular.X, _pointLight.Specular.Y, _pointLight.Specular.Z, txtSpecularAlpha.TextToFloat());
            }

            ShowColors();
        }

        private void MaterialDlg_Load(object sender, EventArgs e)
        {
            if (_pointLight == null) _pointLight = new PointLight();

            ShowColors();
        }

        private void ShowColors()
        {
            pnlAmbient.BackColor = _pointLight.Ambient.ToSystemColor();
            pnlDiffuse.BackColor = _pointLight.Diffuse.ToSystemColor();
            pnlSpecular.BackColor = _pointLight.Specular.ToSystemColor();

            txtAmbientAlpha.Text = _pointLight.Ambient.W.ToInvariantText();
            txtDiffuseAlpha.Text = _pointLight.Diffuse.W.ToInvariantText();
            txtSpecularAlpha.Text = _pointLight.Specular.W.ToInvariantText();
        }
    }
}
