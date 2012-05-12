using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace iGL.Engine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class RequiredComponent : Attribute
    {
        public Type ComponentType { get; private set; }
        public string Id { get; private set; }

        public RequiredComponent(Type componentType, string id)
        {
            ComponentType = componentType;
            Id = id;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredChild : Attribute
    {
        public Type ChildType { get; private set; }
        public string Id { get; private set; }

        public RequiredChild(Type childType, string id)
        {
            ChildType = childType;
            Id = id;
        }
    }
}
