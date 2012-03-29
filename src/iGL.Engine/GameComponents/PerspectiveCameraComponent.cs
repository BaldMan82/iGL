using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    public class PerspectiveCameraComponent : CameraComponent
    {
        public float FieldOfViewRadians { get; set; }
        public float AspectRatio { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public PerspectiveCameraComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public PerspectiveCameraComponent() { }

        protected override void Init()
        {
            Up = new Vector3(0.0f, 1.0f, 0.0f);

            FieldOfViewRadians = MathHelper.DegreesToRadians(45.0f);
            AspectRatio = 3.0f / 2.0f;
            ZNear = 1.00f;
            ZFar = 1000.0f;

            Update();
        }

        public void Update()
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfViewRadians, AspectRatio, ZNear, ZFar);
        }

        public override bool InternalLoad()
        {
            return true;
        }

        public override void Tick(float timeElapsed)
        {
            ModelViewMatrix = Matrix4.LookAt(GameObject.Position, Target, Up);
            ModelViewProjectionMatrix = ModelViewMatrix * ProjectionMatrix;
        }
    }
}
