using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public interface CameraProperties { };

    public class PerspectiveProperties : CameraProperties
    {
        public float FieldOfViewRadians { get; set; }
        public float AspectRatio { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }
    }


    public class OrtographicProperties : CameraProperties
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }
    }

    public class CameraComponent : GameComponent
    {
        public enum Type
        {
            Perspective,
            Orthographic
        }       
       
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ModelViewMatrix { get; private set; }
        public Matrix4 ModelViewProjectionMatrix { get; private set; }
        
        private CameraProperties _properties;

        public CameraComponent()
            : this(new PerspectiveProperties()
            {
                AspectRatio = MathHelper.DegreesToRadians(45.0f),
                FieldOfViewRadians = 3.0f / 2.0f,
                ZNear = 1.00f,
                ZFar = 1000.0f
            }) { }
        
        public CameraComponent(CameraProperties properties)
        {
            _properties = properties;

            if (_properties is PerspectiveProperties){
                var perspective = _properties as PerspectiveProperties;
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(perspective.FieldOfViewRadians, perspective.AspectRatio, perspective.ZNear, perspective.ZFar);
            }
            else if (_properties is OrtographicProperties)
            {
                var orthograpgic = _properties as OrtographicProperties;
                ProjectionMatrix = Matrix4.CreateOrthographic(orthograpgic.Width, orthograpgic.Height, orthograpgic.ZNear, orthograpgic.ZFar);
            }
            else
            {
                throw new NotSupportedException(properties.GetType().ToString());
            }

            Up = new Vector3(0.0f, 1.0f, 0.0f);                                               
        }       

        public override void InternalLoad()
        {
          
        }

        public override void Tick(float timeElapsed)
        {
            /* must be sure Target has ticked first ? */
          
            ModelViewMatrix = Matrix4.LookAt(GameObject.Position, Target, Up);
            ModelViewProjectionMatrix = ModelViewMatrix * ProjectionMatrix;           
        }
    }
}
