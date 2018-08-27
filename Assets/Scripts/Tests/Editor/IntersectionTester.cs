//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace IMRE.HandWaver.Solver
//{
//    class IntersectionTester : MonoBehaviour
//    {
//		private void Update()
//		{
//			//if (Input.GetKeyDown(KeyCode.F12))
//			//{
//			//	test2();
//			//}
//			//else if (Input.GetKeyDown(KeyCode.F11))
//			//{
//			//	test();
//			//}
//		}

//		private void test2()
//		{
//			AbstractPoint p1 = GeoObjConstruction.dPoint(Vector3.up+Vector3.up*.5f);
//			AbstractPoint p2 = GeoObjConstruction.dPoint(Vector3.right + Vector3.up * .5f);
//		}

//		private void test()
//        {
//            AbstractPoint p1 = GeoObjConstruction.dPoint(Vector3.up*.5f + Vector3.up * .5f);
//			AbstractPoint p2 = GeoObjConstruction.dPoint(Vector3.right*.5f + Vector3.up * .5f);
			
//            AbstractSphere s1 = GeoObjConstruction.dSphere(p1, p2);
//            AbstractSphere s2 = GeoObjConstruction.dSphere(p2, p1);

//            intersectionFigData fig0 = IntersectionMath.SphereSphereIntersection(s1, s2);

//            AbstractPoint p3 = GeoObjConstruction.dPoint(fig0.vectordata[2]);

//            AbstractSphere s3 = GeoObjConstruction.dSphere(p3,p1);

//            intersectionFigData fig1 = IntersectionMath.SphereSphereIntersection(s1, s2);
//            intersectionFigData fig2 = IntersectionMath.SphereSphereIntersection(s1, s3);
//            intersectionFigData fig3 = IntersectionMath.SphereSphereIntersection(s2, s3);

//            IntersectionMath.CircleCircleIntersection(fig1.vectordata[0], fig2.vectordata[0], fig1.vectordata[1], fig2.vectordata[1], fig1.floatdata[0], fig2.floatdata[0]).printFigDataToDebug();
//        }
//    }
//}
