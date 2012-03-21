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
    [GameObjectDialog(typeof(LightComponent))]
    public partial class LightComponentDlg : ComponentControl
    {
        public LightComponentDlg()
        {
            InitializeComponent();
        }

        public override void UpdateComponent()
        {
            LightComponent lightComponent = Component as LightComponent;

            lightComponent.Light = pointLightDlg1.PointLight;
        }

        private void LightComponentDlg_Load(object sender, EventArgs e)
        {
            LightComponent lightComponent = Component as LightComponent;

            pointLightDlg1.PointLight = lightComponent.Light as PointLight;
        }
    }
}
