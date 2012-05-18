using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using FarseerPhysics.Dynamics;

using Xna = Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using System.Xml.Linq;
using FarseerPhysics.Collision.Shapes;

namespace iGL.Engine
{
    [Serializable]
    public class RigidBodyFarseerComponent : RigidBodyBaseComponent
    {
        private float _mass { get; set; }
        private float _friction { get; set; }
        private float _restitution { get; set; }
        private bool _isStatic { get; set; }
        private bool _isGravitySource { get; set; }

        public float Mass
        {
            get
            {
                return _mass;
            }

            set
            {
                _mass = value;
                UpdateRigidBody();
            }
        }

        public bool IsGravitySource
        {
            get
            {
                return _isGravitySource;
            }
            set
            {
                _isGravitySource = value;
                Reload();
            }
        }

        public bool IsStatic
        {
            get
            {
                return _isStatic;
            }
            set
            {
                _isStatic = value;
                Reload();
            }
        }

        public float Restitution
        {
            get
            {
                return _restitution;
            }
            set
            {
                _restitution = value;
                UpdateRigidBody();
            }
        }

        public float Friction
        {
            get
            {
                return _friction;
            }
            set
            {
                _friction = value;
                UpdateRigidBody();
            }
        }        

        [XmlIgnoreAttribute]
        public Vector3 AngularVelocity
        {
            get
            {
                if (!IsLoaded) return new Vector3(0);

                return new Vector3(0, 0, RigidBody.AngularVelocity);
            }
            set
            {
                if (!IsLoaded) return;

                RigidBody.AngularVelocity = value.Z;
            }
        }

        [XmlIgnoreAttribute]
        public Vector3 LinearVelocity
        {
            get
            {
                if (!IsLoaded) return new Vector3(0);

                return new Vector3(RigidBody.LinearVelocity.X, RigidBody.LinearVelocity.Y, 0);
            }
            set
            {
                if (!IsLoaded) return;

                RigidBody.LinearVelocity = new Xna.Vector2(value.X, value.Y);
            }
        }

        public bool IsActive
        {
            get
            {
                if (!IsLoaded) return false;
                return RigidBody.Awake;
            }
        }

        public ColliderFarseerComponent ColliderComponent { get; private set; }

        internal Body RigidBody { get; private set; }

        public RigidBodyFarseerComponent(XElement xmlElement) : base(xmlElement) { }

        public RigidBodyFarseerComponent() { }

        protected override void Init()
        {
            RigidBodyTransform = Matrix4.Identity;

            _mass = 100.0f;
            _isStatic = false;
            _isGravitySource = false;

            _friction = 0.6f;
            _restitution = 0.2f;

            AutoReloadBody = true;
        }

        internal void Reload()
        {
            if (!IsLoaded) return;

            if (RigidBody != null)
            {
                var world = GameObject.Scene.Physics.GetWorld() as World;
                world.RemoveBody(RigidBody);

                ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderFarseerComponent) as ColliderFarseerComponent;
                if (ColliderComponent != null)
                {
                    ColliderComponent.Reload();
                    if (!LoadRigidBody()) throw new InvalidOperationException();
                }
            }
        }

        public override bool InternalLoad()
        {
            if (LoadRigidBody())
            {
                /* subscribe to change events and reload when fired */
                GameObject.OnMove += (a, b) => { if (AutoReloadBody) Reload(); };
                GameObject.OnScale += (a, b) => { if (AutoReloadBody) Reload(); };
                GameObject.OnRotate += (a, b) => { if (AutoReloadBody) Reload(); };

                return true;
            }
            
            return false;
        }
       
        private bool LoadRigidBody()
        {
            /* game object must have a collider component */
            ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderFarseerComponent) as ColliderFarseerComponent;

            if (ColliderComponent == null)
            {
                return false;
            }

            /* load the collider first */
            if (!ColliderComponent.IsLoaded)
            {
                if (!ColliderComponent.Load()) return false;
            }

            /* create a composite transform without this object's scale */

            Matrix4 transform = Matrix4.Identity;

            var mRotationX = Matrix4.CreateRotationX(GameObject._rotation.X);
            var mRotationY = Matrix4.CreateRotationY(GameObject._rotation.Y);
            var mRotationZ = Matrix4.CreateRotationZ(GameObject._rotation.Z);
            var translation = Matrix4.CreateTranslation(GameObject._position);

            transform = mRotationX * mRotationY * mRotationZ * translation;

            if (GameObject.Parent != null)
            {
                var parentTransform = GameObject.Parent.GetCompositeTransform();
                transform = transform * parentTransform;
            }

            var world = GameObject.Scene.Physics.GetWorld() as World;
            if (world == null) throw new InvalidOperationException("Not a farseer physics world.");

            RigidBody = new Body(world, GameObject);
            RigidBody.Mass = _mass;

            var pos = transform.Translation();
            Vector3 eulerRotation;

            transform.EulerAngles(out eulerRotation);
            
            if (ColliderComponent.CollisionShape is MultiShape)
            {
                var multiPoly = ColliderComponent.CollisionShape as MultiShape;
                multiPoly.Shapes.ForEach(p => RigidBody.CreateFixture(p));
            }
            else
            {
                RigidBody.CreateFixture(ColliderComponent.CollisionShape);
            }

            RigidBody.Position = new Xna.Vector2(pos.X, pos.Y);
            RigidBody.Rotation = eulerRotation.Z;
            RigidBody.IsStatic = _isStatic;
            RigidBody.Restitution = _restitution;
            RigidBody.Friction = _friction;

            GameObject.Scene.Physics.AddBody(RigidBody);

            UpdateTransform();

            return true;
        }

        public void ApplyForce(Vector3 force)
        {
            RigidBody.ApplyForce(new Xna.Vector2(force.X, force.Y));
        }

        private void UpdateRigidBody()
        {
            if (!IsLoaded) return;

            RigidBody.IsStatic = _isStatic;
            RigidBody.Mass = _mass;         
            RigidBody.Restitution = _restitution;
            RigidBody.Friction = _friction;
        }

        private void UpdateTransform()
        {
            /* scale must be taking into account */
            Transform transform;
            RigidBody.GetTransform(out transform);

            RigidBodyTransform = Math.Matrix4.Scale(GameObject.Scale) * transform.ToOpenTK();
        }

        public override void Tick(float timeElapsed)
        {
            UpdateTransform();
        }
    }
}
