using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [RequiredComponent(typeof(CameraComponent), PerspectiveCamera.CameraComponentId)]
    public class PerspectiveCamera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }

        public float FieldOfViewRadians { get; set; }
        public float AspectRatio { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        private const string CameraComponentId = "7d719186-50f7-49c1-bb2b-7b7cd85dadbc";

        public Vector3 Target
        {
            get
            {
                return CameraComponent.Target;
            }
            set
            {
                CameraComponent.Target = value;
            }
        }


        public Vector4 ClearColor { get; set; }

        public PerspectiveCamera(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PerspectiveCamera() { }

        protected override void Init()
        {
            CameraComponent = Components.Single(c => c.Id == CameraComponentId) as CameraComponent;

            /* defaults */

            FieldOfViewRadians = MathHelper.DegreesToRadians(45.0f);
            AspectRatio = 3.0f / 2.0f;
            ZNear = 1.00f;
            ZFar = 1000.0f;

            ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            Position = new Vector3(0, 0, 10);
        }

        private void LoadCamera()
        {
            CameraComponent.Properties = new PerspectiveProperties()
            {
                FieldOfViewRadians = this.FieldOfViewRadians,
                AspectRatio = this.AspectRatio,
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

