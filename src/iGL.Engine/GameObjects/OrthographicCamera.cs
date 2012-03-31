using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [RequiredComponent(typeof(OrthogonalCameraComponent), OrthographicCamera.CameraComponentId)]
    public class OrthographicCamera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }
       
        private const string CameraComponentId = "7d719186-50f7-49c1-bb2b-7b7cd85dadbc";

        public OrthographicCamera(SerializationInfo info, StreamingContext context) : base(info, context) { }
           
        public OrthographicCamera() { }

        protected override void Init()
        {
            CameraComponent = Components.First(c => c.Id == CameraComponentId) as CameraComponent;          
            CameraComponent.ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);

            Position = new Vector3(0, 0, 10);
        }      
    }
}

