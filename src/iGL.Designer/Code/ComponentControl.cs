﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using System.ComponentModel;
using System.Xml.Serialization;

namespace iGL.Designer
{
    public class ComponentControl : UserControl
    {
        [XmlIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GameComponent Component { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Controls.HookAllChangeEvents(() => UpdateComponent());
        }

        public virtual void UpdateComponent() { }
    }
}
