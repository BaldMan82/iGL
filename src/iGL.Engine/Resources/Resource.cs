using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using System.Reflection;

namespace iGL.Engine
{
    public abstract class Resource : IDisposable
    {
        public static Assembly ResourceAssembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.Contains("TestGame"));
        public static string[] AssemblyResources; 

        public string ResourceName { get; set; }
        public bool IsLoaded { get; private set; }
        public string Name { get; set; }
        public Scene Scene { get; internal set; }

        public static IGL GL { get { return Game.GL; } }

        public Resource()
        {
            if (Resource.AssemblyResources == null){
                AssemblyResources = ResourceAssembly.GetManifestResourceNames();
            }
        }

        protected abstract bool InternalLoad();

        public void Load()
        {
            IsLoaded = InternalLoad();
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual void Dispose()
        {           
        }
    }
}
