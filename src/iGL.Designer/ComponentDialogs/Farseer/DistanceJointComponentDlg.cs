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
    [GameObjectDialog(typeof(DistanceJointFarseerComponent))]
    public partial class DistanceJointFarseerComponentDlg : ComponentControl
    {
        private bool internalUpdate = false;
        private DistanceJointFarseerComponent distanceJoint;

        public DistanceJointFarseerComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
            if (internalUpdate) return;
        }

        private void DistanceJointFarseerComponentDlg_Load(object sender, EventArgs e)
        {
            distanceJoint = this.Component as DistanceJointFarseerComponent;

            internalUpdate = true;

            var objects = EditorGame.Instance().Scene.GameObjects.Where(g => g.Components.Any(c => c is RigidBodyFarseerComponent) && g != distanceJoint.GameObject);

            foreach (var o in objects)
            {
                ddObject.Items.Add(o);
                ddObject.DisplayMember = "Name";
                ddObject.ValueMember = "Id";
           
            }

            ddObject.SelectedItem = EditorGame.Instance().Scene.GameObjects.FirstOrDefault(g => g.Id == distanceJoint.OtherObjectId);

            internalUpdate = false;

            ddObject.SelectedValueChanged +=ddObject_SelectedValueChanged;
        }

        void ddObject_SelectedValueChanged(object sender, EventArgs e)
        {
            distanceJoint.OtherObjectId = ((GameObject)ddObject.SelectedItem).Id;
        }
    }
}
