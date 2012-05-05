using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace iGL.Engine.GameObjects
{
    public class StatisticsText : TextObject
    {
        public StatisticsText(XElement element) : base(element) { }

        public StatisticsText() { }

        protected override void Init()
        {
            base.Init();

            if (!Game.InDesignMode)
            {
                this.RenderQueuePriority = -1;
            }
        }

        public override void Render(bool overrideParentTransform = false)
        {           
            GL.Clear(ClearBufferMask.DepthBufferBit);           

            base._textComponent.Text = "FPS: " + ((int)(10000000.0f / Scene.Statistics.LastRenderDuration.Ticks)).ToString();
            base._textComponent.Reload();

            base.Render(overrideParentTransform);
        }     
    }
}
