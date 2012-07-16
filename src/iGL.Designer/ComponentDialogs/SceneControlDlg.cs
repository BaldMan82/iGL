using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Designer.ComponentDialogs;
using iGL.Engine.Triggers;

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

            var objects = Scene.GameObjects.ToList();
            objects.ForEach(o =>
            {
                int index = ddPlayerObjects.Items.Add(o);
                if (Scene.PlayerObject != null && Scene.PlayerObject == o) ddPlayerObjects.SelectedIndex = index;
            });

            pnlAmbient.BackColor = Scene.AmbientColor.ToSystemColor();
            txtAmbientAlpha.Text = Scene.AmbientColor.W.ToInvariantText();

            _loaded = true;

            UpdateTriggerList();
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

        private void ddPlayerObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            Scene.SetPlayerObject(ddPlayerObjects.SelectedItem as GameObject);
        }

        private void txtAmbientAlpha_TextChanged(object sender, EventArgs e)
        {
            Scene.AmbientColor = new Engine.Math.Vector4(Scene.AmbientColor.X, Scene.AmbientColor.Y, Scene.AmbientColor.Z, txtAmbientAlpha.TextToFloat());
        }

        private void btnRemoveTrigger_Click(object sender, EventArgs e)
        {
            var selectedTrigger = listTriggers.SelectedItem as Trigger;
            if (selectedTrigger == null) return;

            Scene.RemoveTrigger(selectedTrigger);

            UpdateTriggerList();
        }

        private void btnAddTrigger_Click(object sender, EventArgs e)
        {
            var dlg = new TriggerDlg();
            
            dlg.Trigger = new Trigger();
            dlg.Scene = Scene;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Scene.AddTrigger(dlg.Trigger);
                UpdateTriggerList();
            }
        }

        private void UpdateTriggerList()
        {
            listTriggers.Items.Clear();

            foreach (var trigger in Scene.Triggers)
            {
                listTriggers.Items.Add(trigger);
            }
        }

        private void listTriggers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var dlg = new TriggerDlg();
            var selectedTrigger = listTriggers.SelectedItem as Trigger;

            var editTrigger = new Trigger();
            selectedTrigger.CopyPublicValues(editTrigger);

            dlg.Trigger = editTrigger;
            dlg.Scene = Scene;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Scene.RemoveTrigger(selectedTrigger);
                Scene.AddTrigger(dlg.Trigger);
                UpdateTriggerList();
            }
        }      
    }
}
