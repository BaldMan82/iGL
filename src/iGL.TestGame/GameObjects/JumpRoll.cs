using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;
using iGL.Engine;

namespace iGL.TestGame.GameObjects
{
    [Serializable]
    [RequiredComponent(typeof(MeshComponent), JumpRoll.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), JumpRoll.MeshRenderComponentId)]
    [RequiredComponent(typeof(RigidBodyFarseerComponent), JumpRoll.RigidBodyFarseerComponentId)]
    [RequiredComponent(typeof(CircleColliderFarseerComponent), JumpRoll.CircleColliderFarseerComponentId)]
    [RequiredComponent(typeof(FixedRevoluteJointComponent), JumpRoll.FixedRevoluteJointComponentId)]
    public class JumpRoll : GameObject
    {
        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        public float Height
        {
            get { return Scale.Y; }
        }
        public float Width
        {
            get { return Scale.X; }
        }
        public float Depth
        {
            get { return Scale.Z; }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;
        private new RigidBodyFarseerComponent _rigidBodyComponent;
        private CircleColliderFarseerComponent _circleColliderComponent;
        private FixedRevoluteJointComponent _fixedRevoluteJoinComponent;

        private const string MeshComponentId = "c0bfb81f-fbf1-42e2-a970-02f2eb798df8";
        private const string MeshRenderComponentId = "a8967a9d-8c90-41fa-9344-dec053def5b0";
        private const string RigidBodyFarseerComponentId = "e59ebd40-28f0-4d3b-95cf-e9811570f514";
        private const string CircleColliderFarseerComponentId = "b9bbe6d6-0108-48f2-9e83-6fd4760040fd";
        private const string FixedRevoluteJointComponentId = "e8d04b4e-9362-461f-b8d4-c61fcb925c53";
        
        private bool _shouldWeld = false;
        public JumpRoll(XElement element) : base(element) { }

        public JumpRoll() { }

        protected override void Init()
        {
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;
            _rigidBodyComponent = Components.Single(c => c.Id == RigidBodyFarseerComponentId) as RigidBodyFarseerComponent;
            _circleColliderComponent = Components.Single(c => c.Id == CircleColliderFarseerComponentId) as CircleColliderFarseerComponent;
            _fixedRevoluteJoinComponent = Components.Single(c => c.Id == FixedRevoluteJointComponentId) as FixedRevoluteJointComponent;

            _meshComponent.MeshResourceName = "cylinder";
            _meshComponent.Material.TextureName = "fabric";
         
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(_meshComponent.MeshResourceName))
            {
                _meshComponent.MeshResourceName = "cylinder";
            }

            this.OnObjectCollision += new EventHandler<Engine.Events.ObjectCollisionEvent>(JumpRoll_OnObjectCollision);

            base.Load();          
        }

        void JumpRoll_OnObjectCollision(object sender, Engine.Events.ObjectCollisionEvent e)
        {
            var ball = e.Object as SlingshotBallFarseer2D;

            if (ball != null && ball.CurrentState != SlingshotBallFarseer2D.State.AttachedToJumpRoll && 
                                ball.CurrentState != SlingshotBallFarseer2D.State.DetachingFromJumpRoll)
            {
                _shouldWeld = true;

                var collider = ball.Components.First(c => c is CircleColliderFarseerComponent) as CircleColliderFarseerComponent;
                var direction = ball.WorldPosition - this.WorldPosition;
                direction.Normalize();

                ball.Position = this.WorldPosition + direction * (_circleColliderComponent.Radius + collider.Radius);
            }
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);

            if (_shouldWeld)
            {
                var ball = Scene.PlayerObject as SlingshotBallFarseer2D;

                if (ball != null && ball.CurrentState != SlingshotBallFarseer2D.State.AttachedToJumpRoll &&
                                  ball.CurrentState != SlingshotBallFarseer2D.State.DetachingFromJumpRoll)
                {
                    var distanceJoint = new WeldJointFarseerComponent();
                    distanceJoint.OtherObjectId = this.Id;

                    Scene.PlayerObject.AddComponent(distanceJoint);

                    var cam = Scene.CurrentCamera.GameObject as PanViewFollowCamera3d;
                    cam.Follow(this, true);

                    ball.CurrentState = SlingshotBallFarseer2D.State.AttachedToJumpRoll;
                }

                _shouldWeld = false;
            }

           
        }
    }
}
