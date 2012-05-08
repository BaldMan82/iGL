﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    [RequiredComponent(typeof(MeshComponent), RigidFarseerSphere.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), RigidFarseerSphere.MeshRenderComponentId)]
    [RequiredComponent(typeof(RigidBodyFarseerComponent), RigidFarseerSphere.RigidBodyFarseerComponentId)]
    [RequiredComponent(typeof(SphereColliderFarseerComponent), RigidFarseerSphere.SphereColliderFarseerComponentId)]
    public class RigidFarseerSphere : GameObject
    {
        public int Rings { get; set; }
        public int Segments { get; set; }

        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;
        private RigidBodyFarseerComponent _rigidBodyComponent;
        private SphereColliderFarseerComponent _sphereColliderComponent;

        private const string MeshComponentId = "64d23823-aa44-4aeb-a742-57dbe16883e4";
        private const string MeshRenderComponentId = "8ed3d915-18ef-427e-bbde-7906f8375e6c";
        private const string RigidBodyFarseerComponentId = "5aae1f68-eace-5029-a45a-dfe3d95ec2ea";
        private const string SphereColliderFarseerComponentId = "1a256ff2-1f46-4151-8197-8f502b4343ab";

        protected override void Init()
        {
            /* todo: re-use rendercomponent, like cube !! */
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;
            _rigidBodyComponent = Components.Single(c => c.Id == RigidBodyFarseerComponentId) as RigidBodyFarseerComponent;
            _sphereColliderComponent = Components.Single(c => c.Id == SphereColliderFarseerComponentId) as SphereColliderFarseerComponent;

            Rings = 16;
            Segments = 16;
        }

        public RigidFarseerSphere(XElement element) : base(element) { }

        public RigidFarseerSphere() { }

        private void LoadSphere()
        {
            // code: http://www.ogre3d.org/tikiwiki/ManualSphereMeshes

            List<short> indices = new List<short>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            float radius = 0.5f;
            float fDeltaRingAngle = (float)(System.Math.PI / (double)Rings);
            float fDeltaSegAngle = (float)(2.0d * System.Math.PI / (double)Segments);
            ushort wVerticeIndex = 0;

            for (int ring = 0; ring <= Rings; ring++)
            {
                float r0 = (float)((double)radius * System.Math.Sin((float)ring * fDeltaRingAngle));
                float y0 = (float)((double)radius * System.Math.Cos((float)ring * fDeltaRingAngle));

                for (int seg = 0; seg <= Segments; seg++)
                {
                    float x0 = (float)(r0 * System.Math.Sin((float)seg * fDeltaSegAngle));
                    float z0 = (float)(r0 * System.Math.Cos((float)seg * fDeltaSegAngle));

                    Vector3 pos = new Vector3(x0, y0, z0);
                    vertices.Add(pos);

                    Vector3 norm = new Vector3(pos);
                    norm.Normalize();
                    normals.Add(norm);

                    Vector2 uv = new Vector2((float)seg / (float)Segments, (float)ring / (float)Rings);
                    uvs.Add(uv);

                    if (ring != Rings)
                    {
                        indices.Add((short)(wVerticeIndex + Segments + 1));
                        indices.Add((short)(wVerticeIndex));
                        indices.Add((short)(wVerticeIndex + Segments));
                        indices.Add((short)(wVerticeIndex + Segments + 1));
                        indices.Add((short)(wVerticeIndex + 1));
                        indices.Add((short)(wVerticeIndex));

                        wVerticeIndex++;
                    }
                }

            }

            _meshComponent.Vertices = vertices.ToArray();
            _meshComponent.Indices = indices.ToArray();
            _meshComponent.Normals = normals.ToArray();
            _meshComponent.UV = uvs.ToArray();

            //_meshComponent.CalculateNormals();

        }

        public override void Load()
        {
            LoadSphere();

            base.Load();
        }
    }
}
