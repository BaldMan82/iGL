using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine.Events
{
    public class ScaleEvent : EventArgs
    {
        public Vector3 OldScale { get; set; }
        public Vector3 NewScale { get; set; }
    }
}
