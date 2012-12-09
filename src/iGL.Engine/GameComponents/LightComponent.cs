using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{
    public interface ILight { }

    [Serializable]
    public class PointLight : ILight
    {
        public Vector4 Ambient { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector4 Specular { get; set; }
        public Vector4 WorldPosition { get; set; }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            this.WriteXml(writer);
        }
    }

    [Serializable]
    public class LightComponent : GameComponent
    {
        public ILight Light { get; set; }

        public LightComponent(XElement xmlElement) : base(xmlElement) { }

        public LightComponent() { }      

        protected override void Init()
        {
            Light = new PointLight()
                {
                    Ambient = new Vector4(1, 1, 1, 1),
                    Diffuse = new Vector4(1, 1, 1, 1),       
                };
        }

        public LightComponent(ILight light)
        {
            Light = light;
        }

        public override bool InternalLoad()
        {
            return true;
        }

        public override void Tick(float timeElapsed)
        {

        }
    }
}
