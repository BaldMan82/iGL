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
    [GameObjectDialog(typeof(CameraComponent))]
    public partial class CameraComponentDlg : ComponentControl
    {       
        public CameraComponentDlg()
        {
            InitializeComponent();           
        }

        private void CameraComponentDlg_Load(object sender, EventArgs e)
        {
            CameraComponent camera = Component as CameraComponent;

            if (camera.Properties is OrtographicProperties)
            {
                var orthographicProperties = camera.Properties as OrtographicProperties;
                txtOrthogonalWidth.Text = orthographicProperties.Width.ToInvariantText();
                txtOrthogonalHeight.Text = orthographicProperties.Height.ToInvariantText();
                txtOrthogonalNearPlane.Text = orthographicProperties.ZNear.ToInvariantText();
                txtOrthogonalFarPlane.Text = orthographicProperties.ZFar.ToInvariantText();

                radioOrthogonal.Checked = true;

            }
            else if (camera.Properties is PerspectiveProperties)
            {
                var perspectiveProperties = camera.Properties as PerspectiveProperties;
                txtPerspectiveAspectRatio.Text = perspectiveProperties.AspectRatio.ToInvariantText();
                txtPerspectiveFOV.Text = perspectiveProperties.FieldOfViewRadians.ToInvariantText();
                txtPerspectiveFarPlane.Text = perspectiveProperties.ZFar.ToInvariantText();
                txtPerspectiveNearPlane.Text = perspectiveProperties.ZNear.ToInvariantText();

                radioPerspective.Checked = true;
            }
        }

        public override void UpdateComponent() 
        {
            CameraComponent camera = Component as CameraComponent;

            if (radioOrthogonal.Checked)
            {
                var orthographicProperties = new OrtographicProperties();
                orthographicProperties.Width = txtOrthogonalWidth.TextToFloat();
                orthographicProperties.Height = txtOrthogonalHeight.TextToFloat();
                orthographicProperties.ZNear = txtOrthogonalNearPlane.TextToFloat();
                orthographicProperties.ZFar = txtOrthogonalFarPlane.TextToFloat();

                camera.Properties = orthographicProperties;
            }
            else
            {
                var perspectiveProperties = new PerspectiveProperties();
                perspectiveProperties.AspectRatio = txtPerspectiveAspectRatio.TextToFloat();
                perspectiveProperties.FieldOfViewRadians = txtPerspectiveFOV.TextToFloat();
                perspectiveProperties.ZFar = txtPerspectiveFarPlane.TextToFloat();
                perspectiveProperties.ZNear = txtPerspectiveNearPlane.TextToFloat();

                camera.Properties = perspectiveProperties;
            }

            camera.Update();
        }

        private void radioPerspective_CheckedChanged(object sender, EventArgs e)
        {
            SetDefaults();
        }

        private void radioOrthogonal_CheckedChanged(object sender, EventArgs e)
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            txtOrthogonalWidth.Text = (15.0f).ToInvariantText();
            txtOrthogonalHeight.Text = (10.0f).ToInvariantText();
            txtOrthogonalNearPlane.Text = (1.0f).ToInvariantText();
            txtOrthogonalFarPlane.Text = (1000.0f).ToInvariantText();

            txtPerspectiveAspectRatio.Text = (3.0f / 2.0f).ToInvariantText();
            txtPerspectiveFOV.Text = MathHelper.DegreesToRadians(45.0f).ToInvariantText();
            txtPerspectiveNearPlane.Text = (1.0f).ToInvariantText();
            txtPerspectiveFarPlane.Text = (1000.0f).ToInvariantText();
        }
    }
}
