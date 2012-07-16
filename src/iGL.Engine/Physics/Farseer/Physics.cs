using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using iGL.Engine.Events;


namespace iGL.Engine
{
    public class PhysicsFarseer : PhysicsBase
    {
        private World _world;
        private event EventHandler<CollisionEvent> OnCollisionEvent;
        private CollisionEvent _collisionEvent = new CollisionEvent();

        public PhysicsFarseer()
        {
            _world = new World(new Vector2(0, -53f));

        }
        public override void Step(float timeStep)
        {
            _world.Step(timeStep);
        }

        public override void AddBody(object body)
        {
            var farseerBody = body as Body;
            farseerBody.OnCollision += OnCollision;            
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
            _world.RemoveBody(body as Body);
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
