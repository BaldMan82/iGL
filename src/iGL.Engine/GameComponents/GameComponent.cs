using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml.Linq;

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
        public string Id { get; set; }

        public GameComponent()
        {
            CreationMode = GameComponent.CreationModeEnum.Additional;
            Id = Guid.NewGuid().ToString();

            Init();
        }

        public GameComponent(SerializationInfo info, StreamingContext context)
            : this()
        {
            var props = this.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes(false).Any(o => o is XmlIgnoreAttribute)) continue;

                prop.SetValue(this, info.GetValue(prop.Name, prop.PropertyType), null);
            }
        }

        protected virtual void Init() { }

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



        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var props = this.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes(false).Any(o => o is XmlIgnoreAttribute)) continue;

                info.AddValue(prop.Name, prop.GetValue(this, null));
            }
        }           
    }
}
