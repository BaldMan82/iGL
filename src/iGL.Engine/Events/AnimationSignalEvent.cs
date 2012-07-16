using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class AnimationSignalEvent : EventArgs
    {
        public iGL.Engine.AnimationComponent.State SignalState { get; set; }
    }
}
