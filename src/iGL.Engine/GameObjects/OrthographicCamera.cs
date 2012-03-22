using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class OrthographicCamera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }

        public float Width { get; set; }
        public float Height { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public Vector3 Target { get; set; }
        public Vector4 ClearColor { get; set; }

        private Guid _cameraComponentId = new Guid("7d719186-50f7-49c1-bb2b-7b7cd85dadbc");

        public OrthographicCamera()
        {
            CameraComponent = new CameraComponent() { CreationMode = GameComponent.CreationModeEnum.Internal, Id =_cameraComponentId };

            /* defaults */

            Height = 20.0f;
            Width = 20.0f * (3.0f / 2.0f);
            ZNear = 1.00f;
            ZFar = 1000.0f;

            ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            Position = new Vector3(0, 0, 10);

            AddComponent(CameraComponent);
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

