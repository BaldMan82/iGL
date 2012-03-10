using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public class Timer
    {
        public Action Action { get; set; }
        public TimeSpan Interval { get; set; }
        internal DateTime LastTick { get; set; }
    }
}
