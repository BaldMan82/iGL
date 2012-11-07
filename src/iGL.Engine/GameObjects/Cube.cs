using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{

    [Serializable]
    [RequiredComponent(typeof(MeshComponent), Cube.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), Cube.MeshRenderComponentId)]
    public class Cube : GameObject
    {
        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        public float Height
        {
            get { return _scale.Y; }
        }
        public float Width
        {
            get { return _scale.X; }
        }
        public float Depth
        {
            get { return _scale.Z; }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;

        private static MeshComponent _staticMeshComponent;
        private static MeshRenderComponent _staticMeshRenderComponent;

        private const string MeshComponentId = "b2dae056-2ff7-443f-aed1-0afd3db7b0be";
        private const string MeshRenderComponentId = "56af2307-be79-453a-a8ab-54bad0d21525";

        public Cube(XElement element) : base(element) { }

        public Cube() { }

        protected override void Init()
        {            
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;

            _meshComponent.MeshResourceName = "cube";
        }
      
    }
}
