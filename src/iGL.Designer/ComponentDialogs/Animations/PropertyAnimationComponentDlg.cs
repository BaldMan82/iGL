using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using System.Xml.Serialization;

namespace iGL.Designer.ComponentDialogs
{
    [GameObjectDialog(typeof(PropertyAnimationComponent))]
    public partial class PropertyAnimationComponentDlg : ComponentControl
    {
        public PropertyAnimationComponentDlg()
        {
            InitializeComponent();
            this.Load += new EventHandler(PropertyAnimationComponentDlg_Load);

        }

        void PropertyAnimationComponentDlg_Load(object sender, EventArgs e)
        {
            var component = Component as PropertyAnimationComponent;
            txtDuration.Text = component.DurationSeconds.ToInvariantText();

            var props = component.GameObject.GetType().GetProperties().Where(p => p.GetSetMethod() != null && !p.GetCustomAttributes(true).Any(attr => attr is XmlIgnoreAttribute));

            foreach (var prop in props)
            {
                var index = ddProperties.Items.Add(prop.Name);
                if (prop.Name == component.Property) ddProperties.SelectedIndex = index;
            }

            foreach (var mode in Enum.GetNames(typeof(iGL.Engine.AnimationComponent.Mode)))
            {
                var index = ddAnimMode.Items.Add(mode);
                if (component.PlayMode.ToString() == mode) ddAnimMode.SelectedIndex = index;
            }

            txtStartValue.Text = component.StartValue;
            txtStopValue.Text = component.StopValue;

            ddProperties.SelectedIndexChanged += new EventHandler(ddProperties_SelectedIndexChanged);
            ddAnimMode.SelectedIndexChanged += (a, b) => UpdateComponent();
        }

        void ddProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComponent();
        }

      
        public override void UpdateComponent()
        {
            var component = Component as PropertyAnimationComponent;
            component.DurationSeconds = txtDuration.TextToFloat();

            component.Property = (string)ddProperties.SelectedItem;
            component.PlayMode = (AnimationComponent.Mode)Enum.Parse(typeof(AnimationComponent.Mode), ddAnimMode.SelectedItem.ToString());
            component.StartValue = txtStartValue.Text;
            component.StopValue = txtStopValue.Text;

        }

     

    }
}
