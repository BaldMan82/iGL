using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using FarseerPhysics.Common;

namespace iGL.Engine
{
    public static class Extensions
    {       
        public static Matrix4 ToOpenTK(this Transform transform)
        {
            return new Matrix4(transform.R.Col1.X,
                            transform.R.Col1.Y,
                          0f,
                          0f,

                          transform.R.Col2.X,
                          transform.R.Col2.Y,
                          0f,
                          0f,

                          0f,
                          0f,
                          1f,
                          0.0f,

                          transform.Position.X,
                          transform.Position.Y,
                          0, 
                          1.0f
                          );           
        }

        public static float PlaneDistance(this Vector3 point, Vector3 planePoint, Vector3 planeNormal)
        {
            return Vector3.Dot(point - planePoint, planeNormal) / planeNormal.Length;
        }

        public static Vector3 Translation(this Matrix4 matrix)
        {
            return new Vector3(matrix.M41, matrix.M42, matrix.M43);
        }

        public static void CopyPublicValues(this object obj, object destObj)
        {
            var props = obj.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                var val = prop.GetValue(obj, null);

                if (val == null) continue;

                if (!prop.PropertyType.IsValueType && !prop.PropertyType.IsArray && prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    var instance = Activator.CreateInstance(prop.PropertyType);
                    val.CopyPublicValues(instance);
                    prop.SetValue(destObj, instance, null);
                }
                else
                {                    
                    prop.SetValue(destObj, val, null);
                }
            }
        }      

        public static void EulerAngles(this Matrix4 matrix, out Vector3 eulerRotation)
        {
            eulerRotation = new Vector3(0);

            if (matrix.M31 != -1 || matrix.M32 != 1)
            {
                eulerRotation.Y = -(float)System.Math.Asin(matrix.M13);
                eulerRotation.X = (float)System.Math.Atan2(matrix.M23 / System.Math.Cos(eulerRotation.X), matrix.M33 / System.Math.Cos(eulerRotation.X));
                eulerRotation.Z = (float)System.Math.Atan2(matrix.M12 / System.Math.Cos(eulerRotation.X), matrix.M11 / System.Math.Cos(eulerRotation.X));
            }
        }      

        public static Microsoft.Xna.Framework.Vector2 ToXNA(this Vector2 vector)
        {
            return new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
        }

        public static void DefaultToXml(this object obj, System.Xml.XmlWriter writer)
        {
            var props = obj.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                writer.WriteElementString(prop.Name, prop.GetValue(obj, null).ToString());
            }      
        }
    }
}
