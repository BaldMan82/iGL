using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [RequiredComponent(typeof(LightComponent), LightObject.LightComponentId)]
    public class LightObject : GameObject
    {
        public LightComponent LightComponent { get; private set; }

        public ILight Light { get; set; }

        private const string LightComponentId = "a7a1fffa-f12a-4a52-8e89-c79630bdf223";

        public LightObject(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public LightObject() { }

        protected override void Init()
        {
            LightComponent = Components.Single(c => c.Id == LightComponentId) as LightComponent;
            Light = LightComponent.Light;

            var sphere = new Sphere();
            sphere.Material.Ambient = new Vector4(1, 1, 0, 1);
            sphere.Designer = true;

            AddChild(sphere);
        }
    }
}
