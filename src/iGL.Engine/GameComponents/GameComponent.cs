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
    public abstract class GameComponent : IXmlSerializable, IDisposable
    {
        public enum CreationModeEnum
        {
            Additional,
            Required,
            Runtime
        }

        public bool IsLoaded { get; private set; }
        public GameObject GameObject { get; internal set; }
        public CreationModeEnum CreationMode { get; internal set; }
        public string Id { get; set; }

        private XElement _xmlElement;

        public GameComponent()
        {
            CreationMode = GameComponent.CreationModeEnum.Additional;
            Id = Guid.NewGuid().ToString();

            Init();
        }

        public GameComponent(XElement xmlElement)
        {
            _xmlElement = xmlElement;

            /* set id property */
            Id = _xmlElement.Elements("Id").Single().Value;

            Init();
        }

        internal void LoadFromXml()
        {
            if (_xmlElement == null) return;

            #region Load Properties
            var props = this.GetType()
                               .GetProperties()
                               .Where(p => p.GetSetMethod() != null && !p.GetCustomAttributes(true).Any(attr => attr is XmlIgnoreAttribute));

            foreach (var prop in props)
            {
                var element = _xmlElement.Elements().FirstOrDefault(e => e.Name == prop.Name);

                if (element != null)
                {
                    prop.SetValue(this, XmlHelper.FromXml(element, prop.PropertyType), null);
                }
            }
            #endregion
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
            if (!IsLoaded)
            {
                Debug.WriteLine("Not loading: " + this.GetType().Name + " of " + GameObject.Name);
            }
            return IsLoaded;
        }
       
        public XElement ToXml(XElement element)
        {
            var props = this.GetType()
                                .GetProperties()
                                .Where(p => p.GetSetMethod() != null && !p.GetCustomAttributes(true).Any(attr => attr is XmlIgnoreAttribute));

            element.Add(props.Select(p => XmlHelper.ToXml(p.GetValue(this, null), p.Name)));

            return element;
        }

        public virtual void Dispose()
        {
            
        }
    }
}
