using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Events;
using iGL.Engine.Math;
using Jitter.Dynamics;
using Jitter.LinearMath;
using Jitter.Collision;
using System.Diagnostics;

namespace iGL.Engine
{
    public class Scene
    {
        public CameraComponent CurrentCamera { get; private set; }
        public LightComponent CurrentLight { get; private set; }
        public GameObject LastMouseDownTarget { get { return _currentMouseDownObj; } }
        public IEnumerable<GameObject> GameObjects { get { return _gameObjects.AsEnumerable(); } }
        public Vector4? LastNearPlaneMousePosition { get; private set; }
        public Game Game { get; internal set; }
        public bool Loaded { get; internal set; }

        private TimeSpan _mouseUpdateRate = TimeSpan.FromSeconds(1.0 / 50.0);
        private DateTime _lastMouseUpdate = DateTime.MinValue;
        private List<GameObject> _gameObjects { get; set; }
        private List<Timer> _timers = new List<Timer>();
        private Point? _mousePosition = null;
        private GameObject _currentMouseOverObj = null;
        private GameObject _currentMouseDownObj = null;
        
        private Vector4 _ambientColor;

        private event EventHandler<TickEvent> OnTickEvent;
        private event EventHandler<MouseMoveEvent> OnMouseMoveEvent;
                       
        internal ShaderProgram ShaderProgram { get; set; }
        public IPhysics Physics { get; private set; }

        public Dictionary<MouseButton, bool> MouseButtonState { get; private set; }        

        public Vector4 AmbientColor
        {
            get
            {
                return _ambientColor;
            }
            set
            {
                ShaderProgram.SetAmbientColor(_ambientColor);
                _ambientColor = value;
            }
        }

        public Scene(IPhysics physics)
        {
            _gameObjects = new List<GameObject>();

            ShaderProgram = new PointLightShader();

            ShaderProgram.Load();
            ShaderProgram.Use();

            Physics = new Physics2d();

            MouseButtonState = new Dictionary<MouseButton, bool>();
            
            MouseButtonState.Add(MouseButton.Button1, false);
            MouseButtonState.Add(MouseButton.Button2, false);
            MouseButtonState.Add(MouseButton.ButtonMiddle, false);

            Physics = physics;
        }

        public void Render()
        {            
            if (CurrentCamera == null) return;

            Game.GL.ClearColor(CurrentCamera.ClearColor.X, CurrentCamera.ClearColor.Y, CurrentCamera.ClearColor.Z, CurrentCamera.ClearColor.W);

            Game.GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

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
            if (OnTickEvent != null) OnTickEvent(this, new TickEvent() { Elapsed = timeElapsed });

            try
            {
                float step = timeElapsed;
                if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;

                Physics.Step(timeElapsed);
            }
            catch { }

            _gameObjects.ForEach(g => g.Tick(timeElapsed));

            var obsoleteTimers = new List<Timer>();

            foreach (var timer in _timers)
            {
                if (timer.LastTick.Add(timer.Interval) < DateTime.UtcNow)
                {
                    timer.Action.Invoke();
                    timer.LastTick = DateTime.UtcNow;
                    if (timer.Mode == Timer.TimerMode.Once)
                    {
                        obsoleteTimers.Add(timer);
                    }
                }
            }

            foreach (var obsoleteTimer in obsoleteTimers)
            {
                _timers.Remove(obsoleteTimer);
            }

            if (DateTime.UtcNow > _lastMouseUpdate.Add(_mouseUpdateRate))
            {
                /* update mouse position */

                ProcessInteractiviy();

                _lastMouseUpdate = DateTime.UtcNow;
            }
        }

        public void AddTimer(Timer timer)
        {
            if (_timers.Contains(timer)) throw new InvalidOperationException();
            
            timer.LastTick = DateTime.UtcNow;

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

        public event EventHandler<MouseMoveEvent> OnMouseMove
        {
            add
            {
                OnMouseMoveEvent += value;
            }
            remove
            {
                OnMouseMoveEvent -= value;
            }
        }

        public void SetCurrentCamera(GameObject camera)
        {
            if (camera == null) CurrentCamera = null;

            if (!_gameObjects.Contains(camera)) throw new Exception("Camera is not part of this scene");

            var component = camera.Components.FirstOrDefault(c => c is CameraComponent) as CameraComponent;

            if (component == null) throw new Exception("GameObject does not have a camera component");

            CurrentCamera = component;
        }

        public void SetCurrentLight(GameObject light)
        {
            if (light == null) CurrentLight = null;

            if (!_gameObjects.Contains(light)) throw new Exception("Light is not part of this scene");

            var component = light.Components.FirstOrDefault(c => c is LightComponent) as LightComponent;

            if (component == null) throw new Exception("GameObject does not have a light component");

            CurrentLight = component;
        }

        public virtual void Load() 
        {
            Loaded = true;
            _gameObjects.ForEach(g => g.Load());
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject.IsLoaded) throw new InvalidOperationException("GameObject cannot be loaded");

            gameObject.Scene = this;           

            _gameObjects.Add(gameObject);     
      
            /* order game object list according to zbuffer enable/disable */

            _gameObjects = _gameObjects.OrderByDescending(g => g.RenderQueuePriority).ToList();

            if (Loaded) gameObject.Load();
        }

        public void ScreenPointToWorld(Point screenPoint, out Vector4 nearPlane, out Vector4 farPlane)
        {
            float planeX = (float)(2 * (screenPoint.X + 1) - Game.WindowSize.Width) / (float)Game.WindowSize.Width;
            float planeY = (float)(Game.WindowSize.Height - 2 * (screenPoint.Y + 1)) / (float)Game.WindowSize.Height;

            Matrix4 projectionMatrix = Matrix4.Identity;

            if (CurrentCamera != null)
            {             
                var cam = CurrentCamera;

                projectionMatrix = cam.ModelViewMatrix * cam.ProjectionMatrix;
                projectionMatrix.Invert();
            }

            nearPlane = Vector4.Transform(new Vector4(planeX, planeY, -1, 1), projectionMatrix);
            farPlane = Vector4.Transform(new Vector4(planeX, planeY, 1, 1), projectionMatrix);

            nearPlane.W = 1.0f / nearPlane.W;
            nearPlane.X *= nearPlane.W;
            nearPlane.Y *= nearPlane.W;
            nearPlane.Z *= nearPlane.W;

            farPlane.W = 1.0f / farPlane.W;
            farPlane.X *= farPlane.W;
            farPlane.Y *= farPlane.W;
            farPlane.Z *= farPlane.W;
        }

        private void ProcessInteractiviy()
        {
            if (_mousePosition == null) return;

            Vector4 nearPlane, farPlane;
            ScreenPointToWorld(_mousePosition.Value, out nearPlane, out farPlane);

            /* hold a reference to the nearplane vector in order to calculate mouse input directional vector */
            if (LastNearPlaneMousePosition != null)
            {
                var direction = nearPlane - LastNearPlaneMousePosition.Value;
                if (direction.LengthSquared != 0)
                {                   
                    if (OnMouseMoveEvent != null)
                    {
                        OnMouseMoveEvent(this, new MouseMoveEvent()
                        {
                            DirectionOnNearPlane = new Vector3(direction.X, direction.Y, direction.Z),
                            NearPlane = new Vector3(nearPlane.X, nearPlane.Y, nearPlane.Z)
                        });
                    }
                }
            }

            UpdateCurrentMousePosition();

            var ray = new Vector4(farPlane - nearPlane);

            //RigidBody body;
            //JVector normal;
            //float fraction;

            //Physics.World.CollisionSystem.Raycast(nearPlane.ToJitter(), ray.ToJitter(), (a, b, c) => true, out body, out normal, out fraction);

            //GameObject result = null;
            //if (body != null) result = body.Tag as GameObject;


            /* raycast through non-rigidbodies */

            GameObject result = RayCast(nearPlane, ray);

            /* call events on raycast results */

            if (result != null)
            {               
                /* target changed */
                if (_currentMouseOverObj != null && _currentMouseOverObj != result)
                {
                    _currentMouseOverObj.OnMouseOutEvent(new MouseOutEvent());
                    result.OnMouseInEvent(new MouseInEvent());

                }
                else if (_currentMouseOverObj == null)
                {
                    result.OnMouseInEvent(new MouseInEvent());
                }

                _currentMouseOverObj = result;
            }
            else if (_currentMouseOverObj != null)
            {
                Console.WriteLine("...");

                _currentMouseOverObj.OnMouseOutEvent(new MouseOutEvent());
                _currentMouseOverObj = null;
            }            
            
        }

        public GameObject RayCast(Vector4 nearPlane, Vector4 ray)
        {
            float minDistance = float.MaxValue;
            GameObject result = null;

            var near = new Vector3(nearPlane);
            var dir = new Vector3(ray);

            foreach (var gameObject in _gameObjects)
            {
                var rayHit = gameObject.RayTest(near, dir);

                if (rayHit != null)
                {
                    var transform = rayHit.GetCompositeTransform();
                    var center = new Vector3(transform.M41, transform.M42, transform.M43);
                    var distance = (center - near).LengthSquared;

                    if (distance < minDistance || gameObject is Gizmo)
                    {
                        result = rayHit;
                        minDistance = distance;

                        if (gameObject is Gizmo) break;
                    }

                    Console.WriteLine("Hit");
                }
            }
            return result;
        }

        internal void UpdateMouseButton(MouseButton button, bool down, int x, int y)
        {
            MouseButtonState[button] = down;

            _mousePosition = new Point(x, y);
            ProcessInteractiviy();

            /* selected target is now updated, process button event */

            if (down)
            {
                if (_currentMouseOverObj != null)
                {
                    _currentMouseOverObj.OnMouseDownEvent(new MouseButtonDownEvent() { Button = button });
                }

                /* can be null */
                _currentMouseDownObj = _currentMouseOverObj;
            }
            else
            {
                /* send a mouse up event to the last target, even if it is not the current hover target anymore */
                if (_currentMouseDownObj != null && _currentMouseDownObj != _currentMouseOverObj)
                {
                    _currentMouseDownObj.OnMouseUpEvent(new MouseButtonUpEvent() { Button = button });
                }

                /* send mouse up to the current obj */
                if (_currentMouseOverObj != null)
                {
                    _currentMouseOverObj.OnMouseUpEvent(new MouseButtonUpEvent() { Button = button });
                }
            }
        }

        internal void MouseMove(int x, int y)
        {
            _mousePosition = new Point(x, y);
        }

        public void UnloadObject(GameObject gameObject)
        {
            if (!GameObjects.Contains(gameObject)) throw new InvalidOperationException();

            _gameObjects.Remove(gameObject);
        }

        public void UpdateCurrentMousePosition()
        {
            if (_mousePosition == null || CurrentCamera == null) return;

            CurrentCamera.Tick(0);

            Vector4 nearPlane, farPlane;
            ScreenPointToWorld(_mousePosition.Value, out nearPlane, out farPlane);

            LastNearPlaneMousePosition = nearPlane;
        }
    }
}
