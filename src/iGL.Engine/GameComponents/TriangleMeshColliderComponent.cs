using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Jitter.Collision.Shapes;
using Jitter.Collision;
using Jitter.LinearMath;

namespace iGL.Engine.GameComponents
{
    [Serializable]
    public class TriangleMeshColliderComponent : ColliderComponent
    {
        public TriangleMeshColliderComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public TriangleMeshColliderComponent() { }

        public override bool InternalLoad()
        {
            return LoadCollider();
        }

        private bool LoadCollider()
        {
            /* find a mesh component to create box from */
            var meshComponent = this.GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent == null)
            {
                return false;
            }

            if (!meshComponent.IsLoaded) meshComponent.Load();

            var vertices = meshComponent.Vertices.Select(v => new JVector(v.X * GameObject.Scale.X, v.Y * GameObject.Scale.Y, v.Z * GameObject.Scale.Z)).ToList();
            var indices = new List<TriangleVertexIndices>();

            for (int i = meshComponent.Indices.Length-1; i >= 2; i -= 3)
            {
                indices.Add(new TriangleVertexIndices(meshComponent.Indices[i], meshComponent.Indices[i-1], meshComponent.Indices[i-2]));
            }

            var shape = new TriangleMeshShape(new Octree(vertices, indices));
            shape.FlipNormals = true;
           
            shape.Tag = this.GameObject;

            this.CollisionShape = shape;

            return true;
        }

        internal override void Reload()
        {
            if (!LoadCollider()) throw new InvalidOperationException();
        }
    }
}
