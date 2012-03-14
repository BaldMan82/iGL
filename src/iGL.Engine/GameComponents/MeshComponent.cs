using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iGL.Engine.Math;
using Jitter.LinearMath;

namespace iGL.Engine
{
    public class MeshComponent : GameComponent
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }  

        public short[] Indices { get; set; }

        public Material Material { get; set; }

        private JBBox _boundingBox;        

        public MeshComponent()
        {
            Material = new Material();
        }       

        public override void InternalLoad()
        {
            Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var vertex in Vertices)
            {
                if (vertex.X < vMin.X) vMin.X = vertex.X;
                if (vertex.X > vMax.X) vMax.X = vertex.X;

                if (vertex.Y < vMin.Y) vMin.Y = vertex.Y;
                if (vertex.Y > vMax.Y) vMax.Y = vertex.Y;

                if (vertex.Z < vMin.Z) vMin.Z = vertex.Z;
                if (vertex.Z > vMax.Z) vMax.Z = vertex.Z;
            }

            _boundingBox.Max = vMax.ToJitter();
            _boundingBox.Min = vMin.ToJitter();
        }

        public bool RayTest(Vector3 origin, Vector3 direction)
        {          
            var transform = GameObject.GetCompositeTransform();
            var jTransform = transform.ToJitter();
            var position = new JVector(transform.M41, transform.M42, transform.M43);

            var transformedBox = _boundingBox;
            transformedBox.Max += position;
            transformedBox.Min += position;

            transformedBox.Transform(ref jTransform);

            return transformedBox.RayIntersect(origin.ToJitter(), direction.ToJitter());            
        }


        public override void Tick(float timeElapsed)
        {
           
        }
    }
}
