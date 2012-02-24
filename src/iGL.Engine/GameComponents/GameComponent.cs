﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;

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

        public IGL GL { get { return Game.GL; } }

        public abstract void InternalLoad();
        public abstract void Tick(float timeElapsed);

        public void Load()
        {
            if (IsLoaded) return;

            InternalLoad();
            IsLoaded = true;
        }        
       
    }
}
