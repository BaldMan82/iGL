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

namespace iGL.Designer.ComponentDialogs
{
    [GameObjectDialog(typeof(LinearVelocitAnimationComponent))]
    public partial class LinearVelocityAnimationComponentDlg : ComponentControl
    {
        LinearVelocitAnimationComponent _component;

        public LinearVelocityAnimationComponentDlg()
        {
            InitializeComponent();
            Load += new EventHandler(LinearVelocityAnimationComponentDlg_Load);
        }

        void LinearVelocityAnimationComponentDlg_Load(object sender, EventArgs e)
        {
            _component = Component as LinearVelocitAnimationComponent;
            baseAnimationComponentDlg.AnimationComponent = _component;

            UpdateUI();
        }

        private void UpdateUI()
        {
            _txtEndPointAX.Text = _component.EndPointA.X.ToInvariantText();
            _txtEndPointAY.Text = _component.EndPointA.Y.ToInvariantText();
            _txtEndPointAZ.Text = _component.EndPointA.Z.ToInvariantText();

            _txtEndPointBX.Text = _component.EndPointB.X.ToInvariantText();
            _txtEndPointBY.Text = _component.EndPointB.Y.ToInvariantText();
            _txtEndPointBZ.Text = _component.EndPointB.Z.ToInvariantText();
           
        }
       
        public override void UpdateComponent()
        {
            var component = Component as LinearVelocitAnimationComponent;           
            
            component.EndPointA = new Engine.Math.Vector3(_txtEndPointAX.TextToFloat(), _txtEndPointAY.TextToFloat(), _txtEndPointAZ.TextToFloat());
            component.EndPointB = new Engine.Math.Vector3(_txtEndPointBX.TextToFloat(), _txtEndPointBY.TextToFloat(), _txtEndPointBZ.TextToFloat());         
        }

        private void btnResetEndPointA_Click(object sender, EventArgs e)
        {
            _component.EndPointA = Vector3.Zero;
            UpdateUI();
        }

        private void btnResetEndPointB_Click(object sender, EventArgs e)
        {
            _component.EndPointB = Vector3.Zero;
            UpdateUI();
        }

        private void linkCopyPosition_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _component.EndPointA = _component.GameObject.WorldPosition;
            UpdateUI();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _component.EndPointB = _component.GameObject.WorldPosition;
            UpdateUI();
        }      
    }
}
