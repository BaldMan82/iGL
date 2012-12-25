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
    public class FixedRevoluteJointComponent : JointBaseFarseerComponent
    {
        public FixedRevoluteJointComponent(XElement xmlElement) : base(xmlElement) { }
        public FixedRevoluteJointComponent() { }

        public bool MotorEnabled { get; set; }
        public float MotorSpeed { get; set; }
        public float MotorTorque { get; set; }
        public float MaxMotorTorque { get; set; }

        public override bool InternalLoad()
        {
            var myRigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            if (!myRigidBody.IsLoaded) myRigidBody.Load();
            if (!myRigidBody.IsLoaded) return false;

            var world = GameObject.Scene.Physics.GetWorld() as World;
            if (world == null) throw new InvalidOperationException("Not a farseer physics world.");

            var worldPos = this.GameObject.WorldPosition;
            Joint = JointFactory.CreateFixedRevoluteJoint(world, myRigidBody.RigidBody, Vector2.Zero, new Vector2(worldPos.X, worldPos.Y));

            UpdateMotorProperties();

            return true;
        }

        public void UpdateMotorProperties()
        {           
            var revoluteJoint = Joint as FixedRevoluteJoint;
            if (revoluteJoint == null) return;

            revoluteJoint.MotorEnabled = MotorEnabled;
            revoluteJoint.MotorSpeed = MotorSpeed;
            revoluteJoint.MotorTorque = MotorTorque;
            revoluteJoint.MaxMotorTorque = MaxMotorTorque;
        }

        public override void Tick(float timeElapsed)
        {
           
        }
    }
}
