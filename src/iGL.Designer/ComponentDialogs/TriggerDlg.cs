using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Engine.Triggers;

namespace iGL.Designer.ComponentDialogs
{    
    public partial class TriggerDlg : Form
    {
        public Trigger Trigger { get; set; }
        public Scene Scene { get; set; }

        public TriggerDlg()
        {
            InitializeComponent();

            this.Load += new EventHandler(TriggerDlg_Load);
        }

        void TriggerDlg_Load(object sender, EventArgs e)
        {
            
            var actions = Enum.GetNames(typeof(Trigger.TriggerAction));
            foreach (var action in actions)
            {
                var index = ddTriggerAction.Items.Add(action);
                if (action == Trigger.Action.ToString()) ddTriggerAction.SelectedIndex = index;
            }

            var types = Enum.GetNames(typeof(Trigger.TriggerType));
            foreach (var type in types)
            {
                var index = ddTriggerType.Items.Add(type);
                if (type == Trigger.Type.ToString()) ddTriggerType.SelectedIndex = index;
            }
        
            foreach (var animObject in Scene.GameObjects.Where(g => !g.Designer))
            {
                var index = ddTarget.Items.Add(animObject);
                if (animObject.Id == Trigger.TargetObjectId) ddTarget.SelectedIndex = index;
            }

            foreach (var sceneObject in Scene.GameObjects.Where(g => !g.Designer))
            {
                var index = ddTriggerSource.Items.Add(sceneObject);
                if (sceneObject.Id == Trigger.SourceObjectId) ddTriggerSource.SelectedIndex = index;
            }

            ddTriggerAction.SelectedIndexChanged += (a, b) => UpdateComponent();
            ddTriggerType.SelectedIndexChanged += (a, b) => UpdateComponent();
            ddTarget.SelectedIndexChanged += new EventHandler(ddTarget_SelectedIndexChanged);
            ddTargetComponent.SelectedIndexChanged += new EventHandler(ddTargetComponent_SelectedIndexChanged);
            ddTriggerSource.SelectedIndexChanged += new EventHandler(ddTriggerSource_SelectedIndexChanged);
            
            FillComponents();
        }

        void ddTriggerSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trigger.SourceObjectId = ((GameObject)(ddTriggerSource.SelectedItem)).Id;
        }

        void ddTargetComponent_SelectedIndexChanged(object sender, EventArgs e)
        {           
            Trigger.TargetComponentId = ((GameComponent)(ddTargetComponent.SelectedItem)).Id;
        }

        private void FillComponents()
        {
            if (ddTarget.SelectedItem == null) return;          

            ddTargetComponent.Items.Clear();
            foreach (var component in ((GameObject)(ddTarget.SelectedItem)).Components.Where(c => c is AnimationComponent))
            {
                var index = ddTargetComponent.Items.Add(component);
                if (component.Id == Trigger.TargetComponentId) ddTargetComponent.SelectedIndex = index;
            }
        }

        void ddTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trigger.TargetObjectId = ((GameObject)(ddTarget.SelectedItem)).Id;
            FillComponents();
        }
       
        public void UpdateComponent()
        {
            Trigger.Action = (Trigger.TriggerAction)Enum.Parse(typeof(Trigger.TriggerAction), (string)ddTriggerAction.SelectedItem);
            Trigger.Type = (Trigger.TriggerType)Enum.Parse(typeof(Trigger.TriggerType), (string)ddTriggerType.SelectedItem);
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
     
    }
}
