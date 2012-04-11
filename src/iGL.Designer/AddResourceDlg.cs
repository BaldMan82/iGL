using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;

namespace iGL.Designer
{
    public partial class AddResourceDlg : Form
    {
        public Resource Resource { get; private set; }

        public AddResourceDlg()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty((string)comboResource.SelectedItem))
            {
                MessageBox.Show("Invalid resource");
                return;
            }

            Resource = new Texture()
            {
                ResourceName = (string)comboResource.SelectedItem,
                Name = txtName.Text
            };

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void AddResourceDlg_Load(object sender, EventArgs e)
        {
            var resources = AppDomain.CurrentDomain.GetAssemblies().Where(asm => asm.GetTypes().Any(t => t.BaseType == typeof(Scene))).SelectMany(asm => asm.GetManifestResourceNames());

            foreach (var resource in resources)
            {
                comboResource.Items.Add(resource);
            }
        }
    }
}
