using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace iGL.Engine
{
    public class FixedRevoluteJointComponent : JointBaseComponent
    {
        public FixedRevoluteJointComponent(XElement xmlElement) : base(xmlElement) { }
        public FixedRevoluteJointComponent() { }

        private FixedRevoluteJoint _joint;

        public override bool InternalLoad()
        {
            var myRigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            if (!myRigidBody.IsLoaded) myRigidBody.Load();
            if (!myRigidBody.IsLoaded) return false;

            var world = GameObject.Scene.Physics.GetWorld() as World;
            if (world == null) throw new InvalidOperationException("Not a farseer physics world.");

            var worldPos = this.GameObject.WorldPosition;
            _joint = JointFactory.CreateFixedRevoluteJoint(world, myRigidBody.RigidBody, Vector2.Zero, new Vector2(worldPos.X, worldPos.Y));
            _joint.MotorEnabled = true;
            _joint.MotorSpeed = 20.0f;
            _joint.MotorTorque = 10000.0f;
            _joint.MaxMotorTorque = 100000.0f;

            return true;
        }

        public override void Tick(float timeElapsed)
        {
           
        }
    }
}
