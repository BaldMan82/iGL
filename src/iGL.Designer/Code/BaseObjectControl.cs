using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;

namespace iGL.Designer
{
    public class BaseObjectControl : UserControl
    {
        public GameObject GameObject { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            /* hook up all textboxes for change events */
            foreach (var control in Controls)
            {
                if (control is TextBox)
                {
                    var t = control as TextBox;
                    t.TextChanged += (a, b) => UpdateGameObject();

                }
            }
        }

        public virtual void UpdateGameObject() { }
    }
}
