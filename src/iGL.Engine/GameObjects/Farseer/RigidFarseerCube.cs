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
    [RequiredComponent(typeof(MeshComponent), RigidFarseerCube.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), RigidFarseerCube.MeshRenderComponentId)]
    [RequiredComponent(typeof(RigidBodyFarseerComponent), RigidFarseerCube.RigidBodyFarseerComponentId)]
    [RequiredComponent(typeof(BoxColliderFarseerComponent), RigidFarseerCube.BoxColliderFarseerComponentId)]
    public class RigidFarseerCube : GameObject
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
        private RigidBodyFarseerComponent _rigidBodyComponent;
        private BoxColliderFarseerComponent _boxColliderComponent;

        private const string MeshComponentId = "f0bfb81f-fbf1-42e2-a970-02f2eb798df8";
        private const string MeshRenderComponentId = "f8967a9d-8c90-41fa-9344-dec053def5b0";
        private const string RigidBodyFarseerComponentId = "e59ebd40-28f0-4d3b-95cf-e9811570f514";
        private const string BoxColliderFarseerComponentId = "09bbe6d6-0108-48f2-9e83-6fd4760040fd";

        public RigidFarseerCube(XElement element) : base(element) { }

        public RigidFarseerCube() { }

        protected override void Init()
        {
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;
            _rigidBodyComponent = Components.Single(c => c.Id == RigidBodyFarseerComponentId) as RigidBodyFarseerComponent;
            _boxColliderComponent = Components.Single(c => c.Id == BoxColliderFarseerComponentId) as BoxColliderFarseerComponent;

            _meshComponent.MeshResourceName = "cube";
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(_meshComponent.MeshResourceName))
            {
                _meshComponent.MeshResourceName = "cube";
            }

            base.Load();
        }
    }
}
