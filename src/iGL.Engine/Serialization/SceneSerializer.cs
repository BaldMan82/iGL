using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using iGL.Engine.Math;

namespace iGL.Engine
{
    [Serializable]
    public class SceneSerializer 
    {
        public string CurrentCameraId { get; set; }
        public string CurrentLightId { get; set; }

        public Vector4 AmbientColor { get; set; }

        public List<GameObject> GameObjects { get; set; }      
    }
}
