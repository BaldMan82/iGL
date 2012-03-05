using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Dynamics;
using Jitter.LinearMath;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class RigidBodyComponent : GameComponent
    {
        private float _mass { get; set; }
        private bool _isStatic { get; set; }

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

        public ColliderComponent ColliderComponent { get; private set; }

        internal RigidBody RigidBody { get; private set; }

        public RigidBodyComponent(GameObject gameObject, float mass = 10.0f, bool isStatic = false) : base(gameObject)
        {
            _mass = mass;
            _isStatic = isStatic;
        }

        public override void InternalLoad()
        {
            /* game object must have a collider component */
            ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderComponent) as ColliderComponent;

            if (ColliderComponent == null) throw new NotSupportedException("Rigid body must have a collider component");

            /* load the collider first */
            if (!ColliderComponent.IsLoaded) ColliderComponent.Load();                  

            //if (ColliderComponent is CompoundColliderComponent)
            //{
            //    //var compoundShape = ((CompoundColliderComponent)ColliderComponent).CollisionShape as CompoundShape;

            //    //var masses = new float[compoundShape.GetNumChildShapes()];
            //    //for (int i = 0; i < compoundShape.GetNumChildShapes(); i++)
            //    //{
            //    //    masses[i] = _mass / compoundShape.GetNumChildShapes();
            //    //}

            //    //var principal = Matrix.Identity;
            //    //Vector3 compoundInertia;
            //    //compoundShape.CalculatePrincipalAxisTransform(masses.ToList(), ref principal, out compoundInertia);

            //    //var newBoxCompound = new CompoundShape();
            //    //for (int i = 0; i < compoundShape.GetNumChildShapes(); i++)
            //    //{
            //    //    var newChildTransform = principal.Inverse() * compoundShape.GetChildTransform(i);          
            //    //    newBoxCompound.AddChildShape(ref newChildTransform, compoundShape.GetChildShape(i));
            //    //}

            //    //ColliderComponent.CollisionShape = newBoxCompound;

            //    //motionState = new DefaultMotionState(transform * principal, Matrix.Identity);
            //    //newBoxCompound.CalculateLocalInertia(_mass, out compoundInertia);

            //    //RigidBody = new RigidBody(_mass, motionState, ColliderComponent.CollisionShape, compoundInertia);
            
            //}
            //else
            {
                RigidBody = new RigidBody(ColliderComponent.CollisionShape);
                //RigidBody.Mass = _mass;
                

                //RigidBody.SetMassProperties();

                var mRotationX = JMatrix.CreateRotationX(GameObject.Rotation.X);
                var mRotationY = JMatrix.CreateRotationY(GameObject.Rotation.Y);
                var mRotationZ = JMatrix.CreateRotationZ(GameObject.Rotation.Z);

                var transform = mRotationX * mRotationY * mRotationZ * JMatrix.Identity;

                //RigidBody.Orientation = transform;
                RigidBody.Position = new JVector(GameObject._position.X, GameObject._position.Y, GameObject._position.Z);              
                RigidBody.IsStatic = _isStatic;
            }                     
                          

            GameObject.Scene.Physics.World.AddBody(RigidBody);
            
        }

        public void SetStatic(bool isStatic)
        {
            if (!IsLoaded) throw new InvalidOperationException("Object not loaded");
            RigidBody.IsStatic = isStatic;
        }

        private void UpdateRigidBody()
        {
            if (!IsLoaded) return;                     
        }

        public override void Tick(float timeElapsed)
        {          
            /* update game object position */

            var transform = RigidBody.Orientation.ToOpenTK();
            var trans = Matrix4.CreateTranslation(new Vector3(RigidBody.Position.X, RigidBody.Position.Y, RigidBody.Position.Z));

            /* scale must be taking into account */

            GameObject.Transform = Math.Matrix4.Scale(GameObject.Scale) * transform * trans ;

            GameObject._position = new iGL.Engine.Math.Vector3(RigidBody.Position.X, RigidBody.Position.Y, RigidBody.Position.Z);
        }
    }
}
