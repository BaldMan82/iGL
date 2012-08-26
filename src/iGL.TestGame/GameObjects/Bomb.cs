using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    public class Bomb : RigidFarseerSphere
    {
        public Bomb(XElement element) : base(element) { }

        public Bomb() { }

        public enum State
        {
            Ease,
            Triggered,
            Exploded
        }

        private State _state;
        private DateTime _triggerTime;
        private DateTime _flickerTime;
        private TimeSpan _triggerDuration = TimeSpan.FromSeconds(4);
        private MeshComponent _meshComponent;

        public override void Load()
        {
            base.Load();

            this.OnObjectCollision += ObjectCollision;
            _meshComponent = this.Components.Single(c => c is MeshComponent) as MeshComponent;
        }

        private void ObjectCollision(object sender, Engine.Events.ObjectCollisionEvent e)
        {
            if (this.Scene.PlayerObject == e.Object && _state == State.Ease)
            {
                _triggerTime = DateTime.UtcNow;
                _flickerTime = _triggerTime;
                _state = State.Triggered;
            }
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);

            if (_state != State.Triggered) return;

            var timeLeft = _triggerDuration - (DateTime.UtcNow - _triggerTime);
            if (timeLeft.TotalSeconds < 0)
            {
                Explode();
            }
            else
            {
                var p = timeLeft.TotalSeconds / _triggerDuration.TotalSeconds;
                if (_flickerTime + TimeSpan.FromSeconds(p / 2.0f) < DateTime.UtcNow)
                {
                    _flickerTime = DateTime.UtcNow;
                    if (_meshComponent.Material.Ambient.X == 0)
                    {
                        _meshComponent.Material.Ambient = new Vector4(1, 0, 0, 1);
                    }
                    else
                    {
                        _meshComponent.Material.Ambient = new Vector4(0, 0, 0, 1);
                    }
                }
            }
        }

        private void Explode()
        {
            _state = State.Exploded;

            var ignoreList = new List<GameObject>() { this };

            var ball = Scene.GameObjects.FirstOrDefault(g => g is SlingshotBallFarseer3D);
            if (ball != null) ignoreList.Add(ball.Children.Single(c => c is Sphere));

            var pos = this.WorldPosition;

            

            //foreach (var body in Scene.GameObjects.Select(g => g.Components.FirstOrDefault(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent).Where(c => c != null))
            //{
            //    if (!body.IsStatic && !body.IsSensor && body.IsLoaded)
            //    {                   
            //        var vDistance = body.GameObject.WorldPosition - pos;

            //        Vector3 hit;
            //        var gameObject = this.Scene.RayTest(ref pos, ref vDistance, out hit, ignoreList);
                    
            //        if (vDistance.Length == 0 || gameObject != body.GameObject) continue;

            //        var force = 2000.0f / vDistance.Length;
            //        vDistance.Normalize();

            //        body.ApplyForce(vDistance * force);
            //    }
            //}

            Scene.DisposeGameObject(this);
        }
    }
}
