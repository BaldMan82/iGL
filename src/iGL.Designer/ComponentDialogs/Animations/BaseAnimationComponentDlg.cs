using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;

namespace iGL.Designer.ComponentDialogs.Animations
{
    public partial class BaseAnimationComponentDlg : UserControl
    {
        private AnimationComponent _animationComponent;
        private bool _internalUpdate = false;

        public AnimationComponent AnimationComponent
        {
            get
            {
                return _animationComponent;
            }
            set
            {
                _animationComponent = value;
                UpdateUI();
            }
        }

        public BaseAnimationComponentDlg()
        {
            InitializeComponent();
            Controls.HookAllChangeEvents(() => UpdateAnimComponent());
        }

        private void BaseAnimationComponentDlg_Load(object sender, EventArgs e)
        {
            if (_internalUpdate) return;

            foreach (var mode in Enum.GetNames(typeof(iGL.Engine.AnimationComponent.Mode)))
            {
                var index = ddAnimMode.Items.Add(mode);
                if (_animationComponent != null && _animationComponent.PlayMode.ToString() == mode) ddAnimMode.SelectedIndex = index;
            }

            ddAnimMode.SelectedIndexChanged += (a, b) => UpdateAnimComponent();
           
        }

        private void UpdateAnimComponent()
        {
            if (_animationComponent == null || _internalUpdate) return;

            _animationComponent.PlayMode = (AnimationComponent.Mode)Enum.Parse(typeof(AnimationComponent.Mode), ddAnimMode.SelectedItem.ToString());
            _animationComponent.AutoStart = cbAutoStart.Checked;
            _animationComponent.DurationSeconds = txtDuration.TextToFloat();
            _animationComponent.IntervalSeconds = txtInterval.TextToFloat();
        }

        private void UpdateUI()
        {
            if (_animationComponent == null) return;

            _internalUpdate = true;

            txtDuration.Text = _animationComponent.DurationSeconds.ToInvariantText();
            txtInterval.Text = _animationComponent.IntervalSeconds.ToInvariantText();
            cbAutoStart.Checked = _animationComponent.AutoStart;
            ddAnimMode.SelectedItem = _animationComponent.PlayMode.ToString();

            _internalUpdate = false;
        }

      
    }
}
