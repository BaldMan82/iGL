using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iGL.Engine.Math;
using Jitter.LinearMath;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using iGL.Engine.Resources;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    public class MeshComponent : GameComponent
    {
        [XmlIgnore]
        public Vector3[] Vertices { get; set; }

        [XmlIgnore]
        public Vector3[] Normals { get; set; }

        [XmlIgnore]
        public Vector2[] UV { get; set; }

        [XmlIgnore]
        public short[] Indices { get; set; }

        public Material Material { get; set; }

        public Texture Texture { get; private set; }

        public string MeshResourceName { get; set; }

        public JBBox BoundingBox { get; private set; }

        public MeshComponent(XElement xmlElement) : base(xmlElement) { }

        public MeshComponent() { }

        protected override void Init()
        {
            Material = new Material()
            {
                Ambient = new Vector4(0, 0, 0, 1),
                Diffuse = new Vector4(0.4f, 0.4f, 0.4f, 1),
                TextureTilingX = 1,
                TextureTilingY = 1,
                TextureRepeatX = false,
                TextureRepeatY = false          
            };

            Vertices = new Vector3[0];
            Normals = new Vector3[0];
            Indices = new short[0];
            UV = new Vector2[0];
        }

        public override bool InternalLoad()
        {
            if (!string.IsNullOrEmpty(MeshResourceName))
            {
                var meshResource = GameObject.Scene.Resources.FirstOrDefault(t => t.Name == MeshResourceName) as ColladaMesh;
                if (meshResource == null) return false;

                this.Normals = meshResource.Normals;
                this.Vertices = meshResource.Vertices;
                this.Indices = meshResource.Indices;
            }

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

            var box = new JBBox(vMin.ToJitter(), vMax.ToJitter());

            BoundingBox = box;

            RefreshTexture();

            return true;
        }

        public void Reload()
        {
            InternalLoad();
        }

        public void RefreshTexture()
        {
            Texture = GameObject.Scene.Resources.FirstOrDefault(t => t.Name == Material.TextureName) as Texture;
        }

        public bool RayTest(Vector3 origin, Vector3 direction, out Vector3 hitLocation)
        {
            hitLocation = new Vector3(0);

            Matrix4 transform;

            /* if there is a rigid body active in this object, we need that transform as it will always describe its world orientation */
            var rigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

            if (rigidBody != null)
            {
                transform = rigidBody.RigidBodyTransform;
            }
            else
            {
                transform = GameObject.GetCompositeTransform();
            }

            transform.Invert();

            var orientation = transform;
            orientation.M41 = 0;
            orientation.M42 = 0;
            orientation.M43 = 0;
            orientation.M44 = 1;

            /* transform ray to object space */

            var o = Vector3.Transform(origin, transform);
            var d = Vector3.Transform(direction, orientation);

            if (BoundingBox.RayIntersect(o.ToJitter(), d.ToJitter()))
            {
                Vector3 r0 = o;
                Vector3 r1 = o + (d * 1000.0f);

                List<Vector3> hits = new List<Vector3>();

                /* test face / ray intersection */
                for (int i = 0; i < Indices.Length; i += 3)
                {
                    if (FaceRayIntersect(ref r0, ref r1, ref Vertices[Indices[i]], ref Vertices[Indices[i + 1]], ref Vertices[Indices[i + 2]]))
                    {
                        hitLocation = Vertices[Indices[i]] + Vertices[Indices[i + 1]] + Vertices[Indices[i + 2]];
                        hitLocation /= 3.0f;

                        hits.Add(hitLocation);
                    }
                }

                if (hits.Count > 0)
                {                    
                    hitLocation = hits.OrderBy(hit => (hit - origin).LengthSquared).First();
                    hitLocation = Vector3.Transform(hitLocation, GameObject.GetCompositeTransform());

                    return true;
                }
            }

            return false;

        }

        private bool FaceRayIntersect(ref Vector3 r0, ref Vector3 r1, ref Vector3 t0, ref Vector3 t1, ref Vector3 t2)
        {
            var R1 = r1 - r0;
            var v0 = t0 - r0;
            var v1 = t1 - r0;
            var v2 = t2 - r0;

            int sign = (Det(ref R1, ref v0, ref v1) >= 0.0f) ? 1 : 0;
            sign += (Det(ref R1, ref v1, ref v2) >= 0.0f) ? 2 : 0;
            sign += (Det(ref R1, ref v2, ref v0) > 0.0f) ? 4 : 0;

            if (sign == 0 || sign == 7)
                return true;

            return false;
        }

        private float Det(ref Vector3 x, ref Vector3 y, ref Vector3 z)
        {
            return Vector3.Dot(x, Vector3.Cross(y, z));
        }

        public override void Tick(float timeElapsed)
        {

        }

        public void CalculateNormals()
        {
            var indices = Indices;
            var vertices = Vertices;
            var normals = Normals;

            if (indices.Length % 3 != 0)
                throw new NotSupportedException("Invalid indices count for normal calc");

            normals = new Vector3[vertices.Length];

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] v = new Vector3[3] { vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]] };

                var norm = Vector3.Cross(v[1] - v[0], v[2] - v[0]);

                for (int j = 0; j < 3; j++)
                {
                    Vector3 a = v[(j + 1) % 3] - v[j];
                    Vector3 b = v[(j + 2) % 3] - v[j];

                    float weight = (float)System.Math.Acos(Vector3.Dot(a, b) / (a.Length * b.Length));
                    normals[indices[i + j]] += weight * norm;
                }
            }

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i].Normalize();
            }

            Normals = normals;
        }
    }
}
