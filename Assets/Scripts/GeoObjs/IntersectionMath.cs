/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
using UnityEngine;

namespace IMRE.HandWaver.Solver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	internal struct intersectionFigData
    {
        internal GeoObjType figtype;
        internal Vector3[] vectordata;
        internal float[] floatdata;

        internal string printFigDataToDebug()
        {
            string message = figtype.ToString();
            if (figtype != GeoObjType.none)
            {
                if (vectordata != null)
                {
                    foreach (Vector3 vec in vectordata)
                    {
                        message += ",  VECTOR: " + vec.ToString();
                    }
                }
                if (floatdata != null)
                {
                    foreach (float f in floatdata)
                    {
                        message += ",  FLOAT: " + f.ToString();
                    }
                }
            }
            //Debug.Log(message);
            return message;

        }
    }

    internal class IntersectionMath
    {
        internal static bool withinEpsilon(float value1, float value2)
        {
            return Mathf.Abs(value1 - value2) < .0001f;
        }

        internal static intersectionFigData SphereLineIntersection(AbstractSphere sphere, straightEdgeBehave line)
        {
            return SphereLineIntersection(sphere.centerPosition, sphere.radius, line.center, line.normalDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        internal static intersectionFigData SphereLineIntersection(AbstractSphere sphere, AbstractLineSegment line)
        {
            return SphereLineIntersection(sphere.centerPosition, sphere.radius, line.vertex0, line.vertex0 - line.vertex1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spherePos"></param>
        /// <param name="sphereRadius"></param>
        /// <param name="linePos"></param>
        /// <param name="LineDir"></param>
        /// <returns></returns>
        internal static intersectionFigData SphereLineIntersection(Vector3 spherePos, float sphereRadius, Vector3 linePos, Vector3 lineDir)
        {
            Vector3 planeNorm = Vector3.right;
            if (Vector3.Cross(planeNorm,Vector3.right).magnitude == 0)
            {
                planeNorm = Vector3.up;
            }
            planeNorm = Vector3.Cross(planeNorm, lineDir);
            planeNorm = planeNorm / planeNorm.magnitude;

            intersectionFigData intersectData = SpherePlaneIntersection(spherePos, sphereRadius, planeNorm, linePos);
            if (intersectData.figtype == GeoObjType.circle)
            {
                //circle is produced, check against line.
                return CircleLineIntersection(linePos, lineDir, intersectData.vectordata[0], intersectData.vectordata[1], intersectData.floatdata[0]);
            }
            else
            {
                //no intersection
                return new intersectionFigData() { figtype = GeoObjType.none };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="circle"></param>
        /// <returns></returns>
        internal static intersectionFigData SphereCircleIntersection(AbstractSphere sphere, AbstractCircle circle)
        {
            return SphereCircleIntersection(sphere.centerPosition, sphere.radius, circle.centerPos, circle.normalDir, circle.Radius);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spherePos"></param>
        /// <param name="sphereRadius"></param>
        /// <param name="circlePos"></param>
        /// <param name="circleNorm"></param>
        /// <param name="circleRadius"></param>
        /// <returns></returns>
        internal static intersectionFigData SphereCircleIntersection(Vector3 spherePos, float sphereRadius, Vector3 circlePos, Vector3 circleNorm, float circleRadius)
        {
            intersectionFigData planeSphere = SpherePlaneIntersection(spherePos, sphereRadius, circleNorm, circlePos);
            if (planeSphere.figtype == GeoObjType.circle)
            {
                return CircleCircleIntersection(planeSphere.vectordata[0], circlePos, planeSphere.vectordata[1], circleNorm, planeSphere.floatdata[0], circleRadius);
            }
            else
            {
                //no intersection
                return new intersectionFigData() { figtype = GeoObjType.none };
            }
        }


        internal static intersectionFigData SpherePlaneIntersection(AbstractSphere sphere, flatfaceBehave flatface)
        {
            return SpherePlaneIntersection(sphere, flatface.Position3, flatface.normalDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="planeNorm"></param>
        /// <param name="planePos"></param>
        /// <returns></returns>
        internal static intersectionFigData SpherePlaneIntersection(AbstractSphere sphere, Vector3 planeNorm, Vector3 planePos)
        {
            return SpherePlaneIntersection(sphere.centerPosition, sphere.radius, planeNorm, planePos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spherePos"></param>
        /// <param name="sphereRadius"></param>
        /// <param name="planeNorm"></param>
        /// <param name="planePos"></param>
        /// <returns></returns>
        internal static intersectionFigData SpherePlaneIntersection(Vector3 spherePos, float sphereRadius, Vector3 planeNorm, Vector3 planePos)
        {
            Vector3 circleCenter = Vector3.Project(planePos-spherePos, planeNorm) + spherePos;
           // Vector3 circleNorm = planeNorm; NEVER USED

            //check the radius
            if (sphereRadius == (circleCenter - spherePos).magnitude)
            {
                //one point to return
                return new intersectionFigData() { figtype = GeoObjType.point, vectordata = new Vector3[] { circleCenter } };
            }
            else if (sphereRadius < (circleCenter - spherePos).magnitude)
            {
                //float circleRadius = Mathf.Sqrt((circleCenter - spherePos).magnitude * (circleCenter - spherePos).magnitude + sphereRadius * sphereRadius);
                Vector3 position2 = spherePos + 2f * (circleCenter - spherePos);
                return SphereSphereIntersection(spherePos, position2, sphereRadius, sphereRadius);
                //return new intersectionFigData() { figtype = GeoObjType.circle, vectordata = new Vector3[] { circleCenter, circleNorm }, floatdata = new float[] { circleRadius } };
            }
            else
            {
                //no intersection
                return new intersectionFigData() { figtype = GeoObjType.none };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="flatface"></param>
        /// <returns></returns>
        internal static intersectionFigData CirclePlaneIntersection(AbstractCircle circle, flatfaceBehave flatface)
        {
            return CirclePlaneIntersection(circle, flatface.Position3, flatface.normalDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="planePos"></param>
        /// <param name="planeNorm"></param>
        /// <returns></returns>
        internal static intersectionFigData CirclePlaneIntersection(AbstractCircle circle, Vector3 planePos, Vector3 planeNorm)
        {
            return CirclePlaneIntersection(circle.centerPos, circle.normalDir, circle.Radius, planePos, planeNorm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="circlePos"></param>
        /// <param name="circleNorm"></param>
        /// <param name="radius"></param>
        /// <param name="planePos"></param>
        /// <param name="planeNorm"></param>
        /// <returns></returns>
        internal static intersectionFigData CirclePlaneIntersection(Vector3 circlePos, Vector3 circleNorm, float circleRadius, Vector3 planePos, Vector3 planeNorm)
        {
            intersectionFigData figDat = PlanePlaneIntersection(circlePos, circleNorm, planePos, planeNorm);
            Vector3[] lineData = figDat.vectordata;
            if (figDat.figtype == GeoObjType.line) {
                return CircleLineIntersection(lineData[0], lineData[1], circlePos, circleNorm, circleRadius);
            }
            else
            {
                return new intersectionFigData() { figtype = GeoObjType.none };
            }
        }


        /// <summary>
        /// Finds the intersection of two spheres from MGO input.
        /// </summary>
        /// <param name="sphere1">The first sphere</param>
        /// <param name="sphere2">The second sphere</param>
        /// <returns></returns>
        internal static intersectionFigData SphereSphereIntersection(AbstractSphere sphere1, AbstractSphere sphere2)
        {
            //get info from inputs
            Vector3 c = sphere1.centerPosition;
            Vector3 p = sphere2.centerPosition;
            float r1 = sphere1.radius;
            float r2 = sphere2.radius;

            return SphereSphereIntersection(c, p, r1, r2);
        }

        /// <summary>
        /// Finds the intersection of two spheres
        /// </summary>
        /// <param name="c">Center1</param>
        /// <param name="p">Center2</param>
        /// <param name="r1">Radius 1</param>
        /// <param name="r2">Radidus 2</param>
        /// <returns></returns>
        internal static intersectionFigData SphereSphereIntersection(Vector3 c, Vector3 p, float r1, float r2)
        {
            //distance between spheres
            Vector3 d = p - c;


            // sum of the radii			NEVER USED
            //float R = r1 + r2;

			float x = (r1 * r1 - r2 * r2 - d.magnitude * d.magnitude) / (-2f * d.magnitude);
			float h = Mathf.Sqrt(r1 * r1 - x * x);

			if(h != Mathf.Sqrt(r2 * r2 - (d.magnitude - x) * (d.magnitude - x)))
			{
				Debug.LogWarning("Symmetry Broke");
			}

			//float h = Mathf.Sqrt((4 * r1 * r1 + d.magnitude * d.magnitude - Mathf.Pow(r1 * r1 - r2 * r2 + d.magnitude * d.magnitude, 2) / (4 * d.magnitude * d.magnitude)));
			//float h = 0.5f * Mathf.Sqrt(d.magnitude * d.magnitude - r1 * r1 - r2 * r2);

			//if(r1 < h || d.magnitude < r1 || d.magnitude < r2)
            if(r1 < h)
			{
				Debug.LogWarning("Houstain, we have a problem");
			}

            //normal of the circle
            Vector3 v = d.normalized;

            //point of tangency.
            Vector3 q = c + r1 * v;

            //vector perpendicular to the normal
            Vector3 nu = Vector3.up;
            if(Vector3.Cross(nu,v).magnitude == 0)
            {
                nu = Vector3.forward;
            }
            nu = Vector3.Cross(nu, v).normalized;

            //center of circle
            Vector3 newCenter = c + Mathf.Sqrt(r1 * r1 - h * h) * v;

			//point on circle.
			Vector3 u = newCenter + h * nu;

			//cases with a solution.
			if (d.magnitude == r1 + r2)
            {
                //the spheres are tangential and neither sphere is insdie the other.
                intersectionFigData point1 = new intersectionFigData();
                point1.figtype = GeoObjType.point;
                point1.vectordata = new Vector3[] { q };
                return point1;
            }
            else if (d.magnitude == Mathf.Abs(r1 - r2))
            {
                //the spheres are tangential and one sphere is inside the other.
                intersectionFigData point2 = new intersectionFigData();
                point2.figtype = GeoObjType.point;
                point2.vectordata = new Vector3[] { q };
                return point2;
            }
            else if (Mathf.Abs(r1 - r2) < d.magnitude && d.magnitude < r1 + r2)
            {
                //the spheres intersect making a cirlce on both spheres.
                intersectionFigData circle1 = new intersectionFigData();
                circle1.vectordata = new Vector3[] { newCenter, v, u };
                circle1.figtype = GeoObjType.circle;
                circle1.floatdata = new float[] { (newCenter - u).magnitude };
                return circle1;
            }
            else
            {
                return new intersectionFigData { figtype = GeoObjType.none };
                //there is no solution.
            }

        }

        /// <summary>
        /// Finds the intersection of two circles from MGO input
        /// </summary>
        /// <param name="circle1">First Circle</param>
        /// <param name="circle2">Second Circle</param>
        /// <returns></returns>
        internal static intersectionFigData CircleCircleIntersection(AbstractCircle circle1, AbstractCircle circle2)
        {
            Vector3 center1 = circle1.centerPos;
            Vector3 center2 = circle2.centerPos;

            Vector3 norm1 = circle1.normalDir;
            Vector3 norm2 = circle2.normalDir;

            float r1 = circle1.Radius;
            float r2 = circle2.Radius;

            return CircleCircleIntersection(center1, center2, norm1, norm2, r1, r2);
        }

        /// <summary>
        /// Finds the intersection of two circles
        /// </summary>
        /// <param name="center1">Center of the first circle</param>
        /// <param name="center2">Center of the second circle</param>
        /// <param name="norm1">Normal direction of theh first circle</param>
        /// <param name="norm2">Normal direction of the second circle</param>
        /// <param name="r1">radius of circle 1</param>
        /// <param name="r2">radisu of circle 2</param>
        /// <returns></returns>
        internal static intersectionFigData CircleCircleIntersection(Vector3 center1, Vector3 center2, Vector3 norm1, Vector3 norm2, float r1, float r2)
        {
            //begin by finding the circle of intersection of two spheres with same center and radius
            intersectionFigData circleFromSpheres = SphereSphereIntersection(center1, center2, r1, r2);

            circleFromSpheres.printFigDataToDebug();

            Vector3 q = circleFromSpheres.vectordata[0];
			//Debug.Log(q);
            Vector3 nu = circleFromSpheres.vectordata[1];
			//Debug.Log(q);
            float r = circleFromSpheres.floatdata[0];
            //Debug.Log(r);

            //intersection of the plane defined by the first circle and the plane defined by the circle producted in the sphere sphere intersection.
            intersectionFigData data2 = PlanePlaneIntersection(q, nu, center1, norm1);
            Vector3[] lineData = data2.vectordata;

            //intersection of the line from plane plane and the circle formt he sphere sphere.
            intersectionFigData data3 = CircleLineIntersection(lineData[0], lineData[1], q, nu, r);
            data3.printFigDataToDebug();
            return data3;
        }

        internal static intersectionFigData CircleLineIntersection(AbstractCircle circle, straightEdgeBehave line)
        {
            return CircleLineIntersection(circle, line.center, line.normalDir);
        }

        /// <summary>
        /// Finds the intersection between a circle and a line with MGO input.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="linePos"></param>
        /// <param name="lineDir"></param>
        /// <returns></returns>
        internal static intersectionFigData CircleLineIntersection(AbstractCircle circle,Vector3 linePos, Vector3 lineDir)
        {
            return CircleLineIntersection(linePos, lineDir, circle.centerPos, circle.normalDir, circle.Radius);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linePos"></param>
        /// <param name="lineDirection"></param>
        /// <param name="circlePos"></param>
        /// <param name="circleNorm"></param>
        /// <param name="circleRadius"></param>
        /// <returns></returns>
        internal static intersectionFigData CircleLineIntersection(Vector3 linePos, Vector3 lineDirection, Vector3 circlePos, Vector3 circleNorm, float circleRadius)
        {
            //careful of rounding.  this caused mysery for months.
            if (Vector3.Dot(lineDirection,circleNorm) == 0 && Vector3.Project(linePos-circlePos,circleNorm).magnitude < .00001)
            {
                //if the line is coplanar with the circle

                //start by definingg a basis system to move into 2-space.
                Vector3 basis1 = Vector3.right;

                if(Vector3.Cross(Vector3.right, circleNorm).magnitude == 0)
                {
                    basis1 = Vector3.up;
                }

                basis1 = Vector3.ProjectOnPlane(basis1, circleNorm);
                Vector3 basis2 = Vector3.Cross(circleNorm, basis1);
                Vector3.OrthoNormalize(ref circleNorm, ref basis1, ref basis2);

                float x1 = Vector3.Dot(linePos - circlePos, basis1);
                float y1 = Vector3.Dot(linePos - circlePos, basis2);

                float x2 = Vector3.Dot(linePos + lineDirection - circlePos, basis1);
                float y2 = Vector3.Dot(linePos + lineDirection - circlePos, basis2);

                float deltax = x2 - x1;
                float deltay = y2 - y1;
                float deltar = Mathf.Sqrt(deltax * deltax + deltay * deltay);
                float determinant = x1 * y2 - y1 * x2;

                float discriminant = circleRadius * circleRadius * deltar * deltar - determinant * determinant;
                float rootDiscriminant = Mathf.Sqrt(discriminant);

                if (discriminant == 0)
                {
                    //intersects at one point.
                    float x5 = (determinant * deltay) / (deltar * deltar);
                    float y5 = (-determinant * deltax) / (deltar * deltar);

                    Vector3 position = x5 * basis1 + y5 * basis2 + circlePos;

                    return new intersectionFigData() { figtype = GeoObjType.point, vectordata = new Vector3[] { position } };
                }
                else if (discriminant > 0)
                {
                    //intersects at two points.
                    float x3 = (determinant * deltay + Mathf.Sign(deltay) * deltax * rootDiscriminant) / (deltar * deltar);
                    float y3 = (-determinant * deltax + Mathf.Abs(deltay) * rootDiscriminant) / (deltar * deltar);
                    Vector3 position1 = x3 * basis1 + y3 * basis2 + circlePos;

                    float x4 = (determinant * deltay - Mathf.Sign(deltay) * deltax * rootDiscriminant) / (deltar * deltar);
                    float y4 = (-determinant * deltax - Mathf.Abs(deltay) * rootDiscriminant) / (deltar * deltar);
                    Vector3 position2 = x4 * basis1 + y4 * basis2 + circlePos;

                    //two points are returned.
                    return new intersectionFigData() { figtype = GeoObjType.point, vectordata = new Vector3[] { position1,position2 },floatdata = new float[] { 2f } };
                }
                else
                {
                    return new intersectionFigData() { figtype = GeoObjType.none };
                    //intersects at no points.
                }
            }
            else if (Vector3.Dot(lineDirection, circleNorm) == 0 && Vector3.Project(linePos - circlePos, circleNorm).magnitude  > 0)
            {
				//if the line is noncoplanar with the circle and not parallel - parallel case would go above
				//Vector3 pos = linePos + ((Vector3.Dot(circleNorm, circlePos) - Vector3.Dot(circleNorm, linePos)) / (Vector3.Dot(circleNorm, lineDirection))) * lineDirection;
				//return new intersectionFigData { figtype = GeoObjType.point, vectordata = new Vector3[] { pos } };

				//check plane line and then check if on circle.
				intersectionFigData pl = LinePlaneIntersection(linePos, lineDirection, circlePos, circleNorm);
				if(pl.figtype == GeoObjType.none || (pl.figtype == GeoObjType.point && Vector3.Distance(pl.vectordata[0],circlePos) == circleRadius))
				{
					return pl;
				}
				else
				{
					return new intersectionFigData() { figtype = GeoObjType.none };
				}
			}
            else
            {
                return new intersectionFigData() { figtype = GeoObjType.none };
            }
        }

        internal static intersectionFigData PlanePlaneIntersection(flatfaceBehave plane1, flatfaceBehave plane2)
        {
            return PlanePlaneIntersection(plane1.Position3, plane1.normalDir, plane2.Position3, plane2.normalDir);
        }

        /// <summary>
        /// Finds the intersection between two planes
        /// </summary>
        /// <param name="pos1">Point on plane 1</param>
        /// <param name="norm1">Normal direction of plane1</param>
        /// <param name="pos2">Point on plane 2</param>
        /// <param name="norm2">Normal direction of plane 2</param>
        /// <returns></returns>
        internal static intersectionFigData PlanePlaneIntersection(Vector3 pos1, Vector3 norm1, Vector3 pos2, Vector3 norm2)
        {
            Vector3 lineDirection = Vector3.Cross(norm1, norm2);

            //instead find a line on the plane1 that intersects plane 2
            Vector3 crossInput = Vector3.right;
            if (Vector3.Cross(crossInput, norm1).magnitude == 0)
            {
                crossInput = Vector3.up;
            }

            Vector3 tLineDir = Vector3.Cross(crossInput, norm1);

            if (Vector3.Dot(tLineDir, norm2) == 0)
            {
                crossInput = Vector3.forward;
                tLineDir = Vector3.Cross(crossInput, norm1);
            }
            //then use line line

            intersectionFigData figDataTemp = LinePlaneIntersection(pos1, tLineDir, pos2, norm2);

            figDataTemp.printFigDataToDebug();

            if (lineDirection.magnitude > 0 && figDataTemp.figtype == GeoObjType.point)
            {
                Vector3 linePosition = figDataTemp.vectordata[0];
                return new intersectionFigData { vectordata = new Vector3[2] { linePosition, lineDirection }, figtype = GeoObjType.line };
            }
            else
            {
                return new intersectionFigData { figtype = GeoObjType.none };
            }
        }

        /// <summary>
        /// Returns the intersection between two striaghtedges.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        internal static intersectionFigData LineLineIntersection(straightEdgeBehave line1, straightEdgeBehave line2)
        {
            return LineLineIntersection(line1.center, line2.center, line1.normalDir, line2.normalDir);
        }

        /// <summary>
        /// Finds the intersection between two lines
        /// </summary>
        /// <param name="line1pts">two or more points on the first line</param>
        /// <param name="line2pts">two or more points on the second line</param>
        /// <returns></returns>
        internal static intersectionFigData LineLineIntersection(Vector3[] line1pts, Vector3[] line2pts)
        {
            return LineLineIntersection(line1pts[0], line2pts[0], line1pts[0] - line1pts[1], line2pts[0] - line2pts[1]);
        }

        /// <summary>
        /// Finds the intersection between two lines
        /// </summary>
        /// <param name="linePos1">a point on the first line</param>
        /// <param name="linePos2">a point on the second line</param>
        /// <param name="lineDir1">the direction of the first line</param>
        /// <param name="lineDir2">the direction of the second line</param>
        /// <returns></returns>
        internal static intersectionFigData LineLineIntersection(Vector3 linePos1, Vector3 linePos2, Vector3 lineDir1, Vector3 lineDir2)
        {
            

            //check for skew lines and parallel lines.
            bool tBool1 = Vector3.Cross(lineDir1, lineDir2).magnitude >0;
            bool tBool2 = Vector3.Project(linePos1 - linePos2, Vector3.Cross(lineDir1, lineDir2)).magnitude == 0;

            //if the two lines intersect, they will intersect at the intersection point of line1 and the plane of line1 and point 2 on the plane, where 
            if (tBool1 && tBool2)
            {
                return LinePlaneIntersection(linePos1, lineDir1, linePos2, Vector3.Cross(Vector3.Cross(lineDir1, lineDir2), lineDir2));
            }
            else
            {
                return new intersectionFigData { figtype = GeoObjType.none };
            }
        }

        /// <summary>
        /// Finds the point of intersection between a flatface and a straightedge
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        internal static intersectionFigData LinePlaneIntersection(flatfaceBehave plane, straightEdgeBehave line)
        {
            return LinePlaneIntersection(line.center, line.normalDir, plane.Position3, plane.normalDir);
        }

        /// <summary>
        /// Finds the point of intersection between a line and a plane
        /// </summary>
        /// <param name="linePos">A point on the line</param>
        /// <param name="lineDir">The direction of the line</param>
        /// <param name="planePos">A point on the plane</param>
        /// <param name="planeNorm">The normal direction of the plane</param>
        /// <returns></returns>
        internal static intersectionFigData LinePlaneIntersection(Vector3 linePos, Vector3 lineDir, Vector3 planePos, Vector3 planeNorm)
        {
            //        //using Ians math.  Something isn't consistant.  Try using unity math instead.
            //        if (Vector3.ProjectOnPlane(lineDir,planeNorm).magnitude > 0)
            //        {
            //            Vector3 w = (linePos-planePos);
            //            Vector3 u = lineDir.normalized;
            //            Vector3 n = planeNorm.normalized;
            //Vector3 v_0 = planePos;
            //float s = -Vector3.Dot(n, w) / Vector3.Dot(n, u);
            //Vector3 pointPos = s * u + w + v_0;
            //            return new intersectionFigData { figtype = GeoObjType.point, vectordata = new Vector3[] { pointPos}};
            //        }
            //        else
            //        {
            //            return new intersectionFigData { figtype = GeoObjType.none };
            //        }

            Plane p = new Plane(planeNorm, planePos);
            Ray l = new Ray(linePos, lineDir.normalized);
            float dist = Mathf.Infinity;
            p.Raycast(l, out dist);
            if(dist == Mathf.Infinity)
            {
                l = new Ray(linePos, -lineDir.normalized);
                p.Raycast(l, out dist);
            }
            if (dist == Mathf.Infinity)
            {
                return new intersectionFigData { figtype = GeoObjType.none };
            }
            else
            {
                Vector3 result = linePos + lineDir.normalized * dist;
                return new intersectionFigData { figtype = GeoObjType.point, vectordata = new Vector3[] { result } };
            }


        }

        internal static intersectionFigData SegmentPlaneIntersection(AbstractLineSegment line, flatfaceBehave plane)
        {
            return SegmentPlaneIntersection(line.vertex0, line.vertex1, plane.Position3, plane.normalDir);
        }

        internal static intersectionFigData SegmentPlaneIntersection(Vector3 linePos1, Vector3 linePos2, Vector3 planePos, Vector3 planeNorm)
        {

            intersectionFigData canidates = LinePlaneIntersection(linePos1, (linePos1 - linePos2).normalized, planePos, planeNorm);

            //for some reason this condition works with the UNit checker but fails in the Shearing scene???  No result when there definately should be one.
            //temp fix.  use LinePlane intersection in Shearing scene.
            if (canidates.figtype == GeoObjType.point && (canidates.vectordata[0] - linePos1).magnitude + (canidates.vectordata[0] - linePos2).magnitude - (linePos1 - linePos2).magnitude < .001f)
            {
                return canidates;
            }
            else
            {
                return new intersectionFigData { figtype = GeoObjType.none };
            }
        }
    }
}
