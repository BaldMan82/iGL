using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;

namespace iGL.Engine
{
    public abstract class GameComponent : Object 
    {
        public enum CreationModeEnum
        {
            Additional,
            Internal
        }

        public bool IsLoaded { get; private set; }
        public GameObject GameObject { get; internal set; }
        public CreationModeEnum CreationMode { get; internal set; }
        public Guid Id { get; internal set; }

        public GameComponent()
        {
            CreationMode = GameComponent.CreationModeEnum.Additional;
            Id = Guid.NewGuid();
        }      

        public IGL GL { get { return Game.GL; } }

        public abstract bool InternalLoad();
        public abstract void Tick(float timeElapsed);

        public GameComponent Clone()
        {
            var obj = Activator.CreateInstance(this.GetType()) as GameComponent;

            this.CopyPublicValues(obj);

            return obj;
        }

        public bool Load()
        {
            if (IsLoaded) return false;

            if (GameObject == null)
            {
                throw new NotSupportedException("Cannot load component without game object");
            }

            IsLoaded = InternalLoad();

            return IsLoaded;
        }        
       

    }
}
