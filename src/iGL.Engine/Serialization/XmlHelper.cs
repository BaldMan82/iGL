using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using iGL.Engine.Math;
using System.Reflection;
using System.Xml.Serialization;

namespace iGL.Engine
{
    public static class XmlHelper
    {
        public static XElement ToXml(object value, string name)
        {
            if (value == null) return new XElement(name);

            XElement element = null;
            Type type = value.GetType();           

            if (type.GetInterfaces().Contains(typeof(IXmlSerializable)))
            {
                element = new XElement(name);
                element = ((IXmlSerializable)value).ToXml(element);
            }
            else
            {
                if (type.IsValueType || type == typeof(string))
                {
                    element = new XElement(name, value);
                }
                else
                {
                    var props = type.GetProperties().Where(p => p.GetSetMethod() != null && !p.GetCustomAttributes(true).Any(attr => attr is XmlIgnoreAttribute));
                    element = new XElement(name, props.Select(p => ToXml(p.GetValue(value, null), p.Name)));
                }

            }

            if (!(type.IsValueType || type == typeof(string)))
            {
                /* save derived type as attribute */
                element.SetAttributeValue("Type", type.AssemblyQualifiedName);
            }

            return element;
        }

        public static object FromXml(XElement element, Type type)
        {
            var typeAttrib = element.Attribute("Type");
            if (typeAttrib != null) type = Type.GetType(typeAttrib.Value);

            if (type == null)
            {
                /* unknown type */
                throw new Exception("Unknown type: " + typeAttrib.Value);
            }

            if (type.GetInterfaces().Contains(typeof(IXmlSerializable)))
            {
                /* deserialize game objects */
                return Activator.CreateInstance(type, new object[] { element });
            }
            else
            {
                if (type.IsValueType || type == typeof(string))
                {
                    #region Value Types
                    /* parse value */
                    if (type == typeof(float))
                    {
                        float result;
                        if (float.TryParse(element.Value, out result))
                        {
                            return result;
                        }
                    }
                    else if (type == typeof(bool))
                    {
                        bool result;
                        if (bool.TryParse(element.Value, out result))
                        {
                            return result;
                        }
                    }
                    else if (type == typeof(string))
                    {
                        return element.Value;
                    }
                    else if (type == typeof(int))
                    {
                        int result;
                        if (int.TryParse(element.Value, out result))
                        {
                            return result;
                        }
                    }
                    else if (type.IsEnum)
                    {
                        return Enum.Parse(type, element.Value);
                    }
                    else
                    {
                        throw new NotSupportedException(type.ToString());
                    }

                    return null;
                    #endregion
                }
                else
                {
                    #region Reference types
                   
                    type = Type.GetType(element.Attribute("Type").Value);
                    
                    var obj = Activator.CreateInstance(type);
                    var props = type.GetProperties().Where(p => p.GetSetMethod() != null);
                    foreach (var prop in props)
                    {
                        var childElement = element.Elements().FirstOrDefault(e => e.Name == prop.Name);
                        if (childElement != null)
                        {
                            prop.SetValue(obj, FromXml(childElement, prop.PropertyType), null);
                        }
                    }

                    return obj;
                    #endregion
                }
            }
        }
    }
}
