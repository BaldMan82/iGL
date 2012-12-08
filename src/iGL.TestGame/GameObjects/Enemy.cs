using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;
using System.Diagnostics;

namespace iGL.TestGame.GameObjects
{
    [RequiredComponent(typeof(CircleColliderFarseerComponent), Enemy.CircleColliderFarseerComponentId)]
    [RequiredComponent(typeof(RigidBodyFarseerComponent), Enemy.RigidBodyFarseerComponentId)]
    [RequiredChild(typeof(Plane), Enemy.LeftEyeId)]
    [RequiredChild(typeof(Plane), Enemy.RightEyeId)]
    public class Enemy : Plane
    {
        private RigidBodyFarseerComponent _rigidBody;
        private Plane _rightEye;
        private Plane _leftEye;
        private Vector3 _eyePosition = new Vector3(0.2f, 0, 0.0f);
        private bool _awakening = false;

        public Enemy(XElement element) : base(element) { }

        public Enemy() { }

        private const string LeftEyeId = "823a1056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string RightEyeId = "956a1056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string CircleColliderFarseerComponentId = "11af2307-be79-653b-a8ab-54bad0d51535";
        private const string RigidBodyFarseerComponentId = "55da1056-2ff7-443f-aed1-0afd3db7b0bf";

        protected override void Init()
        {
            base.Init();

            var circleCollider = this.Components.First(c => c is CircleColliderFarseerComponent) as CircleColliderFarseerComponent;
            circleCollider.Radius = 0.5f;

            _rigidBody = this.Components.First(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;

            Material.TextureName = "enemy";

            _leftEye = this.Children.First(c => c.Id == LeftEyeId) as Plane;
            _rightEye = this.Children.First(c => c.Id == RightEyeId) as Plane;

            _leftEye.Material.TextureName = "eye";
            _rightEye.Material.TextureName = "eye";

            _leftEye.DistanceSorting = true;
            _rightEye.DistanceSorting = true;

            _leftEye.Position = -_eyePosition;
            _rightEye.Position = _eyePosition;

            _rightEye.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            _leftEye.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            this.OnObjectCollision +=  Enemy_OnObjectCollision;

        }

        public override void Load()
        {
            SetEyeLookatTarget(this.WorldPosition);
            base.Load();
        }

        void Enemy_OnObjectCollision(object sender, Engine.Events.ObjectCollisionEvent e)        
        {
            if (e.Object != Scene.PlayerObject) return;

            var hitVector = e.Object.WorldPosition - this.WorldPosition;

            Vector2 a = new Vector2(hitVector.X, hitVector.Y);
            Vector2 b = new Vector2(0, 1);

            var angle = (float)(Math.Atan2((a.X * b.Y) - (b.X * a.Y), (a.X * b.X) + (a.Y * b.Y)) % (2 * Math.PI)) * 57.2957795f;
            if (angle > -70 && angle < 70)
            {
                /* should die */
                var flare = new StarFlare();

                flare.Position = this.Position + new Vector3(0, 0, 0.5f);           

                Scene.AddGameObject(flare);

                flare.PlayAnimation();

                Scene.AddTimer(new Timer() { Action = () => Scene.DisposeGameObject(flare), Interval = TimeSpan.FromSeconds(0.2), Mode = Timer.TimerMode.Once });

                Scene.DisposeGameObject(this);

                var rigidBody = Scene.PlayerObject.Components.First(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
                hitVector.Normalize();
                
                rigidBody.ClearForces();
                rigidBody.ApplyForce(hitVector*500);
            }
        }

        public override void Tick(float timeElapsed)
        {
            _rightEye.Scale = new Vector3(0.12f);
            _leftEye.Scale = new Vector3(0.12f);

            if (Game.InDesignMode) return;

            base.Tick(timeElapsed);

            var player = Scene.PlayerObject;
            if (player != null)
            {
                var playerDistance = (player.WorldPosition - this.WorldPosition).Length;

                if (_rigidBody.Sleeping && playerDistance < 8 && !_awakening)
                {                    
                    /* should awaken */
                    _awakening = true;

                    TextObject obj = new TextObject();
                    Scene.AddGameObject(obj);
                    obj.Position = this.WorldPosition + new Vector3(0.25f, 2, 0);
                    obj.SetText("!");
                    obj.Material.Ambient = new Vector4(1);
                    obj.Material.Diffuse = new Vector4(1);

                    obj.Scale = new Vector3(5);
                    
                    SetEyeLookatTarget(player.WorldPosition);                

                    Scene.AddTimer(new Timer() { Action = () => { Scene.DisposeGameObject(obj); _rigidBody.Sleeping = false; }, Interval = TimeSpan.FromSeconds(1.0), Mode = Timer.TimerMode.Once });
                    
                }
                else if (playerDistance >= 10 && !_rigidBody.Sleeping)
                {
                    SetEyeLookatTarget(new Vector3(0,-5,0));

                    _rigidBody.Sleeping = true;
                    _awakening = false;

                    /* gonna sleep now */

                    TextObject obj = new TextObject();
                    Scene.AddGameObject(obj);
                    obj.Position = this.WorldPosition + new Vector3(0.25f, 2, 0);
                    obj.Material.Ambient = new Vector4(1);
                    obj.Material.Diffuse = new Vector4(1);
                    obj.Scale = new Vector3(5);
                    obj.SetText("?");

                    Scene.AddTimer(new Timer() { Action = () => { Scene.DisposeGameObject(obj); }, Interval = TimeSpan.FromSeconds(1.0), Mode = Timer.TimerMode.Once });
                }

                if (!_rigidBody.Sleeping)
                {
                    _awakening = false;

                    var pos = player.WorldPosition;
                    var direction = pos - WorldPosition;

                    if (direction.X > 0)
                    {
                        _rigidBody.ApplyForce(new Engine.Math.Vector3(5, 0, 0));
                    }
                    else
                    {
                        _rigidBody.ApplyForce(new Engine.Math.Vector3(-5, 0, 0));
                    }

                    SetEyeLookatTarget(player.WorldPosition);

                   
                }
            }
        }

        void SetEyeLookatTarget(Vector3 worldTarget)
        {
            var trans = this.Transform;
            var zCorrect = new Vector3(0, 0, 0.1f);
            trans.Invert();

            var arrowPos = Vector3.Transform(worldTarget, trans);

            var eyeDirection = _eyePosition - arrowPos;
            eyeDirection.Normalize();
            eyeDirection = eyeDirection * 0.1f;

            _rightEye.Position = -_eyePosition - eyeDirection + zCorrect;

            eyeDirection = -_eyePosition - arrowPos;
            eyeDirection.Normalize();
            eyeDirection = eyeDirection * 0.1f;

            _leftEye.Position = _eyePosition - eyeDirection + zCorrect;

        }

        public override void Render(bool overrideParentTransform = false)
        {
            Scene.Shader.SetBlackBorder(true);

            base.Render(overrideParentTransform);

            Scene.Shader.SetBlackBorder(false);
        }
      
    }
}
