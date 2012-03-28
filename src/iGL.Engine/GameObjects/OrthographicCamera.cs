using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [RequiredComponent(typeof(CameraComponent), OrthographicCamera.CameraComponentId)]
    public class OrthographicCamera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }

        public float Width { get; set; }
        public float Height { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public Vector3 Target { get; set; }
        public Vector4 ClearColor { get; set; }

        private const string CameraComponentId = "7d719186-50f7-49c1-bb2b-7b7cd85dadbc";

        public OrthographicCamera(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public OrthographicCamera() { }

        protected override void Init()
        {
            CameraComponent = Components.First(c => c.Id == CameraComponentId) as CameraComponent;

            /* defaults */

            Height = 20.0f;
            Width = 20.0f * (3.0f / 2.0f);
            ZNear = 1.00f;
            ZFar = 1000.0f;

            ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            Position = new Vector3(0, 0, 10);
        }

        private void LoadCamera()
        {
            CameraComponent.Properties = new OrtographicProperties()
            {
                Width = this.Width,
                Height = this.Height,
                ZNear = this.ZNear,
                ZFar = this.ZFar
            };

            CameraComponent.Update();

            CameraComponent.Target = Target;
            CameraComponent.ClearColor = ClearColor;
        }

        public override void Load()
        {
            base.Load();

            LoadCamera();
        }
    }
}

