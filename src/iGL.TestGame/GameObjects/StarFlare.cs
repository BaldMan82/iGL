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

        public Vector3 StartScale { get; set; }
        public Vector3 StopScale { get; set; }
        public Vector4 StartFlareColor { get; set; }
        public Vector4 StopFlareColor { get; set; }

        public float ScaleDuration { get; set; }
        public float ColorDuration { get; set; }

        public StarFlare(XElement element)
            : base(element)
        {

        }

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

            StartScale = new Vector3(0.1f, 0.1f, 0.1f);
            StopScale = new Vector3(4f, 4f, 4f);
            StartFlareColor = new Vector4(1, 1, 0, 1);
            StopFlareColor = new Vector4(1, 1, 0, 0);
            ScaleDuration = 0.2f;
            ColorDuration = 0.2f;

            DistanceSorting = true;

        }

        public override void Load()
        {
            _scaleAnimationComponent = Components.First(c => c.Id == ScaleAnimationComponentId) as PropertyAnimationComponent;
            _scaleAnimationComponent.Property = "Scale";
            _scaleAnimationComponent.StartValue = StartScale.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            _scaleAnimationComponent.StopValue = StopScale.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            _scaleAnimationComponent.DurationSeconds = ScaleDuration;

            _alphaAnimationComponent = Components.First(c => c.Id == AlphaAnimationComponentId) as PropertyAnimationComponent;
            _alphaAnimationComponent.Property = "FlareColor";
            _alphaAnimationComponent.StartValue = StartFlareColor.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            _alphaAnimationComponent.StopValue = StopFlareColor.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            _alphaAnimationComponent.DurationSeconds = ColorDuration;

            base.Load();
		
        }

        public void PlayAnimation()
        {
            _scaleAnimationComponent.Play();
            _alphaAnimationComponent.Play();
        }
    }
}
