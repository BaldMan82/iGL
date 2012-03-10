using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Events;
using iGL.Engine.Math;
using Jitter.Dynamics;
using Jitter.LinearMath;
using Jitter.Collision;

namespace iGL.Engine
{
    public abstract class Scene
    {
        private List<GameObject> _gameObjects { get; set; }
        private List<Timer> _timers = new List<Timer>();

        public CameraComponent CurrentCamera { get; private set; }
        public LightComponent CurrentLight { get; private set; }

        public Game Game { get; internal set; }
        public ShaderProgram ShaderProgram { get; internal set; }

        internal Physics Physics { get; private set; }
        private event EventHandler<TickEvent> OnTickEvent;
        private bool _tickedOnce = false;

        private Point? _mousePosition = null;

        public Scene()
        {
            _gameObjects = new List<GameObject>();

            ShaderProgram = new PointLightShader();

            ShaderProgram.Load();
            ShaderProgram.Use();

            Physics = new Physics();
        }

        public void Render()
        {
            if (CurrentCamera == null) return;

            if (CurrentLight != null)
            {
                /* update shader's light parameters */

                var pointLightShader = ShaderProgram as PointLightShader;
                pointLightShader.SetLight(CurrentLight.Light, new Vector4(CurrentLight.GameObject.Position));
            }

            /* load the current camera projection matrix in the shader program */
            
            foreach (var gameObject in _gameObjects)
            {               
                gameObject.Render(Matrix4.Identity);
            }
            
        }

        public void Tick(float timeElapsed)
        {
            _tickedOnce = true;

            OnTickEvent(this, new TickEvent() { Elapsed = timeElapsed });
            
            try
            {
                //float step = timeElapsed;
                //if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;
                Physics.World.Step(timeElapsed, false);
            }
            catch { }

            _gameObjects.ForEach(g => g.Tick(timeElapsed));

            foreach (var timer in _timers)
            {
                if (timer.LastTick.Add(timer.Interval) < DateTime.UtcNow)
                {
                    timer.Action.Invoke();
                    timer.LastTick = DateTime.UtcNow;
                }
            }

        }

        public void AddTimer(Timer timer)
        {
            if (_timers.Contains(timer)) throw new InvalidOperationException();

            _timers.Add(timer);
        }

        public event EventHandler<TickEvent> OnTick
        {
            add
            {
                OnTickEvent += value;
            }
            remove
            {
                OnTickEvent -= value;
            }
        }

        

        public void SetCurrentCamera(GameObject camera)
        {
            if (!_gameObjects.Contains(camera)) throw new Exception("Camera is not part of this scene");           

            var component = camera.Components.FirstOrDefault(c => c is CameraComponent) as CameraComponent;

            if (component == null) throw new Exception("GameObject does not have a camera component");

            CurrentCamera = component;
        }

        public void SetCurrentLight(GameObject light)
        {
            if (!_gameObjects.Contains(light)) throw new Exception("Light is not part of this scene");

            var component = light.Components.FirstOrDefault(c => c is LightComponent) as LightComponent;

            if (component == null) throw new Exception("GameObject does not have a light component");

            CurrentLight = component;           
        }

        public abstract void Load();

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.Scene = this;
            gameObject.Load();

            _gameObjects.Add(gameObject);
        }

        private void ProcessInteractiviy()
        {
            if (_mousePosition == null) return;

            float planeX = (float)(2 * (_mousePosition.Value.X + 1) - Game.WindowSize.Width) / (float)Game.WindowSize.Width;
            float planeY = (float)(Game.WindowSize.Height - 2 * (_mousePosition.Value.Y + 1)) / (float)Game.WindowSize.Height;

            var cam = CurrentCamera;

            var pmv = cam.ModelViewMatrix * cam.ProjectionMatrix;
            pmv.Invert();

            var nearPlane = Vector4.Transform(new Vector4(planeX, planeY, -1, 1), pmv);
            var farPlane = Vector4.Transform(new Vector4(planeX, planeY, 1, 1), pmv);

            nearPlane.W = 1.0f / nearPlane.W;
            nearPlane.X *= nearPlane.W;
            nearPlane.Y *= nearPlane.W;
            nearPlane.Z *= nearPlane.W;

            farPlane.W = 1.0f / farPlane.W;
            farPlane.X *= farPlane.W;
            farPlane.Y *= farPlane.W;
            farPlane.Z *= farPlane.W;

            var ray = new Vector4(farPlane - nearPlane);         
        }

        internal void MouseMove(int x, int y)
        {
            //_mousePosition = new Point(x, y);

                              
            
            //RigidBody body;
            //JVector normal;
            //float fraction;          

            //Physics.World.CollisionSystem.Raycast(nearPlane.ToJitter(), ray.ToJitter(), RaycastCallback, out body, out normal, out fraction);

            //var worldIn = new BulletXNA.LinearMath.Vector3(nearPlane.X, nearPlane.Y, nearPlane.Z);
            //var worldOut = new BulletXNA.LinearMath.Vector3(farPlane.X, farPlane.Y, farPlane.Z);

            //ClosestRayResultCallback resultCallback = new ClosestRayResultCallback(worldIn, worldOut);
            //Physics.World.RayTest(ref worldIn, ref worldOut, resultCallback);

            //if (resultCallback.HasHit)
            //{
            //    var gameObject = _gameObjects.Single(g => g.Components.Contains(resultCallback.m_collisionObject.UserObject));

            //    var rigidBody = gameObject.Components.First(c => c is RigidBodyComponent) as RigidBodyComponent;
            //    rigidBody.RigidBody.Activate();

            //    var force = new BulletXNA.LinearMath.Vector3(ray.X * 10.0f, ray.Y * 10.0f, ray.Z * 10.0f) * 10000.0f;
            //    var pos = new BulletXNA.LinearMath.Vector3();
            //    rigidBody.RigidBody.ApplyForce(ref force, ref pos); 
            //}
        }

        private bool RaycastCallback(RigidBody body, JVector normal, float fraction)
        {
            if (body.Shape.Tag != null && body.IsStatic == false)
            {
                int a = 0;
            }
            return false;
        }
    }
}
