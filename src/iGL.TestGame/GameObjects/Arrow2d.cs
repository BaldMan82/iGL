using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    public class Arrow2d : Plane
    {
        public Arrow2d(XElement element) : base(element) { }

        public Arrow2d() { }

        protected override void Init()
        {
            base.Init();

            DistanceSorting = true;
            Visible = false;

            Material.TextureName = "arrow";
            Material.Diffuse = new Vector4(0);

            RenderQueuePriority = -1;
        }

        public override void Render(bool overrideParentTransform = false)
        {
            GL.Clear(ClearBufferMask.DepthBufferBit);

            Scene.Shader.SetBlackBorder(true);
            
            /* give the arrow coordinates in the world, as it would be a lot of overhead to calculate
             * position inside the game sphere */
            base.Render(true);

            Scene.Shader.SetBlackBorder(false);
        }
    }
}
