using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using Jitter.LinearMath;

namespace iGL.Engine
{
    public static class Extensions
    {
        public static JMatrix ToJitter(this Matrix4 matrix)
        {
            return new JMatrix(matrix.Row0.X, matrix.Row0.Y, matrix.Row0.Z,
                               matrix.Row1.X, matrix.Row1.Y, matrix.Row1.Z,
                               matrix.Row2.X, matrix.Row2.Y, matrix.Row2.Z);
        }       

        public static Matrix4 ToOpenTK(this JMatrix matrix, JVector position)
        {
            return new Matrix4(matrix.M11,
                           matrix.M12,
                           matrix.M13,
                           0.0f,
                           matrix.M21,
                           matrix.M22,
                           matrix.M23,
                           0.0f,
                           matrix.M31,
                           matrix.M32,
                           matrix.M33,
                           0.0f, 
                           position.X,
                           position.Y, 
                           position.Z, 1.0f);           
        }

        public static JVector ToJitter(this Vector4 vector)
        {
            return new JVector(vector.X, vector.Y, vector.Z);
        }

        public static JVector ToJitter(this Vector3 vector)
        {
            return new JVector(vector.X, vector.Y, vector.Z);
        }

        public static Vector3 ToOpenTK(this JVector vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
