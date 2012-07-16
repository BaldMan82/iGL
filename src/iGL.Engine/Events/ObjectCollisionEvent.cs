using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class ObjectCollisionEvent : EventArgs
    {
        public GameObject Object { get; set; }      
    }
}
