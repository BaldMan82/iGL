﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace iGL.Engine
{
    public abstract class CameraComponent : GameComponent
    {
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public Vector4 ClearColor { get; set; }

        [JsonIgnoreAttribute]
        public Matrix4 ProjectionMatrix { get; protected set; }

        [JsonIgnoreAttribute]
        public Matrix4 ModelViewMatrix { get; protected set; }

        [JsonIgnoreAttribute]
        public Matrix4 ModelViewProjectionMatrix { get; protected set; }

        public CameraComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public CameraComponent() { }
       
    }
}
