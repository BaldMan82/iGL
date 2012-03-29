using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Engine.Math;

namespace iGL.Designer
{
    [GameObjectDialog(typeof(PerspectiveCameraComponent))]
    public partial class OrhogonalCameraComponentDlg : ComponentControl
    {
        public OrhogonalCameraComponentDlg()
        {
            InitializeComponent();           
        }

        private void OrhogonalCameraComponentDlg_Load(object sender, EventArgs e)
        {
            var camera = Component as OrthogonalCameraComponent;

            txtOrthogonalWidth.Text = camera.Width.ToInvariantText();
            txtOrthogonalHeight.Text = camera.Height.ToInvariantText();
            txtOrthogonalNearPlane.Text = camera.ZNear.ToInvariantText();
            txtOrthogonalFarPlane.Text = camera.ZFar.ToInvariantText();
        }

        public override void UpdateComponent() 
        {
            var camera = Component as OrthogonalCameraComponent;
          
            camera.Width = txtOrthogonalWidth.TextToFloat();
            camera.Height = txtOrthogonalHeight.TextToFloat();
            camera.ZNear = txtOrthogonalNearPlane.TextToFloat();
            camera.ZFar = txtOrthogonalFarPlane.TextToFloat();
                      
            camera.Update();
        }      

        private void SetDefaults()
        {
            txtOrthogonalWidth.Text = (15.0f).ToInvariantText();
            txtOrthogonalHeight.Text = (10.0f).ToInvariantText();
            txtOrthogonalNearPlane.Text = (1.0f).ToInvariantText();
            txtOrthogonalFarPlane.Text = (1000.0f).ToInvariantText();
       
        }
            
    }
}
