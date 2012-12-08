using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using iGL.Engine.Events;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;


namespace iGL.Engine
{
    public class PhysicsFarseer : PhysicsBase
    {
        private World _world;
        private event EventHandler<CollisionEvent> OnCollisionEvent;
        private CollisionEvent _collisionEvent = new CollisionEvent();
        private List<Body> _gravityBodies = new List<Body>();

        public PhysicsFarseer()
        {
            _world = new World(new Vector2(0, -35.81f));
            _world.ContactManager.PostSolve += PostSolve;

        }
        public override void Step(float timeStep)
        {
            //ProcessGravitySources();
            _world.Step(timeStep);
        }

        public void PostSolve(Contact contact, ContactConstraint impulse)
        {

        }

        private void ProcessGravitySources()
        {
            foreach (var body in _world.BodyList.Where(b => !b.IsStatic))
            {
                var totalForce = new Vector2();

                foreach (var gravityBody in _gravityBodies)
                {
                    var rigidComponent = ((GameObject)gravityBody.UserData).Components.First(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
                    var mass = rigidComponent.Mass;

                    var G = (float)(6.67300 * System.Math.Pow(10, -1));
                    var r = gravityBody.Position - body.Position;

                    /* only gravity spheres for now ... */
                    if (r.Length() >= rigidComponent.GravityRange + rigidComponent.GameObject.Scale.X) continue;

                    var gravitySize = -G * (body.Mass * mass) / r.LengthSquared();

                    var force = (body.Position - gravityBody.Position);
                    force.Normalize();

                    force = Vector2.Multiply(force, gravitySize);

                    totalForce += force;
                }
                
                body.ApplyForce(totalForce);
            }
        }

        public override void AddBody(object body)
        {
            var farseerBody = body as Body;
            farseerBody.OnCollision += OnCollision;

            var gameObject = farseerBody.UserData as GameObject;
            var rigidBodyComponent = gameObject.Components.First(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            if (rigidBodyComponent.IsGravitySource)
            {
                _gravityBodies.Add(farseerBody);
            }
        }

        public override event EventHandler<CollisionEvent> CollisionEvent
        {
            add
            {
                OnCollisionEvent += value;
            }
            remove
            {
                OnCollisionEvent -= value;
            }
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            contact.FlagForFiltering();

            if (OnCollisionEvent != null)
            {                

                _collisionEvent.ObjectA = fixtureA.Body.UserData as GameObject;
                _collisionEvent.ObjectB = fixtureB.Body.UserData as GameObject;

                OnCollisionEvent(this, _collisionEvent);
            }

            return true;
        }

        public override void RemoveBody(object body)
        {
            var farseerBody = body as Body;
            _world.RemoveBody(farseerBody);

            _gravityBodies.Remove(farseerBody);
        }

        public override object GetWorld()
        {
            return _world;
        }

        public override void Dispose()
        {            

        }
    }
}
