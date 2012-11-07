using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;

namespace iGL.TestGame.GameObjects
{
    public class LevelEndSensor : RigidFarseerCube
    {
        public LevelEndSensor(XElement element) : base(element) { }

        public LevelEndSensor()
        {

        }

        protected override void Init()
        {
            base.Init();

            var rigidBodyComponent = this.Components.Single(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            rigidBodyComponent.IsSensor = true;
            rigidBodyComponent.IsStatic = true;

            this.OnObjectCollision += LevelEndSensor_OnObjectCollision;
        }

        public override void Load()
        {
            base.Load();

            this.Visible = Game.InDesignMode;
        }

        void LevelEndSensor_OnObjectCollision(object sender, Engine.Events.ObjectCollisionEvent e)
        {
            if (e.Object == Scene.PlayerObject)
            {
                if (Scene.Game is TestGame)
                {
                    ((TestGame)Scene.Game).EndGame();
                }
            }
        }
    }
}
