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
    [GameObjectDialog(typeof(TextAnimatorComponent))]
    public partial class TextAnimatorComponentDlg : ComponentControl
    {
        public TextAnimatorComponentDlg()
        {
            InitializeComponent();
            Load += new EventHandler(TextAnimatorComponentDlg_Load);
        }

        void TextAnimatorComponentDlg_Load(object sender, EventArgs e)
        {
            var component = Component as TextAnimatorComponent;

            txtCharacterDelay.Text = component.CharacterInterval.ToInvariantText();
            txtLineDelay.Text = component.LineDelay.ToInvariantText();
            txtText.Lines = component.Text.Split('\n');
        }
       
        public override void UpdateComponent()
        {
            var component = Component as TextAnimatorComponent;
            component.CharacterInterval = txtCharacterDelay.TextToFloat();
            component.LineDelay = txtLineDelay.TextToFloat();
            component.AutoStart = cbAutoStart.Checked;

            component.Text = txtText.Text;            
        }      
    }
}
