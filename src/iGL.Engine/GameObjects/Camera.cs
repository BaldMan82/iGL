using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public class Camera : GameObject
    {
        public CameraComponent CameraComponent { get; private set; }

        public Camera(float fieldOfViewRadians, float aspectRatio, float zNear, float zFar)
        {
            CameraComponent = new CameraComponent(this, fieldOfViewRadians, aspectRatio, zNear, zFar);
            AddComponent(CameraComponent);
        }     
    }
}
