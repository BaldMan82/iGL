﻿using System;
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
    public class WeldJointFarseerComponent : JointBaseFarseerComponent
    {
        public string OtherObjectId { get; set; }

        private WeldJoint _joint;

        public WeldJointFarseerComponent(XElement xmlElement) : base(xmlElement) { }
        public WeldJointFarseerComponent() { }

        private bool LoadJoint()
        {
            var otherObject = GameObject.Scene.GameObjects.FirstOrDefault(g => g.Id == OtherObjectId) as GameObject;
            if (otherObject == null) return false;

            var otherRigidBody = otherObject.Components.FirstOrDefault(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            if (otherRigidBody == null) return false;

            if (!otherObject.IsLoaded) otherObject.Load();
            if (!otherRigidBody.IsLoaded) return false;

            var myRigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            if (!myRigidBody.IsLoaded) myRigidBody.Load();
            if (!myRigidBody.IsLoaded) return false;

            var world = GameObject.Scene.Physics.GetWorld() as World;
            if (world == null) throw new InvalidOperationException("Not a farseer physics world.");

            _joint = JointFactory.CreateWeldJoint(world, myRigidBody.RigidBody, otherRigidBody.RigidBody, Vector2.Zero);
                          
            //world.AddJoint(_joint);
           
            return true;
        }

        public override bool InternalLoad()
        {
            return LoadJoint();
        }

        public void Reload()
        {
            var world = GameObject.Scene.Physics.GetWorld() as World;
            world.RemoveJoint(_joint);

            LoadJoint();
        }


        public override void Dispose()
        {
            base.Dispose();

            var world = GameObject.Scene.Physics.GetWorld() as World;
            world.RemoveJoint(_joint);
        }
        public override void Tick(float timeElapsed)
        {
            
        }
    }
}
