﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [Serializable]
    public class OrthogonalCameraComponent : CameraComponent
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public OrthogonalCameraComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public OrthogonalCameraComponent() { }

        protected override void Init()
        {
            Up = new Vector3(0.0f, 1.0f, 0.0f);

            Height = 20.0f;
            Width = 20.0f * (3.0f / 2.0f);
            ZNear = 1.00f;
            ZFar = 1000.0f;          
        }

        public void Update()
        {
            ProjectionMatrix = Matrix4.CreateOrthographic(Width, Height, ZNear, ZFar);
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
