using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Events;

namespace iGL.Engine
{
    public abstract class Scene
    {
        private List<GameObject> _gameObjects { get; set; }
        
        public CameraComponent CurrentCamera { get; private set; }
        public LightComponent CurrentLight { get; private set; }

        public Game Game { get; internal set; }
        public ShaderProgram ShaderProgram { get; internal set; }

        private event EventHandler<TickEvent> OnTickEvent;

        public Scene()
        {
            _gameObjects = new List<GameObject>();

            ShaderProgram = new PointLightShader();

            ShaderProgram.Load();
            ShaderProgram.Use();
        }

        public void Render()
        {
            if (CurrentCamera == null) return;

            /* update shader's light parameters */

            var pointLightShader = ShaderProgram as PointLightShader;
            pointLightShader.SetLight(CurrentLight.Light, new OpenTK.Vector4(CurrentLight.GameObject.Position));        

            /* load the current camera projection matrix in the shader program */
            
            foreach (var gameObject in _gameObjects)
            {
                var modelviewProjection = gameObject.Location * CurrentCamera.ModelViewProjectionMatrix;
                ShaderProgram.SetModelViewProjectionMatrix(modelviewProjection);

                gameObject.Render();
            }
            
        }

        public void Tick(double timeElapsed)
        {
            OnTickEvent(this, new TickEvent());                                 

            _gameObjects.ForEach(g => g.Tick(timeElapsed));
        }

        public event EventHandler<TickEvent> TickEvent
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
    }
}
