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

            _txtPositionX.Text = GameObject.Position.X.ToInvariant();
            _txtPositionY.Text = GameObject.Position.Y.ToInvariant();
            _txtPositionZ.Text = GameObject.Position.Z.ToInvariant();

            _txtRotationX.Text = GameObject.Rotation.X.ToInvariant();
            _txtRotationY.Text = GameObject.Rotation.Y.ToInvariant();
            _txtRotationZ.Text = GameObject.Rotation.Z.ToInvariant();

            _txtScaleX.Text = GameObject.Scale.X.ToInvariant();
            _txtScaleY.Text = GameObject.Scale.Y.ToInvariant();
            _txtScaleZ.Text = GameObject.Scale.Z.ToInvariant();
        }

        public override void UpdateGameObject()
        {
            base.UpdateGameObject();

            GameObject.Position = new Vector3(_txtPositionX.TextToFloat(), _txtPositionY.TextToFloat(), _txtPositionZ.TextToFloat());
            GameObject.Rotation = new Vector3(_txtRotationX.TextToFloat(), _txtRotationY.TextToFloat(), _txtRotationZ.TextToFloat());
            GameObject.Scale = new Vector3(_txtScaleX.TextToFloat(), _txtScaleY.TextToFloat(), _txtScaleZ.TextToFloat());
           
        }
    }
}
