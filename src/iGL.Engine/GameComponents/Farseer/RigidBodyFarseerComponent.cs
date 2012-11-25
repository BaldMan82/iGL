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
using FarseerPhysics.Common.PhysicsLogic;

namespace iGL.Engine
{
    [Serializable]
    public class RigidBodyFarseerComponent : RigidBodyBaseComponent
    {
        private float _mass { get; set; }
        private float _friction { get; set; }
        private float _restitution { get; set; }
        private bool _isStatic { get; set; }
        private bool _isKinematic { get; set; }
        private bool _isGravitySource { get; set; }
        private bool _isSensor { get; set; }

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

        public float GravityRange
        {
            get;
            set;
        }

        public bool IsSensor
        {
            get
            {
                return _isSensor;
            }
            set
            {
                _isSensor = value;
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

        public bool IsKinematic
        {
            get
            {
                return _isKinematic;
            }
            set
            {
                _isKinematic = value;
                if (_isKinematic && _isStatic) _isStatic = false;

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

        public override bool Sleeping
        {
            get
            {
                if (!IsLoaded) return false;
                return !RigidBody.Awake;
            }
            set
            {
                if (IsLoaded)
                {
                    RigidBody.Awake = !value;
                }
            }
        }

        public override bool HasContacts
        {
            get
            {
                if (RigidBody.ContactList != null)
                {
                    var contactEdge = RigidBody.ContactList;
                    while (contactEdge != null)
                    {
                        if (contactEdge.Contact.IsTouching()) return true;
                        contactEdge = contactEdge.Next;
                    }
                }
                return false;
            }
        }
        

        public ColliderFarseerComponent ColliderComponent { get; private set; }

        internal Body RigidBody { get; private set; }

        public RigidBodyFarseerComponent(XElement xmlElement) : base(xmlElement) { }

        public RigidBodyFarseerComponent() { }

        protected override void Init()
        {
            RigidBodyTransform = Matrix4.Identity;

            _mass = 1.0f;
            _isStatic = false;
            _isGravitySource = false;

            _friction = 0.6f;
            _restitution = 0.2f;

            GravityRange = 5.0f;

            AutoReloadBody = true;
            
        }

        internal void Reload()
        {
            if (!IsLoaded) return;

            if (RigidBody != null)
            {
                if (Game.InDesignMode)
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

                SetRigidBodyProperties();
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

        

            var world = GameObject.Scene.Physics.GetWorld() as World;
            if (world == null) throw new InvalidOperationException("Not a farseer physics world.");

            RigidBody = new Body(world, GameObject);                       
            RigidBody.Mass = _mass;
            ColliderComponent.CollisionShape.Density = _mass;            

            if (ColliderComponent.CollisionShape is MultiShape)
            {
                var multiPoly = ColliderComponent.CollisionShape as MultiShape;
                multiPoly.Shapes.ForEach(p => RigidBody.CreateFixture(p));
            }
            else
            {
                RigidBody.CreateFixture(ColliderComponent.CollisionShape);
            }
            
            GameObject.Scene.Physics.AddBody(RigidBody);

            SetRigidBodyProperties();

            UpdateTransform();

            return true;
        }

        private void SetRigidBodyProperties()
        {
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


            var pos = transform.Translation();
            Vector3 eulerRotation;

            transform.EulerAngles(out eulerRotation);
            
            RigidBody.ResetDynamics();

            RigidBody.Position = new Xna.Vector2(pos.X, pos.Y);
            RigidBody.Rotation = eulerRotation.Z;
            RigidBody.BodyType = _isStatic ? BodyType.Static : _isKinematic ? BodyType.Kinematic : BodyType.Dynamic;
            RigidBody.Restitution = _restitution;
            RigidBody.Friction = _friction;
            RigidBody.AngularDamping = 5.0f;
            RigidBody.UserData = GameObject;
            RigidBody.IsSensor = _isSensor;
        }

        public void ApplyForce(Vector3 force)
        {
            RigidBody.ApplyForce(new Xna.Vector2(force.X, force.Y));
        }

        private void UpdateRigidBody()
        {
            if (!IsLoaded) return;

            //RigidBody.IsStatic = _isStatic;
            //RigidBody.Mass = _mass;

            //ColliderComponent.CollisionShape.Density = _mass;            

            //RigidBody.Restitution = _restitution;
            //RigidBody.Friction = _friction;
        }

        internal void UpdateTransform()
        {
            /* scale must be taking into account */
            Transform transform;
            RigidBody.GetTransform(out transform);

            /* include XY rotation as this is a 2d physics body */
            var mRotationX = Matrix4.CreateRotationX(this.GameObject._rotation.X);
            var mRotationY = Matrix4.CreateRotationY(this.GameObject._rotation.Y);

            RigidBodyTransform = Math.Matrix4.Scale(GameObject.Scale) * mRotationX * mRotationY * transform.ToOpenTK();
        }

        public void Explode()
        {
            var exp = new Explosion((World)GameObject.Scene.Physics.GetWorld());
            var worldPos = this.GameObject.WorldPosition;

            exp.Activate(new Xna.Vector2(worldPos.X, worldPos.Y), 10, 0.1f);
        }

        public override void Tick(float timeElapsed)
        {
			if (Game.InDesignMode || (RigidBody.Awake && !IsStatic && !IsSensor))
			{
            	UpdateTransform();
			}
        }

        public override void Dispose()
        {
            base.Dispose();
            
            if (IsLoaded) GameObject.Scene.Physics.RemoveBody(RigidBody);
        }

        public void Awake()
        {
            RigidBody.Awake = true;
        }

        public void ClearForces()
        {
            RigidBody.LinearVelocity = Xna.Vector2.Zero;
            RigidBody.AngularVelocity = 0;
        }
    }
}
