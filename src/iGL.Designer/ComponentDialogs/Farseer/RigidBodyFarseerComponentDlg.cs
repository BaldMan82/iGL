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

            rigidBodyComponent.IsStatic = cbStatic.Checked;
            rigidBodyComponent.IsGravitySource = cbGravitySource.Checked;
            rigidBodyComponent.Mass = txtMass.TextToFloat(defaultValue:100.0f);
            rigidBodyComponent.Friction = txtKineticFriction.TextToFloat();          
            rigidBodyComponent.Restitution = txtRestitution.TextToFloat();
            rigidBodyComponent.IsSensor = cbSensor.Checked;
            rigidBodyComponent.GravityRange = txtGravityRange.TextToFloat();
        }

        private void RigidBodyComponentDlg_Load(object sender, EventArgs e)
        {
            var rigidBodyComponent = this.Component as RigidBodyFarseerComponent;

            cbStatic.Checked = rigidBodyComponent.IsStatic;
            cbGravitySource.Checked = rigidBodyComponent.IsGravitySource;
            cbSensor.Checked = rigidBodyComponent.IsSensor;

            txtMass.Text = rigidBodyComponent.Mass.ToInvariantText();
            txtKineticFriction.Text = rigidBodyComponent.Friction.ToInvariantText();  
            txtRestitution.Text = rigidBodyComponent.Restitution.ToInvariantText();
            txtGravityRange.Text = rigidBodyComponent.GravityRange.ToInvariantText();
        }
    }
}
