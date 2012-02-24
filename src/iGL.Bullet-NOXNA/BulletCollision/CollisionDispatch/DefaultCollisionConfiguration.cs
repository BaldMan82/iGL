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

namespace BulletXNA.BulletCollision
{
    public class DefaultCollisionConstructionInfo
    {
    }


    public class DefaultCollisionConfiguration : ICollisionConfiguration
    {
    ///btCollisionConfiguration allows to configure Bullet collision detection
    ///stack allocator, pool memory allocators
    ///@todo: describe the meaning

        protected int m_persistentManifoldPoolSize;
        protected bool m_useEpaPenetrationAlgorithm;

    //btStackAlloc*	m_stackAlloc;
    //bool	m_ownsStackAllocator;

    //btPoolAllocator*	m_persistentManifoldPool;
    //bool	m_ownsPersistentManifoldPool;


    //btPoolAllocator*	m_collisionAlgorithmPool;
    //bool	m_ownsCollisionAlgorithmPool;

	//default simplex/penetration depth solvers
	protected VoronoiSimplexSolver m_simplexSolver;
	protected IConvexPenetrationDepthSolver m_pdSolver;
	
	//default CreationFunctions, filling the m_doubleDispatch table
    CollisionAlgorithmCreateFunc m_convexConvexCreateFunc;
    CollisionAlgorithmCreateFunc m_convexConcaveCreateFunc;
    CollisionAlgorithmCreateFunc m_swappedConvexConcaveCreateFunc;
    CollisionAlgorithmCreateFunc m_compoundCreateFunc;
    CollisionAlgorithmCreateFunc m_swappedCompoundCreateFunc;
    CollisionAlgorithmCreateFunc m_emptyCreateFunc;
    CollisionAlgorithmCreateFunc m_sphereSphereCF;
#if USE_BUGGY_SPHERE_BOX_ALGORITHM
    CollisionAlgorithmCreateFunc m_sphereBoxCF;
    CollisionAlgorithmCreateFunc m_boxSphereCF;
#endif //USE_BUGGY_SPHERE_BOX_ALGORITHM

    CollisionAlgorithmCreateFunc m_boxBoxCF;
    CollisionAlgorithmCreateFunc m_sphereTriangleCF;
    CollisionAlgorithmCreateFunc m_triangleSphereCF;
    CollisionAlgorithmCreateFunc m_planeConvexCF;
    CollisionAlgorithmCreateFunc m_convexPlaneCF;
	
    public DefaultCollisionConfiguration() : this(new DefaultCollisionConstructionInfo())
    {
    }
	public DefaultCollisionConfiguration(DefaultCollisionConstructionInfo constructionInfo)
    {
    	m_simplexSolver = new VoronoiSimplexSolver();
        m_pdSolver = new GjkEpaPenetrationDepthSolver();
        //m_pdSolver = new MinkowskiPenetrationDepthSolver();
        m_useEpaPenetrationAlgorithm = true;
//#define USE_EPA 1
//#ifdef USE_EPA
//    mem = btAlignedAlloc(sizeof(btGjkEpaPenetrationDepthSolver),16);
//    m_pdSolver = new (mem)btGjkEpaPenetrationDepthSolver;
//#else
//    mem = btAlignedAlloc(sizeof(btMinkowskiPenetrationDepthSolver),16);
//    m_pdSolver = new (mem)btMinkowskiPenetrationDepthSolver;
//#endif//USE_EPA	
	

	    //default CreationFunctions, filling the m_doubleDispatch table
	    m_convexConvexCreateFunc = new ConvexConvexCreateFunc(m_simplexSolver,m_pdSolver);
	    m_convexConcaveCreateFunc = new ConvexConcaveCreateFunc();
	    m_swappedConvexConcaveCreateFunc = new SwappedConvexConcaveCreateFunc();
	    m_compoundCreateFunc = new CompoundCreateFunc();
	    m_swappedCompoundCreateFunc = new SwappedCompoundCreateFunc();
	    m_emptyCreateFunc = new EmptyCreateFunc();
	
    	m_sphereSphereCF = new SphereSphereCreateFunc();
//#ifdef USE_BUGGY_SPHERE_BOX_ALGORITHM
//    mem = btAlignedAlloc(sizeof(btSphereBoxCollisionAlgorithm::CreateFunc),16);
//    m_sphereBoxCF = new(mem) btSphereBoxCollisionAlgorithm::CreateFunc;
//    mem = btAlignedAlloc(sizeof(btSphereBoxCollisionAlgorithm::CreateFunc),16);
//    m_boxSphereCF = new (mem)btSphereBoxCollisionAlgorithm::CreateFunc;
//    m_boxSphereCF->m_swapped = true;
//#endif //USE_BUGGY_SPHERE_BOX_ALGORITHM

	    m_sphereTriangleCF = new SphereTriangleCreateFunc();
	    m_triangleSphereCF = new SphereTriangleCreateFunc();
	    m_triangleSphereCF.m_swapped = true;
    	
	    m_boxBoxCF = new BoxBoxCreateFunc();

	    //convex versus plane
	    m_convexPlaneCF = new ConvexPlaneCreateFunc();
	    m_planeConvexCF = new ConvexPlaneCreateFunc();
	    m_planeConvexCF.m_swapped = true;
    	
	    ///calculate maximum element size, big enough to fit any collision algorithm in the memory pool
        //int maxSize = sizeof(btConvexConvexAlgorithm);
        //int maxSize2 = sizeof(btConvexConcaveCollisionAlgorithm);
        //int maxSize3 = sizeof(btCompoundCollisionAlgorithm);
        //int sl = sizeof(btConvexSeparatingDistanceUtil);
        //sl = sizeof(btGjkPairDetector);
        //int	collisionAlgorithmMaxElementSize = btMax(maxSize,maxSize2);
        //collisionAlgorithmMaxElementSize = btMax(collisionAlgorithmMaxElementSize,maxSize3);

        //if (constructionInfo.m_stackAlloc)
        //{
        //    m_ownsStackAllocator = false;
        //    this->m_stackAlloc = constructionInfo.m_stackAlloc;
        //} else
        //{
        //    m_ownsStackAllocator = true;
        //    void* mem = btAlignedAlloc(sizeof(btStackAlloc),16);
        //    m_stackAlloc = new(mem)btStackAlloc(constructionInfo.m_defaultStackAllocatorSize);
        //}
    		
        //if (constructionInfo.m_persistentManifoldPool)
        //{
        //    m_ownsPersistentManifoldPool = false;
        //    m_persistentManifoldPool = constructionInfo.m_persistentManifoldPool;
        //} else
        //{
        //    m_ownsPersistentManifoldPool = true;
        //    void* mem = btAlignedAlloc(sizeof(btPoolAllocator),16);
        //    m_persistentManifoldPool = new (mem) btPoolAllocator(sizeof(btPersistentManifold),constructionInfo.m_defaultMaxPersistentManifoldPoolSize);
        //}
    	
        //if (constructionInfo.m_collisionAlgorithmPool)
        //{
        //    m_ownsCollisionAlgorithmPool = false;
        //    m_collisionAlgorithmPool = constructionInfo.m_collisionAlgorithmPool;
        //} else
        //{
        //    m_ownsCollisionAlgorithmPool = true;
        //    void* mem = btAlignedAlloc(sizeof(btPoolAllocator),16);
        //    m_collisionAlgorithmPool = new(mem) btPoolAllocator(collisionAlgorithmMaxElementSize,constructionInfo.m_defaultMaxCollisionAlgorithmPoolSize);
        //}
    }

    public virtual void Cleanup()
    {
    }

		///memory pools
    //virtual btPoolAllocator* getPersistentManifoldPool()
    //{
    //    return m_persistentManifoldPool;
    //}

    //virtual btPoolAllocator* getCollisionAlgorithmPool()
    //{
    //    return m_collisionAlgorithmPool;
    //}

    //virtual btStackAlloc*	getStackAllocator()
    //{
    //    return m_stackAlloc;
    //}

    public virtual VoronoiSimplexSolver GetSimplexSolver()
    {
        return m_simplexSolver;
    }

    public virtual CollisionAlgorithmCreateFunc GetCollisionAlgorithmCreateFunc(BroadphaseNativeType proxyType0, BroadphaseNativeType proxyType1)
    {
	    if ((proxyType0 == BroadphaseNativeType.SphereShape) && (proxyType1==BroadphaseNativeType.SphereShape))
	    {
		    return	m_sphereSphereCF;
	    }
#if USE_BUGGY_SPHERE_BOX_ALGORITHM
	    if ((proxyType0 == BroadphaseNativeType.SphereShape) && (proxyType1==BroadphaseNativeType.BoxShape))
	    {
		    return	m_sphereBoxCF;
	    }

	    if ((proxyType0 == BroadphaseNativeType.BoxShape) && (proxyType1==BroadphaseNativeType.SphereShape))
	    {
		    return	m_boxSphereCF;
	    }
#endif //USE_BUGGY_SPHERE_BOX_ALGORITHM


	    if ((proxyType0 == BroadphaseNativeType.SphereShape ) && (proxyType1==BroadphaseNativeType.TriangleShape))
	    {
		    return	m_sphereTriangleCF;
	    }

	    if ((proxyType0 == BroadphaseNativeType.TriangleShape  ) && (proxyType1==BroadphaseNativeType.SphereShape))
	    {
		    return	m_triangleSphereCF;
	    } 

	    if ((proxyType0 == BroadphaseNativeType.BoxShape) && (proxyType1 == BroadphaseNativeType.BoxShape))
	    {
		    return m_boxBoxCF;
	    }
    	
	    if (BroadphaseProxy.IsConvex(proxyType0) && (proxyType1 == BroadphaseNativeType.StaticPlane))
	    {
		    return m_convexPlaneCF;
	    }

	    if (BroadphaseProxy.IsConvex(proxyType1) && (proxyType0 == BroadphaseNativeType.StaticPlane))
	    {
		    return m_planeConvexCF;
	    }
	    if (BroadphaseProxy.IsConvex(proxyType0) && BroadphaseProxy.IsConvex(proxyType1))
	    {
		    return m_convexConvexCreateFunc;
	    }

	    if (BroadphaseProxy.IsConvex(proxyType0) && BroadphaseProxy.IsConcave(proxyType1))
	    {
		    return m_convexConcaveCreateFunc;
	    }

	    if (BroadphaseProxy.IsConvex(proxyType1) && BroadphaseProxy.IsConcave(proxyType0))
	    {
		    return m_swappedConvexConcaveCreateFunc;
	    }

	    if (BroadphaseProxy.IsCompound(proxyType0))
	    {
		    return m_compoundCreateFunc;
	    } 
        else
	    {
		    if (BroadphaseProxy.IsCompound(proxyType1))
		    {
			    return m_swappedCompoundCreateFunc;
		    }
	    }

	    //failed to find an algorithm
	    return m_emptyCreateFunc;
    }

	///Use this method to allow to generate multiple contact points between at once, between two objects using the generic convex-convex algorithm.
	///By default, this feature is disabled for best performance.
	///@param numPerturbationIterations controls the number of collision queries. Set it to zero to disable the feature.
	///@param minimumPointsPerturbationThreshold is the minimum number of points in the contact cache, above which the feature is disabled
	///3 is a good value for both params, if you want to enable the feature. This is because the default contact cache contains a maximum of 4 points, and one collision query at the unperturbed orientation is performed first.
	///See Bullet/Demos/CollisionDemo for an example how this feature gathers multiple points.
	///@todo we could add a per-object setting of those parameters, for level-of-detail collision detection.
	    public void	SetConvexConvexMultipointIterations()
        {
            SetConvexConvexMultipointIterations(3,3);
        }
        public void	SetConvexConvexMultipointIterations(int numPerturbationIterations, int minimumPointsPerturbationThreshold)
        {
            ConvexConvexCreateFunc convexConvex = (ConvexConvexCreateFunc)m_convexConvexCreateFunc;
            convexConvex.m_numPerturbationIterations = numPerturbationIterations;
            convexConvex.m_minimumPointsPerturbationThreshold = minimumPointsPerturbationThreshold;
        }

        public void SetPlaneConvexMultipointIterations()
        {
            SetPlaneConvexMultipointIterations(3, 3);
        }
        public void SetPlaneConvexMultipointIterations(int numPerturbationIterations, int minimumPointsPerturbationThreshold)
        {
            ConvexPlaneCreateFunc planeCreateFunc = (ConvexPlaneCreateFunc)m_planeConvexCF;
	        planeCreateFunc.m_numPerturbationIterations = numPerturbationIterations;
	        planeCreateFunc.m_minimumPointsPerturbationThreshold = minimumPointsPerturbationThreshold;
        }
    }
}
