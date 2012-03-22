using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine.Events
{
    public class RotateEvent : EventArgs
    {
        public Vector3 OldRotation { get; set; }
        public Vector3 NewRotation { get; set; }
    }
}
