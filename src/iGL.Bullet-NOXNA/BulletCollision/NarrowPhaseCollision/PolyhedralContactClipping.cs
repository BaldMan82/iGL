﻿#define TEST_INTERNAL_OBJECTS
using System.Diagnostics;
using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
    public static class PolyhedralContactClipping
    {
        public static int gActualSATPairTests = 0;

        public static int gExpectedNbTests = 0;
        public static int gActualNbTests = 0;
        public static bool gUseInternalObject = true;



#if TEST_INTERNAL_OBJECTS

public static void BoxSupport(ref Vector3 extents, ref Vector3 sv, ref Vector3 p)
{
	// This version is ~11.000 cycles (4%) faster overall in one of the tests.
//	IR(p[0]) = IR(extents[0])|(IR(sv[0])&SIGN_BITMASK);
//	IR(p[1]) = IR(extents[1])|(IR(sv[1])&SIGN_BITMASK);
//	IR(p[2]) = IR(extents[2])|(IR(sv[2])&SIGN_BITMASK);
	p[0] = sv[0] < 0.0f ? -extents[0] : extents[0];
	p[1] = sv[1] < 0.0f ? -extents[1] : extents[1];
	p[2] = sv[2] < 0.0f ? -extents[2] : extents[2];
}

public static void InverseTransformPoint3x3(out Vector3 outVec, ref Vector3 input, ref Matrix tr)
{
	IndexedBasisMatrix rot = tr._basis;
	Vector3 r0 = rot[0];
	Vector3 r1 = rot[1];
	Vector3 r2 = rot[2];

	float x = r0.X*input.X + r1.X*input.Y + r2.X*input.Z;
	float y = r0.Y*input.X + r1.Y*input.Y + r2.Y*input.Z;
	float z = r0.Z*input.X + r1.Z*input.Y + r2.Z*input.Z;

	outVec = new Vector3(x, y, z);
}

public static bool TestInternalObjects( ref Matrix trans0, ref Matrix trans1, ref Vector3 delta_c, ref Vector3 axis, ConvexPolyhedron convex0, ConvexPolyhedron convex1, float dmin)
{
	float dp = delta_c.Dot(ref axis);

	Vector3 localAxis0;
	InverseTransformPoint3x3(out localAxis0, ref axis,ref trans0);
	Vector3 localAxis1;
	InverseTransformPoint3x3(out localAxis1, ref axis,ref trans1);

	Vector3 p0 = Vector3.Zero;
    BoxSupport(ref convex0.m_extents, ref localAxis0, ref p0);
    Vector3 p1 = Vector3.Zero;
    BoxSupport(ref convex1.m_extents, ref localAxis1, ref p1);

	float Radius0 = p0[0]*localAxis0.X + p0[1]*localAxis0.Y + p0[2]*localAxis0.Z;
	float Radius1 = p1[0]*localAxis1.X + p1[1]*localAxis1.Y + p1[2]*localAxis1.Z;

	float MinRadius = Radius0>convex0.m_radius ? Radius0 : convex0.m_radius;
	float MaxRadius = Radius1>convex1.m_radius ? Radius1 : convex1.m_radius;

	float MinMaxRadius = MaxRadius + MinRadius;
	float d0 = MinMaxRadius + dp;
	float d1 = MinMaxRadius - dp;

	float depth = d0<d1 ? d0:d1;
    if (depth > dmin)
    {
        return false;
    }
	return true;
}
#endif //TEST_INTERNAL_OBJECTS



        public static void ClipHullAgainstHull(Vector3 separatingNormal, ConvexPolyhedron hullA, ConvexPolyhedron hullB, Matrix transA, Matrix transB, float minDist, float maxDist, IDiscreteCollisionDetectorInterfaceResult resultOut)
        {
            ClipHullAgainstHull(ref separatingNormal, hullA, hullB, ref transA, ref transB, minDist, maxDist, resultOut);
        }

        public static void ClipHullAgainstHull(ref Vector3 separatingNormal1, ConvexPolyhedron hullA, ConvexPolyhedron hullB, ref Matrix transA, ref Matrix transB, float minDist, float maxDist, IDiscreteCollisionDetectorInterfaceResult resultOut)
        {

            Vector3 separatingNormal = separatingNormal1.Normalized();
            Vector3 c0 = transA * hullA.m_localCenter;
            Vector3 c1 = transB * hullB.m_localCenter;
            Vector3 DeltaC2 = c0 - c1;
            float curMaxDist = maxDist;
            int closestFaceB = -1;
            float dmax = float.MinValue;

            {
                for (int face = 0; face < hullB.m_faces.Count; face++)
                {
                    Vector3 Normal = new Vector3(hullB.m_faces[face].m_plane[0], hullB.m_faces[face].m_plane[1], hullB.m_faces[face].m_plane[2]);
                    Vector3 WorldNormal = transB._basis * Normal;

                    float d = Vector3.Dot(WorldNormal, separatingNormal);
                    if (d > dmax)
                    {
                        dmax = d;
                        closestFaceB = face;
                    }
                }
            }


            // setup initial clip face (minimizing face from hull B)
            ObjectArray<Vector3> worldVertsB1 = new ObjectArray<Vector3>();
            {
                Face polyB = hullB.m_faces[closestFaceB];
                int numVertices = polyB.m_indices.Count;
                for (int e0 = 0; e0 < numVertices; e0++)
                {
                    Vector3 b = hullB.m_vertices[polyB.m_indices[e0]];
                    // check this to see if it is transposed version
                    worldVertsB1.Add(transB * b);
                }
            }
            if (closestFaceB >= 0)
            {
                ClipFaceAgainstHull(ref separatingNormal, hullA, ref transA, worldVertsB1, minDist, maxDist, resultOut);
            }

        }

        public static void ClipFaceAgainstHull(Vector3 separatingNormal, ConvexPolyhedron hullA, Matrix transA, ObjectArray<Vector3> worldVertsB1, float minDist, float maxDist, IDiscreteCollisionDetectorInterfaceResult resultOut)
        {
            ClipFaceAgainstHull(ref separatingNormal, hullA, ref transA, worldVertsB1, minDist, maxDist, resultOut);
        }


        public static void ClipFaceAgainstHull(ref Vector3 separatingNormal, ConvexPolyhedron hullA, ref Matrix transA, ObjectArray<Vector3> worldVertsB1, float minDist, float maxDist, IDiscreteCollisionDetectorInterfaceResult resultOut)
        {
            ObjectArray<Vector3> worldVertsB2 = new ObjectArray<Vector3>();
            ObjectArray<Vector3> pVtxIn = worldVertsB1;
            ObjectArray<Vector3> pVtxOut = worldVertsB2;
            pVtxOut.Capacity = pVtxIn.Count;

            int closestFaceA = -1;
            {
                float dmin = float.MaxValue;
                for (int face = 0; face < hullA.m_faces.Count; face++)
                {
                    Vector3 Normal = new Vector3(hullA.m_faces[face].m_plane[0], hullA.m_faces[face].m_plane[1], hullA.m_faces[face].m_plane[2]);
                    Vector3 faceANormalWS = transA._basis * Normal;

                    float d = Vector3.Dot(faceANormalWS, separatingNormal);
                    if (d < dmin)
                    {
                        dmin = d;
                        closestFaceA = face;
                    }
                }
            }
            if (closestFaceA < 0)
                return;

            Face polyA = hullA.m_faces[closestFaceA];

            // clip polygon to back of planes of all faces of hull A that are adjacent to witness face
            int numContacts = pVtxIn.Count;
            int numVerticesA = polyA.m_indices.Count;
            for (int e0 = 0; e0 < numVerticesA; e0++)
            {
		        Vector3 a = hullA.m_vertices[polyA.m_indices[e0]];
                Vector3 b = hullA.m_vertices[polyA.m_indices[(e0 + 1) % numVerticesA]];
                Vector3 edge0 = a - b;
                Vector3 WorldEdge0 = transA._basis * edge0;
                Vector3 worldPlaneAnormal1 = transA._basis * new Vector3(polyA.m_plane[0], polyA.m_plane[1], polyA.m_plane[2]);

                Vector3 planeNormalWS1 = -WorldEdge0.Cross(worldPlaneAnormal1);//.cross(WorldEdge0);
                Vector3 worldA1 = transA * a;
		        float planeEqWS1 = -worldA1.Dot(planeNormalWS1);
		
//int otherFace=0;
#if BLA1
		int otherFace = polyA.m_connectedFaces[e0];
		btVector3 localPlaneNormal (hullA.m_faces[otherFace].m_plane[0],hullA.m_faces[otherFace].m_plane[1],hullA.m_faces[otherFace].m_plane[2]);
		btScalar localPlaneEq = hullA.m_faces[otherFace].m_plane[3];

		btVector3 planeNormalWS = transA.getBasis()*localPlaneNormal;
		btScalar planeEqWS=localPlaneEq-planeNormalWS.dot(transA.getOrigin());
#else 
                Vector3 planeNormalWS = planeNormalWS1;
		float planeEqWS=planeEqWS1;
		
#endif                //clip face

                ClipFace(pVtxIn, pVtxOut, ref planeNormalWS, planeEqWS);

                //btSwap(pVtxIn,pVtxOut);
                ObjectArray<Vector3> temp = pVtxIn;
                pVtxIn = pVtxOut;
                pVtxOut = temp;

                pVtxOut.Clear();
            }



            //#define ONLY_REPORT_DEEPEST_POINT

            Vector3 point;


            // only keep points that are behind the witness face
            {
                Vector3 localPlaneNormal = new Vector3(polyA.m_plane[0], polyA.m_plane[1], polyA.m_plane[2]);
                float localPlaneEq = polyA.m_plane[3];
                Vector3 planeNormalWS = transA._basis * localPlaneNormal;
                float planeEqWS = localPlaneEq - Vector3.Dot(planeNormalWS, transA.Translation);
                for (int i = 0; i < pVtxIn.Count; i++)
                {

                    float depth = Vector3.Dot(planeNormalWS, pVtxIn[i]) + planeEqWS;
                    if (depth <= minDist)
                    {
                        //				printf("clamped: depth=%f to minDist=%f\n",depth,minDist);
                        depth = minDist;
                    }

                    if (depth <= maxDist && depth >= minDist)
                    {
                        Vector3 point2 = pVtxIn[i];
#if ONLY_REPORT_DEEPEST_POINT
				curMaxDist = depth;
#else
#if false
				if (depth<-3)
				{
					printf("error in btPolyhedralContactClipping depth = %f\n", depth);
					printf("likely wrong separatingNormal passed in\n");
				} 
#endif
                        resultOut.AddContactPoint(ref separatingNormal, ref point2, depth);
#endif
                    }
                }
            }
#if ONLY_REPORT_DEEPEST_POINT
	if (curMaxDist<maxDist)
	{
		resultOut.AddContactPoint(ref separatingNormal,ref point,curMaxDist);
	}
#endif //ONLY_REPORT_DEEPEST_POINT


        }

        public static bool FindSeparatingAxis(ConvexPolyhedron hullA, ConvexPolyhedron hullB, Matrix transA, Matrix transB, out Vector3 sep)
        {
            return FindSeparatingAxis(hullA, hullB, ref transA, ref transB, out sep);
        }

        public static bool FindSeparatingAxis(ConvexPolyhedron hullA, ConvexPolyhedron hullB, ref Matrix transA, ref Matrix transB, out Vector3 sep)
        {
            gActualSATPairTests++;
            // dummy value to satisfy exit points.
            sep = new Vector3(0, 1, 0);
#if TEST_INTERNAL_OBJECTS
            Vector3 c0 = transA * hullA.m_localCenter;
            Vector3 c1 = transB * hullB.m_localCenter;
	Vector3 DeltaC2 = c0 - c1;
#endif

            float dmin = float.MaxValue;
            int curPlaneTests = 0;

            int numFacesA = hullA.m_faces.Count;
            // Test normals from hullA
            for (int i = 0; i < numFacesA; i++)
            {
                Vector3 Normal = new Vector3(hullA.m_faces[i].m_plane[0], hullA.m_faces[i].m_plane[1], hullA.m_faces[i].m_plane[2]);
                Vector3 faceANormalWS = transA._basis * Normal;

                curPlaneTests++;
#if TEST_INTERNAL_OBJECTS
		gExpectedNbTests++;
		if(gUseInternalObject && !TestInternalObjects(ref transA,ref transB,ref DeltaC2, ref faceANormalWS, hullA, hullB, dmin))
			continue;
		gActualNbTests++;
#endif

                float d;
                if (!TestSepAxis(hullA, hullB, ref transA, ref transB, ref faceANormalWS, out d))
                    return false;

                if (d < dmin)
                {
                    dmin = d;
                    sep = faceANormalWS;
                }
            }

            int numFacesB = hullB.m_faces.Count;
            // Test normals from hullB
            for (int i = 0; i < numFacesB; i++)
            {
                Vector3 Normal = new Vector3(hullB.m_faces[i].m_plane[0], hullB.m_faces[i].m_plane[1], hullB.m_faces[i].m_plane[2]);
                Vector3 WorldNormal = transB._basis * Normal;

                curPlaneTests++;
#if TEST_INTERNAL_OBJECTS
		gExpectedNbTests++;
		if(gUseInternalObject && !TestInternalObjects(ref transA,ref transB,ref DeltaC2, ref WorldNormal, hullA, hullB, dmin))
			continue;
		gActualNbTests++;
#endif

                float d;
                if (!TestSepAxis(hullA, hullB, ref transA, ref transB, ref WorldNormal, out d))
                    return false;

                if (d < dmin)
                {
                    dmin = d;
                    sep = WorldNormal;
                }
            }

            Vector3 edgeAstart, edgeAend, edgeBstart, edgeBend;

            int curEdgeEdge = 0;
            // Test edges
            for (int e0 = 0; e0 < hullA.m_uniqueEdges.Count; e0++)
            {
                Vector3 edge0 = hullA.m_uniqueEdges[e0];
                Vector3 WorldEdge0 = transA._basis * edge0;
                for (int e1 = 0; e1 < hullB.m_uniqueEdges.Count; e1++)
                {
                    Vector3 edge1 = hullB.m_uniqueEdges[e1];
                    Vector3 WorldEdge1 = transB._basis * edge1;

                    Vector3 Cross = Vector3.Cross(WorldEdge0, WorldEdge1);
                    curEdgeEdge++;
                    if (!MathUtil.IsAlmostZero(ref Cross))
                    {
                        Cross.Normalize();

#if TEST_INTERNAL_OBJECTS
				gExpectedNbTests++;
				if(gUseInternalObject && !TestInternalObjects(ref transA,ref transB,ref DeltaC2, ref Cross, hullA, hullB, dmin))
					continue;
				gActualNbTests++;
#endif

                        float dist;
                        if (!TestSepAxis(hullA, hullB, ref transA, ref transB, ref Cross, out dist))
                        {
                            return false;
                        }

                        if (dist < dmin)
                        {
                            dmin = dist;
                            sep = Cross;
                        }
                    }
                }

            }

            Vector3 deltaC = transB.Translation - transA.Translation;
            if ((Vector3.Dot(deltaC, sep)) > 0.0f)
            {
                sep = -sep;
            }

            return true;

        }

        ///the clipFace method is used internally
        public static void ClipFace(ObjectArray<Vector3> pVtxIn, ObjectArray<Vector3> ppVtxOut, ref Vector3 planeNormalWS, float planeEqWS)
        {
            int ve;
            float ds, de;
            int numVerts = pVtxIn.Count;
            if (numVerts < 2)
                return;

            Vector3 firstVertex = pVtxIn[pVtxIn.Count - 1];
            Vector3 endVertex = pVtxIn[0];

            Vector3.Dot(ref planeNormalWS, ref firstVertex, out ds);
            ds += planeEqWS;

            for (ve = 0; ve < numVerts; ve++)
            {
                endVertex = pVtxIn[ve];

                Vector3.Dot(ref planeNormalWS, ref endVertex, out de);
                de += planeEqWS;

                if (ds < 0)
                {
                    if (de < 0)
                    {
                        // Start < 0, end < 0, so output endVertex
                        ppVtxOut.Add(endVertex);
                    }
                    else
                    {
                        // Start < 0, end >= 0, so output intersection
                        ppVtxOut.Add(MathUtil.Vector3Lerp(ref firstVertex, ref endVertex, (float)(ds * 1.0f / (ds - de))));
                    }
                }
                else
                {
                    if (de < 0)
                    {
                        // Start >= 0, end < 0 so output intersection and end
                        ppVtxOut.Add(MathUtil.Vector3Lerp(ref firstVertex, ref endVertex, (float)(ds * 1.0f / (ds - de))));
                        ppVtxOut.Add(endVertex);
                    }
                }
                firstVertex = endVertex;
                ds = de;
            }

        }

        public static bool TestSepAxis(ConvexPolyhedron hullA, ConvexPolyhedron hullB, ref Matrix transA, ref Matrix transB, ref Vector3 sep_axis, out float depth)
        {
            float Min0, Max0;
            float Min1, Max1;
            hullA.Project(ref transA, ref sep_axis, out Min0, out Max0);
            hullB.Project(ref transB, ref sep_axis, out Min1, out Max1);

            if (Max0 < Min1 || Max1 < Min0)
            {
                depth = 0;
                return false;
            }

            float d0 = Max0 - Min1;
            Debug.Assert(d0 >= 0.0f);
            float d1 = Max1 - Min0;
            Debug.Assert(d1 >= 0.0f);
            depth = d0 < d1 ? d0 : d1;
            return true;
        }


    }
}
