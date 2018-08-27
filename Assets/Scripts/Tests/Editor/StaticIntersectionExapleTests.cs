
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using NUnit.Framework;
//using UnityEngine.TestTools;
//using System.Runtime.CompilerServices;

//namespace IMRE.HandWaver.Solver
//{
/// <summary>
/// This script does ___.
/// The main contributor(s) to this script is __
/// Status: ???
/// </summary>
//    class StaticIntersectionExampleTests : IntersectionMath
//    {
//        private static float epsilon = .0001f;
//        [Test]
//        public void ThreeSpheresTest()
//        {
//            intersectionFigData twoSpheres0 = SphereSphereIntersection(Vector3.zero, Vector3.up, 1f, 1f);
//            intersectionFigData twoSpheres1 = SphereSphereIntersection(Vector3.zero, twoSpheres0.vectordata[0], 1f, 1f);
//            intersectionFigData twoSpheres2 = SphereSphereIntersection(Vector3.up, twoSpheres0.vectordata[0], 1f, 1f);

//            intersectionFigData twoCircles0 = CircleCircleIntersection(twoSpheres0.vectordata[0], twoSpheres1.vectordata[0], twoSpheres0.vectordata[1], twoSpheres1.vectordata[1], twoSpheres0.floatdata[0], twoSpheres1.floatdata[0]);
//            intersectionFigData twoCircles1 = CircleCircleIntersection(twoSpheres2.vectordata[0], twoSpheres1.vectordata[0], twoSpheres2.vectordata[1], twoSpheres1.vectordata[1], twoSpheres2.floatdata[0], twoSpheres1.floatdata[0]);
//            intersectionFigData twoCircles2 = CircleCircleIntersection(twoSpheres0.vectordata[0], twoSpheres2.vectordata[0], twoSpheres0.vectordata[1], twoSpheres2.vectordata[1], twoSpheres0.floatdata[0], twoSpheres2.floatdata[0]);

//            bool equals01 = ifdEquality(twoCircles0, twoCircles1);
//            bool equals02 = ifdEquality(twoCircles0, twoCircles2);
//            bool equals12 = ifdEquality(twoCircles1, twoCircles2);

//            bool equalsAll = equals01 && equals02 && equals12;

//            Assert.IsTrue(equalsAll);
//        }

//        #region IntersectionFig Data Equality Bools

//        private bool ifdEquality(intersectionFigData result, intersectionFigData expected)
//        {
//            if (result.figtype != expected.figtype)
//            {
//                Debug.LogWarning("FIG TYPE MISMATCH" + "  EXPECTED: " + expected.figtype.ToString() + "  RESULT: " + result.figtype.ToString());
//                return false;
//            }
//            else if (result.floatdata != expected.floatdata || result.vectordata != expected.vectordata)
//            {
//                bool correct = true;
//                if (result.vectordata != expected.vectordata)
//                {
//                    int minLength = expected.vectordata.Length;
//                    if (result.vectordata.Length != minLength)
//                    {
//                        correct = false;
//                        Debug.Log("VECTOR DATA LENGTH MISMATCH");
//                        if (result.vectordata.Length > expected.vectordata.Length)
//                        {
//                            minLength = expected.vectordata.Length;
//                        }
//                        else
//                        {
//                            minLength = result.vectordata.Length;
//                        }
//                    }

//                    for (int i = 0; i < minLength; i++)
//                    {
//                        if ((expected.vectordata[i] - result.vectordata[i]).magnitude < epsilon)
//                        {
//                            Debug.Log("VEC #" + i + "  EXPECTED: " + expected.vectordata[i] + "  RESULT: " + result.vectordata[i]);
//                            correct = false;
//                        }
//                    }

//                    //consider permutations with circle circle intersections resulting in two points.
//                    if (correct == false && expected.figtype == GeoObjType.point && expected.vectordata.Length == 2 && result.vectordata.Length == 2)
//                    {
//                        bool pair1 = (expected.vectordata[0] - result.vectordata[1]).magnitude < epsilon;
//                        bool pair2 = (expected.vectordata[0] - result.vectordata[1]).magnitude < epsilon;
//                        if (pair1 && pair2)
//                        {
//                            correct = true;
//                        }
//                    }
//                }
//                if (result.floatdata != expected.floatdata)
//                {
//                    int minLength = expected.floatdata.Length;
//                    if (result.floatdata.Length != minLength)
//                    {
//                        Debug.Log("FLOAT LENGTH MISMATCH");
//                        correct = false;

//                        if (result.floatdata.Length > expected.floatdata.Length)
//                        {
//                            minLength = expected.floatdata.Length;
//                        }
//                        else
//                        {
//                            minLength = result.floatdata.Length;
//                        }
//                    }

//                    for (int i = 0; i < minLength; i++)
//                    {
//                        if (expected.floatdata[i] - result.floatdata[i] < epsilon)
//                        {
//                            Debug.Log("FLOAT #" + i + "  EXPECTED: " + expected.floatdata[i] + "  RESULT: " + result.floatdata[i]);
//                            correct = false;
//                        }
//                    }
//                }
//                return correct;
//            }
//            else
//            {
//                return true;
//            }
//        }

//        private bool ifdEqualsNone(intersectionFigData result)
//        {
//            return ifdEquality(result, new intersectionFigData() { figtype = GeoObjType.none });
//        }

//        private bool ifdEquality(intersectionFigData result, GeoObjType type, Vector3 vec, float fl)
//        {
//            return ifdEquality(result, type, new Vector3[] { vec }, new float[] { fl });
//        }


//        private bool ifdEquality(intersectionFigData result, GeoObjType type, Vector3 vec)
//        {
//            return ifdEquality(result, type, new Vector3[] { vec });
//        }

//        private bool ifdEquality(intersectionFigData result, GeoObjType type, Vector3[] vecs)
//        {
//            intersectionFigData expected = new intersectionFigData { figtype = type, vectordata = vecs };
//            return ifdEquality(result, expected);
//        }

//        private bool ifdEquality(intersectionFigData result, GeoObjType type, Vector3[] vecs, float[] floats)
//        {
//            intersectionFigData expected = new intersectionFigData { figtype = type, vectordata = vecs, floatdata = floats };
//            return ifdEquality(result, expected);
//        }

//        #endregion

//    }

//}