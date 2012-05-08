using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Runtime.Serialization;
using iGL.Engine.Math;
using System.Xml.Linq;

namespace iGL.TestGame.GameObjects
{
    [Serializable]
    [RequiredComponent(typeof(PerspectiveCameraComponent), PanViewFollowCamera3d.CameraComponentId)]
    public class PanViewFollowCamera3d : GameObject
    {
        public PerspectiveCameraComponent CameraComponent { get; private set; }

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }        

        public PanViewFollowCamera3d(XElement element) : base(element) { }

        public PanViewFollowCamera3d() { }

        public bool FollowingEnabled
        {
            get;
            set;
        }

        private const string CameraComponentId = "d7ca3165-4ccb-4ee4-8db3-05dc706efeda";

        private GameObject _target;

        protected override void Init()
        {
            CameraComponent = Components.Single(c => c.Id == CameraComponentId) as PerspectiveCameraComponent;

            CameraComponent.ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);

            Position = new Vector3(0, 0, 30);
        }

        public override void Load()
        {
            base.Load();

            if (!Game.InDesignMode)
            {
                //this.Scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(Scene_OnMouseMove);
                this.Scene.OnMouseZoom += new EventHandler<Engine.Events.MouseZoomEvent>(Scene_OnMouseZoom);
            }                
        }
      
        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
            
            if (_target == null) return;

            if (FollowingEnabled)
            {
                CameraComponent.Target = Vector3.Lerp(CameraComponent.Target, _target.WorldPosition, timeElapsed * 2);             
                CameraComponent.GameObject.Position = CameraComponent.Target + new Vector3(0, 0, 30);
            }
        }

        public void Follow(GameObject target)
        {
            _target = target;           
        }     
      
        void Scene_OnMouseZoom(object sender, Engine.Events.MouseZoomEvent e)
        {
            //if (Scene.LastNearPlaneMousePosition == null || !Enabled) return;

            //float amount = -(e.Amount / 100.0f);

            //var mousePos = Scene.MousePosition.Value;
            //Vector4 nearPlane, farPlane;
            
            //Scene.ScreenPointToWorld(mousePos, out nearPlane, out farPlane);

            //CameraComponent.Width += amount;
            //CameraComponent.Height = CameraComponent.Width * 2.0f / 3.0f;
            //CameraComponent.Update();

            //Vector4 nearPlaneNew, farPlaneNew;

            //Scene.ScreenPointToWorld(mousePos, out nearPlaneNew, out farPlaneNew);

            //Position -= new Vector3(nearPlaneNew - nearPlane);
            //CameraComponent.Target -= new Vector3(nearPlaneNew - nearPlane);
        }

        void Scene_OnMouseMove(object sender, Engine.Events.MouseMoveEvent e)
        {
            if (!Enabled) return;

            //if (Scene.MouseButtonState[Engine.Events.MouseButton.Button1])
            //{
            //    Position -= e.DirectionOnNearPlane;
            //    CameraComponent.Target = Position + new Vector3(0, 0, -1);
            //}
        }
    }
}
