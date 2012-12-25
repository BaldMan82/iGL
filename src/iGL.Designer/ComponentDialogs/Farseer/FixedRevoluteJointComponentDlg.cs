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
    [GameObjectDialog(typeof(FixedRevoluteJointComponent))]
    public partial class FixedRevoluteJointComponentDlg : ComponentControl
    {
        public FixedRevoluteJointComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
            var revoluteComponent = this.Component as FixedRevoluteJointComponent;
            revoluteComponent.MotorEnabled = cbMotorEnabled.Checked;
            revoluteComponent.MaxMotorTorque = txtMaxTorque.TextToFloat();
            revoluteComponent.MotorSpeed = txtMotorSpeed.TextToFloat();
            revoluteComponent.MotorTorque = txtMaxTorque.TextToFloat();

            revoluteComponent.UpdateMotorProperties();
        }

        private void FixedRevoluteJointComponentDlg_Load(object sender, EventArgs e)
        {
            var revoluteComponent = this.Component as FixedRevoluteJointComponent;
            cbMotorEnabled.Checked = revoluteComponent.MotorEnabled;
            txtMaxTorque.Text = revoluteComponent.MaxMotorTorque.ToInvariantText();
            txtMotorSpeed.Text = revoluteComponent.MotorSpeed.ToInvariantText();
            txtTorque.Text = revoluteComponent.MotorTorque.ToInvariantText();
        }
    }
}
