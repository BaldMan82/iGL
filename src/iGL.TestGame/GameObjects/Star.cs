using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    [RequiredComponent(typeof(MeshComponent), MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), MeshRenderComponentId)]
    [RequiredComponent(typeof(RigidBodyFarseerComponent), RigidBodyFarseerComponentId)]
    [RequiredComponent(typeof(BoxColliderFarseerComponent), BoxColliderFarseerComponentId)]
    public class Star : GameObject
    {
        public Star(XElement element) : base(element) { }

        public Star() { }

        private const string MeshComponentId = "b2da1016-2ff7-333f-aed1-0afd3db7b0bf";
        private const string MeshRenderComponentId = "a4af2307-be09-453b-a8ab-54bad0d51525";
        private const string RigidBodyFarseerComponentId = "f3dda926-cc99-4c6a-b4b6-d02b47a049fc";
        private const string BoxColliderFarseerComponentId = "3a728731-0f1e-4661-969f-f8b3d4cac27f";

        protected override void Init()
        {
            base.Init();

            var meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            meshComponent.MeshResourceName = "star";
            meshComponent.Material.TextureName = "star";

            this.Scale = new Vector3(0.05f, 0.05f, 0.05f);

            var body = Components.Single(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            body.IsSensor = true;

            this.OnObjectCollision += Star_OnObjectCollision;


        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }

        void Star_OnObjectCollision(object sender, Engine.Events.ObjectCollisionEvent e)
        {
            if (IsDisposing) return;

            //if (e.Object is SlingshotBallFarseer3D)
            {
                Scene.DisposeGameObject(this);
                var flare = new StarFlare();

                flare.Position = this.Position;
                Scene.AddGameObject(flare);

                flare.PlayAnimation();

                Scene.AddTimer(new Timer() { Action = () => Scene.DisposeGameObject(flare), Interval = TimeSpan.FromSeconds(0.2), Mode = Timer.TimerMode.Once });

            }
        }
    }
}
