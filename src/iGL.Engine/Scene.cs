using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Events;
using iGL.Engine.Math;
using BulletXNA.BulletCollision;

namespace iGL.Engine
{
    public abstract class Scene
    {
        private List<GameObject> _gameObjects { get; set; }
        
        public CameraComponent CurrentCamera { get; private set; }
        public LightComponent CurrentLight { get; private set; }

        public Game Game { get; internal set; }
        public ShaderProgram ShaderProgram { get; internal set; }

        internal Physics Physics { get; private set; }
        private event EventHandler<TickEvent> OnTickEvent;

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
            OnTickEvent(this, new TickEvent() { Elapsed = timeElapsed });
            
            try
            {
                Physics.World.StepSimulation(timeElapsed, 1);
            }
            catch { }

            _gameObjects.ForEach(g => g.Tick(timeElapsed));
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

        public void MouseMove(float x, float y)
        {           
            var cam = CurrentCamera;

            var pmv = cam.ModelViewMatrix * cam.ProjectionMatrix;
            pmv.Invert();

            var nearPlane = Vector4.Transform(new Vector4(x, y, -1, 1), pmv);
            var farPlane = Vector4.Transform(new Vector4(x, y, 1, 1), pmv);

            nearPlane.W = 1.0f / nearPlane.W;
            nearPlane.X *= nearPlane.W;
            nearPlane.Y *= nearPlane.W;
            nearPlane.Z *= nearPlane.W;

            farPlane.W = 1.0f / farPlane.W;
            farPlane.X *= farPlane.W;
            farPlane.Y *= farPlane.W;
            farPlane.Z *= farPlane.W;

            var ray = new Vector4(farPlane - nearPlane);
            ray.Normalize();           

            var worldIn = new BulletXNA.LinearMath.Vector3(nearPlane.X, nearPlane.Y, nearPlane.Z);
            var worldOut = new BulletXNA.LinearMath.Vector3(farPlane.X, farPlane.Y, farPlane.Z);

            ClosestRayResultCallback resultCallback = new ClosestRayResultCallback(worldIn, worldOut);
            Physics.World.RayTest(ref worldIn, ref worldOut, resultCallback);

            if (resultCallback.HasHit)
            {
                var gameObject = _gameObjects.Single(g => g.Components.Contains(resultCallback.m_collisionObject.UserObject));

                var rigidBody = gameObject.Components.First(c => c is RigidBodyComponent) as RigidBodyComponent;
                rigidBody.RigidBody.Activate();

                var force = new BulletXNA.LinearMath.Vector3(ray.X * 10.0f, ray.Y * 10.0f, ray.Z * 10.0f) * 10000.0f;
                var pos = new BulletXNA.LinearMath.Vector3();
                rigidBody.RigidBody.ApplyForce(ref force, ref pos); 
            }
        }
    }
}
