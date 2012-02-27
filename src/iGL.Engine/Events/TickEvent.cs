using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class TickEvent : EventArgs
    {
        public float Elapsed { get; set; }
    }
}
