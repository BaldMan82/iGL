using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class Box
    {
        public Vector3 Minumum { get; set; }
        public Vector3 Maximum { get; set; }

        private const float Epsilon = 1.192092896e-012f;

        internal bool RayIntersect(ref Vector3 origin, ref Vector3 direction)
        {             
            float enter = 0.0f, exit = float.MaxValue;

            if (!Intersect1D(origin.X, direction.X, Minumum.X, Maximum.X, ref enter, ref exit))
                return false;

            if (!Intersect1D(origin.Y, direction.Y, Minumum.Y, Maximum.Y, ref enter, ref exit))
                return false;

            if (!Intersect1D(origin.Z, direction.Z, Minumum.Z, Maximum.Z, ref enter, ref exit))
                return false;

            return true;
        
        }

        private bool Intersect1D(float start, float dir, float min, float max,
           ref float enter, ref float exit)
        {
            if (dir * dir < Epsilon * Epsilon) return (start >= min && start <= max);

            float t0 = (min - start) / dir;
            float t1 = (max - start) / dir;

            if (t0 > t1) { float tmp = t0; t0 = t1; t1 = tmp; }

            if (t0 > exit || t1 < enter) return false;

            if (t0 > enter) enter = t0;
            if (t1 < exit) exit = t1;
            return true;
        }
    }
}
