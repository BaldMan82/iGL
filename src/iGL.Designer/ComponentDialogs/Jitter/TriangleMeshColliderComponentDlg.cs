using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Engine.GameComponents;

namespace iGL.Designer.ComponentDialogs
{
    [GameObjectDialog(typeof(TriangleMeshColliderComponent))]
    public partial class TriangleMeshColliderComponentDlg : ComponentControl
    {
        public TriangleMeshColliderComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
           
        }
    }
}
