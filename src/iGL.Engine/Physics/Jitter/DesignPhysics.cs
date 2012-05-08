using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;

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
            CollisionSystem collision = new CollisionSystemBrute();
            _world = new Jitter.World(collision);
            _world.AllowDeactivation = true;
            _world.Gravity = new Jitter.LinearMath.JVector(0, 0, 0);

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

            Func<GameObject, bool> hasGravity = g => ((RigidBodyComponent)g.Components.Single(c => c is RigidBodyComponent)).IsGravitySource;

            var staticBodies = _world.RigidBodies.Where(rb => rb.IsStatic && hasGravity((GameObject)rb.Tag)).ToList();
            var dynamicBodies = _world.RigidBodies.Where(rb => !rb.IsStatic && rb.IsActive).ToList();

            foreach (var body in dynamicBodies)
            {
                var totalForce = new JVector();

                foreach (var staticBody in staticBodies)
                {
                    var G = (float)(6.67300 * System.Math.Pow(10, -1));
                    var r = staticBody.Position - body.Position;

                    var gravitySize = -G * (body.Mass * staticBody.Mass) / r.LengthSquared();

                    var force = (body.Position - staticBody.Position);
                    force.Normalize();

                    force = JVector.Multiply(force, gravitySize);

                    totalForce += force;
                }

                body.AddForce(totalForce);
            }
        }

        void Events_PostStep(float timestep)
        {
            //foreach (var body in _world.RigidBodies)
            //{
            //    var rBody = body as RigidBody;
            //    rBody.IsStatic = _bodyStatics[rBody];
            //    if (!rBody.IsStatic)
            //    {
            //        rBody.AngularVelocity = new Jitter.LinearMath.JVector(0, 0, 0);
            //        rBody.LinearVelocity = new Jitter.LinearMath.JVector(0, 0, 0);
            //    }
            //}
        }

        public void Step(float timeStep)
        {           
           _world.Step(timeStep, false);            
        }

        public void AddBody(object body)
        {
            var jitterBody = body as RigidBody;

            _world.AddBody(jitterBody);
            _world.Events.DeactivatedBody += Events_DeactivatedBody;

            if (!jitterBody.IsStatic)
            {
                //_world.AddConstraint(new Constraint2D(body));
            }
        }

        public void RemoveBody(object body)
        {
            var jitterBody = body as RigidBody;
            _world.Events.DeactivatedBody -= Events_DeactivatedBody;
            _world.RemoveBody(jitterBody);
        }

        void Events_DeactivatedBody(RigidBody obj)
        {
            ((GameObject)obj.Tag).OnSleepEvent(obj.Tag, new Events.SleepEvent());
        }


        public object GetWorld()
        {
            return _world;
        }
    }
}
