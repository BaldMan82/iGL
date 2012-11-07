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
    [GameObjectDialog(typeof(MeshScaleAnimationComponent))]
    public partial class MeshScaleAnimationComponentDlg : ComponentControl
    {
        public MeshScaleAnimationComponentDlg()
        {
            InitializeComponent();
            this.Load += new EventHandler(MeshScaleAnimationComponentDlg_Load);

        }

        void MeshScaleAnimationComponentDlg_Load(object sender, EventArgs e)
        {
            var scaleComponent = Component as MeshScaleAnimationComponent;
            txtDuration.Text = scaleComponent.DurationSeconds.ToInvariantText();
        }

        public override void UpdateComponent()
        {
            var scaleComponent = Component as MeshScaleAnimationComponent;
            scaleComponent.DurationSeconds = txtDuration.TextToFloat();
            scaleComponent.AutoStart = cbAutoStart.Checked;
        }

    }
}
