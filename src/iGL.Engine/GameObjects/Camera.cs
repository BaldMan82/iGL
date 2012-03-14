using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class Camera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }

        public Camera()
        {
            CameraComponent = new CameraComponent();
            AddComponent(CameraComponent);

            Position = new Vector3(0, 0, 10);
            CameraComponent.Target = new Vector3(0, 0, 0);
            CameraComponent.ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1);
        }

        public Camera(CameraProperties properties)
        {           
            CameraComponent = new CameraComponent(properties);
            AddComponent(CameraComponent);
        }     
    }
}
