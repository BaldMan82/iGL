using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Designer
{
    public class GameObjectDialogAttribute : Attribute
    {
        public Type GameObjectType { get; private set; }

        public GameObjectDialogAttribute(Type gameObjectType)
        {
            GameObjectType = gameObjectType;
        }
    }
}
