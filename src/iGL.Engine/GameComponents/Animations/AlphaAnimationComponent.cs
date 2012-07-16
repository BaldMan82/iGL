using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using iGL.Engine.Math;
using System.Xml.Linq;

namespace iGL.Engine
{
    public class AlphaAnimationComponent : AnimationComponent
    {
        public AlphaAnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public AlphaAnimationComponent() : base() { }
        
        DateTime _tickTime;
        bool _isPlaying;
        
        float _ambientAlpha;
        float _diffuseAlpha;
        float _specularAlpha;

        private MeshComponent _meshComponent;

        public float DurationSeconds { get; set; }
        
        public override void Play()
        {
            base.Play();

            _tickTime = DateTime.UtcNow;
            _isPlaying = true;
        }

        public override void Stop()
        {
            base.Stop();

            _isPlaying = false;
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override bool InternalLoad()
        {
            _meshComponent = GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;

            _ambientAlpha = _meshComponent.Material.Ambient.W;
            _diffuseAlpha = _meshComponent.Material.Diffuse.W;
            _specularAlpha = _meshComponent.Material.Specular.W;

            return true;
        }

        private void Step()
        {
            float percentage = (float)(DateTime.UtcNow - _tickTime).TotalSeconds / (DurationSeconds);

            if (percentage > 1) percentage = 1;

            _meshComponent.Material.Ambient = new Vector4(_meshComponent.Material.Ambient.X,
                                                          _meshComponent.Material.Ambient.Y,
                                                          _meshComponent.Material.Ambient.Z,
                                                          _ambientAlpha * percentage);

            _meshComponent.Material.Diffuse = new Vector4(_meshComponent.Material.Diffuse.X,
                                                          _meshComponent.Material.Diffuse.Y,
                                                          _meshComponent.Material.Diffuse.Z,
                                                          _diffuseAlpha * percentage);

            _meshComponent.Material.Specular = new Vector4(_meshComponent.Material.Specular.X,
                                                          _meshComponent.Material.Specular.Y,
                                                          _meshComponent.Material.Specular.Z,
                                                          _specularAlpha * percentage);

            if (percentage >= 1)
            {
                Stop();
            }
        }

        public override void Tick(float timeElapsed)
        {
            if (!_isPlaying) return;

            Step();
        }
    }
}
