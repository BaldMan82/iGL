using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    [Serializable]
    public class CompoundObject : GameObject
    {
        private List<GameObject> _compoundChildren { get; set; }
        private CompoundColliderComponent _collider { get; set; }
        private RigidBodyComponent _rigidBody { get; set; }


        public IEnumerable<GameObject> CompoundChildren { get { return _compoundChildren.AsEnumerable(); } }

        public CompoundObject(IEnumerable<GameObject> children, float totalMass, bool isStatic = false)
        {
            _compoundChildren = children.ToList();

            var colliders = _compoundChildren.Select(c => c.Components.Single(cc => cc is ColliderComponent) as ColliderComponent).ToList();

            _collider = new CompoundColliderComponent(colliders);

            AddComponent(_collider);

            _rigidBody = new RigidBodyComponent();
            _rigidBody.IsStatic = isStatic;

            _rigidBody.Mass = totalMass;

            AddComponent(_rigidBody);
        }

        public override void Load()
        {
            base.Load();

            _compoundChildren.ForEach(g =>
            {
                g.Scene = this.Scene;
                g.Load();
            });
        }

        public override void Render(Matrix4 parentTransform)
        {
            var thisTransform = Transform * parentTransform;

            base.Render(parentTransform);

            foreach (var child in _compoundChildren)
            {
                var childCollider = child.Components.Single(c => c is ColliderComponent) as ColliderComponent;
                var transform = _collider.GetChildTransform(childCollider);

                child.Transform = Math.Matrix4.Scale(child.Scale) * transform;

                child.Render(thisTransform);
            }
        }
    }
}
