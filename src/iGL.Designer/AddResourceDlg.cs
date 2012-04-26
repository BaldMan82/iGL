using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using iGL.Engine.Resources;

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

            if ((string)comboType.SelectedItem == "Texture")
            {
                Resource = new Texture()
                {
                    ResourceName = (string)comboResource.SelectedItem,
                    Name = txtName.Text
                };
            }
            else if ((string)comboType.SelectedItem == "Font")
            {
                Resource = new iGL.Engine.Resources.Font()
                {
                    ResourceName = (string)comboResource.SelectedItem,
                    Name = txtName.Text
                };
            }
            else if ((string)comboType.SelectedItem == "ColladaMesh")
            {
                Resource = new ColladaMesh()
                {
                    ResourceName = (string)comboResource.SelectedItem,
                    Name = txtName.Text
                };
            }
            else
            {
                throw new NotSupportedException(comboType.SelectedValue.ToString());
            }

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

        private void comboResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = comboResource.SelectedItem as string;
            var parts = item.Split('.');

            if (parts.Length < 2) return;

            if (parts.Last() == "text")
            {
                comboType.SelectedItem = "Texture";
            }
            else if (parts.Last() == "fnt")
            {
                comboType.SelectedItem = "Font";
            }
            else if (parts.Last() == "dae")
            {
                comboType.SelectedItem = "ColladaMesh";
            }
            else
            {
                return;
            }

            txtName.Text = parts[parts.Length - 2];
        }
    }
}
