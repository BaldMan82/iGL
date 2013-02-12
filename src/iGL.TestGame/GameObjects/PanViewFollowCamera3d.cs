using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Runtime.Serialization;
using iGL.Engine.Math;
using System.Xml.Linq;
using System.Diagnostics;

namespace iGL.TestGame.GameObjects
{
    [Serializable]
    [RequiredComponent(typeof(PerspectiveCameraComponent), PanViewFollowCamera3d.CameraComponentId)]
    public class PanViewFollowCamera3d : GameObject
    {
        public PerspectiveCameraComponent CameraComponent { get; private set; }

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        public float LerpFactor { get; set; }

        public PanViewFollowCamera3d(XElement element) : base(element) { }

        public PanViewFollowCamera3d() { }

        public bool FollowingEnabled
        {
            get;
            set;
        }

        private const string CameraComponentId = "d7ca3165-4ccb-4ee4-8db3-05dc706efeda";

        private GameObject _target;
        private BackgroundComponent _backGroundComponent;
        private bool _gameOver;

        public Vector3 _adjustment;

        private float _distance;

        protected override void Init()
        {
            CameraComponent = Components.Single(c => c.Id == CameraComponentId) as PerspectiveCameraComponent;

            CameraComponent.ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);

            Position = new Vector3(0, 0, 30);
            _distance = 20.0f;

            LerpFactor = 2.0f;
        }

        void Scene_OnLoaded(object sender, Engine.Events.LoadedEvent e)
        {
            //var background = Scene.GameObjects.FirstOrDefault(g => g.Name.ToLower() == "background");
            //if (background != null)
            //{
            //    var meshComponent = background.Components.First(c => c is MeshComponent) as MeshComponent;

            //    LimitX = meshComponent.MaxBox.X * background.Scale.X - (float)(Math.Sin(CameraComponent.FieldOfViewRadians) * Math.Abs(background.Position.Z));
            //    LimitY = meshComponent.MaxBox.Y * background.Scale.Y - (float)(Math.Sin(CameraComponent.FieldOfViewRadians) * Math.Abs(background.Position.Z));

            //}


            _backGroundComponent = Scene.GameObjects.SelectMany(g => g.Components).FirstOrDefault(c => c is BackgroundComponent) as BackgroundComponent;
        }

        public override void Load()
        {
            Scene.OnLoaded += new EventHandler<Engine.Events.LoadedEvent>(Scene_OnLoaded);

            base.Load();

            if (!Game.InDesignMode)
            {
                //this.Scene.OnMouseMove += new EventHandler<Engine.Events.MouseMoveEvent>(Scene_OnMouseMove);
                this.Scene.OnMouseZoom += new EventHandler<Engine.Events.MouseZoomEvent>(Scene_OnMouseZoom);
            }
        }

        public override void OverrideLoadedProperties()
        {
            _gameOver = false;
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);

            if (_target == null) return;

            if (FollowingEnabled)
            {
                var lerp = timeElapsed * LerpFactor;
                if (lerp > 1) lerp = 1;
                
                var target = Vector3.Lerp(CameraComponent.Target, _target.WorldPosition + _adjustment, lerp);
             
                var position = target + new Vector3(0, 0, _distance);

                var viewBounds = Math.Sin(CameraComponent.FieldOfViewRadians) * _distance;

                if (_backGroundComponent == null ||
                    (target.X - viewBounds > _backGroundComponent.MinBounds.X && target.X + viewBounds < _backGroundComponent.MaxBounds.X &&
                    target.Y - viewBounds > _backGroundComponent.MinBounds.Y && target.Y + viewBounds < _backGroundComponent.MaxBounds.Y))
                {
                    CameraComponent.Target = target;
                    CameraComponent.GameObject.Position = position;
                }
                else if (!_gameOver)
                {
                    (Scene.PlayerObject as SlingshotBallFarseer2D).FixLightPosition();
                    
                    if (Scene.Game is TestGame)
                    {
                        ((TestGame)Scene.Game).GameOver();
                    }
                    //Scene.AddTimer(new Timer()
                    //{
                    //    Action = () =>
                    //    {
                    //        if (Scene.Game is TestGame)
                    //        {
                                
                    //        }
                    //    },
                    //    Interval = TimeSpan.FromSeconds(1),
                    //    Mode = Timer.TimerMode.Once
                    //});

                    _gameOver = true;
                }              
            }
        }

        public void Follow(GameObject target, bool smoothFollow = false)
        {
            _target = target;

            if (_target == null) return;

            if (_target is SlingshotBallFarseer2D)
            {
                _adjustment = new Vector3(0, 2, 0);
            }
            else
            {
                _adjustment = Vector3.Zero;
            }

            if (!smoothFollow)
            {
                CameraComponent.Target = _target.WorldPosition + _adjustment;
                CameraComponent.GameObject.Position = _target.WorldPosition + new Vector3(0, 0, _distance);
            }
        }

        void Scene_OnMouseZoom(object sender, Engine.Events.MouseZoomEvent e)
        {
            if (Scene.LastNearPlaneMousePosition == null || !Enabled) return;

            _distance += -e.Amount;

            if (_distance > 30) _distance = 30;
            else if (_distance < 10) _distance = 10;
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
