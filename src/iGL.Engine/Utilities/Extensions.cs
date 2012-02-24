using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public static class Extensions
    {
        public static BulletXNA.LinearMath.Matrix ToBullet(this Matrix4 matrix)
        {
            return new BulletXNA.LinearMath.Matrix(matrix.M11, 
                                                   matrix.M21, 
                                                   matrix.M31, 
                                                   matrix.M21, 
                                                   matrix.M22, 
                                                   matrix.M23, 
                                                   matrix.M31, 
                                                   matrix.M32, 
                                                   matrix.M33, 
                                                   matrix.M41, 
                                                   matrix.M42, 
                                                   matrix.M43);           
        }

        public static Matrix4 ToOpenTK(this BulletXNA.LinearMath.Matrix matrix)
        {
            return new Matrix4(new Vector4(matrix._basis._Row0.X, matrix._basis._Row1.X, matrix._basis._Row2.X, 0),
                               new Vector4(matrix._basis._Row0.Y, matrix._basis._Row1.Y, matrix._basis._Row2.Y, 0),
                               new Vector4(matrix._basis._Row0.Z, matrix._basis._Row1.Z, matrix._basis._Row2.Z, 0),
                               new Vector4(matrix.Translation.X, matrix.Translation.Y, matrix.Translation.Z, 1));

        }
    }
}
