using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Events;

namespace iGL.Engine.Triggers
{
    public class Trigger : IDisposable
    {
        public enum TriggerType
        {
            MouseDown,
            AnimationPlay,
            AnimationStopped,
            AnimationPaused,
            PlayerCollision,
            SceneLoad
        }

        public enum TriggerAction
        {
            PlayAnimation,
            PauseAnimation,
            StopAnimation,
            LoadObject
        }

        public TriggerType Type { get; set; }
        public TriggerAction Action { get; set; }

        public string SourceObjectId { get; set; }

        public string TargetObjectId { get; set; }
        public string TargetComponentId { get; set; }

        public Scene Scene { get; internal set; }

        private GameObject _sourceGameObject;
        private GameObject _targetGameObject;
        private GameComponent _targetGameComponent;


        private EventHandler<MouseButtonDownEvent> _mouseDownHandler;
        private EventHandler<AnimationSignalEvent> _animationSignalHandler;
        private EventHandler<LoadedEvent> _sceneLoadEventHandler;
        private EventHandler<ObjectCollisionEvent> _collisionEventHandler;

        public void Load()
        {
            if (Scene == null) throw new NotSupportedException();

            _targetGameObject = Scene.GameObjects.FirstOrDefault(g => g.Id == TargetObjectId);
            _sourceGameObject = Scene.GameObjects.FirstOrDefault(g => g.Id == SourceObjectId);

            if (_targetGameObject != null) _targetGameComponent = _targetGameObject.Components.FirstOrDefault(c => c.Id == TargetComponentId);

            if (_sourceGameObject != null)
            {
                /* object triggers */
                if (Type == TriggerType.MouseDown)
                {
                    _mouseDownHandler = (a, b) => ExecuteTrigger();
                    _sourceGameObject.OnMouseDown += _mouseDownHandler;
                }
                else if (Type == TriggerType.AnimationPlay)
                {
                    _animationSignalHandler = (a, b) => { if (b.SignalState == iGL.Engine.AnimationComponent.State.Playing) ExecuteTrigger(); };
                    _sourceGameObject.OnAnimationSignal += _animationSignalHandler;
                }
                else if (Type == TriggerType.AnimationStopped)
                {
                    _animationSignalHandler = (a, b) => { if (b.SignalState == iGL.Engine.AnimationComponent.State.Stopped) ExecuteTrigger(); };
                    _sourceGameObject.OnAnimationSignal += _animationSignalHandler;
                }
                else if (Type == TriggerType.AnimationPaused)
                {
                    _animationSignalHandler = (a, b) => { if (b.SignalState == iGL.Engine.AnimationComponent.State.Paused) ExecuteTrigger(); };
                    _sourceGameObject.OnAnimationSignal += _animationSignalHandler;
                }
                else if (Type == TriggerType.PlayerCollision)
                {
                    _collisionEventHandler = (a, b) =>
                    {
                        if (b.Object != null && b.Object == this.Scene.PlayerObject)
                        {
                            ExecuteTrigger();
                        }
                    };
                    _sourceGameObject.OnObjectCollision += _collisionEventHandler;
                }
            }

            if (Type == TriggerType.SceneLoad)
            {
                _sceneLoadEventHandler = (a,b) => ExecuteTrigger();
                Scene.OnLoaded += _sceneLoadEventHandler;
            }
        }
   
        public override string ToString()
        {
            return string.Format("{0}.{1} -> {2}.{3}", _sourceGameObject != null ? _sourceGameObject.ToString() : "?", Type.ToString(),
                                                       _targetGameObject != null ? _targetGameObject.ToString() : "?", Action.ToString());
                                                       
        }

        private void ExecuteTrigger()
        {
            if (Game.InDesignMode) return;                       

            if (Action == TriggerAction.PlayAnimation)
            {
                var animationComponent = _targetGameComponent as AnimationComponent;
                if (animationComponent != null && !animationComponent.IsPlaying())
                {
                    if (!animationComponent.GameObject.IsLoaded) animationComponent.GameObject.Load();

                    animationComponent.Play();
                }
            }
            else if (Action == TriggerAction.LoadObject)
            {
                if (_targetGameObject != null && !_targetGameObject.IsLoaded) _targetGameObject.Load();
            }

        }

        public void Dispose()
        {
            if (_sourceGameObject != null)
            {
                _sourceGameObject.OnMouseDown -= _mouseDownHandler;
                _sourceGameObject.OnAnimationSignal -= _animationSignalHandler;
                _sourceGameObject.OnObjectCollision -= _collisionEventHandler;                
            }

            Scene.OnLoaded -= _sceneLoadEventHandler;
        }
    }
}
