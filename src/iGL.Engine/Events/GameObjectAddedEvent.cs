using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine.Events
{
    public class GameObjectAddedEvent : EventArgs
    {
        public GameObject GameObject { get; set; }
    }
}
