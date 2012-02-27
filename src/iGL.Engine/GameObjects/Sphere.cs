using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class Sphere : GameObject
    {
        public float Depth { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;

        public Sphere(float r, int rings = 16, int segments = 16)
        {
            _meshComponent = new MeshComponent(this);

            // code: http://www.ogre3d.org/tikiwiki/ManualSphereMeshes

            List<short> indices = new List<short>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();

            float fDeltaRingAngle = (float)(System.Math.PI / (double)rings);
            float fDeltaSegAngle = (float)(2.0d * System.Math.PI / (double)segments);
            ushort wVerticeIndex = 0;

            for (int ring = 0; ring <= rings; ring++)
            {
                float r0 = (float)((double)r * System.Math.Sin((float)ring * fDeltaRingAngle));
                float y0 = (float)((double)r * System.Math.Cos((float)ring * fDeltaRingAngle));

                for (int seg = 0; seg <= segments; seg++)
                {
                    float x0 = (float)(r0 * System.Math.Sin((float)seg * fDeltaSegAngle));
                    float z0 = (float)(r0 * System.Math.Cos((float)seg * fDeltaSegAngle));

                    Vector3 pos = new Vector3(x0, y0, z0);
                    vertices.Add(pos);

                    Vector3 norm = new Vector3(pos);
                    norm.Normalize();
                    normals.Add(norm);

                    // texture : (float) seg / (float) nSegments, (float) ring / (float) nRings

                    if (ring != rings)
                    {
                        indices.Add((short)(wVerticeIndex + segments + 1));
                        indices.Add((short)(wVerticeIndex));
                        indices.Add((short)(wVerticeIndex + segments));
                        indices.Add((short)(wVerticeIndex + segments + 1));
                        indices.Add((short)(wVerticeIndex + 1));
                        indices.Add((short)(wVerticeIndex));

                        wVerticeIndex++;                  
                    }
                }

            }
          
            _meshComponent.Vertices = vertices.ToArray();
            _meshComponent.Indices = indices.ToArray();
            _meshComponent.Normals = normals.ToArray();

            //_meshComponent.CalculateNormals();

            AddComponent(_meshComponent);

            _meshRenderComponent = new MeshRenderComponent(this);

            AddComponent(_meshRenderComponent);
        }
    }
}
