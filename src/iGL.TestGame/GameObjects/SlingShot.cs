using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.TestGame.GameObjects
{
    public class SlingShot : GameObject
    {
        private List<GameObject> _ammo;
        private Cube _slingShot;
        private bool _inAimMode = false;
        private GameObject _currentBullet;
        private Vector3 _bulletStartPosition;
        private float _slingShotRadius = 2.0f;
        private float _springConstant = 10000f;

        public int NumBullets { get; set; }

        public SlingShot(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public SlingShot()
        {
            Name = "SlingShot";
        }

        protected override void Init()
        {
            _slingShot = new Cube() { Scale = new Vector3(0.5f, 3, 0.5f) };
            _slingShot.Name = "Slingshot Tower";
            _slingShot.Material.Ambient = new Vector4(0.5f, 0.5f, 0.5f, 1);
            _slingShot.Position = new Vector3(0, _slingShot.Height / 2.0f, 0);

            _ammo = new List<GameObject>();

            NumBullets = 2;

            AddChild(_slingShot);

        }

        private void LoadSlingshot()
        {
            for (int i = 0; i < NumBullets; i++)
            {
                var bullet = new Sphere() { Scale = new Vector3(0.25f) };
                //bullet.AddComponent(new SphereColliderComponent());
                //bullet.AddComponent(new RigidBodyComponent());
                bullet.Name = "Bullet" + i.ToString();
                bullet.Position = new Vector3(-i - 1.0f, bullet.Scale.X, 0);
                _ammo.Add(bullet);

            }

            _ammo.ForEach(b =>
            {              
                this.AddChild(b);
            });
                

            LoadBullet();
        }

        public override void Load()
        {
            base.Load();

            LoadSlingshot();

            Scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(Scene_OnMouseMove);
        }

        void Scene_OnMouseMove(object sender, Engine.Events.MouseMoveEvent e)
        {
            if (_inAimMode)
            {
                var lookAt = Scene.CurrentCamera.Target - Scene.CurrentCamera.GameObject.Position;
                lookAt.Normalize();

                var p = Scene.CurrentCamera.GameObject.Position + lookAt;
                var planeDistance = _currentBullet.WorldPosition.PlaneDistance(p, lookAt);

                var dirNearPlane = e.NearPlane - (Scene.CurrentCamera.GameObject.Position + lookAt);

                var newWorldPosition = e.NearPlane + (lookAt * planeDistance);

                if (Scene.CurrentCamera is PerspectiveCameraComponent)
                {
                    newWorldPosition += (dirNearPlane * Math.Abs(planeDistance));
                }

                var transform = _currentBullet.Parent.GetCompositeTransform();
                transform.Invert();

                _currentBullet.Position = Vector3.Transform(newWorldPosition, transform);

                var distance = (_currentBullet.Position - _bulletStartPosition).Length;
                if (distance > _slingShotRadius)
                {
                    var norm = (_currentBullet.Position - _bulletStartPosition);
                    norm.Normalize();

                    norm = Vector3.Multiply(norm, distance - _slingShotRadius);

                    _currentBullet.Position -= norm;
                }


            }
        }

        public void LoadBullet()
        {
            _currentBullet = _ammo.FirstOrDefault();
            if (_currentBullet == null) return;

            _bulletStartPosition = _slingShot.Position + new Vector3(0, _slingShot.Height / 2.0f + 1.0f, 0);
            _currentBullet.Sleep();

            _ammo.Remove(_currentBullet);

            _currentBullet.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(bullet_OnMouseDown);
            _currentBullet.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(bullet_OnMouseUp);
            _currentBullet.Position = _bulletStartPosition;

            _ammo.ForEach(b =>                
                b.Position += new Vector3(1.0f, 0, 0));
        }

        void bullet_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            var bullet = sender as Sphere;

            var rigidBody = bullet.Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;

            var fireDirection = _bulletStartPosition - bullet.Position;
            rigidBody.IsStatic = false;
            rigidBody.ApplyForce(fireDirection * _springConstant);

            _inAimMode = false;

            Scene.AddTimer(new Timer()
            {
                Action = () =>
                {
                    LoadBullet();
                },
                Interval = TimeSpan.FromSeconds(1),
                Mode = Timer.TimerMode.Once
            });
        }

        void bullet_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            var bullet = sender as Sphere;
            bullet.Material.Ambient = new Vector4(1, 0, 0, 0);

            _inAimMode = true;
        }


    }
}
