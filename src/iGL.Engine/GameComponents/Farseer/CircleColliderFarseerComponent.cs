using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FarseerPhysics.Collision.Shapes;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    public class CircleColliderFarseerComponent : ColliderFarseerComponent
    {        
        public CircleColliderFarseerComponent(XElement xmlElement) : base(xmlElement) { }

        public CircleColliderFarseerComponent() { }

        public float Radius { get; set; }

        public override bool InternalLoad()
        {
            base.InternalLoad();

            return LoadCollider();
        }

        private bool LoadCollider()
        {
            CollisionShape = new CircleShape(Radius, 1.0f);
            
            return true;
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }

        internal override void Reload()
        {
            if (!LoadCollider()) throw new InvalidOperationException();
        }
    }
}
