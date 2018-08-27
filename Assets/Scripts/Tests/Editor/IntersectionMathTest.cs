
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
//    class IntersectionMathTest : IntersectionMath
//    {
//        #region LEVEL 1 TESTS

//        private static float epsilon = .0001f;

//        [Test]
//        public void planePlaneTest()
//        {
//            Vector3 n1 = UnityEngine.Random.onUnitSphere;
//            Vector3 nLine = Vector3.ProjectOnPlane(UnityEngine.Random.onUnitSphere,n1).normalized;
//            Vector3 p1 = Random.insideUnitSphere;
//            Vector3 pLine = Vector3.ProjectOnPlane(Random.insideUnitSphere, n1) + p1;

//            Vector3 n2 = Vector3.Cross(n1, nLine).normalized;
//            Vector3 p2 = Vector3.Project(Random.insideUnitSphere, n2) + pLine;

//            intersectionFigData result = PlanePlaneIntersection(n1, n2, p1, p2);
//            result.printFigDataToDebug();

//            Assert.IsTrue(ifdEquality(result, GeoObjType.line,new Vector3[] {pLine, nLine }));
//        }

//        [Test]
//        public void linePlaneTest()
//        {
//            Vector3 a = Random.insideUnitSphere;
//            Vector3 b = Random.insideUnitSphere;
//            while(withinEpsilon((a-b).magnitude, 0))
//            {
//                b = Random.insideUnitSphere;
//            }
//            Vector3 c = Random.insideUnitSphere;
//            Vector3 d = Random.insideUnitSphere;
//            Vector3 e = (a - b).normalized * Random.value + a;

//            //make sure that the three points define a plane, where ab is not colinear
//            while (withinEpsilon(Vector3.Cross(c - d, c - e).magnitude,0) || withinEpsilon(Vector3.Project(a-b, Vector3.Cross(c - d, c - e)).magnitude, 0))
//            {
//                c = Random.insideUnitSphere;
//                d = Random.insideUnitSphere;
//            }

//            intersectionFigData result = LinePlaneIntersection(a, (a - b).normalized, c, Vector3.Cross(c - d, c - e).normalized);

//            result.printFigDataToDebug();

//            Assert.IsTrue(ifdEquality(result,GeoObjType.point,e));
//        }

//        [Test]
//        public void segmentPlaneTest()
//        {
//            Vector3 a = Random.insideUnitSphere;
//            Vector3 b = Random.insideUnitSphere;
//            while ( withinEpsilon( (a - b).magnitude, 0))
//            {
//                b = Random.insideUnitSphere;
//            }
//            Vector3 c = Random.insideUnitSphere;
//            Vector3 d = Random.insideUnitSphere;
//            Vector3 e = (a - b).normalized * Random.value + a;

//            //make sure that the three points define a plane, where ab is not colinear
//            while ( withinEpsilon( Vector3.Cross(c - d, c - e).magnitude, 0) || withinEpsilon( Vector3.Project(a - b, Vector3.Cross(c - d, c - e)).magnitude, 0))
//            {
//                c = Random.insideUnitSphere;
//                d = Random.insideUnitSphere;
//            }

//            intersectionFigData result = SegmentPlaneIntersection(a, b, c, Vector3.Cross(c - d, c - e).normalized);

//            result.printFigDataToDebug();

//            //check to see if the point falls within the segment.
//            if (withinEpsilon ((e-a).magnitude + (e-b).magnitude, (a - b).magnitude))            {
//                Assert.IsTrue(ifdEquality(result, GeoObjType.point, e));
//            }
//            else
//            {
//                Assert.IsTrue(ifdEqualsNone(result));
//            }

//        }

//        [Test]
//        public void sphereSphereToPoint()
//        {
//            Vector3 expected = Random.insideUnitSphere;
//            Vector3 sphereCenter1 = Random.insideUnitSphere;

//            float radius1 = (sphereCenter1 - expected).magnitude;
//            float radius2 = Random.Range(.01f,100f);

//            Vector3 sphereCenter2 = (sphereCenter1 - expected) * (radius1 + radius2);

//            intersectionFigData result = SphereSphereIntersection(sphereCenter1, sphereCenter2, radius1, radius2);

//            result.printFigDataToDebug();

//            Assert.IsTrue(ifdEquality(result,GeoObjType.point,expected));
//        }

//        [Test]
//        public void sphereSphereToCircle()
//        {
//            Vector3 sphereCenter1 = Random.insideUnitSphere;
//            Vector3 sphereCenter2 = Random.insideUnitSphere;

//            float dist1 = Random.value*(sphereCenter1-sphereCenter2).magnitude;
//            float dist2 = (sphereCenter1 - sphereCenter2).magnitude - dist1;

//            Vector3 expectedCenter = sphereCenter1 + (sphereCenter2 - sphereCenter1) * dist1;

//            float expectedRadius = Random.value * (sphereCenter1 - sphereCenter2).magnitude;

//            float radius1 = Mathf.Sqrt(dist1 * dist1 + expectedRadius * expectedRadius);
//            float radius2 = Mathf.Sqrt(dist2 * dist2 + expectedRadius * expectedRadius);

//            intersectionFigData result = SphereSphereIntersection(sphereCenter1, sphereCenter2, radius1, radius2);

//            result.printFigDataToDebug();

//            Assert.IsTrue(ifdEquality(result, GeoObjType.circle, new Vector3[] { expectedCenter, (sphereCenter1 - sphereCenter2).normalized }, new float[] { expectedRadius }));
//        }

//        [Test]
//        public void PlaneSphereTest()
//        {
//            Vector3 SpherePos = Random.insideUnitSphere;
//            Vector3 planePos = Random.insideUnitSphere;
//            Vector3 planeNorm = Random.onUnitSphere;
//            float SphereRadius = Random.Range(.001f,50f);

//            Vector3 expectedCenter = Vector3.Project((planePos - SpherePos), planeNorm) + SpherePos;
//            Vector3 expectedNorm = (planePos - SpherePos).normalized;
//            float expectedRadius = Mathf.Sqrt(SphereRadius * SphereRadius - Vector3.Project((planePos - SpherePos), planeNorm).magnitude * Vector3.Project((planePos - SpherePos), planeNorm).magnitude);

//            intersectionFigData result = SpherePlaneIntersection(SpherePos, SphereRadius, planeNorm, planePos);

//            result.printFigDataToDebug(); 

//            if( withinEpsilon( SphereRadius , Vector3.Project(SpherePos - planePos, planeNorm).magnitude))
//            {
//                Assert.IsTrue(ifdEquality(result, GeoObjType.point, expectedCenter));
//            }else if (SphereRadius > Vector3.Project(SpherePos - planePos, planeNorm).magnitude)
//            {
//                Assert.IsTrue(ifdEquality(result, GeoObjType.circle, new Vector3[] { expectedCenter, expectedNorm }, new float[] { expectedRadius }));
//            }
//            else
//            {
//                Assert.IsTrue(ifdEqualsNone(result));
//            }

//        }

//        #endregion

//        #region LEVEL 2 TESTS
//        [Test]
//        public void LineLineTest()
//        {
//            Vector3 expectedPoint = Random.insideUnitSphere;
//            Vector3 p1 = Random.insideUnitSphere;
//            Vector3 p2 = Random.insideUnitSphere;

//            Vector3 n1 = (expectedPoint - p1).normalized;
//            Vector3 n2 = (expectedPoint - p2).normalized;

//            intersectionFigData result = LineLineIntersection(p1, p2, n1, n2);

//            result.printFigDataToDebug();

//            Assert.True(ifdEquality(result, GeoObjType.point, expectedPoint));
//        }

//        [Test]
//        public void CircleLineTest()
//        {
//            Vector3[] expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere };
//            while( withinEpsilon( (expectedPoints[0] - expectedPoints[1]).magnitude, 0)) { expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere }; }
//            Vector3 lDir = (expectedPoints[0] - expectedPoints[1]).normalized;
//            Vector3 lCenter = Vector3.Project(Random.insideUnitSphere, lDir) + expectedPoints[0];
//            float cRadius = Random.Range((expectedPoints[0] - expectedPoints[1]).magnitude / 2f, 10f);

//            //for starters, put the line coplanar with the circle.
//            Vector3 cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while(withinEpsilon(cNorm.magnitude, 0)) { cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            // Use current cNorm value to construct a isos triangle, where the congruant sides are radii, and the remaining is a segment of the line.
//            float s = (expectedPoints[0] - expectedPoints[1]).magnitude;
//            float h = Mathf.Sqrt(cRadius * cRadius - s * s);

//            Vector3 cCenter = ((expectedPoints[0] - expectedPoints[1]) / 2f) + cNorm.normalized * h;

//            //the other cNorm is now in the plane, reset Cnorm to be the new planes norm.
//            cNorm = Vector3.Cross(cNorm, lDir);

//            intersectionFigData result = CircleLineIntersection(lCenter, lDir, cCenter, cNorm, cRadius);

//            result.printFigDataToDebug();

//            Assert.True(ifdEquality(result, GeoObjType.point, expectedPoints));
//        }
//        #endregion

//        #region Level 3 Tests
//        [Test]
//        public void CircleCircleTest()
//        {
//            Vector3[] expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere };
//            while ( withinEpsilon((expectedPoints[0] - expectedPoints[1]).magnitude, 0)) { expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere }; }
//            Vector3 lDir = (expectedPoints[0] - expectedPoints[1]).normalized;
//            Vector3 lCenter = Vector3.Project(Random.insideUnitSphere, lDir) + expectedPoints[0];
//            float cRadius = Random.Range((expectedPoints[0] - expectedPoints[1]).magnitude / 2f, 10f);

//            //for starters, put the line coplanar with the circle.
//            Vector3 cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while ( withinEpsilon(cNorm.magnitude, 0)) { cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            // Use current cNorm value to construct a isos triangle, where the congruant sides are radii, and the remaining is a segment of the line.
//            float s = (expectedPoints[0] - expectedPoints[1]).magnitude;
//            float h = Mathf.Sqrt(cRadius * cRadius - s * s);

//            Vector3 cCenter = ((expectedPoints[0] - expectedPoints[1]) / 2f) + cNorm.normalized * h;

//            //the other cNorm is now in the plane, reset Cnorm to be the new planes norm.
//            cNorm = Vector3.Cross(cNorm, lDir);

//            float cRadius2 = Random.Range((expectedPoints[0] - expectedPoints[1]).magnitude / 2f, 10f);

//            //for starters, put the line coplanar with the circle.
//            Vector3 cNorm2 = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while (withinEpsilon(cNorm.magnitude, 0)) { cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            // Use current cNorm value to construct a isos triangle, where the congruant sides are radii, and the remaining is a segment of the line.
//            float s2 = (expectedPoints[0] - expectedPoints[1]).magnitude;
//            float h2 = Mathf.Sqrt(cRadius2 * cRadius2 - s * s);

//            Vector3 cCenter2 = ((expectedPoints[0] - expectedPoints[1]) / 2f) + cNorm2.normalized * h2;

//            //the other cNorm is now in the plane, reset Cnorm to be the new planes norm.
//            cNorm2 = Vector3.Cross(cNorm2, lDir);

//            intersectionFigData result = CircleCircleIntersection(cCenter, cCenter2, cNorm, cNorm2, cRadius, cRadius2);

//            result.printFigDataToDebug();

//            Assert.True(ifdEquality(result, GeoObjType.point, expectedPoints));
//        }

//        [Test]
//        public void CirclePlaneTest()
//        {
//            Vector3[] expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere };
//            while (withinEpsilon((expectedPoints[0] - expectedPoints[1]).magnitude, 0)) { expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere }; }
//            Vector3 lDir = (expectedPoints[0] - expectedPoints[1]).normalized;
//            Vector3 lCenter = Vector3.Project(Random.insideUnitSphere, lDir) + expectedPoints[0];
//            float cRadius = Random.Range((expectedPoints[0] - expectedPoints[1]).magnitude / 2f, 10f);

//            //for starters, put the line coplanar with the circle.
//            Vector3 cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while (withinEpsilon(cNorm.magnitude, 0)) { cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            // Use current cNorm value to construct a isos triangle, where the congruant sides are radii, and the remaining is a segment of the line.
//            float s = (expectedPoints[0] - expectedPoints[1]).magnitude;
//            float h = Mathf.Sqrt(cRadius * cRadius - s * s);

//            Vector3 cCenter = ((expectedPoints[0] - expectedPoints[1]) / 2f) + cNorm.normalized * h;

//            //the other cNorm is now in the plane, reset Cnorm to be the new planes norm.
//            cNorm = Vector3.Cross(cNorm, lDir);

//            Vector3 pNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while ( withinEpsilon(pNorm.magnitude, 0)) { pNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            Vector3 pCenter = Vector3.ProjectOnPlane(Random.onUnitSphere, pNorm) + expectedPoints[0];

//            intersectionFigData result = CirclePlaneIntersection(cCenter, cNorm, cRadius, pCenter, pNorm);

//            result.printFigDataToDebug();

//            Assert.True(ifdEquality(result, GeoObjType.point, expectedPoints));
//        }

//        [Test]
//        public void SphereLineTest()
//        {
//            Vector3[] expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere };
//            while (withinEpsilon ((expectedPoints[0] - expectedPoints[1]).magnitude, 0)) { expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere }; }
//            Vector3 lDir = (expectedPoints[0] - expectedPoints[1]).normalized;
//            Vector3 lCenter = Vector3.Project(Random.insideUnitSphere, lDir) + expectedPoints[0];
//            float cRadius = Random.Range((expectedPoints[0] - expectedPoints[1]).magnitude / 2f, 10f);

//            //for starters, put the line coplanar with the circle.
//            Vector3 cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while (withinEpsilon(cNorm.magnitude, 0)) { cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            // Use current cNorm value to construct a isos triangle, where the congruant sides are radii, and the remaining is a segment of the line.
//            float s = (expectedPoints[0] - expectedPoints[1]).magnitude;
//            float h = Mathf.Sqrt(cRadius * cRadius - s * s);

//            Vector3 cCenter = ((expectedPoints[0] - expectedPoints[1]) / 2f) + cNorm.normalized * h;

//            intersectionFigData result = SphereLineIntersection(cCenter, cRadius, lCenter, lDir);

//            result.printFigDataToDebug();

//            Assert.True(ifdEquality(result, GeoObjType.point, expectedPoints));
//        }
//        #endregion

//        #region LEVEL 4 TESTS
//        [Test]
//        public void SphereCircleTest()
//        {
//            Vector3[] expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere };
//            while (withinEpsilon((expectedPoints[0] - expectedPoints[1]).magnitude, 0)) { expectedPoints = new Vector3[] { Random.insideUnitSphere, Random.insideUnitSphere }; }
//            Vector3 lDir = (expectedPoints[0] - expectedPoints[1]).normalized;
//            Vector3 lCenter = Vector3.Project(Random.insideUnitSphere, lDir) + expectedPoints[0];
//            float cRadius = Random.Range((expectedPoints[0] - expectedPoints[1]).magnitude / 2f, 10f);

//            //for starters, put the line coplanar with the circle.
//            Vector3 cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while ( withinEpsilon( cNorm.magnitude, 0)) { cNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            // Use current cNorm value to construct a isos triangle, where the congruant sides are radii, and the remaining is a segment of the line.
//            float s = (expectedPoints[0] - expectedPoints[1]).magnitude;
//            float h = Mathf.Sqrt(cRadius * cRadius - s * s);

//            Vector3 cCenter = ((expectedPoints[0] - expectedPoints[1]) / 2f) + cNorm.normalized * h;

//            //the other cNorm is now in the plane, reset Cnorm to be the new planes norm.
//            cNorm = Vector3.Cross(cNorm, lDir);

//            Vector3 pNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized;
//            while (withinEpsilon(pNorm.magnitude, 0)) { pNorm = Vector3.Cross(Random.onUnitSphere, lDir).normalized; }

//            Vector3 pCenter = Vector3.ProjectOnPlane(Random.onUnitSphere, pNorm) + expectedPoints[0];

//            intersectionFigData result = SpherePlaneIntersection(cCenter, cRadius, pNorm, pCenter);

//            result.printFigDataToDebug();

//            Assert.True(ifdEquality(result, GeoObjType.point, expectedPoints));
//        }
//        #endregion

//        #region IntersectionFig Data Equality Bools

//        private bool ifdEquality(intersectionFigData result, intersectionFigData expected)
//        {
//            bool correct = true;

//            //crude check
//            if (result.figtype != expected.figtype)
//            {
//                Debug.LogWarning("FIG TYPE MISMATCH" + "  EXPECTED: " + expected.figtype.ToString() + "  RESULT: " + result.figtype.ToString());
//                correct = false;
//            }
//            else
//            {
//                //the crude check should be sufficient for figure type NONE and POINT.
//                switch (expected.figtype)
//                {
//                    case GeoObjType.line:
//                        correct = checkLineEquality(expected, result);
//                        break;
//                    case GeoObjType.flatface:
//                        correct = checkPlaneEquality(expected, result);
//                        break;
//                    case GeoObjType.circle:
//                        correct = checkCircleEquality(expected, result);
//                        break;
//                    case GeoObjType.sphere:
//                        correct = checkSphereEquality(expected, result);
//                        break;
//                    case GeoObjType.point:
//                        correct = checkPointEquality(expected, result);
//                        break;
//                    case GeoObjType.none:
//                        correct = true;
//                        break;
//                }
//            }
//            return correct;
//        }
//        #region Shape Specific Logic
//        private bool checkSphereEquality(intersectionFigData expected, intersectionFigData result)
//        {
//            Vector3 expectedCenter = expected.vectordata[0];
//            Vector3 resultCenter = result.vectordata[0];

//            float expectedRadius = result.floatdata[0];
//            float resultRadius = result.floatdata[0];

//            //radius should be equal;
//            bool checkRadius = Mathf.Abs(expectedRadius - resultRadius) < epsilon;
//            if (!checkRadius)
//            {
//                Debug.Log("Sphere Radii NOT EQUAL.  EXPECTED : " + expectedRadius + " RESULT: " + resultRadius + " ABS DIFF: " + Mathf.Abs(expectedRadius - resultRadius));
//            }

//            //center should be equal;
//            bool checkCenter = (expectedCenter - resultCenter).magnitude < epsilon;

//            if (!checkCenter)
//            {
//                Debug.Log("Sphere Centers NOT EQUAL.  EXPECTED : " + expectedCenter + " RESULT: " + resultCenter + " ABS DIFF: " + (expectedCenter - resultCenter).magnitude);
//            }

//            return checkRadius && checkCenter;
//        }

//        private bool checkCircleEquality(intersectionFigData expected, intersectionFigData result)
//        {
//            Vector3 expectedPosition = expected.vectordata[0];
//            Vector3 expectedNormal = expected.vectordata[1];
//            float expectedRadius = expected.floatdata[0];
//            Vector3 resultPosition = result.vectordata[0];
//            Vector3 resultNormal = result.vectordata[1];
//            float resultRadius = result.floatdata[0];

//            //centers should be equal;
//            bool checkCenters = (expectedPosition - resultPosition).magnitude < epsilon;

//            if (!checkCenters)
//            {
//                Debug.Log("Circle Centers NOT EQUAL.  EXPECTED : " + expectedPosition + " RESULT: " + resultPosition + " ABS DIFF: " + (expectedPosition - resultPosition).magnitude);
//            }

//            //radius should be equal;
//            bool checkRadius = Mathf.Abs(expectedRadius - resultRadius) < epsilon;
//            if (!checkRadius)
//            {
//                Debug.Log("Circle Radii NOT EQUAL.  EXPECTED : " + expectedRadius + " RESULT: " + resultRadius + " ABS DIFF: " + Mathf.Abs(expectedRadius - resultRadius));
//            }

//            //consider equal normals but opposite
//            bool checkNormals = Vector3.Angle(expectedNormal, resultNormal) < epsilon || Vector3.Angle(expectedNormal,-resultNormal) < epsilon;
//            if (!checkNormals)
//            {
//                Debug.Log("Circle Normals NOT EQUAL.  EXPECTED : " + expectedNormal + " RESULT: " + resultNormal + " ABS DIFF: " + (expectedNormal - resultNormal).magnitude);
//            }

//            return checkCenters && checkRadius && checkNormals;
//        }

//        private bool checkPointEquality(intersectionFigData expected, intersectionFigData result)
//        {
//            if (expected.vectordata.Length == 1)
//            {
//                return ((expected.vectordata[0] - result.vectordata[0]).magnitude < epsilon);
//            }
//            else
//            {
//                bool perm1 = ((expected.vectordata[0] - result.vectordata[0]).magnitude < epsilon) && ((expected.vectordata[1] - result.vectordata[1]).magnitude < epsilon);
//                bool perm2 = ((expected.vectordata[0] - result.vectordata[1]).magnitude < epsilon) && ((expected.vectordata[1] - result.vectordata[0]).magnitude < epsilon);
//                return perm1 || perm2;
//            }
//        }

//        private bool checkPlaneEquality(intersectionFigData expected, intersectionFigData result)
//        {
//            //planes.
//            Vector3 expectedPosition = expected.vectordata[0];
//            Vector3 expectedNormal = expected.vectordata[1];
//            Vector3 resultPosition = result.vectordata[0];
//            Vector3 resultNormal = result.vectordata[1];

//            //consider opposite but equal normals
//            bool checkNormals = Vector3.Angle(expectedNormal, resultNormal) < epsilon || Vector3.Angle(expectedNormal,-resultNormal) < epsilon;
//            if (!checkNormals)
//            {
//                Debug.Log("Plane Normals NOT EQUAL.  EXPECTED : " + expectedNormal + " RESULT: " + resultNormal + " ABS DIFF: " + (expectedNormal - resultNormal).magnitude);
//            }
//            //consider positions on same plane
//            bool checkPosition = Vector3.Project(expectedPosition - resultPosition, expectedNormal).magnitude < epsilon;
//            if (!checkPosition)
//            {
//                Debug.Log("Plane Centers NOT EQUAL.  EXPECTED : " + expectedPosition + " RESULT: " + resultPosition + " ABS DIFF: " + (expectedPosition - resultPosition).magnitude);
//            }
//            return checkNormals && checkPosition;
//        }

//        private bool checkLineEquality(intersectionFigData expected, intersectionFigData result)
//        {
//            Vector3 expectedPosition = expected.vectordata[0];
//            Vector3 expectedDirection = expected.vectordata[1];
//            Vector3 resultPosition = result.vectordata[0];
//            Vector3 resultDirection = result.vectordata[1];

//            //consider multiple posisitions on the same line
//            bool positionCheck = Vector3.ProjectOnPlane(expectedPosition - resultPosition, expectedDirection).magnitude < epsilon;
//            if (!positionCheck)
//            {
//                Debug.Log("Line Centers NOT EQUAL.  EXPECTED : " + expectedPosition + " RESULT: " + resultPosition + " ABS DIFF: " + (expectedPosition - resultPosition).magnitude);
//            }

//            //consider opposite but equivalent directions.
//            bool directionCheck = Vector3.Angle(expectedDirection, resultDirection) < epsilon || Vector3.Angle(expectedDirection,-resultDirection) < epsilon;
//            if (!directionCheck)
//            {
//                Debug.Log("Line Direction NOT EQUAL.  EXPECTED : " + expectedDirection + " RESULT: " + expectedPosition + " ABS DIFF: " + (expectedDirection - expectedPosition).magnitude);
//            }
//            return positionCheck && directionCheck;
//        }
//        #endregion  
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