using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;
using iGL.TestGame.GameObjects;
using System.Resources;
using System.Reflection;
using System.IO;

namespace iGL.TestGame
{
    public class UIScene : Scene
    {

        public UIScene()
            : base(new PhysicsFarseer(), ShaderProgram.ProgramType.UI)
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(UIScene_OnTick);         
        }


        void UIScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            
        }

        public override void Load()
        {           
            base.Load();          
            
            var pauseButton = this.GameObjects.First(g => g.Name.ToLower().Contains("pause"));

            var buttonAnimation = new PropertyAnimationComponent();
            buttonAnimation.Property = "Scale";
            buttonAnimation.StartValue = "1,1,1";
            buttonAnimation.StopValue = "1.1,1.1,1";
            buttonAnimation.DurationSeconds = 0.2f;

            pauseButton.AddComponent(buttonAnimation);

            pauseButton.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(pauseButton_OnMouseDown);
            pauseButton.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(pauseButton_OnMouseUp);
            pauseButton.OnMouseOut += new EventHandler<Engine.Events.MouseOutEvent>(pauseButton_OnMouseOut);
        }

        void pauseButton_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            var button = sender as GameObject;
            var animComponent = button.Components.First(c => c is PropertyAnimationComponent) as PropertyAnimationComponent;

            animComponent.StartValue = button.Scale.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            animComponent.StopValue = "1,1,1";
            animComponent.Reload();
            animComponent.Play();               

            if (Game.IsPaused) Game.Continue();
            else Game.Pause();
        }

        void pauseButton_OnMouseOut(object sender, Engine.Events.MouseOutEvent e)
        {
            var button = sender as GameObject;
            var animComponent = button.Components.First(c => c is PropertyAnimationComponent) as PropertyAnimationComponent;

            animComponent.StartValue = button.Scale.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            animComponent.StopValue = "1,1,1";
            animComponent.Reload();
            animComponent.Play();        
        }

        void pauseButton_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            var button = sender as GameObject;
            var animComponent = button.Components.First(c => c is PropertyAnimationComponent) as PropertyAnimationComponent;

            animComponent.StartValue = button.Scale.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            animComponent.StopValue = "1.1,1.1,1";
            animComponent.Reload();
            animComponent.Play();
        }

    }
}
