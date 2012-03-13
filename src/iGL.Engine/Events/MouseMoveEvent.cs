using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine.Events
{
    public class MouseMoveEvent : EventArgs
    {
        public Vector3 NearPlane { get; set; }
        public Vector3 Direction { get; set; }        
    }
}
