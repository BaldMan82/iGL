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

using System;
using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
    public class SphereShape : ConvexInternalShape
    {
	    public SphereShape (float radius)
	    {
		    m_shapeType = BroadphaseNativeType.SphereShape;
		    m_implicitShapeDimensions.X  = radius;
		    m_collisionMargin = radius;
	    }
	
	    public override Vector3	LocalGetSupportingVertex(ref Vector3 vec)
        {
	        Vector3 supVertex;
	        supVertex = LocalGetSupportingVertexWithoutMargin(ref vec);

	        Vector3 vecnorm = vec;
	        if (vecnorm.LengthSquared() < (MathUtil.SIMD_EPSILON*MathUtil.SIMD_EPSILON))
	        {
		        vecnorm = new Vector3(-1f);
	        } 
	        vecnorm.Normalize();
	        supVertex+= Margin * vecnorm;
	        return supVertex;
        }

        public override Vector3 LocalGetSupportingVertexWithoutMargin(ref Vector3 vec)
        {
            return Vector3.Zero;
        }

        //notice that the vectors should be unit length
		public override void BatchedUnitVectorGetSupportingVertexWithoutMargin(Vector3[] vectors, Vector4[] supportVerticesOut, int numVectors) 
        {
	        for (int i=0;i<numVectors;i++)
	        {
		        supportVerticesOut[i] = Vector4.Zero;
	        }
        }


        public override void CalculateLocalInertia(float mass, out Vector3 inertia)
        {
            float elem = 0.4f * mass * Margin*Margin;
            inertia = new Vector3(elem);
        }

        public override void GetAabb(ref Matrix t, out Vector3 aabbMin, out Vector3 aabbMax)
        {
	        Vector3 center = t.Translation;
            Vector3 extent = new Vector3(Margin);
	        aabbMin = center - extent;
	        aabbMax = center + extent;
        }

        public virtual float Radius 
        {
            get
            {
                return m_implicitShapeDimensions.X * m_localScaling.X;
            }
        }

	    public void	SetUnscaledRadius(float	radius)
	    {
		    m_implicitShapeDimensions.X = radius;
		    Margin = radius;
	    }

	    //debugging
	    public override String GetName()
        {
            return "SPHERE";
        }


        public override float Margin
	    {
            get
            {
                //to improve gjk behaviour, use radius+margin as the full margin, so never get into the penetration case
                //this means, non-uniform scaling is not supported anymore
                return Radius;
            }
	    }


    }
}
