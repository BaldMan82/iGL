using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class MouseButtonUpEvent : EventArgs
    {
        public MouseButton Button { get; set; }
    }
}
