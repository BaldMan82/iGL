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
    [GameObjectDialog(typeof(MeshComponent))]
    public partial class MeshComponentDlg : ComponentControl
    {
        public MeshComponentDlg()
        {
            InitializeComponent();
        }

        private void MeshComponentDlg_Load(object sender, EventArgs e)
        {
            var meshComponent = this.Component as MeshComponent;        
            materialDlg.Material = meshComponent.Material;
        }
    }
}
