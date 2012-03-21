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
    public partial class ComponentSelectDlg : Form
    {
        public Type SelectedComponentType { get; private set; }

        public ComponentSelectDlg()
        {
            InitializeComponent();
        }

        private void ComponentSelectDlg_Load(object sender, EventArgs e)
        {
            listComponents.Items.Clear();

            foreach (var component in EngineAssets.Instance.Components)
            {
                listComponents.Items.Add(component.Name);
            }
        }

        private void ComponentSelectDlg_Shown(object sender, EventArgs e)
        {                       
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var component = EngineAssets.Instance.Components.FirstOrDefault(c => c.Name == listComponents.SelectedItem.ToString());
            SelectedComponentType = component;

            this.Close();
        }

        private void listComponents_DoubleClick(object sender, EventArgs e)
        {
            btnOk_Click(sender, e);
        }
      
    }
}
