using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class CollisionEvent : EventArgs
    {
        public GameObject ObjectA { get; set; }
        public GameObject ObjectB { get; set; }
    }
}
