﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Dynamics;
using Jitter.LinearMath;
using iGL.Engine.Math;
using Jitter.Collision.Shapes;

namespace iGL.Engine
{
    public class RigidBodyComponent : GameComponent
    {
        private float _mass { get; set; }
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
                UpdateRigidBody();
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
                UpdateRigidBody();
            }
        }

        public Matrix4 RigidBodyTransform { get; private set; }

        public ColliderComponent ColliderComponent { get; private set; }

        internal RigidBody RigidBody { get; private set; }

        public RigidBodyComponent()
            : this(10.0f, false, false)
        {
            RigidBodyTransform = Matrix4.Identity;
        }

        public RigidBodyComponent(float mass = 10.0f, bool isStatic = false, bool isGravitySource = false)
        {
            _mass = mass;
            _isStatic = isStatic;
            _isGravitySource = isGravitySource;
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
                GameObject.OnMove += GameObject_OnMove;
                GameObject.OnScale += GameObject_OnScale;
                GameObject.OnRotate += GameObject_OnRotate;

                return true;
            }
            else return false;
        }

        void GameObject_OnRotate(object sender, Events.RotateEvent e)
        {           
            Reload();
        }

        void GameObject_OnScale(object sender, Events.ScaleEvent e)
        {           
            Reload();
        }

        void GameObject_OnMove(object sender, Events.MoveEvent e)
        {
            Reload();
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

            var mRotationX = Matrix4.CreateRotationX(GameObject.Rotation.X);
            var mRotationY = Matrix4.CreateRotationY(GameObject.Rotation.Y);
            var mRotationZ = Matrix4.CreateRotationZ(GameObject.Rotation.Z);
            var translation = Matrix4.CreateTranslation(GameObject.Position);

            transform = mRotationX * mRotationY * mRotationZ * translation;
            
            if (GameObject.Parent != null)
            {
                var parentTransform = GameObject.Parent.GetCompositeTransform();
                transform = transform * parentTransform;
            }       

            if (ColliderComponent is CompoundColliderComponent)
            {
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

            //if (!_isStatic)
            //{
            //    GameObject.Scene.Physics.World.AddConstraint(new iGL.Engine.Physics.Constraint2D(RigidBody));
            //}

            RigidBody.Tag = GameObject;

            GameObject.Scene.Physics.AddBody(RigidBody);

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

        public override void Tick(float timeElapsed)
        {
            /* scale must be taking into account */

            RigidBodyTransform = Math.Matrix4.Scale(GameObject.Scale) * RigidBody.Orientation.ToOpenTK(RigidBody.Position);            
        }
    }
}
