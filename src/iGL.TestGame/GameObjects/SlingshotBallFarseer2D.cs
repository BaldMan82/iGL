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
    [RequiredChild(typeof(Arrow2d), SlingshotBallFarseer2D.Arrow2dId)]
    [RequiredChild(typeof(Sphere), SlingshotBallFarseer2D.DisplaySphereId)]
    [RequiredChild(typeof(LightObject), SlingshotBallFarseer2D.LightObjectId)]
    [RequiredChild(typeof(Plane), SlingshotBallFarseer2D.LeftEyeId)]
    [RequiredChild(typeof(Plane), SlingshotBallFarseer2D.RightEyeId)]
    [RequiredComponent(typeof(CircleColliderFarseerComponent), SlingshotBallFarseer2D.CircleColliderFarseerComponentId)]
    [RequiredComponent(typeof(RigidBodyFarseerComponent), SlingshotBallFarseer2D.RigidBodyFarseerComponentId)]
    public class SlingshotBallFarseer2D : Plane
    {
        private Sphere _aimSphere;
        private bool _inAimMode;
        private Vector3 _triggerPosition;
        private float _slingShotRadius = 5.0f;
        private float _springConstant = 500f;
        private Arrow2d _arrow2d;
        private bool _canFire;
        private PanViewFollowCamera3d _followCamera;
        private Vector3 _lastAngularVelocity;
        private Vector4 _mouseDownPosition;       
        private DateTime _triggerTime;
        private DateTime _flickerTime;
        private TimeSpan _triggerDuration = TimeSpan.FromSeconds(2);
        private MeshComponent _meshComponent;
        private RigidBodyFarseerComponent _rigidBodyComponent;
        private Plane _rightEye;
        private Plane _leftEye;
        private Vector3 _eyePosition = new Vector3(0.2f, 0, 0.0f);
        private Vector3 _previousPosition;

        public LightObject _lightObject;
        
        public SlingshotBallFarseer2D(XElement element) : base(element) { }

        public SlingshotBallFarseer2D() { }

        private const string Arrow2dId = "92da1056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string LeftEyeId = "123a1056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string RightEyeId = "456a1056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string DisplaySphereId = "86af2307-be79-453b-a8ab-54bad0d51525";
        private const string LightObjectId = "70af2307-be79-453b-a8ab-54bad0d51525";
        private const string CircleColliderFarseerComponentId = "10af2307-be79-653b-a8ab-54bad0d51535";
        private const string RigidBodyFarseerComponentId = "15da1056-2ff7-443f-aed1-0afd3db7b0bf";
        
        protected override void Init()
        {
            base.Init();

            _aimSphere = Children.First(c => c.Id == DisplaySphereId) as Sphere;
            _aimSphere.Name = "aimSphere";
            _aimSphere.Scale = new Vector3(5);
            _aimSphere.Material.Ambient = new Vector4(1, 0, 0, 0.5f);
            _aimSphere.Material.TextureName = "ball";
            _aimSphere.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(_aimSphere_OnMouseDown);
            _aimSphere.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(_aimSphere_OnMouseUp);
            _aimSphere.Visible = false;

            _arrow2d = Children.First(c => c.Id == Arrow2dId) as Arrow2d;

            _lightObject = Children.First(c => c.Id == LightObjectId) as LightObject;
            _lightObject.Visible = false;
            _lightObject.Enabled = false;

            _meshComponent = this.Components.First(c => c is MeshComponent) as MeshComponent;
            _rigidBodyComponent = this.Components.First(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;

            var circleCollider = this.Components.First(c => c is CircleColliderFarseerComponent) as CircleColliderFarseerComponent;
            circleCollider.Radius = 0.5f;

            Material.TextureName = "greenball";

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
        }

        public override void Load()
        {
            _previousPosition = this.Position;
        
            this.Material.ShaderProgram = ShaderProgram.ProgramType.POINTLIGHT;

            Material.TextureName = "greenball";
            DistanceSorting = true;

            base.Load();

            if (!Game.InDesignMode)
            {
                var lightAnim = new PropertyAnimationComponent();

                lightAnim.StartValue = "0,0,0";
                lightAnim.StopValue = "0,0,10";
                lightAnim.DurationSeconds = 5;
                lightAnim.PlayMode = AnimationComponent.Mode.Play;
                lightAnim.Property = "Position";

                _lightObject.AddComponent(lightAnim);

                lightAnim.Play();
            }

            _aimSphere.Enabled = !Game.InDesignMode;

            if (!Game.InDesignMode)
            {
                Scene.OnLoaded += new EventHandler<Engine.Events.LoadedEvent>(Scene_OnLoaded);
                SetAsTarget();
            }
        }

        void Scene_OnLoaded(object sender, Engine.Events.LoadedEvent e)
        {
            SetAsTarget();
        }

        void SetAsTarget()
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
            _previousPosition = this.Position;

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
                SetEyeLookatTarget(_arrow2d.Position);
            }
            else
            {
                var direction = (_previousPosition - this.Position) * 100.0f;

                if (direction.Length != 0)
                {
                    direction.Normalize();
                    SetEyeLookatTarget(this.Position + direction);
                }               
            }


            /* damping */
            var body = Components.Single(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            if (_lastAngularVelocity.LengthSquared > body.AngularVelocity.LengthSquared)
            {
                body.AngularVelocity = body.AngularVelocity * 0.9f;
            }

            if (body.AngularVelocity.LengthSquared < 4.0f && body.LinearVelocity.LengthSquared < 4.0f)
            {
                Material.TextureName = "greenball";         
                _canFire = true;               
            }
            else
            {                
                Material.TextureName = "redball";              

                _inAimMode = false;
                _canFire = false;
                _arrow2d.Visible = false;

                if (_followCamera != null) _followCamera.FollowingEnabled = true;
            }

            _meshComponent.RefreshTexture();

            _lastAngularVelocity = body.AngularVelocity;        

        }

        void SetArrowPosition(float triggerDistance)
        {
            var direction = this.Position - _triggerPosition;

            var normDirection = direction;
            normDirection.Normalize();

            _arrow2d.Position = this.Position;
            _arrow2d.Position += normDirection * (Scale.Y + _arrow2d.Scale.Y / 2.0f);

            Vector2 a = new Vector2(direction.X, direction.Y);
            Vector2 b = new Vector2(0, 1);

            var angle = (float)(Math.Atan2((a.X * b.Y) - (b.X * a.Y), (a.X * b.X) + (a.Y * b.Y)) % (2 * Math.PI));
            _arrow2d.Rotation = new Vector3(0, 0, -angle);
            _arrow2d.Scale = new Vector3(1 + direction.Length, 1 + direction.Length, 1);

           
            float colorFactor = triggerDistance / _slingShotRadius;
            colorFactor = (float)Math.Pow(colorFactor, 10);
            
            _arrow2d.Material.Ambient = new Vector4(colorFactor, 1 - colorFactor, 0, 1);
           
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

        void _aimSphere_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            if (Game.InDesignMode || !_inAimMode) return;
           
            _inAimMode = false;           

            var rigidBody = Components.Single(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;

            var fireDirection = this.Position - _triggerPosition;          
            rigidBody.ApplyForce(fireDirection * _springConstant);

            _arrow2d.Visible = false;

            if (_followCamera != null) _followCamera.FollowingEnabled = true;

        }

        void _aimSphere_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            if (Game.InDesignMode) return;

            if (!_canFire) return;
          
            _inAimMode = true;           

            _triggerTime = DateTime.UtcNow;
            _flickerTime = _triggerTime;

            _mouseDownPosition = Scene.LastNearPlaneMousePosition.Value;
            _triggerPosition = this.Position;

            SetArrowPosition(0f);

            _arrow2d.Visible = true;

            if (_followCamera != null) _followCamera.FollowingEnabled = false;
        }

    }
}
