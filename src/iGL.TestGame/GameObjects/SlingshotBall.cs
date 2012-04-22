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
    public class SlingshotBall : GameObject
    {
        private Sphere _aimSphere;
        private RigidSphere _displaySphere;
        private bool _inAimMode;
        private Vector3 _triggerPosition;
        private float _slingShotRadius = 4.0f;
        private float _springConstant = 900000f;
        private Arrow2d _arrow2d;
        private bool _canFire;
        private PanViewFollowCamera3d _followCamera;

        public GameObject _lightObject;

        public SlingshotBall(XElement element) : base(element) { }

        public SlingshotBall() { }       

        public override void Load()
        {
            base.Load();

            Scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(Scene_OnMouseMove);

            _displaySphere = new RigidSphere();
            var rigidComponent = _displaySphere.Components.First(c => c is RigidBodyComponent) as RigidBodyComponent;
            rigidComponent.Mass = 1000;
            rigidComponent.KineticFriction = 1.0f;
            rigidComponent.StaticFriction = 1.0f;
            rigidComponent.Restitution = 0.5f;

           AddChild(_displaySphere);

            _aimSphere = new Sphere();
            _aimSphere.Scale = new Vector3(5);            
            _aimSphere.Material.Ambient = new Vector4(1, 0, 0, 0.5f);
            _aimSphere.Material.TextureName = "ball";
            _aimSphere.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(_aimSphere_OnMouseDown);
            _aimSphere.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(_aimSphere_OnMouseUp);
            _aimSphere.Visible = false;

            AddChild(_aimSphere);

            _arrow2d = new Arrow2d();               

            Scene.AddGameObject(_arrow2d);

            _lightObject = new GameObject();
            
            var lightComponent = new LightComponent();
            
            var pointLight = new PointLight();
            pointLight.Ambient = new Vector4(0);
            pointLight.Diffuse = new Vector4(1, 1, 1, 1);
            lightComponent.Light = pointLight;

            _lightObject.AddComponent(lightComponent);

            Scene.AddGameObject(_lightObject);

            Scene.OnLoaded += new EventHandler<Engine.Events.LoadedEvent>(Scene_OnLoaded);
        }

         void Scene_OnLoaded(object sender, Engine.Events.LoadedEvent e)
        {
            if (Scene.CurrentCamera.GameObject is PanViewFollowCamera3d)
            {
                _followCamera = Scene.CurrentCamera.GameObject as PanViewFollowCamera3d;

                _followCamera.Follow(_displaySphere);
                _followCamera.Enabled = false;
                _followCamera.FollowingEnabled = true;
            }

            Scene.SetCurrentLight(_lightObject);
        }

        void Scene_OnMouseMove(object sender, Engine.Events.MouseMoveEvent e)
        {
            
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);

            _aimSphere.Position = _displaySphere.Position;

            if (_inAimMode)
            {
                var nearPlane = new Vector3(Scene.LastNearPlaneMousePosition.Value);

                /* slingshot dynamics */

                var lookAt = Scene.CurrentCamera.Target - Scene.CurrentCamera.GameObject.Position;
                lookAt.Normalize();

                var p = Scene.CurrentCamera.GameObject.Position + lookAt;
                var planeDistance = _displaySphere.WorldPosition.PlaneDistance(p, lookAt);

                var dirNearPlane = nearPlane - (Scene.CurrentCamera.GameObject.Position + lookAt);

                var newWorldPosition = nearPlane + (lookAt * planeDistance);

                if (Scene.CurrentCamera is PerspectiveCameraComponent)
                {
                    newWorldPosition += (dirNearPlane * Math.Abs(planeDistance));
                }            

                _triggerPosition = newWorldPosition;

                var distance = (_triggerPosition - _displaySphere.WorldPosition).Length;
                if (distance > _slingShotRadius)
                {
                    var norm = (_triggerPosition - _displaySphere.WorldPosition);
                    norm.Normalize();

                    norm = Vector3.Multiply(norm, distance - _slingShotRadius);

                    _triggerPosition -= norm;
                }


                /* adjust arrow image */

                _arrow2d.Position = _displaySphere.WorldPosition;

                var direction = _displaySphere.WorldPosition - _triggerPosition;
                var normDirection = direction;
                normDirection.Normalize();

                _arrow2d.Position += normDirection * (_displaySphere.Scale.Y + _arrow2d.Scale.Y / 2.0f);              

                Vector2 a = new Vector2(direction.X, direction.Y);
                Vector2 b = new Vector2(0, 1);

                var angle = (float)(Math.Atan2((a.X * b.Y) - (b.X * a.Y), (a.X * b.X) + (a.Y * b.Y)) % (2 * Math.PI));
                _arrow2d.Rotation = new Vector3(0, 0, -angle);
                _arrow2d.Scale = new Vector3(1+direction.Length/2.0f, 1+direction.Length, 1);

                float colorFactor = distance / _slingShotRadius;

                _arrow2d.Material.Ambient = new Vector4(colorFactor, 1 - colorFactor, 0, 1);
                Debug.WriteLine(MathHelper.RadiansToDegrees(angle));
            }


            /* damping */
            var body = _displaySphere.Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;
            body.AngularVelocity = body.AngularVelocity * 0.92f;

            if (body.AngularVelocity.LengthSquared < 0.4f && body.LinearVelocity.LengthSquared < 0.4f)
            {
                _displaySphere.Material.Ambient = new Vector4(0, 1, 0, 1);
                _canFire = true;
            }
            else
            {
                _displaySphere.Material.Ambient = new Vector4(1, 0, 0, 1);
                _inAimMode = false;
                _canFire = false;
                _arrow2d.Visible = false;
            }

            /* light */

            _lightObject.Position = _displaySphere.WorldPosition + new Vector3(0, 0, 5);
        }

        void _aimSphere_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            if (Game.InDesignMode || !_inAimMode) return;

            _displaySphere.Material.Ambient = new Vector4(1, 0, 0, 1);
            _inAimMode = false;

            var rigidBody = _displaySphere.Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;

            var fireDirection = _displaySphere.WorldPosition - _triggerPosition;
            rigidBody.IsStatic = false;
            rigidBody.ApplyForce(fireDirection * _springConstant);

            _arrow2d.Visible = false;

            if (_followCamera != null) _followCamera.FollowingEnabled = true;
            
        }

        void _aimSphere_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            if (Game.InDesignMode) return;

            var body = _displaySphere.Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;
            if (!_canFire) return;

            _displaySphere.Material.Ambient = new Vector4(0, 1, 0, 1);
            _inAimMode = true;

            _triggerPosition = _displaySphere.WorldPosition;
            _arrow2d.Visible = true;

            if (_followCamera != null) _followCamera.FollowingEnabled = false;
        }

    }
}
