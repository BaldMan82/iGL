using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    public class SlingShot : GameObject
    {
        public List<GameObject> Ammo;
        private Cube _slingShot;
        private bool _inAimMode = false;
        private GameObject _currentBullet;
        private Vector3 _bulletStartPosition;
        private float _slingShotRadius = 1.5f;
        private float _springConstant = 20000f;

        public SlingShot(int numBullets = 20)
        {
            _slingShot = new Cube(0.5f, 3, 0.5f);
            _slingShot.Material.Ambient = new Vector4(0.5f, 0.5f, 0.5f, 1);
            _slingShot.Position = new Vector3(0, _slingShot.Height / 2.0f, 0);

            Ammo = new List<GameObject>();

            for (int i = 0; i < numBullets; i++)
            {
                var bullet = new Sphere(0.25f);
                bullet.AddComponent(new SphereColliderComponent());
                bullet.AddComponent(new RigidBodyComponent(isStatic: true));

                bullet.Position = new Vector3(-i - 1.0f, bullet.Radius, 0);
                Ammo.Add(bullet);

            }
        }

        public override void Load()
        {
            base.Load();

            _slingShot.Position += this.Position;

            Scene.AddGameObject(_slingShot);
            Ammo.ForEach(b =>
            {
                b.Position += this.Position;
                Scene.AddGameObject(b);
            }
                );

            LoadBullet();

            Scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(Scene_OnMouseMove);
        }

        void Scene_OnMouseMove(object sender, Engine.Events.MouseMoveEvent e)
        {
            if (_inAimMode)
            {
                _currentBullet.Position = new Vector3(e.NearPlane.X, e.NearPlane.Y, _currentBullet.Position.Z);

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
            _currentBullet = Ammo.FirstOrDefault();
            if (_currentBullet == null) return;

            _bulletStartPosition = _slingShot.Position + new Vector3(0, _slingShot.Height / 2.0f + 1.0f, 0);
            Ammo.Remove(_currentBullet);

            _currentBullet.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(bullet_OnMouseDown);
            _currentBullet.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(bullet_OnMouseUp);
            _currentBullet.Position = _bulletStartPosition;

            Ammo.ForEach(b =>
                b.Position += new Vector3(1.0f, 0, 0));
        }

        void bullet_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            var bullet = sender as Sphere;

            var rigidBody = bullet.Components.Single(c => c is RigidBodyComponent) as RigidBodyComponent;

            var fireDirection = _bulletStartPosition - bullet.Position;
            rigidBody.SetStatic(false);
            rigidBody.ApplyForce(fireDirection * _springConstant);

            _inAimMode = false;

            Scene.AddTimer(new Timer()
            {
                Action = () =>
                {
                    LoadBullet();
                },
                Interval = TimeSpan.FromSeconds(2),
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
