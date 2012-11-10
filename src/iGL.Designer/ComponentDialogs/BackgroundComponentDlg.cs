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
    [GameObjectDialog(typeof(BackgroundComponent))]
    public partial class BackgroundComponentDlg : ComponentControl
    {
        public BackgroundComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
            
        }
    }
}
