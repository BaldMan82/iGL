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
    [GameObjectDialog(typeof(AlphaAnimationComponent))]
    public partial class AlphaAnimationComponentDlg : ComponentControl
    {
        public AlphaAnimationComponentDlg()
        {
            InitializeComponent();
            this.Load += new EventHandler(AlphaAnimationComponentDlg_Load);

        }

        void AlphaAnimationComponentDlg_Load(object sender, EventArgs e)
        {
            var scaleComponent = Component as AlphaAnimationComponent;
            txtDuration.Text = scaleComponent.DurationSeconds.ToInvariantText();
        }

        public override void UpdateComponent()
        {
            var scaleComponent = Component as AlphaAnimationComponent;
            scaleComponent.DurationSeconds = txtDuration.TextToFloat();
            scaleComponent.AutoStart = cbAutoStart.Checked;
        }

    }
}
