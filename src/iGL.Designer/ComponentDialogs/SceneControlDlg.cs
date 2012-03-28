﻿using System;
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

        private void ddCameras_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;

            Scene.SetCurrentCamera(ddCameras.SelectedItem as GameObject);
        }

        private void SceneControlDlg_Load(object sender, EventArgs e)
        {
            var cameras = Scene.GameObjects.Where(g => g.Components.Any(c => c is CameraComponent)).ToList();

            cameras.ForEach(c => { 
                int index = ddCameras.Items.Add(c);
                if (Scene.CurrentCamera != null && Scene.CurrentCamera.GameObject == c) ddCameras.SelectedIndex = index;
            });          

            var lights = Scene.GameObjects.Where(g => g.Components.Any(c => c is LightComponent)).ToList();
            lights.ForEach(l => {
                int index = ddLights.Items.Add(l);
                if (Scene.CurrentLight != null && Scene.CurrentLight.GameObject == l) ddLights.SelectedIndex = index;
            });          

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
    }
}