using System.Linq;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    [RequiredComponent(typeof(LightComponent), LightObject.LightComponentId)]
    [RequiredChild(typeof(Sphere), LightObject.LightSphereId)]
    public class LightObject : GameObject
    {
        public LightComponent LightComponent { get; private set; }

        public ILight Light { get { return LightComponent.Light; } set { LightComponent.Light = value; } }

        private const string LightComponentId = "a7a1fffa-f12a-4a52-8e89-c79630bdf223";
        private const string LightSphereId = "b7a1fffa-f12a-4a52-8e89-c79630bdf223";

        public LightObject(XElement element) : base(element) { }

        public LightObject() { }

        protected override void Init()
        {
            LightComponent = Components.Single(c => c.Id == LightComponentId) as LightComponent;
            Light = LightComponent.Light;

            var sphere = Children.First(c => c.Id == LightSphereId) as Sphere;
            sphere.Material.Ambient = new Vector4(1, 1, 0, 1);
            //sphere.Designer = true;            
        }
    }
}
