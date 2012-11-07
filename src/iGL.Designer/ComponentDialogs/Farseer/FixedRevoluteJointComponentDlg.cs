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
           
        }
    }
}
