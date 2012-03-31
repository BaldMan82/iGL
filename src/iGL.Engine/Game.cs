using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;
using iGL.Engine.Events;
using Newtonsoft.Json;

namespace iGL.Engine
{
    public abstract class Game
    {
        public Scene Scene { get; private set; }
        public Size WindowSize { get; private set; }        
        public static IGL GL { get; private set; }
        public static bool InDesignMode { get; set; }

        public Game(IGL gl)
        {
            GL = gl;
        }

        public void Resize(int width, int height)
        {
            WindowSize = new Size(width, height);
            GL.Viewport(0, 0, width, height);
        }

        public virtual void Load()
        {          
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2d);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.SrcColor);
        }

        public void MouseMove(int x, int y)
        {
            Scene.MouseMove(x, y);           
        }

        public void MouseButton(MouseButton button, bool down, int x, int y)
        {
            Scene.UpdateMouseButton(button,down, x, y);
        }

        public void MouseZoom(int amount)
        {
            Scene.MouseZoom(amount);
        }

        public void Render()
        {           
            Scene.Render();
        }

        public void Tick(float timeElapsed, bool tickPhysics = true)
        {
            Scene.Tick(timeElapsed, tickPhysics);
        }

        public void SetScene(Scene scene)
        {
            scene.Game = this;
            Scene = scene;
        }

        public void LoadScene()
        {
            Scene.Load();
        }

        public void LoadFromJson(string json)
        {
            if (Scene == null) throw new InvalidCastException("Must load into a scene");

            var obj = JsonConvert.DeserializeObject<SceneSerializer>(json, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });

            foreach (var gameObject in obj.GameObjects)
            {
                Scene.AddGameObject(gameObject);
            }

            if (!string.IsNullOrEmpty(obj.CurrentCameraId))
            {
                Scene.SetCurrentCamera(obj.GameObjects.FirstOrDefault(g => g.Id == obj.CurrentCameraId));
            }

            if (!string.IsNullOrEmpty(obj.CurrentLightId))
            {
                Scene.SetCurrentLight(obj.GameObjects.FirstOrDefault(g => g.Id == obj.CurrentLightId));
            }

            Scene.AmbientColor = obj.AmbientColor;
        }

        public string SaveSceneToJson()
        {
            var sceneSerializer = new SceneSerializer();

            if (Scene.CurrentCamera != null) sceneSerializer.CurrentCameraId = Scene.CurrentCamera.GameObject.Id;
            if (Scene.CurrentLight != null) sceneSerializer.CurrentLightId = Scene.CurrentLight.GameObject.Id;

            sceneSerializer.AmbientColor = Scene.AmbientColor;

            sceneSerializer.GameObjects = Scene.GameObjects.Where(g => !g.Designer);

            string json = JsonConvert.SerializeObject(sceneSerializer,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Objects
                });

            return json;                    
        }

    }
}
