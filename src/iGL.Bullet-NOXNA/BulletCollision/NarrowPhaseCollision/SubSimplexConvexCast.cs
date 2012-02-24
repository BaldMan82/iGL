﻿/*
 * C# / XNA  port of Bullet (c) 2011 Mark Neale <xexuxjy@hotmail.com>
 *
 * Bullet Continuous Collision Detection and Physics Library
 * Copyright (c) 2003-2008 Erwin Coumans  http://www.bulletphysics.com/
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose, 
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
    public class SubSimplexConvexCast : IConvexCast
    {
        public SubSimplexConvexCast(ConvexShape shapeA, ConvexShape shapeB, ISimplexSolverInterface simplexSolver)
        {
            m_convexA = shapeA;
            m_convexB = shapeB;
            m_simplexSolver = simplexSolver;
        }


        ///SimsimplexConvexCast calculateTimeOfImpact calculates the time of impact+normal for the linear cast (sweep) between two moving objects.
        ///Precondition is that objects should not penetration/overlap at the start from the interval. Overlap can be tested using btGjkPairDetector.
        public virtual bool CalcTimeOfImpact(ref Matrix fromA, ref Matrix toA, ref Matrix fromB, ref Matrix toB, CastResult result)
        {
            m_simplexSolver.Reset();

            Vector3 linVelA = toA.Translation - fromA.Translation;
            Vector3 linVelB = toB.Translation - fromB.Translation;

            float lambda = 0f;

            Matrix interpolatedTransA = fromA;
            Matrix interpolatedTransB = fromB;

            ///take relative motion
            Vector3 r = (linVelA - linVelB);
            Vector3 v;

            Vector3 supVertexA = fromA * m_convexA.LocalGetSupportingVertex(-r * fromA._basis);
            Vector3 supVertexB = fromB * m_convexB.LocalGetSupportingVertex(r * fromB._basis);
            v = supVertexA - supVertexB;
            int maxIter = MAX_ITERATIONS;

            Vector3 n = Vector3.Zero;

            bool hasResult = false;
            Vector3 c;

            float lastLambda = lambda;


            float dist2 = v.LengthSquared();
            float epsilon = 0.0001f;
            Vector3 w, p;
            float VdotR;

            while ((dist2 > epsilon) && (maxIter-- > 0))
            {
                supVertexA = interpolatedTransA * (m_convexA.LocalGetSupportingVertex(-v * interpolatedTransA._basis));
                supVertexB = interpolatedTransB * (m_convexB.LocalGetSupportingVertex(v * interpolatedTransB._basis));

                w = supVertexA - supVertexB;

                float VdotW = Vector3.Dot(v, w);

                if (lambda > 1.0f)
                {
                    return false;
                }

                if (VdotW > 0f)
                {
                    VdotR = Vector3.Dot(v, r);

                    if (VdotR >= -(MathUtil.SIMD_EPSILON * MathUtil.SIMD_EPSILON))
                    {
                        return false;
                    }
                    else
                    {
                        lambda = lambda - VdotW / VdotR;
                        //interpolate to next lambda
                        //	x = s + lambda * r;

                        interpolatedTransA.Translation = MathUtil.Interpolate3(fromA.Translation, toA.Translation, lambda);
                        interpolatedTransB.Translation = MathUtil.Interpolate3(fromB.Translation, toB.Translation, lambda);
                        //m_simplexSolver.reset();
                        //check next line
                        w = supVertexA - supVertexB;
                        lastLambda = lambda;
                        n = v;
                        hasResult = true;
                    }
                }
                ///Just like regular GJK only add the vertex if it isn't already (close) to current vertex, it would lead to divisions by zero and NaN etc.
                if (!m_simplexSolver.InSimplex(ref w))
                {
                    m_simplexSolver.AddVertex(ref w, ref supVertexA, ref supVertexB);
                }

                if (m_simplexSolver.Closest(out v))
                {
                    dist2 = v.LengthSquared();
                    hasResult = true;
                    //todo: check this normal for validity
                    //n=v;
                    //printf("V=%f , %f, %f\n",v[0],v[1],v[2]);
                    //printf("DIST2=%f\n",dist2);
                    //printf("numverts = %i\n",m_simplexSolver.numVertices());
                }
                else
                {
                    dist2 = 0f;
                }
            }

            //int numiter = MAX_ITERATIONS - maxIter;
            //	printf("number of iterations: %d", numiter);

            //don't report a time of impact when moving 'away' from the hitnormal

            result.m_fraction = lambda;
            if (n.LengthSquared() >= (MathUtil.SIMD_EPSILON * MathUtil.SIMD_EPSILON))
            {
                result.m_normal = Vector3.Normalize(n);
            }
            else
            {
                result.m_normal = Vector3.Zero;
            }

            //don't report time of impact for motion away from the contact normal (or causes minor penetration)
            if (Vector3.Dot(result.m_normal, r) >= -result.m_allowedPenetration)
            {
                return false;
            }

            Vector3 hitA, hitB;
            m_simplexSolver.ComputePoints(out hitA, out hitB);
            result.m_hitPoint = hitB;
            return true;

        }




        private ISimplexSolverInterface m_simplexSolver;
        private ConvexShape m_convexA;
        private ConvexShape m_convexB;

        private static readonly int MAX_ITERATIONS = 32;
    }
}
