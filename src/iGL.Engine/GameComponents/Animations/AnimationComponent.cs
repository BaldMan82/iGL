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
            Paused,
            Rewinding
        }

        public enum Mode
        {
            Play,
            Repeat,
            RepeatInverted
        }

        private AnimationSignalEvent _animationSignalEvent = new AnimationSignalEvent();
        private DateTime _rewindStartUtc;

        public State AnimationState { get; private set; }
        public Mode PlayMode { get; set; }
        public bool AutoStart { get; set; }
        public float DurationSeconds { get; set; }
        public float IntervalSeconds { get; set; }

        public bool IsPlaying() { return AnimationState == State.Playing; }
        public bool IsPaused() { return AnimationState == State.Paused; }
        public bool IsStopped() { return AnimationState == State.Stopped; }
        public bool IsRewinding() { return AnimationState == State.Rewinding; }

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

        public virtual void Rewind()
        {
            _rewindStartUtc = DateTime.UtcNow;

            AnimationState = State.Rewinding;
            _animationSignalEvent.SignalState = AnimationState;
            GameObject.OnAnimationSignalEvent(this, _animationSignalEvent); 
        }

        public AnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public AnimationComponent() : base() { }


        public override bool InternalLoad()
        {
            if (AutoStart && !Game.InDesignMode) Play();

            return true;
        }

        public override void Tick(float timeElapsed)
        {
            if (AnimationState == State.Rewinding && (DateTime.UtcNow - _rewindStartUtc).TotalSeconds > IntervalSeconds)
            {
                Play();
            }
        }
       
    }
}

