using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Dynamics;
using Jitter.LinearMath;
using iGL.Engine.Math;
using Jitter.Collision.Shapes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace iGL.Engine
{
    [Serializable]
    public class RigidBodyComponent : GameComponent
    {
        private float _mass { get; set; }
        private float _staticFriction { get; set; }
        private float _kineticFriction { get; set; }
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

        public float StaticFriction
        {
            get
            {
                return _staticFriction;
            }
            set
            {
                _staticFriction = value;
                UpdateRigidBody();
            }
        }

        public float KineticFriction
        {
            get
            {
                return _kineticFriction;
            }
            set
            {
                _kineticFriction = value;
                UpdateRigidBody();
            }
        }

        [XmlIgnoreAttribute]
        public Vector3 AngularVelocity
        {
            get
            {
                if (!IsLoaded) return new Vector3(0);

                return RigidBody.AngularVelocity.ToOpenTK();
            }
            set
            {
                if (!IsLoaded) return;

                RigidBody.AngularVelocity = value.ToJitter();
            }
        }

        [XmlIgnoreAttribute]
        public Vector3 LinearVelocity
        {
            get
            {
                if (!IsLoaded) return new Vector3(0);

                return RigidBody.LinearVelocity.ToOpenTK();
            }
            set
            {
                if (!IsLoaded) return;

                RigidBody.LinearVelocity = value.ToJitter();
            }
        }

        public bool IsActive
        {
            get
            {
                if (!IsLoaded) return false;
                return RigidBody.IsActive;
            }
        }

        internal Matrix4 RigidBodyTransform { get; private set; }

        public ColliderComponent ColliderComponent { get; private set; }

        internal RigidBody RigidBody { get; private set; }

        public RigidBodyComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public RigidBodyComponent() { }

        protected override void Init()
        {
            RigidBodyTransform = Matrix4.Identity;

            _mass = 100.0f;
            _isStatic = false;
            _isGravitySource = false;

            _kineticFriction = 0.3f;
            _staticFriction = 0.6f;
            _restitution = 0.0f;
        }

        internal void Reload()
        {
            if (!IsLoaded) return;

            if (RigidBody != null)
            {
                GameObject.Scene.Physics.RemoveBody(RigidBody);

                ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderComponent) as ColliderComponent;
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
                GameObject.OnMove += (a, b) => Reload();
                GameObject.OnScale += (a, b) => Reload();
                GameObject.OnRotate += (a, b) => Reload();

                return true;
            }
            
            return false;
        }
       
        private bool LoadRigidBody()
        {
            /* game object must have a collider component */
            ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderComponent) as ColliderComponent;

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

            if (ColliderComponent is CompoundColliderComponent)
            {
                /* not currently used */
                var compoundCollider = ColliderComponent as CompoundColliderComponent;
                var compoundShape = compoundCollider.CollisionShape as CompoundShape;

                RigidBody = new RigidBody(compoundShape);
                RigidBody.Orientation = transform.ToJitter();
                RigidBody.Position = transform.Translation().ToJitter() - compoundShape.Shift;

                RigidBody.Mass = _mass;
                RigidBody.IsStatic = _isStatic;
            }
            else
            {
                RigidBody = new RigidBody(ColliderComponent.CollisionShape);
                RigidBody.Mass = _mass;

                RigidBody.Orientation = transform.ToJitter();
                RigidBody.Position = transform.Translation().ToJitter();

                RigidBody.IsStatic = _isStatic;
            }

            RigidBody.Tag = GameObject;
            RigidBody.Material.Restitution = _restitution;
            RigidBody.Material.KineticFriction = _kineticFriction;
            RigidBody.Material.StaticFriction = _staticFriction;
            
            GameObject.Scene.Physics.AddBody(RigidBody);

            UpdateTransform();

            return true;
        }

        public void ApplyForce(Vector3 force)
        {
            RigidBody.AddForce(force.ToJitter());
        }

        private void UpdateRigidBody()
        {
            if (!IsLoaded) return;

            RigidBody.IsStatic = _isStatic;
            RigidBody.Mass = _mass;

        }

        private void UpdateTransform()
        {
            /* scale must be taking into account */

            RigidBodyTransform = Math.Matrix4.Scale(GameObject.Scale) * RigidBody.Orientation.ToOpenTK(RigidBody.Position);
        }

        public override void Tick(float timeElapsed)
        {
            UpdateTransform();
        }
    }
}
