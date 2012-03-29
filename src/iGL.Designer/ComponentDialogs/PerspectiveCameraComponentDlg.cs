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
    public partial class PerspectiveCameraComponentDlg : ComponentControl
    {
        public PerspectiveCameraComponentDlg()
        {
            InitializeComponent();           
        }

        private void PerspectiveCameraComponentDlg_Load(object sender, EventArgs e)
        {
            var camera = Component as PerspectiveCameraComponent;

            txtPerspectiveAspectRatio.Text = camera.AspectRatio.ToInvariantText();
            txtPerspectiveFOV.Text = camera.FieldOfViewRadians.ToInvariantText();
            txtPerspectiveFarPlane.Text = camera.ZFar.ToInvariantText();
            txtPerspectiveNearPlane.Text = camera.ZNear.ToInvariantText();                
        }

        public override void UpdateComponent() 
        {
            var camera = Component as PerspectiveCameraComponent;

            camera.AspectRatio = txtPerspectiveAspectRatio.TextToFloat();
            camera.FieldOfViewRadians = txtPerspectiveFOV.TextToFloat();
            camera.ZFar = txtPerspectiveFarPlane.TextToFloat();
            camera.ZNear = txtPerspectiveNearPlane.TextToFloat();           

            camera.Update();
        }       

        private void SetDefaults()
        {           
            txtPerspectiveAspectRatio.Text = (3.0f / 2.0f).ToInvariantText();
            txtPerspectiveFOV.Text = MathHelper.DegreesToRadians(45.0f).ToInvariantText();
            txtPerspectiveNearPlane.Text = (1.0f).ToInvariantText();
            txtPerspectiveFarPlane.Text = (1000.0f).ToInvariantText();
        }
    }
}
