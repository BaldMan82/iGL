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
        }

        public ColliderComponent ColliderComponent { get; private set; }

        internal RigidBody RigidBody { get; private set; }        

        public RigidBodyComponent(float mass = 10.0f, bool isStatic = false, bool isGravitySource = false)
        {
            _mass = mass;
            _isStatic = isStatic;
            _isGravitySource = isGravitySource;
        }

        public override void InternalLoad()
        {
            /* game object must have a collider component */
            ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderComponent) as ColliderComponent;

            if (ColliderComponent == null) throw new NotSupportedException("Rigid body must have a collider component");

            /* load the collider first */
            if (!ColliderComponent.IsLoaded) ColliderComponent.Load();

            var mRotationX = JMatrix.CreateRotationX(GameObject.Rotation.X);
            var mRotationY = JMatrix.CreateRotationY(GameObject.Rotation.Y);
            var mRotationZ = JMatrix.CreateRotationZ(GameObject.Rotation.Z);

            var transform = mRotationX * mRotationY * mRotationZ * JMatrix.Identity;            

            if (ColliderComponent is CompoundColliderComponent)
            {
                var compoundCollider = ColliderComponent as CompoundColliderComponent;
                var compoundShape = compoundCollider.CollisionShape as CompoundShape;
             
                RigidBody = new RigidBody(compoundShape);
                RigidBody.Orientation = transform;
                RigidBody.Position = GameObject.Position.ToJitter() - compoundShape.Shift;                

                RigidBody.Mass = _mass;                
                               
                RigidBody.IsStatic = _isStatic;
            }
            else
            {
                RigidBody = new RigidBody(ColliderComponent.CollisionShape);
                RigidBody.Mass = _mass;           

                RigidBody.Orientation = transform;
                RigidBody.Position = new JVector(GameObject._position.X, GameObject._position.Y, GameObject._position.Z);

               
                if (_isStatic)
                {
                    RigidBody.IsStatic = true;
                    RigidBody.Material.StaticFriction = 1.0f;
                    RigidBody.Material.KineticFriction = 1.0f;

                }
            }

            if (!_isStatic)
            {
                GameObject.Scene.Physics.World.AddConstraint(new iGL.Engine.Physics.Constraint2D(RigidBody));
            }

            RigidBody.Tag = GameObject;

            GameObject.Scene.Physics.World.AddBody(RigidBody);
            
        }

        public void ApplyForce(Vector3 force)
        {
            RigidBody.AddForce(force.ToJitter());
        }

        public void SetStatic(bool isStatic)
        {
            if (!IsLoaded) throw new InvalidOperationException("Object not loaded");
            RigidBody.IsStatic = isStatic;

            if (!isStatic)
            {
                GameObject.Scene.Physics.World.AddConstraint(new iGL.Engine.Physics.Constraint2D(RigidBody));
            }
            else {
                GameObject.Scene.Physics.World.RemoveConstraint(RigidBody.Constraints.FirstOrDefault());
            }
        }

        private void UpdateRigidBody()
        {
            if (!IsLoaded) return;                     
        }

        public override void Tick(float timeElapsed)
        {          
            /* scale must be taking into account */

            GameObject.Transform = Math.Matrix4.Scale(GameObject.Scale) * RigidBody.Orientation.ToOpenTK(RigidBody.Position); ;
            GameObject._position = new iGL.Engine.Math.Vector3(RigidBody.Position.X, RigidBody.Position.Y, RigidBody.Position.Z);
        }
    }
}
