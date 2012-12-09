using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Events;
using iGL.Engine.Math;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml.Linq;
using iGL.Engine.Triggers;

namespace iGL.Engine
{
    public class Scene : IDisposable
    {
        public CameraComponent CurrentCamera
        {
            get
            {
                return Game.InDesignMode ? DesignCamera : PlayCamera;
            }
            set
            {
                if (Game.InDesignMode)
                {
                    DesignCamera = value;
                }
                else
                {
                    PlayCamera = value;
                }
            }
        }
        public CameraComponent PlayCamera { get; private set; }
        public CameraComponent DesignCamera { get; private set; }
        public LightComponent CurrentLight { get; private set; }
        public Statistics Statistics { get; private set; }
        public GameObject PlayerObject { get; private set; }

        public GameObject LastMouseDownTarget { get { return _currentMouseDownObj; } }

        public IEnumerable<GameObject> GameObjects { get { return _gameObjects.AsEnumerable(); } }
        public IEnumerable<Resource> Resources { get { return _resources.AsEnumerable(); } }
        public IEnumerable<Trigger> Triggers { get { return _triggers.AsEnumerable(); } }

        public Vector4? LastNearPlaneMousePosition { get; private set; }
        public Vector4? LastFarPlaneMousePosition { get; private set; }

        public Game Game { get; internal set; }
        public bool Loaded { get; internal set; }
        public Point MousePosition { get; private set; }
        public bool IsDisposing { get; private set; }
        public bool IsDisposingResources { get; private set; }
        public Dictionary<string, int[]> MeshBufferCache { get; internal set; }

        private TimeSpan _mouseUpdateRate = TimeSpan.FromSeconds(1.0 / 40.0);
        private DateTime _lastMouseUpdate = DateTime.MinValue;
        private List<GameObject> _gameObjects { get; set; }
        private List<Resource> _resources { get; set; }
        private List<Trigger> _triggers { get; set; }

        private List<Timer> _timers = new List<Timer>();

        private GameObject _currentMouseOverObj = null;
        private GameObject _currentMouseDownObj = null;

        private Vector4 _ambientColor;
        private DateTime _lastRenderUtc;
        private bool _compositionChanged;
        protected GameObject[] _sortedObjectsArray;

        private event EventHandler<TickEvent> OnTickEvent;
        private event EventHandler<MouseMoveEvent> OnMouseMoveEvent;
        private event EventHandler<MouseZoomEvent> OnMouseZoomEvent;
        private event EventHandler<GameObjectAddedEvent> OnObjectAddedEvent;
        private event EventHandler<LoadedEvent> OnLoadedEvent;
        private event EventHandler<PreRenderEvent> OnPreRenderEvent;
        private event EventHandler<CollisionEvent> OnCollisionEvent;
        private event EventHandler<DisposeObjectEvent> OnDisposeObjectEvent;

        private MouseButtonDownEvent _mouseButtonDownEvent = new MouseButtonDownEvent();
        private MouseButtonUpEvent _mouseButtonUpEvent = new MouseButtonUpEvent();
        private MouseInEvent _mouseInEvent = new MouseInEvent();
        private MouseOutEvent _mouseOutEvent = new MouseOutEvent();
        private TickEvent _tickEvent = new TickEvent();
        private MouseMoveEvent _mouseMoveEvent = new MouseMoveEvent();
        private MouseZoomEvent _mouseZoomEvent = new MouseZoomEvent();
        private GameObjectAddedEvent _gameObjectAddedEvent = new GameObjectAddedEvent();
        private LoadedEvent _loadedEvent = new LoadedEvent();
        private PreRenderEvent _preRenderEvent = new PreRenderEvent();
        private CollisionEvent _collisionEvent = new CollisionEvent();
        private ObjectCollisionEvent _objectCollisionEvent = new ObjectCollisionEvent();
        private DisposeObjectEvent _disposeObjectEvent = new DisposeObjectEvent();

        public ShaderProgram Shader { get; set; }
        public DesignShader DesignShader { get; set; }
        public PhysicsBase Physics { get; private set; }

        private List<GameObject> _disposableGameObjects = new List<GameObject>();
        public Dictionary<MouseButton, bool> MouseButtonState { get; private set; }

        internal Dictionary<string, GameComponent> ComponentCache
        {
            get;
            private set;
        }

        public Vector4 AmbientColor
        {
            get
            {
                return _ambientColor;
            }
            set
            {
                _ambientColor = value;      
            }
        }

        public Scene(ShaderProgram.ProgramType shaderType)
            : this(new PhysicsFarseer(), shaderType)
        {

        }

        public Scene(PhysicsBase physics, ShaderProgram.ProgramType shaderType)
        {
            _gameObjects = new List<GameObject>();
            _resources = new List<Resource>();
            _triggers = new List<Trigger>();
            ComponentCache = new Dictionary<string, GameComponent>();

            switch (shaderType)
            {
                case ShaderProgram.ProgramType.POINTLIGHT:
                    Shader = new PointLightShader();
                    break;
                case ShaderProgram.ProgramType.UI:
                    Shader = new UIShader();
                    break;
            }
            
            Shader.Load();

            DesignShader = new DesignShader();
            if (Game.InDesignMode) DesignShader.Load();

            Physics = physics;

            MouseButtonState = new Dictionary<MouseButton, bool>();

            MouseButtonState.Add(MouseButton.Button1, false);
            MouseButtonState.Add(MouseButton.Button2, false);
            MouseButtonState.Add(MouseButton.ButtonMiddle, false);

            Physics = physics;

            AmbientColor = new Vector4(1, 1, 1, 1);

            Statistics = new Statistics();

            MeshBufferCache = new Dictionary<string, int[]>();

            Physics.CollisionEvent += new EventHandler<CollisionEvent>(Physics_CollisionEvent);
        }

        void Physics_CollisionEvent(object sender, CollisionEvent e)
        {            
            _objectCollisionEvent.Object = e.ObjectB;
            e.ObjectA.OnObjectCollisionEvent(this, _objectCollisionEvent);

            _objectCollisionEvent.Object = e.ObjectA;
            e.ObjectB.OnObjectCollisionEvent(this, _objectCollisionEvent);
        }

        public void Render()
        {
            var sw = new Stopwatch();
            sw.Start();

            if (OnPreRenderEvent != null)
            {
                OnPreRenderEvent(this, _preRenderEvent);
            }


            if (CurrentCamera == null)
            {
                /* just clear the scene */

                Game.GL.ClearColor(0, 0, 0, 0);
                Game.GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                return;

            }

            //Game.GL.ClearColor(CurrentCamera.ClearColor.X, CurrentCamera.ClearColor.Y, CurrentCamera.ClearColor.Z, CurrentCamera.ClearColor.W);

            if (Game.InDesignMode)
            {
                Game.GL.ClearColor(1, 1, 1, 0);
                Game.GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
            else
            {
                Game.GL.Clear(ClearBufferMask.DepthBufferBit);
            }

            var pointLightShader = Shader as PointLightShader;

            Shader.Use();

            if (pointLightShader != null)
            {
                if (CurrentLight != null)
                {
                    /* update shader's light parameters */                
                    pointLightShader.SetLight(CurrentLight.Light);

                }
                else
                {
                    pointLightShader.ClearLight();
                }
            }

            Shader.SetAmbientColor(ref _ambientColor);
            DesignShader.SetAmbientColor(ref _ambientColor);          

            var allObjects = _gameObjects.SelectMany(g => g.AllChildren).ToList();
            allObjects.AddRange(_gameObjects);

            var sortedObjects = allObjects.OrderByDescending(g => g.RenderQueuePriority).
                                           ThenByDescending(g => g.DistanceSorting ? (g.WorldPosition - CurrentCamera.GameObject.WorldPosition).LengthSquared : float.MaxValue);
           
            foreach (var gameObject in sortedObjects)
            {               
                gameObject.Render();                     
            }

            sw.Stop();

            //Debug.WriteLine("FPS: {0}, ms:{1}", 1000.0d / sw.Elapsed.TotalMilliseconds, sw.Elapsed.TotalMilliseconds);

            Statistics.LastRenderDuration = DateTime.UtcNow - _lastRenderUtc;

            _lastRenderUtc = DateTime.UtcNow;

        }

        public void TickPhysics(float timeElapsed)
        {           
            float step = timeElapsed;
            if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;

            Physics.Step(step);           
        }

        public void Tick(float timeElapsed, bool tickPhysics = true)
        {
            UpdateCompositionCache();

            if (OnTickEvent != null)
            {
                _tickEvent.Elapsed = timeElapsed;
                OnTickEvent(this, _tickEvent);
            }          

            for (int i = 0; i < _sortedObjectsArray.Length; i++)
            {
                _sortedObjectsArray[i].Tick(timeElapsed);
            }

            ProcessTimers();

            if (DateTime.UtcNow > _lastMouseUpdate.Add(_mouseUpdateRate))
            {
                /* update mouse position */

                ProcessInteractiviy();

                _lastMouseUpdate = DateTime.UtcNow;
            }

			/* dispose objects marked for deletion */

			var disposableObject = _disposableGameObjects.FirstOrDefault();
			if (disposableObject != null)
			{
				disposableObject.Dispose();
				_gameObjects.Remove(disposableObject);
				
				/* raise dispose event */
				if (OnDisposeObjectEvent != null)
				{
					_disposeObjectEvent.GameObject = disposableObject;
					OnDisposeObjectEvent(this, _disposeObjectEvent);
				}

				_disposableGameObjects.Remove(disposableObject);
			}


            this.CurrentCamera.GameObject.Tick(0.0f);
        }

        private void UpdateCompositionCache()
        {
            if (!_compositionChanged) return;

            _sortedObjectsArray = _gameObjects.OrderByDescending(g => g.Components.Any(c => c is RigidBodyBaseComponent)).ToArray();

            _compositionChanged = false;
        }

        private void ProcessTimers()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                var timer = _timers[i];

                if (timer.LastTick.Add(timer.Interval) < DateTime.UtcNow)
                {
                    timer.LastTick = DateTime.UtcNow;
                    if (timer.Mode == Timer.TimerMode.Once)
                    {
                        _timers.RemoveAt(i);
                        i--;
                    }

                    timer.Action.Invoke();
                }
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

        public event EventHandler<MouseZoomEvent> OnMouseZoom
        {
            add
            {
                OnMouseZoomEvent += value;
            }
            remove
            {
                OnMouseZoomEvent -= value;
            }
        }

        public event EventHandler<GameObjectAddedEvent> OnObjectAdded
        {
            add
            {
                OnObjectAddedEvent += value;
            }
            remove
            {
                OnObjectAddedEvent -= value;
            }
        }

        public event EventHandler<LoadedEvent> OnLoaded
        {
            add
            {
                OnLoadedEvent += value;
            }
            remove
            {
                OnLoadedEvent -= value;
            }
        }

        public event EventHandler<PreRenderEvent> OnPreRender
        {
            add
            {
                OnPreRenderEvent += value;
            }
            remove
            {
                OnPreRenderEvent -= value;
            }
        }

        public event EventHandler<CollisionEvent> OnCollision
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

        public event EventHandler<DisposeObjectEvent> OnDisposeObject
        {
            add
            {
                OnDisposeObjectEvent += value;
            }
            remove
            {
                OnDisposeObjectEvent -= value;
            }
        }

        public void SetPlayCamera(GameObject camera)
        {
            if (camera == null)
            {
                PlayCamera = null;
                return;
            }

            if (camera.Scene != this) throw new Exception("Camera is not part of this scene");

            var component = camera.Components.FirstOrDefault(c => c is CameraComponent) as CameraComponent;

            if (component == null) throw new Exception("GameObject does not have a camera component");

            PlayCamera = component;
        }

        public void SetDesignCamera(GameObject camera)
        {
            if (camera == null)
            {
                DesignCamera = null;
                return;
            }

            if (camera.Scene != this) throw new Exception("Camera is not part of this scene");

            var component = camera.Components.FirstOrDefault(c => c is CameraComponent) as CameraComponent;

            if (component == null) throw new Exception("GameObject does not have a camera component");

            DesignCamera = component;
        }

        public void SetCurrentLight(GameObject light)
        {
            if (light == null)
            {
                CurrentLight = null;
                return;
            }

            if (light.Scene != this) throw new Exception("Light is not part of this scene");

            var component = light.Components.FirstOrDefault(c => c is LightComponent) as LightComponent;

            if (component == null) throw new Exception("GameObject does not have a light component");

            CurrentLight = component;
        }

        public void SetPlayerObject(GameObject player)
        {
            if (player == null)
            {
                PlayerObject = null;
                return;
            }

            if (player.Scene != this) throw new Exception("Player object is not part of this scene");

            PlayerObject = player;
        }

        public virtual void Load()
        {
            _resources.ForEach(r => r.Load());
            _triggers.ForEach(t => t.Load());

            var objectsToLoad = _gameObjects.Where(g => Game.InDesignMode || g.AutoLoad);

            var gameObjectBeforeLoad = objectsToLoad.SelectMany(g => g.AllChildren).ToList();
            gameObjectBeforeLoad.AddRange(objectsToLoad);

            objectsToLoad.ToList().ForEach(g => g.Load());

            var gameObjectAfterLoad = objectsToLoad.SelectMany(g => g.AllChildren).ToList();
            gameObjectAfterLoad.AddRange(objectsToLoad);

            if (gameObjectAfterLoad.Count != gameObjectBeforeLoad.Count)
            {
                /* objects have been added during load, which is illegal */
                /* all creation should take place in Init phase or after load phase */

                var strb = new StringBuilder();
                gameObjectAfterLoad.ForEach(g => strb.AppendLine(g.ToString()));

                throw new NotSupportedException("Cannot create objects during load phase: " + strb.ToString());
            }

            Loaded = true;

            FireLoadEvent();
        }

        internal void FireLoadEvent()
        {
            if (OnLoadedEvent != null)
            {
                OnLoadedEvent(this, _loadedEvent);
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject.IsLoaded) throw new InvalidOperationException("GameObject cannot be loaded");

            gameObject.Scene = this;

            _gameObjects.Add(gameObject);

            if (Loaded) gameObject.Load();

            if (OnObjectAddedEvent != null)
            {
                _gameObjectAddedEvent.GameObject = gameObject;
                OnObjectAddedEvent(this, _gameObjectAddedEvent);
            }

            _compositionChanged = true;
        }

        public void DisposeGameObject(GameObject gameObject)
        {
            if (!gameObject.IsDisposing)
            {
                //var loadedObj = _gameObjects.First(g => g == gameObject);
                _disposableGameObjects.Add(gameObject);

                gameObject.IsDisposing = true;
            }
        }

        public void AddResource(Resource resource)
        {
            resource.Scene = this;

            if (Loaded) resource.Load();

            _resources.Add(resource);
        }

        public void RemoveResource(Resource res)
        {
            _resources.Remove(res);
        }

        public void AddTrigger(Trigger trigger)
        {
            trigger.Scene = this;

            if (Loaded) trigger.Load();

            _triggers.Add(trigger);
        }

        public void RemoveTrigger(Trigger trigger)
        {
            _triggers.Remove(trigger);
            trigger.Dispose();
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
            Vector4 nearPlane, farPlane;
            ScreenPointToWorld(MousePosition, out nearPlane, out farPlane);

            /* hold a reference to the nearplane vector in order to calculate mouse input directional vector */
            if (LastNearPlaneMousePosition != null)
            {
                var direction = nearPlane - LastNearPlaneMousePosition.Value;
                if (direction.LengthSquared != 0)
                {
                    if (OnMouseMoveEvent != null)
                    {
                        _mouseMoveEvent.DirectionOnNearPlane = new Vector3(direction.X, direction.Y, direction.Z);
                        _mouseMoveEvent.NearPlane = new Vector3(nearPlane.X, nearPlane.Y, nearPlane.Z);

                        OnMouseMoveEvent(this, _mouseMoveEvent);
                    }
                }
            }

            UpdateCurrentMousePosition();

            var ray = new Vector4(farPlane - nearPlane);

            Vector3 hitLocation;

            GameObject result = RayCast(nearPlane, ray, out hitLocation);

            /* call events on raycast results */

            if (result != null)
            {
                /* target changed */
                if (_currentMouseOverObj != null && _currentMouseOverObj != result)
                {
                    _currentMouseOverObj.OnMouseOutEvent(_currentMouseOverObj, new MouseOutEvent());
                    result.OnMouseInEvent(result, new MouseInEvent());

                }
                else if (_currentMouseOverObj == null)
                {
                    result.OnMouseInEvent(result, new MouseInEvent());
                }

                _currentMouseOverObj = result;
            }
            else if (_currentMouseOverObj != null)
            {
                _currentMouseOverObj.OnMouseOutEvent(_currentMouseOverObj, new MouseOutEvent());
                _currentMouseOverObj = null;
            }
        }

        public GameObject RayCast(Vector4 nearPlane, Vector4 ray, out Vector3 hitLocation)
        {
            var near = new Vector3(nearPlane);
            var dir = new Vector3(ray);

            return RayTest(ref near, ref dir, out hitLocation);
        }

        public GameObject RayTest(ref Vector3 position, ref Vector3 dir, out Vector3 hitLocation, List<GameObject> ignoreList = null)
        {
            hitLocation = new Vector3(0);

            float minDistance = float.MaxValue;
            GameObject result = null;

            List<GameObject> objects;

            if (Game.InDesignMode || PlayerObject == null)
            {
                objects = _gameObjects.Where(g => g.Enabled).SelectMany(g => g.AllChildren).ToList();
                objects.AddRange(_gameObjects);
            }
            else
            {
                objects = PlayerObject.AllChildren.ToList();
                objects.Add(PlayerObject);
            }

            foreach (var gameObject in objects)
            {
                Vector3 rayHitLocation;
                var rayHit = gameObject.RayTest(ref position, ref dir, out rayHitLocation);

                if (rayHit != null && (ignoreList == null || !ignoreList.Contains(rayHit)))
                {
                    var distance = (rayHitLocation - position).LengthSquared;

                    if (distance < minDistance || gameObject.Parent is Gizmo)
                    {
                        result = rayHit;
                        minDistance = distance;
                        hitLocation = rayHitLocation;

                        if (gameObject.Parent is Gizmo)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        internal void UpdateMouseButton(MouseButton button, bool down, int x, int y)
        {
            MouseButtonState[button] = down;
            MousePosition = new Point(x, y);
            ProcessInteractiviy();

            /* selected target is now updated, process button event */

            if (down)
            {
                if (_currentMouseOverObj != null)
                {
                    _mouseButtonDownEvent.Button = button;
                    _currentMouseOverObj.OnMouseDownEvent(_currentMouseOverObj, _mouseButtonDownEvent);
                }

                /* can be null */
                _currentMouseDownObj = _currentMouseOverObj;
            }
            else
            {
                /* send a mouse up event to the last target, even if it is not the current hover target anymore */
                if (_currentMouseDownObj != null && _currentMouseDownObj != _currentMouseOverObj)
                {
                    _mouseButtonUpEvent.Button = button;
                    _currentMouseDownObj.OnMouseUpEvent(_currentMouseDownObj, _mouseButtonUpEvent);
                }

                /* send mouse up to the current obj */
                if (_currentMouseOverObj != null)
                {
                    _mouseButtonUpEvent.Button = button;
                    _currentMouseOverObj.OnMouseUpEvent(_currentMouseOverObj, _mouseButtonUpEvent);
                }
            }
        }

        internal void MouseMove(int x, int y)
        {
            MousePosition = new Point(x, y);
        }

        internal void MouseZoom(int amount)
        {
            if (OnMouseZoomEvent != null)
            {
                _mouseZoomEvent.Amount = amount;
                OnMouseZoomEvent(this, _mouseZoomEvent);
            }
        }

        public void UnloadObject(GameObject gameObject)
        {
            if (!GameObjects.Contains(gameObject)) throw new InvalidOperationException();

            _gameObjects.Remove(gameObject);

            if (CurrentCamera != null && gameObject.Components.Contains(CurrentCamera)) CurrentCamera = null;
            if (CurrentLight != null && gameObject.Components.Contains(CurrentLight)) CurrentLight = null;

        }

        public void UpdateCurrentMousePosition()
        {
            if (MousePosition == null || CurrentCamera == null) return;

            CurrentCamera.Tick(0);

            Vector4 nearPlane, farPlane;
            ScreenPointToWorld(MousePosition, out nearPlane, out farPlane);

            LastNearPlaneMousePosition = nearPlane;
            LastFarPlaneMousePosition = farPlane;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposeResources)
        {
            IsDisposing = true;
            IsDisposingResources = disposeResources;

            FinishTimers();

            /* dispose all objects */
            _gameObjects.ForEach(g => g.Dispose());
            if (disposeResources) _resources.ForEach(r => r.Dispose());

            _gameObjects.Clear();
            _resources.Clear();

            Shader.Dispose();
            //FurShader.Dispose();

            _triggers.ForEach(tr => tr.Dispose());
            _triggers.Clear();

            MeshBufferCache = null;
        }

        internal void FinishTimers()
        {
            /* finish all timers */
            _timers.ForEach(t => t.LastTick = DateTime.MinValue);
            ProcessTimers();
            _timers.Clear();
        }     
    }
}
