using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [RequiredComponent(typeof(PerspectiveCameraComponent), PerspectiveCamera.CameraComponentId)]
    public class PerspectiveCamera : GameObject
    {
        public PerspectiveCameraComponent CameraComponent { get; private set; }      

        private const string CameraComponentId = "7d719186-50f7-49c1-bb2b-7b7cd85dadbc";
       
        public PerspectiveCamera(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PerspectiveCamera() { }

        protected override void Init()
        {
            CameraComponent = Components.Single(c => c.Id == CameraComponentId) as PerspectiveCameraComponent;

            /* defaults */

            CameraComponent.FieldOfViewRadians = MathHelper.DegreesToRadians(45.0f);
            CameraComponent.AspectRatio = 3.0f / 2.0f;
            CameraComponent.ZNear = 1.00f;
            CameraComponent.ZFar = 1000.0f;

            CameraComponent.ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            Position = new Vector3(0, 0, 10);
        }

        private void LoadCamera()
        {           
            CameraComponent.Update();          
        }

        public override void Load()
        {
            base.Load();

            LoadCamera();
        }
    }
}

