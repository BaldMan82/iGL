//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using BulletXNA.BulletCollision;
//using BulletXNA.LinearMath;

//namespace iGL.Engine
//{
//    public class CompoundColliderComponent : ColliderComponent
//    {
//        private List<ColliderComponent> _colliderComponents;

//        public CompoundColliderComponent(List<ColliderComponent> colliderComponents, GameObject gameObject)
//            : base(gameObject)
//        {
//            _colliderComponents = colliderComponents;
//        }

//        public override void InternalLoad()
//        {
//            base.InternalLoad();
         
//            var compoundShape = new CompoundShape(true);

//            foreach (var component in _colliderComponents)
//            {
//                if (!component.IsLoaded) component.Load();

//                /* set initalial position / rotation of object 
//                * do not incorporate scale matrix */
//                var gameObject = component.GameObject;

//                var mRotationX = Matrix.CreateRotationX(gameObject.Rotation.X);
//                var mRotationY = Matrix.CreateRotationY(gameObject.Rotation.Y);
//                var mRotationZ = Matrix.CreateRotationZ(gameObject.Rotation.Z);

//                var transform = mRotationX * mRotationY * mRotationZ * Matrix.Identity;
//                transform.Translation = new Vector3(gameObject._position.X, gameObject._position.Y, gameObject._position.Z);             

//                compoundShape.AddChildShape(ref transform, component.CollisionShape);
//            }           

//            CollisionShape = compoundShape;
//        }

//        public Math.Matrix4 GetChildTransform(ColliderComponent component)
//        {
//            CompoundShape shape = this.CollisionShape as CompoundShape;            

//            for (int i = 0; i < shape.GetNumChildShapes(); i++)
//            {
//                if (shape.GetChildShape(i) == component.CollisionShape)
//                {
//                    return shape.GetChildTransform(i).ToOpenTK();
//                }
//            }

//            throw new Exception("Child not found");
//        }

//        public override void Tick(float timeElapsed)
//        {
//            base.Tick(timeElapsed);
//        }
//    }
//}
