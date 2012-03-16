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

            Controls.HookAllChangeEvents(() => UpdateGameObject());
        }

        public virtual void UpdateGameObject() { }
    }
}
