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
    [GameObjectDialog(typeof(GameObject))]
    public partial class GameObjectDlg : BaseObjectControl
    {    
        public GameObjectDlg()
        {
            InitializeComponent();
        }

        private void GameObjectDlg_Load(object sender, EventArgs e)
        {
            if (GameObject == null) throw new NotSupportedException();

            _txtPositionX.Text = GameObject.Position.X.ToInvariantText();
            _txtPositionY.Text = GameObject.Position.Y.ToInvariantText();
            _txtPositionZ.Text = GameObject.Position.Z.ToInvariantText();

            _txtRotationX.Text = GameObject.Rotation.X.ToInvariantText();
            _txtRotationY.Text = GameObject.Rotation.Y.ToInvariantText();
            _txtRotationZ.Text = GameObject.Rotation.Z.ToInvariantText();

            _txtScaleX.Text = GameObject.Scale.X.ToInvariantText();
            _txtScaleY.Text = GameObject.Scale.Y.ToInvariantText();
            _txtScaleZ.Text = GameObject.Scale.Z.ToInvariantText();

            cbEnabled.Checked = GameObject.Enabled;
            cbVisible.Checked = GameObject.Visible;
        }

        public override void UpdateGameObject()
        {
            base.UpdateGameObject();

            GameObject.Position = new Vector3(_txtPositionX.TextToFloat(), _txtPositionY.TextToFloat(), _txtPositionZ.TextToFloat());
            GameObject.Rotation = new Vector3(_txtRotationX.TextToFloat(), _txtRotationY.TextToFloat(), _txtRotationZ.TextToFloat());
            GameObject.Scale = new Vector3(_txtScaleX.TextToFloat(), _txtScaleY.TextToFloat(), _txtScaleZ.TextToFloat());
            GameObject.Visible = cbVisible.Checked;
            GameObject.Enabled = cbEnabled.Checked;
        }

        private void btnResetPosition_Click(object sender, EventArgs e)
        {
            _txtPositionX.Text = (0.0f).ToInvariantText();
            _txtPositionY.Text = (0.0f).ToInvariantText();
            _txtPositionZ.Text = (0.0f).ToInvariantText();                              
        }

        private void btnResetRotation_Click(object sender, EventArgs e)
        {
            _txtRotationX.Text = (0.0f).ToInvariantText();
            _txtRotationY.Text = (0.0f).ToInvariantText();
            _txtRotationZ.Text = (0.0f).ToInvariantText();
        }

        private void btnResetScale_Click(object sender, EventArgs e)
        {
            _txtScaleX.Text = (1.0f).ToInvariantText();
            _txtScaleY.Text = (1.0f).ToInvariantText();
            _txtScaleZ.Text = (1.0f).ToInvariantText();
        }
    }
}
