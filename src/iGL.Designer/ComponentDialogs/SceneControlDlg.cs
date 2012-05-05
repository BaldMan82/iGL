using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;

namespace iGL.Designer
{
    [GameObjectDialog(typeof(Scene))]
    public partial class SceneControlDlg : UserControl
    {
        private bool _loaded = false;

        public Scene Scene { get; set; }

        public SceneControlDlg()
        {
            InitializeComponent();
        }
    

        private void SceneControlDlg_Load(object sender, EventArgs e)
        {
            var cameras = Scene.GameObjects.Where(g => g.Components.Any(c => c is CameraComponent)).ToList();

            cameras.ForEach(c => { 
                int index = ddPlayCameras.Items.Add(c);
                if (Scene.PlayCamera != null && Scene.PlayCamera.GameObject == c) ddPlayCameras.SelectedIndex = index;

                index = ddDesignCameras.Items.Add(c);
                if (Scene.DesignCamera != null && Scene.DesignCamera.GameObject == c) ddDesignCameras.SelectedIndex = index;
            });          

            var lights = Scene.GameObjects.Where(g => g.Components.Any(c => c is LightComponent)).ToList();
            lights.ForEach(l => {
                int index = ddLights.Items.Add(l);
                if (Scene.CurrentLight != null && Scene.CurrentLight.GameObject == l) ddLights.SelectedIndex = index;
            });

            pnlAmbient.BackColor = Scene.AmbientColor.ToSystemColor();
            txtAmbientAlpha.Text = Scene.AmbientColor.W.ToInvariantText();

            _loaded = true;
        }

        private void ddLights_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;

            Scene.SetCurrentLight(ddLights.SelectedItem as GameObject);
        }              

        private void btnAmbient_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                var color = colorDialog.Color.ToVectorColor();
                Scene.AmbientColor = color;

                pnlAmbient.BackColor = colorDialog.Color;
            }
        }

        private void ddPlayCameras_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            Scene.SetPlayCamera(ddPlayCameras.SelectedItem as GameObject);
        }

        private void ddDesignCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            Scene.SetDesignCamera(ddDesignCameras.SelectedItem as GameObject);
        }

        private void txtAmbientAlpha_TextChanged(object sender, EventArgs e)
        {
            Scene.AmbientColor = new Engine.Math.Vector4(Scene.AmbientColor.X, Scene.AmbientColor.Y, Scene.AmbientColor.Z, txtAmbientAlpha.TextToFloat());
        }        
    }
}
