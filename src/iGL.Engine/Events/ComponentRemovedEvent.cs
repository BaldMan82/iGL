using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{   
    public class ComponentRemovedEvent : EventArgs
    {
        public GameComponent Component { get; set; }
    }
}
