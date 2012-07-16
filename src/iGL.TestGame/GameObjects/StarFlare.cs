using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    [RequiredComponent(typeof(PropertyAnimationComponent), ScaleAnimationComponentId)]
    [RequiredComponent(typeof(PropertyAnimationComponent), AlphaAnimationComponentId)]
    public class StarFlare : Plane
    {
        private const string ScaleAnimationComponentId = " 9501d244-0a87-4620-b4f4-41706ad2e0b0";
        private const string AlphaAnimationComponentId = " 7581d245-0a87-4620-b4f4-91706ad2e0b0";
        private PropertyAnimationComponent _scaleAnimationComponent;
        private PropertyAnimationComponent _alphaAnimationComponent;

        public StarFlare(XElement element) : base(element) { }

        public StarFlare() { }

        public Vector4 FlareColor
        {
            get
            {
                return Material.Ambient;
            }
            set
            {
                Material.Ambient = value;
            }
        }

        protected override void Init()
        {
            base.Init();

            Material.TextureName = "starflare";            

            _scaleAnimationComponent = Components.First(c => c.Id == ScaleAnimationComponentId) as PropertyAnimationComponent;
            _scaleAnimationComponent.Property = "Scale";
            _scaleAnimationComponent.StartValue = "0.1,0.1,0.1";
            _scaleAnimationComponent.StopValue = "4,4,4";
            _scaleAnimationComponent.DurationSeconds = 0.2f;

            _alphaAnimationComponent = Components.First(c => c.Id == AlphaAnimationComponentId) as PropertyAnimationComponent;
            _alphaAnimationComponent.Property = "FlareColor";
            _alphaAnimationComponent.StartValue = "1,1,0,1";
            _alphaAnimationComponent.StopValue = "1,1,0,0";
            _alphaAnimationComponent.DurationSeconds = 0.2f;

        }

        public void PlayAnimation()
        {
            _scaleAnimationComponent.Play();
            _alphaAnimationComponent.Play();
        }
    }
}
