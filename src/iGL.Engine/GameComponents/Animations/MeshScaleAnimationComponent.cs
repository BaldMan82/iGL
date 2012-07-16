using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GameComponents;
using iGL.Engine.Math;
using System.Xml.Linq;

namespace iGL.Engine
{
    public class MeshScaleAnimationComponent : AnimationComponent
    {
        public float DurationSeconds { get; set; }

        private DateTime _tickTime;
        private bool _isPlaying;
        private Vector3 _startValue;
        private Vector4 _ambientColor;
        private MeshRenderComponent _renderComponent;
        private MeshComponent _meshComponent;
        private RigidBodyBaseComponent _rigidBodyComponent;
        private float _width;
        private float _xStart;

        public MeshScaleAnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public MeshScaleAnimationComponent() : base() { }

        protected override void Init()
        {
            base.Init();
            DurationSeconds = 5;
        }

        public override void Play()
        {
            if (!IsLoaded) return;

            _isPlaying = true;
            _tickTime = DateTime.UtcNow;
            _startValue = GameObject.Scale;
           

            _renderComponent.BeginMode = BeginMode.Lines;

            _ambientColor = _meshComponent.Material.Ambient;
            _meshComponent.Material.Ambient = new Vector4(1);

            if (_rigidBodyComponent != null) _rigidBodyComponent.AutoReloadBody = false;

            GameObject.Scale = new Vector3(0);

            _width = _meshComponent.BoundingBox.Max.X;
            _xStart = GameObject.Position.X;

            base.Play();
        }

        public override void Stop()
        {
            _isPlaying = false;

          

            base.Stop();
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override bool InternalLoad()
        {
            _renderComponent = GameObject.Components.FirstOrDefault(c => c is MeshRenderComponent) as MeshRenderComponent;
            _meshComponent = GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            _rigidBodyComponent = GameObject.Components.FirstOrDefault(c => c is RigidBodyBaseComponent) as RigidBodyBaseComponent;

            /* loaded if textcomponent is found */
            return _renderComponent != null && _meshComponent != null;
        }

        private void Step()
        {
            float percentageX = (float)(DateTime.UtcNow - _tickTime).TotalSeconds / (DurationSeconds / 2);
            float percentageZ = 0;

            if (percentageX >= 1)
            {
                percentageX = 1;
                percentageZ = ((float)(DateTime.UtcNow - _tickTime).TotalSeconds - (DurationSeconds / 2)) / (DurationSeconds/2);   
            }

            float newX = _xStart - _width * (1.0f - percentageX);
            GameObject.Position = new Vector3(newX, GameObject.Position.Y, GameObject.Position.Z);

            if (percentageZ < 1)
            {
                GameObject.Scale = new Vector3(_startValue.X * percentageX, _startValue.Y, _startValue.Z * percentageZ);            
            }
            else
            {
                if (_rigidBodyComponent != null) _rigidBodyComponent.AutoReloadBody = true; 

                GameObject.Scale = _startValue;

                Stop();
                _renderComponent.BeginMode = BeginMode.Triangles;
                _meshComponent.Material.Ambient = _ambientColor;
            }

        }

        public override void Tick(float timeElapsed)
        {
            if (!_isPlaying) return;

            Step();
        }
    }
}
