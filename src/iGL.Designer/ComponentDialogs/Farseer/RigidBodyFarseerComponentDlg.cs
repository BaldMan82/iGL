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
    [GameObjectDialog(typeof(RigidBodyFarseerComponent))]
    public partial class RigidBodyFarseerComponentDlg : ComponentControl
    {
        public RigidBodyFarseerComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
            var rigidBodyComponent = this.Component as RigidBodyFarseerComponent;

            rigidBodyComponent.IsGravitySource = cbGravitySource.Checked;
            rigidBodyComponent.Mass = txtMass.TextToFloat(defaultValue:100.0f);
            rigidBodyComponent.Friction = txtKineticFriction.TextToFloat();          
            rigidBodyComponent.Restitution = txtRestitution.TextToFloat();
            rigidBodyComponent.IsSensor = cbSensor.Checked;
            rigidBodyComponent.GravityRange = txtGravityRange.TextToFloat();

            if ((string)ddBodyType.SelectedItem == "Static") rigidBodyComponent.IsStatic = true;
            if ((string)ddBodyType.SelectedItem == "Kinematic") rigidBodyComponent.IsKinematic = true;
            if ((string)ddBodyType.SelectedItem == "Dynamic")
            {
                rigidBodyComponent.IsStatic = false;
                rigidBodyComponent.IsKinematic = false;
            }
        }

        private void RigidBodyComponentDlg_Load(object sender, EventArgs e)
        {
            var rigidBodyComponent = this.Component as RigidBodyFarseerComponent;            

            cbGravitySource.Checked = rigidBodyComponent.IsGravitySource;
            cbSensor.Checked = rigidBodyComponent.IsSensor;

            txtMass.Text = rigidBodyComponent.Mass.ToInvariantText();
            txtKineticFriction.Text = rigidBodyComponent.Friction.ToInvariantText();  
            txtRestitution.Text = rigidBodyComponent.Restitution.ToInvariantText();
            txtGravityRange.Text = rigidBodyComponent.GravityRange.ToInvariantText();

            if (rigidBodyComponent.IsStatic) ddBodyType.SelectedItem = "Static";
            else if (rigidBodyComponent.IsKinematic) ddBodyType.SelectedItem = "Kinematic";
            else ddBodyType.SelectedItem = "Dynamic";

            ddBodyType.SelectedIndexChanged += new EventHandler(ddBodyType_SelectedIndexChanged);
        }

        void ddBodyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComponent();
        }
    }
}
