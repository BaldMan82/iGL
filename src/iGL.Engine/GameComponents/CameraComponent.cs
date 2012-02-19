using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

namespace iGL.Engine
{
    public class CameraComponent : GameComponent
    {
        public float FieldOfViewRadians { get; set; }
        public float AspectRatio { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ModelViewMatrix { get; private set; }
        public Matrix4 ModelViewProjectionMatrix { get; private set; }

        public CameraComponent(GameObject gameObject, float fieldOfViewRadians, float aspectRatio, float zNear, float zFar) : base(gameObject)
        {
            FieldOfViewRadians = fieldOfViewRadians;
            AspectRatio = aspectRatio;
            ZNear = zNear;
            ZFar = zFar;

            Up = new Vector3(0.0f, 1.0f, 0.0f);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfViewRadians, AspectRatio, ZNear, ZFar);          
        }       

        public override void InternalLoad()
        {
          
        }

        public override void Tick(double timeElapsed)
        {
            /* must be sure Target has ticked first ? */

            var projectionMatrix = ProjectionMatrix;
            ModelViewMatrix = Matrix4.LookAt(GameObject.Position, Target, Up);

            ModelViewProjectionMatrix = ModelViewMatrix * projectionMatrix;           
        }
    }
}
