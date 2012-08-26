using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    [RequiredComponent(typeof(PropertyAnimationComponent), AlphaAnimationComponentId)]
    public class MagnetHalo : Plane
    {       
        private const string AlphaAnimationComponentId = "1181d245-0a87-4620-b4f4-91706ad2e0b0";    
        private PropertyAnimationComponent _alphaAnimationComponent;

        public MagnetHalo(XElement element) : base(element) { }

        public MagnetHalo() { }

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

            _alphaAnimationComponent = Components.First(c => c.Id == AlphaAnimationComponentId) as PropertyAnimationComponent;
            _alphaAnimationComponent.Property = "FlareColor";
            _alphaAnimationComponent.StartValue = "1,1,0,1";
            _alphaAnimationComponent.StopValue = "1,1,0,0";
            _alphaAnimationComponent.DurationSeconds = 2.0f;
            _alphaAnimationComponent.PlayMode = AnimationComponent.Mode.RepeatInverted;

            DistanceSorting = true;
        }

        public override void Load()
        {
            base.Load();

            _alphaAnimationComponent.Play();
        }


    }
}
