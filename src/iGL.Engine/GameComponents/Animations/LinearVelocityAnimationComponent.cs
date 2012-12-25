using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class LinearVelocitAnimationComponent : AnimationComponent
    {      
        public Vector3 EndPointA { get; set; }
        public Vector3 EndPointB { get; set; }

        private RigidBodyFarseerComponent _rigidBody;
        private Vector3? _velocity;

        public LinearVelocitAnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public LinearVelocitAnimationComponent() : base() { }

        public override bool InternalLoad()
        {
            _rigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;

            if (_rigidBody != null && _rigidBody.IsKinematic)
            {
                return base.InternalLoad();
            }
          
            return false;
        }

        public override void Play()
        {
            var direction = EndPointB - EndPointA;

            if (DurationSeconds > 0)
            {
                _velocity = direction / DurationSeconds;
            }

            _rigidBody.RigidBody.Position = new Microsoft.Xna.Framework.Vector2(EndPointA.X, EndPointA.Y);
            _rigidBody.UpdateTransform();

            base.Play();
                        
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Tick(float timeElapsed)
        {
            if (GameObject.Scene.IsDisposing) return;

            base.Tick(timeElapsed);

            if (Game.InDesignMode) return;

            if (AnimationState == State.Playing)
            {                
                var direction = EndPointB - EndPointA;
                var objDirection = EndPointB - this.GameObject.Position;

                if (Vector3.Dot(direction, objDirection) < 0)
                {
                    _rigidBody.LinearVelocity = Vector3.Zero;
                    if (PlayMode == Mode.RepeatInverted)
                    {
                        var endPointA = EndPointA;
                        EndPointA = EndPointB;
                        EndPointB = endPointA;

                        Rewind();
                    }
                    else if (PlayMode == Mode.Repeat) {
                        Rewind();
                    }
                    else
                    {
                        Stop();
                    }
                }
                else
                {                  
                    _rigidBody.LinearVelocity = _velocity.Value;
                }
            }
        }
    }
}
