using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using iGL.Engine.Events;

namespace iGL.Engine
{
    public abstract class AnimationComponent : GameComponent
    {
        public enum State
        {
            Stopped,
            Playing,
            Paused            
        }

        private AnimationSignalEvent _animationSignalEvent = new AnimationSignalEvent();
        public State AnimationState { get; private set; }

        public bool IsPlaying() { return AnimationState == State.Playing; }
        public bool IsPaused() { return AnimationState == State.Paused; }
        public bool IsStopped() { return AnimationState == State.Stopped; }

        public virtual void Play()
        {
            AnimationState = State.Playing;
            _animationSignalEvent.SignalState = AnimationState;
            GameObject.OnAnimationSignalEvent(this, _animationSignalEvent); 
        }
        public virtual void Stop()
        {
            AnimationState = State.Stopped;
            _animationSignalEvent.SignalState = AnimationState;
            GameObject.OnAnimationSignalEvent(this, _animationSignalEvent); 
        }
        public virtual void Pause()
        {
            AnimationState = State.Paused;
            _animationSignalEvent.SignalState = AnimationState;
            GameObject.OnAnimationSignalEvent(this, _animationSignalEvent); 
        }

        public AnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public AnimationComponent() : base() { }
        
    }
}

