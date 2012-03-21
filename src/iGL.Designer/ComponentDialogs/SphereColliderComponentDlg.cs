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
    [GameObjectDialog(typeof(SphereColliderComponent))]
    public partial class SphereColliderComponentDlg : ComponentControl
    {
        public SphereColliderComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
           
        }
    }
}
