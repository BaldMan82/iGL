using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;
using System.Xml.Linq;
using System.Diagnostics;

namespace iGL.TestGame.GameObjects
{
    [RequiredChild(typeof(Arrow2d), SlingshotBall3D.Arrow2dId)]
    [RequiredChild(typeof(Sphere), SlingshotBall3D.DisplaySphereId)]
    [RequiredChild(typeof(LightObject), SlingshotBall3D.LightObjectId)]
    public class SlingshotBall3D : RigidSphere
    {
        private Sphere _aimSphere;     
        private bool _inAimMode;
        private Vector3 _triggerPosition;
        private float _slingShotRadius = 4.0f;
        private float _springConstant = 100000f;
        private Arrow2d _arrow2d;
        private bool _canFire;
        private PanViewFollowCamera3d _followCamera;
        private Vector3 _lastAngularVelocity;

        public LightObject _lightObject;

        public SlingshotBall3D(XElement element) : base(element) { }

        public SlingshotBall3D() { }       

        private const string Arrow2dId = "e2da1056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string DisplaySphereId = "46af2307-be79-453b-a8ab-54bad0d51525";
        private const string LightObjectId = "60af2307-be79-453b-a8ab-54bad0d51525";

        protected override void Init()
        {
            base.Init();

            _aimSphere = Children.First(c => c.Id == DisplaySphereId) as Sphere;
            _aimSphere.Scale = new Vector3(5);
            _aimSphere.Material.Ambient = new Vector4(1, 0, 0, 0.5f);
            _aimSphere.Material.TextureName = "ball";
            _aimSphere.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(_aimSphere_OnMouseDown);
            _aimSphere.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(_aimSphere_OnMouseUp);
            _aimSphere.Visible = false;

            _arrow2d = Children.First(c => c.Id == Arrow2dId) as Arrow2d;

            _lightObject = Children.First(c => c.Id == LightObjectId) as LightObject;           

               
        }

        public override void Load()
        {
            base.Load();

            _aimSphere.Enabled = !Game.InDesignMode;        

            if (!Game.InDesignMode) Scene.OnLoaded += new EventHandler<Engine.Events.LoadedEvent>(Scene_OnLoaded);
        }

         void Scene_OnLoaded(object sender, Engine.Events.LoadedEvent e)
        {
            if (Scene.CurrentCamera.GameObject is PanViewFollowCamera3d)
            {
                _followCamera = Scene.CurrentCamera.GameObject as PanViewFollowCamera3d;

                _followCamera.Follow(this);             
                _followCamera.FollowingEnabled = true;
            }

            Scene.SetCurrentLight(_lightObject);
        }      

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);          

            if (_inAimMode)
            {
                var nearPlane = new Vector3(Scene.LastNearPlaneMousePosition.Value);

                /* slingshot dynamics */

                var lookAt = Scene.CurrentCamera.Target - Scene.CurrentCamera.GameObject.Position;
                lookAt.Normalize();

                var p = Scene.CurrentCamera.GameObject.Position + lookAt;
                var planeDistance = this.Position.PlaneDistance(p, lookAt);

                var dirNearPlane = nearPlane - (Scene.CurrentCamera.GameObject.Position + lookAt);

                var newWorldPosition = nearPlane + (lookAt * planeDistance);

                if (Scene.CurrentCamera is PerspectiveCameraComponent)
                {
                    newWorldPosition += (dirNearPlane * Math.Abs(planeDistance));
                }            

                _triggerPosition = newWorldPosition;

                var distance = (_triggerPosition - this.Position).Length;
                if (distance > _slingShotRadius)
                {
                    var norm = (_triggerPosition - this.Position);
                    norm.Normalize();

                    norm = Vector3.Multiply(norm, distance - _slingShotRadius);

                    _triggerPosition -= norm;

                    distance = _slingShotRadius;
                }

				SetArrowPosition(distance);
               
            }

            /* damping */
            var body = Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;
            if (_lastAngularVelocity.LengthSquared >= body.AngularVelocity.LengthSquared)
                body.AngularVelocity = body.AngularVelocity * 0.8f;

            if (body.AngularVelocity.LengthSquared < 4.0f && body.LinearVelocity.LengthSquared < 4.0f)
            {
                Material.Ambient = new Vector4(0, 1, 0, 1);
                _canFire = true;
            }
            else
            {
                Material.Ambient = new Vector4(1, 0, 0, 1);
                _inAimMode = false;
                _canFire = false;
                _arrow2d.Visible = false;
				
				if (_followCamera != null) _followCamera.FollowingEnabled = true;
            }

            _lastAngularVelocity = body.AngularVelocity;
 
        }

		void SetArrowPosition(float triggerDistance)
		{		
            var direction = this.Position - _triggerPosition;
            if (direction.LengthSquared == 0) return;

            var normDirection = direction;
            normDirection.Normalize();

            _arrow2d.Position = this.Position;
            _arrow2d.Position += normDirection * (Scale.Y + _arrow2d.Scale.Y / 2.0f);              

            Vector2 a = new Vector2(direction.X, direction.Y);
            Vector2 b = new Vector2(0, 1);

            var angle = (float)(Math.Atan2((a.X * b.Y) - (b.X * a.Y), (a.X * b.X) + (a.Y * b.Y)) % (2 * Math.PI));
            _arrow2d.Rotation = new Vector3(0, 0, -angle);
            _arrow2d.Scale = new Vector3(1+direction.Length, 1+direction.Length, 1);

            float colorFactor = triggerDistance / _slingShotRadius;

            _arrow2d.Material.Ambient = new Vector4(colorFactor, 1 - colorFactor, 0, 1);

		}
		
        void _aimSphere_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            if (Game.InDesignMode || !_inAimMode) return;

            Material.Ambient = new Vector4(1, 0, 0, 1);
            _inAimMode = false;

            var rigidBody = Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;

            var fireDirection = this.Position - _triggerPosition;
            rigidBody.IsStatic = false;
            rigidBody.ApplyForce(fireDirection * _springConstant);

            _arrow2d.Visible = false;

            if (_followCamera != null) _followCamera.FollowingEnabled = true;
            
        }

        void _aimSphere_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            if (Game.InDesignMode) return;

            if (!_canFire) return;

            Material.Ambient = new Vector4(0, 1, 0, 1);
            _inAimMode = true;

            _triggerPosition = this.Position;
			
			SetArrowPosition(0f);
			
            _arrow2d.Visible = true;

            if (_followCamera != null) _followCamera.FollowingEnabled = false;
        }

    }
}
