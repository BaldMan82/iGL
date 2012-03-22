using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine.Events
{
    public class MoveEvent : EventArgs
    {
        public Vector3 OldPosition { get; set; }
        public Vector3 NewPosition { get; set; }
    }
}
