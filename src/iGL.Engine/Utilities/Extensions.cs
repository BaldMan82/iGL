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

        public static Matrix4 ToOpenTK(this JMatrix matrix)
        {
            return new Matrix4(new Vector4(matrix.M11, matrix.M12, matrix.M13, 0),
                               new Vector4(matrix.M21, matrix.M22, matrix.M23, 0),
                               new Vector4(matrix.M31, matrix.M32, matrix.M33, 0),
                               new Vector4(0, 0, 0, 1));
        }
    }
}
