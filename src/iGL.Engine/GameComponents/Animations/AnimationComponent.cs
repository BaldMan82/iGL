using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace iGL.Engine
{
    public abstract class AnimationComponent : GameComponent
    {
        public virtual void Play()
        {
            GameObject.OnAnimationSignalEvent(this, new Events.AnimationSignalEvent() { SignalState = Events.AnimationSignalEvent.State.Playing }); 
        }
        public virtual void Stop()
        {
            GameObject.OnAnimationSignalEvent(this, new Events.AnimationSignalEvent() { SignalState = Events.AnimationSignalEvent.State.Stopped }); 
        }
        public virtual void Pause()
        {
            GameObject.OnAnimationSignalEvent(this, new Events.AnimationSignalEvent() { SignalState = Events.AnimationSignalEvent.State.Paused }); 
        }

        public AnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public AnimationComponent() : base() { }
        
    }
}

