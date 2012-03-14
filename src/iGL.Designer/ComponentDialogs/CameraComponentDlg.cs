using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;

namespace iGL.Designer
{
    [GameObjectDialog(typeof(CameraComponent))]
    public partial class CameraComponentDlg : ComponentControl
    {       
        public CameraComponentDlg()
        {
            InitializeComponent();           
        }

        private void CameraComponentDlg_Load(object sender, EventArgs e)
        {

        }       
    }
}
