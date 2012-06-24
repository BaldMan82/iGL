using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;
using iGL.Engine.Events;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Threading;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using iGL.Engine.Triggers;

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
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);           
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
            lock (typeof(Game))
            {
                Scene.Render();
            }
        }

        public void Tick(float timeElapsed, bool tickPhysics = true)
        {
            lock (typeof(Game))
            {
                Scene.Tick(timeElapsed, tickPhysics);
            }
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

        public string SaveScene()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            using (var writer = new StringWriter())
            {
                XDocument doc = new XDocument();
                XElement element = new XElement("Scene");

                element.Add(new XElement("PlayCameraId", Scene.PlayCamera != null ? Scene.PlayCamera.GameObject.Id : string.Empty));
                element.Add(new XElement("DesignCameraId", Scene.DesignCamera != null ? Scene.DesignCamera.GameObject.Id : string.Empty));
                element.Add(new XElement("LightId", Scene.CurrentLight != null ? Scene.CurrentLight.GameObject.Id : string.Empty));
                element.Add(XmlHelper.ToXml(Scene.AmbientColor, "AmbientColor"));

                element.Add(new XElement("Objects", Scene.GameObjects.Where(g => !g.Designer && g.CreationMode != GameObject.CreationModeEnum.Runtime).Select(go => XmlHelper.ToXml(go, "GameObject"))));
                element.Add(new XElement("Resources", Scene.Resources.Select(res => XmlHelper.ToXml(res, "Resource"))));
                element.Add(new XElement("Triggers", Scene.Triggers.Select(trigger => XmlHelper.ToXml(trigger, "Trigger"))));

                doc.Add(element);
                 
                doc.Save(writer);

                return writer.ToString();
            }           
        }

        public void LoadScene(string xml)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            using (var reader = new StringReader(xml))
            {
                XDocument doc = XDocument.Load(reader);
                if (doc.Root.Name == "Scene")
                {
                    var objects = doc.Root.Elements().FirstOrDefault(e => e.Name == "Objects");
                    if (objects != null)
                    {
                        foreach (var element in objects.Elements())
                        {
                            /* deserialize game objects */
                            var gameObject = XmlHelper.FromXml(element, typeof(GameObject)) as GameObject;

                            Scene.AddGameObject(gameObject);
                        }
                    }

                    var resources = doc.Root.Elements().FirstOrDefault(e => e.Name == "Resources");
                    if (resources != null)
                    {
                        foreach (var resource in resources.Elements())
                        {
                            Scene.AddResource(XmlHelper.FromXml(resource, typeof(Resource)) as Resource);
                        }
                    }

                    var triggers = doc.Root.Elements().FirstOrDefault(e => e.Name == "Triggers");
                    if (triggers != null)
                    {
                        foreach (var trigger in triggers.Elements())
                        {
                            Scene.AddTrigger(XmlHelper.FromXml(trigger, typeof(Trigger)) as Trigger);
                        }
                    }

                    var currentCam = doc.Root.Elements().FirstOrDefault(e => e.Name == "PlayCameraId");
                    if (currentCam != null && !string.IsNullOrEmpty(currentCam.Value))
                    {
                        Scene.SetPlayCamera(Scene.GameObjects.Single(g => g.Id == currentCam.Value));
                    }
                    else
                    {
                        Scene.SetPlayCamera(null);
                    }

                    currentCam = doc.Root.Elements().FirstOrDefault(e => e.Name == "DesignCameraId");
                    if (currentCam != null && !string.IsNullOrEmpty(currentCam.Value))
                    {
                        Scene.SetDesignCamera(Scene.GameObjects.Single(g => g.Id == currentCam.Value));
                    }
                    else
                    {
                        Scene.SetDesignCamera(null);
                    }

                    var currentLight = doc.Root.Elements().FirstOrDefault(e => e.Name == "LightId");
                    if (currentLight != null && !string.IsNullOrEmpty(currentLight.Value))
                    {
                        Scene.SetCurrentLight(Scene.GameObjects.Single(g => g.Id == currentLight.Value));
                    }
                    else
                    {
                        Scene.SetCurrentLight(null);
                    }

                    var ambient = doc.Root.Elements().FirstOrDefault(e => e.Name == "AmbientColor");
                    Scene.AmbientColor = (Vector4)XmlHelper.FromXml(ambient, typeof(Vector4));
                }

            }

            Scene.Load();
            
        }
    }
}
