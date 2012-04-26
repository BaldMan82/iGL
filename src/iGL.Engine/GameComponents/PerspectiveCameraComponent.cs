using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [Serializable]
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
        }

        public void Update()
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfViewRadians, AspectRatio, ZNear, ZFar);

            /* quick hack to get into landscape mode on iphone */

            //var r0 = ProjectionMatrix.Row0;
            //var r1 = ProjectionMatrix.Row1;
            //var r2 = ProjectionMatrix.Row2;
            //var r3 = ProjectionMatrix.Row3;

            //ProjectionMatrix = new Matrix4(new Vector4(r0.Y, -r0.X, r0.Z, r0.W),
            //                                        new Vector4(r1.Y, -r1.X, r1.Z, r1.W),
            //                                        new Vector4(r2.Y, -r2.X, r2.Z, r2.W),
            //                                        new Vector4(r3.Y, -r3.X, r3.Z, r3.W));
        }

        public override bool InternalLoad()
        {
            Update();

            return true;
        }

        public override void Tick(float timeElapsed)
        {
            ModelViewMatrix = Matrix4.LookAt(GameObject.Position, Target, Up);
            ModelViewProjectionMatrix = ModelViewMatrix * ProjectionMatrix;
        }
    }
}
