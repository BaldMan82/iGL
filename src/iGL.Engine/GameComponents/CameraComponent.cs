﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using Newtonsoft.Json;
using System.Runtime.Serialization;

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
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public Vector4 ClearColor { get; set; }

        [JsonIgnoreAttribute]
        public Matrix4 ProjectionMatrix { get; private set; }

        [JsonIgnoreAttribute]
        public Matrix4 ModelViewMatrix { get; private set; }

        [JsonIgnoreAttribute]
        public Matrix4 ModelViewProjectionMatrix { get; private set; }

        public CameraProperties Properties { get; set; }

        public CameraComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public CameraComponent() { }
        
        protected override void Init()
        {
            Properties = new PerspectiveProperties()
            {
                FieldOfViewRadians = MathHelper.DegreesToRadians(45.0f),
                AspectRatio = 3.0f / 2.0f,
                ZNear = 1.00f,
                ZFar = 1000.0f
            };

            Up = new Vector3(0.0f, 1.0f, 0.0f);

            Update();  
        }

        public void Update()
        {
            if (Properties is PerspectiveProperties)
            {
                var perspective = Properties as PerspectiveProperties;
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(perspective.FieldOfViewRadians, perspective.AspectRatio, perspective.ZNear, perspective.ZFar);
            }
            else if (Properties is OrtographicProperties)
            {
                var orthograpgic = Properties as OrtographicProperties;
                ProjectionMatrix = Matrix4.CreateOrthographic(orthograpgic.Width, orthograpgic.Height, orthograpgic.ZNear, orthograpgic.ZFar);
            }
            else
            {
                throw new NotSupportedException(Properties.GetType().ToString());
            }
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
