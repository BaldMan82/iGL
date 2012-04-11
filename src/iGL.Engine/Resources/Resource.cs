using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public abstract class Resource
    {
        public string ResourceName { get; set; }
        public bool IsLoaded { get; private set; }
        public string Name { get; set; }

        public Resource()
        {

        }

        protected abstract bool InternalLoad();

        public void Load()
        {
            IsLoaded = InternalLoad();
        }
    }
}
