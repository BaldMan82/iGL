using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iGL.Engine.Math;

namespace iGL.Engine
{
    public class MeshComponent : GameComponent
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }  

        public short[] Indices { get; set; }

        public Material Material { get; set; }

        public MeshComponent()
        {
            Material = new Material();
        }       

        public override void InternalLoad()
        {

        }

        public override void Tick(float timeElapsed)
        {
           
        }
    }
}
