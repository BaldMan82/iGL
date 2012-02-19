using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public abstract class GameComponent
    {
        public bool IsLoaded { get; private set; }
        public GameObject GameObject { get; private set; }

        public GameComponent(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public abstract void InternalLoad();
        public abstract void Tick(double timeElapsed);

        public void Load()
        {
            if (IsLoaded) return;

            InternalLoad();
            IsLoaded = true;
        }        
       
    }
}
