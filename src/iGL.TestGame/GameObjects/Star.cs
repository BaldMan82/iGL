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
	[RequiredComponent(typeof(PropertyAnimationComponent), RotationAnimationComponentId)]
	[RequiredComponent(typeof(PropertyAnimationComponent), AlphaAnimationComponentId)]
    public class Star : GameObject
    {
        public Star(XElement element) : base(element) { }

        public Star() { }

        private const string MeshComponentId = "b2da1016-2ff7-333f-aed1-0afd3db7b0bf";
        private const string MeshRenderComponentId = "a4af2307-be09-453b-a8ab-54bad0d51525";
		private const string RigidBodyFarseerComponentId = "f3dda926-cc99-4c6a-b4b6-d02b47a049fc";
		private const string BoxColliderFarseerComponentId = "3a728731-0f1e-4661-969f-f8b3d4cac27f";
		private const string RotationAnimationComponentId = "6b728731-0f1e-4661-969f-f8b3d4cac27f";
		private const string AlphaAnimationComponentId = "1c728731-0f1e-4661-969f-f8b3d4cac27f";

		private MeshComponent _meshComponent;
		private PropertyAnimationComponent _rotationAnimationComponent;
		private PropertyAnimationComponent _alphaAnimationComponent;
        private bool _grabbed = false;

		public Vector4 StarColor
		{
			get
			{
				return _meshComponent.Material.Ambient;
			}
			set
			{
				_meshComponent.Material.Ambient = value;
			}
		}

        protected override void Init()
        {
            base.Init();

            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
			_meshComponent.MeshResourceName = "star";
			_meshComponent.Material.TextureName = "star";

            this.Scale = new Vector3(0.05f, 0.05f, 0.05f);

            var body = Components.Single(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            body.IsSensor = true;

            this.OnObjectCollision += Star_OnObjectCollision;


        }

		public override void Load ()
		{
			/* should be set by engine, so reset properties works as expected */
			_alphaAnimationComponent = Components.First(c => c.Id == AlphaAnimationComponentId) as PropertyAnimationComponent;
			_alphaAnimationComponent.Property = "StarColor";
			_alphaAnimationComponent.StartValue = StarColor.ToString().Replace("(", string.Empty).Replace(")", string.Empty);

			var stopColor = new Vector4(StarColor.X, StarColor.Y, StarColor.Z, 0);

			_alphaAnimationComponent.StopValue = stopColor.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
			_alphaAnimationComponent.DurationSeconds = 0.5f;

		
			_rotationAnimationComponent = Components.First(c => c.Id == RotationAnimationComponentId) as PropertyAnimationComponent;
			_rotationAnimationComponent.Property = "Rotation";
			_rotationAnimationComponent.StartValue = new Vector3(0,0,this.Rotation.Z).ToString().Replace("(", string.Empty).Replace(")", string.Empty);
            _rotationAnimationComponent.StopValue = new Vector3(0, 6.28f / 2f, this.Rotation.Z).ToString().Replace("(", string.Empty).Replace(")", string.Empty);
			_rotationAnimationComponent.DurationSeconds = 0.5f;		

			base.Load();
		} 

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }

        void Star_OnObjectCollision(object sender, Engine.Events.ObjectCollisionEvent e)
        {
			if (_grabbed) return;

            _grabbed = true;

            _alphaAnimationComponent.Play();
            _rotationAnimationComponent.Play();

            Scene.AddTimer(new Timer() { Action = () => Scene.DisposeGameObject(this), Interval = TimeSpan.FromSeconds(1), Mode = Timer.TimerMode.Once });
            
			//if (e.Object is SlingshotBallFarseer3D)
            {
                //Scene.DisposeGameObject(this);

                var flare = new StarFlare();

                flare.Position = this.Position;
                Scene.AddGameObject(flare);

                flare.PlayAnimation();

                Scene.AddTimer(new Timer() { Action = () => Scene.DisposeGameObject(flare), Interval = TimeSpan.FromSeconds(0.2), Mode = Timer.TimerMode.Once });

            }
        }
    }
}
