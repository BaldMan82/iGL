using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace iGL.Engine
{
    interface IXmlSerializable
    {
        XElement ToXml(XElement element);        
    }
}
