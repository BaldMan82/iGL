using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine.Events
{
    public class MouseZoomEvent : EventArgs
    {
        public int Amount { get; set; }        
    }
}
