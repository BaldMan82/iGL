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
    [GameObjectDialog(typeof(RigidBodyComponent))]
    public partial class RigidBodyComponentDlg : ComponentControl
    {
        public RigidBodyComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
            var rigidBodyComponent = this.Component as RigidBodyComponent;
            rigidBodyComponent.IsStatic = cbStatic.Checked;
            rigidBodyComponent.IsGravitySource = cbGravitySource.Checked;
            rigidBodyComponent.Mass = txtMass.TextToFloat();
        }

        private void RigidBodyComponentDlg_Load(object sender, EventArgs e)
        {
            var rigidBodyComponent = this.Component as RigidBodyComponent;

            cbStatic.Checked = rigidBodyComponent.IsStatic;
            cbGravitySource.Checked = rigidBodyComponent.IsGravitySource;

            txtMass.Text = rigidBodyComponent.Mass.ToInvariantText();
        }
    }
}
