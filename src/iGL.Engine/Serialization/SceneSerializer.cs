using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    public class SceneSerializer 
    {
        public string CurrentCameraId { get; set; }
        public string CurrentLightId { get; set; }

        public IEnumerable<GameObject> GameObjects { get; set; }      
    }
}
