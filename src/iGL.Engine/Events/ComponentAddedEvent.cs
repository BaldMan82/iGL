using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class AnimationSignalEvent : EventArgs
    {
        public enum State
        {
            Playing,
            Paused,
            Stopped
        }

        public State SignalState { get; set; }
    }
}
