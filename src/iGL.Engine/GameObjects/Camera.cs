using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public class Camera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }

        public Camera(CameraProperties properties)
        {           
            CameraComponent = new CameraComponent(properties);
            AddComponent(CameraComponent);
        }     
    }
}
