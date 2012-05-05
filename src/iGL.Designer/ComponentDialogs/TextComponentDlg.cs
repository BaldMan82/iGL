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
    [GameObjectDialog(typeof(TextComponent))]
    public partial class TextComponentDlg : ComponentControl
    {
        public TextComponentDlg()
        {
            InitializeComponent();

            Load += new EventHandler(TextComponentDlg_Load);
        }

        void TextComponentDlg_Load(object sender, EventArgs e)
        {
            var textComponent = Component as TextComponent;
            var fonts = Component.GameObject.Scene.Resources.Where(r => r is iGL.Engine.Resources.Font);
            foreach (var font in fonts)
            {
                var index = ddFont.Items.Add(font);
                if (font.Name == textComponent.FontName) ddFont.SelectedIndex = index;
            }

            txtText.Text = textComponent.Text;
        }

        public override void UpdateComponent()
        {
            var textComponent = Component as TextComponent;

            if (ddFont.SelectedItem != null)
            {
                textComponent.FontName = ((iGL.Engine.Resources.Font)ddFont.SelectedItem).Name;
            }
            else
            {
                textComponent.FontName = null;
            }

            textComponent.Text = txtText.Text;

            textComponent.Reload();
        }
    }
}
