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
    public class Scene
    {
        private TimeSpan _mouseUpdateRate = TimeSpan.FromSeconds(1.0 / 50.0);
        private DateTime _lastMouseUpdate = DateTime.MinValue;

        private List<GameObject> _gameObjects { get; set; }
        private List<Timer> _timers = new List<Timer>();

        public CameraComponent CurrentCamera { get; private set; }
        public LightComponent CurrentLight { get; private set; }

        public IEnumerable<GameObject> GameObjects { get { return _gameObjects.AsEnumerable(); } }

        public Game Game { get; internal set; }
        public ShaderProgram ShaderProgram { get; internal set; }

        internal Physics Physics { get; private set; }
        private event EventHandler<TickEvent> OnTickEvent;
        private event EventHandler<MouseMoveEvent> OnMouseMoveEvent;

        private Point? _mousePosition = null;
        private GameObject _currentMouseOverObj = null;
        private GameObject _currentMouseDownObj = null;
        private Vector4? _lastNearPlaneMousePosition = null;

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
            if (OnTickEvent != null) OnTickEvent(this, new TickEvent() { Elapsed = timeElapsed });

            try
            {
                float step = timeElapsed;
                if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;

                Physics.World.Step(timeElapsed, false);
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

        public virtual void Load() { }

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

            if (CurrentCamera == null) return;

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

            /* hold a reference to the nearplane vector in order to calculate mouse input directional vector */
            if (_lastNearPlaneMousePosition != null)
            {
                var direction = nearPlane - _lastNearPlaneMousePosition.Value;
                if (direction.LengthSquared != 0)
                {
                    if (OnMouseMoveEvent != null)
                    {
                        OnMouseMoveEvent(this, new MouseMoveEvent()
                        {
                            Direction = new Vector3(direction.X, direction.Y, direction.Z),
                            NearPlane = new Vector3(nearPlane.X, nearPlane.Y, nearPlane.Z)
                        });
                    }
                }
            }

            _lastNearPlaneMousePosition = nearPlane;

            var ray = new Vector4(farPlane - nearPlane);

            RigidBody body;
            JVector normal;
            float fraction;

            Physics.World.CollisionSystem.Raycast(nearPlane.ToJitter(), ray.ToJitter(), (a, b, c) => true, out body, out normal, out fraction);

            /* call events on raycast results */

            if (body != null)
            {
                var gameObj = body.Tag as GameObject;

                /* target changed */
                if (_currentMouseOverObj != null && _currentMouseOverObj != gameObj)
                {
                    _currentMouseOverObj.OnMouseOutEvent(new MouseOutEvent());
                    gameObj.OnMouseInEvent(new MouseInEvent());

                }
                else if (_currentMouseOverObj == null)
                {
                    gameObj.OnMouseInEvent(new MouseInEvent());
                }

                _currentMouseOverObj = gameObj;
            }
            else if (_currentMouseOverObj != null)
            {
                _currentMouseOverObj.OnMouseOutEvent(new MouseOutEvent());
                _currentMouseOverObj = null;
            }

        }

        internal void UpdateMouseButton(MouseButton button, bool down, int x, int y)
        {
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
    }
}
