using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    public class CompoundColliderComponent : ColliderComponent
    {
        private List<ColliderComponent> _colliderComponents;      

        public CompoundColliderComponent(XElement xmlElement) : base(xmlElement) { }

        public CompoundColliderComponent() : base() { }

        public CompoundColliderComponent(List<ColliderComponent> colliderComponents)            
        {
            _colliderComponents = colliderComponents;
        }

        public override bool InternalLoad()
        {
            base.InternalLoad();

            return LoadCollider();
        }

        private bool LoadCollider()
        {
            //var compoundShape = new CompoundShape(

            var transformedShapes = new List<CompoundShape.TransformedShape>();

            foreach (var component in _colliderComponents)
            {
                if (!component.IsLoaded) component.Load();

                /* set initalial position / rotation of object 
                * do not incorporate scale matrix */

                var gameObject = component.GameObject;

                var mRotationX = JMatrix.CreateRotationX(gameObject.Rotation.X);
                var mRotationY = JMatrix.CreateRotationY(gameObject.Rotation.Y);
                var mRotationZ = JMatrix.CreateRotationZ(gameObject.Rotation.Z);

                var transform = mRotationX * mRotationY * mRotationZ * JMatrix.Identity;

                var position = gameObject.Position.ToJitter();
                transformedShapes.Add(new CompoundShape.TransformedShape(component.CollisionShape, transform, position));
            }


            CollisionShape = new CompoundShape(transformedShapes);
            CollisionShape.Tag = GameObject;

            return true;
        }

        public Math.Matrix4 GetChildTransform(ColliderComponent component)
        {
            CompoundShape shape = this.CollisionShape as CompoundShape;

            for (int i = 0; i < shape.Shapes.Length; i++)
            {
                if (shape.Shapes[i].Shape == component.CollisionShape)
                {
                    return shape.Shapes[i].Orientation.ToOpenTK(shape.Shapes[i].Position);
                }
            }

            throw new Exception("Child not found");
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }

        internal override void Reload()
        {
            if (!LoadCollider()) throw new InvalidOperationException();
        }
    }
}
