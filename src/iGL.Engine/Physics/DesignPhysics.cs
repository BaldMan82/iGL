using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision;
using Jitter.Dynamics;

namespace iGL.Engine
{
    public class DesignPhysics : IPhysics
    {
        public Jitter.World _world;
        public GameObject SelectedObject { get; set; }
        internal event EventHandler<EventArgs> _collisionEvent;
        private Dictionary<RigidBody, bool> _bodyStatics = new Dictionary<RigidBody, bool>();

        public bool HadCollision { get; private set; }

        public DesignPhysics()
        {
            CollisionSystem collision = new CollisionSystemPersistentSAP();
            _world = new Jitter.World(collision);
            _world.AllowDeactivation = true;
            _world.Gravity = new Jitter.LinearMath.JVector(0, -0.001f, 0);

            _world.Events.PreStep += new Jitter.World.WorldStep(Events_PreStep);
            _world.Events.PostStep += new Jitter.World.WorldStep(Events_PostStep);
            _world.Events.BodiesBeginCollide += new Action<RigidBody, RigidBody>(Events_BodiesBeginCollide);
        }

        void Events_BodiesBeginCollide(RigidBody arg1, RigidBody arg2)
        {
            HadCollision = true;
            if (_collisionEvent != null) _collisionEvent(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> OnCollision
        {
            add
            {
                _collisionEvent += value;
            }
            remove
            {
                _collisionEvent -= value;
            }

        }

        void Events_PreStep(float timestep)
        {
            HadCollision = false;
            RigidBody selectedBody = null;

            if (SelectedObject != null)
            {
                var component = SelectedObject.Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;
                if (component != null) selectedBody = component.RigidBody;
            }

            _bodyStatics.Clear();

            foreach (var body in _world.RigidBodies)
            {
                var rBody = body as RigidBody;
                _bodyStatics.Add(rBody, rBody.IsStatic);

                if (rBody != selectedBody)
                {
                    rBody.IsStatic = true;
                }
                else
                {
                    rBody.IsActive = true;
                    rBody.IsStatic = false;

                }
            }
        }

        void Events_PostStep(float timestep)
        {
            foreach (var body in _world.RigidBodies)
            {
                var rBody = body as RigidBody;
                rBody.IsStatic = _bodyStatics[rBody];
                if (!rBody.IsStatic)
                {
                    rBody.AngularVelocity = new Jitter.LinearMath.JVector(0, 0, 0);
                    rBody.LinearVelocity = new Jitter.LinearMath.JVector(0, 0, 0);
                }
            }
        }

        public void Step(float timeStep)
        {
            _world.Step(timeStep, false);
        }

        public void AddBody(Jitter.Dynamics.RigidBody body)
        {
            _world.AddBody(body);
        }


        public void RemoveBody(RigidBody body)
        {
            _world.RemoveBody(body);
        }
    }
}
