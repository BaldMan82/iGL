using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace iGL.Engine.GameObjects
{
    [Serializable]
    [RequiredComponent(typeof(MeshComponent), Background.BackgroundComponentId)]   
    public class Background : GameObject
    {       
        private const string BackgroundComponentId = "36af2307-be79-453a-a8bb-54bad0d21525";

        public Background(XElement element) : base(element) { }

        public Background() { }
    }
}
