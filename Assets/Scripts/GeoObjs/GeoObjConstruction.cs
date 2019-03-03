/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using Leap.Unity.Interaction;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver {
	/// <summary>
	/// A script used to interface between the geometry kernel (GeoSolver) and scripts that interpret user input.
	/// Takes a variety of  inputs to allow for construction of figures both at runtime and in onstart scripts.
	/// Will need to be entirely refactored for new Geometery kernel.
	/// </summary>
	class GeoObjConstruction
	{
		/// <summary>
		/// This script Spawns a Regular Polygon
		/// The main contributor(s) to this script is __
		/// Status: ???
		/// </summary>
		/// <param name="nSides">Number of Sides requested</param>
		/// <param name="position">World Space position of spawn location</param>
		/// <param name="normDir">Normal Direction for polygon</param>
		/// <returns></returns>
		public static regularPolygon rPoly(int nSides, float apothem, Vector3 position, Vector3 normDir)
		{
			regularPolygon poly = regularPolygon.Constructor();

			poly.Position3 = position;
			poly.InitRegPoly(nSides, apothem, normDir);

			return poly;
		}

		internal static SnappablePoint snapPoint(MasterGeoObj activeMGO, Vector3 position)
		{
			SnappablePoint snap = SnappablePoint.Constructor();
			snap.Position3 = position;
			snap.attachedObject = activeMGO;
			return snap;

		}

		/// <summary>
		/// Spawns a Regular Polygon
		/// </summary>
		/// <param name="nSides">Number of Sides requested</param>
		/// <param name="position">World Space position of spawn location</param>
		/// <returns></returns>
		public static regularPolygon rPoly(int nSides, float apothem, Vector3 position)
		{
			regularPolygon poly = regularPolygon.Constructor();
			poly.Position3 = position;
			poly.InitRegPoly(nSides, apothem, Vector3.zero);

			return poly;
		}

		public static DependentSphere dSphere(AbstractPoint center, AbstractPoint edge)
		{
			DependentSphere thisSphere = DependentSphere.Constructor();
			thisSphere.center = center;
			thisSphere.centerPosition = center.Position3;
			thisSphere.edge = edge;
			thisSphere.edgePosition = edge.Position3;
			thisSphere.Position3 = center.Position3;

			HW_GeoSolver.ins.AddDependence(thisSphere, center);
			HW_GeoSolver.ins.AddDependence(thisSphere, edge);

			thisSphere.InitializeFigure();
			return thisSphere;
		}

		public static DependentCircle dCircle(AbstractPoint center, AbstractPoint edge, Vector3 normDir)
		{
			DependentCircle dc = DependentCircle.Constructor();

			dc.transform.parent = center.transform.parent;

			dc.center = center;
			dc.centerPos = center.Position3;
			dc.edge = edge;
			dc.edgePos = edge.Position3;
			dc.normalDir = normDir;

			HW_GeoSolver.ins.AddDependence(dc, center);
			HW_GeoSolver.ins.AddDependence(dc, edge);

			dc.InitializeFigure();

			return dc;
		}

		public static DependentRevolvedSurface dRevSurface(AbstractPoint center, AbstractLineSegment attachedLineSegment, Vector3 normDir)
		{
			DependentRevolvedSurface drs = DependentRevolvedSurface.Constructor();

			drs.transform.parent = attachedLineSegment.transform.parent;

			drs.attachedLine = attachedLineSegment;
			drs.endpoint1 = attachedLineSegment.vertex0;
			drs.endpoint2 = attachedLineSegment.vertex1;
			drs.normalDirection = normDir;
			drs.centerPoint = center.Position3;
			drs.center = center;

			HW_GeoSolver.ins.AddDependence(drs, center);
			HW_GeoSolver.ins.AddDependence(drs, attachedLineSegment);

			drs.InitializeFigure();

			return drs;
		}

		#region Spawn Objects from Operators

		public static InteractablePoint iPoint(Vector3 position)
		{
			InteractablePoint point = InteractablePoint.Constructor();
			point.Position3 = position;
			point.InitializeFigure();
			return point;
		}

		public static DependentPoint dPoint(Vector3 position)
		{
			DependentPoint point = DependentPoint.Constructor();
			point.Position3 = position;
			point.InitializeFigure();
			return point;
		}

        public static StaticPoint sPoint(Vector3 position)
        {
            StaticPoint point = StaticPoint.Constructor();
            point.Position3 = position;
			point.InitializeFigure();
			return point;
        }

        public static InteractableLineSegment iLineSegment(AbstractPoint point1, AbstractPoint point2)
		{
			InteractableLineSegment line = InteractableLineSegment.Constructor();
			
			line.point1 = point1;
			line.point2 = point2;
			line.vertex0 = point1.Position3;
			line.vertex1 = point2.Position3;
			line.transform.GetComponent<InteractableLineSegment>().InitializeFigure();

			HW_GeoSolver.ins.addComponent(line);
			HW_GeoSolver.ins.AddDependence(line, point1);
			HW_GeoSolver.ins.AddDependence(line, point2);

			return line;
		}

		public static DependentLineSegment dLineSegment(AbstractPoint point1, AbstractPoint point2)
		{
			DependentLineSegment line = DependentLineSegment.Constructor();

			line.transform.GetComponent<DependentLineSegment>().point1 = point1.transform.GetComponent<AbstractPoint>();
			line.transform.GetComponent<DependentLineSegment>().point2 = point2.transform.GetComponent<AbstractPoint>();
			line.vertex0 = point1.Position3;
			line.vertex1 = point2.Position3;
			line.transform.GetComponent<DependentLineSegment>().InitializeFigure();

			HW_GeoSolver.ins.AddDependence(line, point1);
			HW_GeoSolver.ins.AddDependence(line, point2);

			return line;
		}

		public static InteractablePolygon iPolygon(List<AbstractLineSegment> lineList, List<AbstractPoint> pointList)
		{
			InteractablePolygon plane = InteractablePolygon.Constructor();
			plane.transform.GetComponent<InteractablePolygon>().lineList = lineList;
			plane.transform.GetComponent<InteractablePolygon>().pointList = pointList;
			plane.transform.GetComponent<InteractablePolygon>().InitializeFigure();

			//for some reason this makes a shortcut in the network.
			//foreach (AbstractLineSegment line in lineList)
			//{
			//	HW_GeoSolver.ins.AddDependence(plane.transform, line.transform);
			//}

			foreach (AbstractPoint point in pointList)
			{
				HW_GeoSolver.ins.AddDependence(plane, point);
				if (point.GetComponent<StaticPoint>() != null)
				{
					plane.GetComponent<InteractionBehaviour>().moveObjectWhenGrasped = false;
				}
			}
			plane.AddToRManager();
			return plane;
			#endregion
		}

        public static DependentPolygon dTriangle(InteractableLineSegment lineSegment, AbstractPoint point)
        {
            List<AbstractPoint> pointList = new List<AbstractPoint>() { point, lineSegment.point1, lineSegment.point2 };
            List<AbstractLineSegment> lineList = new List<AbstractLineSegment>() { lineSegment, dLineSegment(point, lineSegment.point1), dLineSegment(point, lineSegment.point2) };
            return dPolygon(lineList, pointList);
        }

		internal static InteractablePolygon iTriangle(InteractableLineSegment lineSegment, AbstractPoint point)
		{
			List<AbstractPoint> pointList = new List<AbstractPoint>() { point, lineSegment.point1, lineSegment.point2 };
			List<AbstractLineSegment> lineList = new List<AbstractLineSegment>() { lineSegment, dLineSegment(point, lineSegment.point1), dLineSegment(point, lineSegment.point2) };
			return iPolygon(lineList, pointList);
		}


		public static DependentPolygon dPolygon(List<AbstractLineSegment> lineList, List<AbstractPoint> pointList)
		{
			DependentPolygon plane = DependentPolygon.Constructor();

			plane.transform.GetComponent<DependentPolygon>().lineList = lineList;
			plane.transform.GetComponent<DependentPolygon>().pointList = pointList;
			plane.transform.GetComponent<DependentPolygon>().InitializeFigure();

			foreach (AbstractLineSegment line in lineList)
			{
				HW_GeoSolver.ins.AddDependence(plane, line);
			}

			foreach (AbstractPoint point in pointList)
			{
				HW_GeoSolver.ins.AddDependence(plane, point);
				if (point.GetComponent<StaticPoint>() != null)
				{
					plane.GetComponent<InteractionBehaviour>().moveObjectWhenGrasped = false;
				}
			}

			return plane;
		}


        internal static InteractablePrism iPrism(List<AbstractPolygon> bases, List<AbstractPolygon> sides)
		{
			InteractablePrism prism = InteractablePrism.Constructor();
			prism.bases = bases;
			prism.sides = sides;
			List<AbstractLineSegment> lines = new List<AbstractLineSegment>();
			bases.ForEach(b => lines.Union(b.lineList));
			sides.ForEach(s => lines.Union(s.lineList));

			bases.ForEach(b => HW_GeoSolver.ins.AddDependence(prism, b));
			sides.ForEach(s => HW_GeoSolver.ins.AddDependence(prism, s));
			lines.ForEach(l => HW_GeoSolver.ins.AddDependence(prism, l));
			prism.vertexPoints.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));
			prism.InitializeFigure();
			return prism;
		}

		internal static InteractablePrism iPrism(List<AbstractLineSegment> edges)
		{
			InteractablePrism prism = InteractablePrism.Constructor();
			prism.bases = new List<AbstractPolygon>();
			prism.sides = new List<AbstractPolygon>();
			prism.lineSegments = edges;
			
			foreach(AbstractLineSegment line in edges)
			{
				if (line.GetComponent<InteractableLineSegment>() != null)
				{
					InteractableLineSegment iLineSegment = line.GetComponent<InteractableLineSegment>();
					if (!prism.vertexPoints.Contains(iLineSegment.point1))
						prism.vertexPoints.Add(iLineSegment.point1);

					if (!prism.vertexPoints.Contains(iLineSegment.point2))
						prism.vertexPoints.Add(iLineSegment.point2);
				}
				else if (line.GetComponent<DependentLineSegment>() != null)
				{
					DependentLineSegment dlineSegment = line.GetComponent<DependentLineSegment>();
					if (!prism.vertexPoints.Contains(dlineSegment.point1))
						prism.vertexPoints.Add(dlineSegment.point1);

					if (!prism.vertexPoints.Contains(dlineSegment.point2))
						prism.vertexPoints.Add(dlineSegment.point2);
				}
			}

			prism.lineSegments.ForEach(l => HW_GeoSolver.ins.AddDependence(prism, l));
			prism.vertexPoints.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));
			prism.InitializeFigure();

			return prism;
		}

		internal static InteractablePrism iPrism(AbstractPolygon polygon, Vector3 base2Pos)
        {

            List<AbstractLineSegment> lineList0 = new List<AbstractLineSegment>();
            List<AbstractPoint> pointList0 = new List<AbstractPoint>();

            lineList0 = polygon.transform.GetComponent<AbstractPolygon>().lineList;
            pointList0 = polygon.transform.GetComponent<AbstractPolygon>().pointList;

            //make new lists for the points and lines in the new plane
            List<AbstractLineSegment> lineList1 = new List<AbstractLineSegment>();
            List<AbstractPoint> pointList1 = new List<AbstractPoint>();

            //create new points for the new plane.
            foreach (AbstractPoint point in pointList0)
            {
                AbstractPoint newPoint = iPoint(point.Position3-polygon.Position3 + base2Pos);
                pointList1.Add(newPoint);
            }

            //create new lines for the new plane.
            foreach (AbstractLineSegment line in lineList0)
            {
                if (line.GetComponent<InteractableLineSegment>() != null)
                {
                    int point1Index = pointList0.IndexOf(line.transform.GetComponent<InteractableLineSegment>().point1);
                    int point2Index = pointList0.IndexOf(line.transform.GetComponent<InteractableLineSegment>().point2);

                    AbstractLineSegment newLine = iLineSegment(pointList1[point1Index].GetComponent<AbstractPoint>(), pointList1[point2Index].GetComponent<AbstractPoint>());
                    lineList1.Add(newLine);

                    HW_GeoSolver.ins.AddDependence(newLine, pointList1[point1Index]);
                    HW_GeoSolver.ins.AddDependence(newLine, pointList1[point2Index]);
                }
                else if (line.GetComponent<DependentLineSegment>() != null)
                {
                    int point1Index = pointList0.IndexOf(line.transform.GetComponent<DependentLineSegment>().point1);
                    int point2Index = pointList0.IndexOf(line.transform.GetComponent<DependentLineSegment>().point2);

                    AbstractLineSegment newLine = iLineSegment(pointList1[point1Index].GetComponent<AbstractPoint>(), pointList1[point2Index].GetComponent<AbstractPoint>());
                    lineList1.Add(newLine);

                    HW_GeoSolver.ins.AddDependence(newLine, pointList1[point1Index]);
                    HW_GeoSolver.ins.AddDependence(newLine, pointList1[point2Index]);
                }
            }

            AbstractPolygon plane1 = iPolygon(lineList1, pointList1);


            foreach (AbstractLineSegment line in lineList1)
            {
                HW_GeoSolver.ins.AddDependence(plane1, line);
            }

            plane1.transform.GetComponent<AbstractPolygon>().InitializeFigure();
            List<AbstractPolygon> sideList = new List<AbstractPolygon>();

            foreach (AbstractLineSegment line0 in lineList0)
            {

                int lineIndex = lineList0.IndexOf(line0);


                AbstractLineSegment lineTest = lineList0[lineIndex];

                sideList.Add(makePlaneWall(lineIndex, lineList0, lineList1));
            }

            InteractablePrism prism = InteractablePrism.Constructor();
			List<AbstractPolygon> baseList = new List<AbstractPolygon>
			{
				polygon,
				plane1
			};

			prism.bases = baseList;
            prism.sides = sideList;

			prism.bases.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));
			prism.sides.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));
			prism.vertexPoints.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));

            prism.InitializeFigure();

            return prism;
        }

		internal static InteractablePrism dPrism(AbstractPolygon polygon, Vector3 base2Pos)
		{

			List<AbstractLineSegment> lineList0 = new List<AbstractLineSegment>();
			List<AbstractPoint> pointList0 = new List<AbstractPoint>();

			lineList0 = polygon.transform.GetComponent<AbstractPolygon>().lineList;
			pointList0 = polygon.transform.GetComponent<AbstractPolygon>().pointList;

			//make new lists for the points and lines in the new plane
			List<AbstractLineSegment> lineList1 = new List<AbstractLineSegment>();
			List<AbstractPoint> pointList1 = new List<AbstractPoint>();

			//create new points for the new plane.
			foreach (AbstractPoint point in pointList0)
			{
				AbstractPoint newPoint = dPoint(point.Position3 - polygon.Position3 + base2Pos);
				pointList1.Add(newPoint);
			}

			//create new lines for the new plane.
			foreach (AbstractLineSegment line in lineList0)
			{
				if (line.GetComponent<InteractableLineSegment>() != null)
				{
					int point1Index = pointList0.IndexOf(line.transform.GetComponent<InteractableLineSegment>().point1);
					int point2Index = pointList0.IndexOf(line.transform.GetComponent<InteractableLineSegment>().point2);

					AbstractLineSegment newLine = iLineSegment(pointList1[point1Index].GetComponent<AbstractPoint>(), pointList1[point2Index].GetComponent<AbstractPoint>());
					lineList1.Add(newLine);

					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point1Index]);
					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point2Index]);
				}
				else if (line.GetComponent<DependentLineSegment>() != null)
				{
					int point1Index = pointList0.IndexOf(line.transform.GetComponent<DependentLineSegment>().point1);
					int point2Index = pointList0.IndexOf(line.transform.GetComponent<DependentLineSegment>().point2);

					AbstractLineSegment newLine = iLineSegment(pointList1[point1Index].GetComponent<AbstractPoint>(), pointList1[point2Index].GetComponent<AbstractPoint>());
					lineList1.Add(newLine);

					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point1Index]);
					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point2Index]);
				}
			}

			AbstractPolygon plane1 = dPolygon(lineList1, pointList1);
			plane1.AddToRManager();

			foreach (AbstractLineSegment line in lineList1)
			{
				HW_GeoSolver.ins.AddDependence(plane1, line);
			}

			plane1.transform.GetComponent<AbstractPolygon>().InitializeFigure();
			List<AbstractPolygon> sideList = new List<AbstractPolygon>();

			foreach (AbstractLineSegment line0 in lineList0)
			{

				int lineIndex = lineList0.IndexOf(line0);


				AbstractLineSegment lineTest = lineList0[lineIndex];

				sideList.Add(makePlaneWall(lineIndex, lineList0, lineList1));
			}

			InteractablePrism prism = InteractablePrism.Constructor();
			List<AbstractPolygon> baseList = new List<AbstractPolygon>
			{
				polygon,
				plane1
			};

			prism.bases = baseList;
			prism.sides = sideList;

			foreach (AbstractPolygon poly in baseList)
			{
				HW_GeoSolver.ins.AddDependence(prism, poly);
			}
			foreach (AbstractPolygon poly in sideList)
			{
				HW_GeoSolver.ins.AddDependence(prism, poly);
			}

			prism.vertexPoints.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));

			prism.InitializeFigure();

			return prism;
		}

		internal static InteractablePrism iPrism(AbstractPolygon polygon)
		{

			List<AbstractLineSegment> lineList0 = polygon.transform.GetComponent<AbstractPolygon>().lineList;
			List<AbstractPoint> pointList0 = polygon.transform.GetComponent<AbstractPolygon>().pointList;

			//make new lists for the points and lines in the new plane
			List<AbstractLineSegment> lineList1 = new List<AbstractLineSegment>();
			List<AbstractPoint> pointList1 = new List<AbstractPoint>();

			//create new points for the new plane.
			foreach (AbstractPoint point in pointList0)
			{
				AbstractPoint newPoint = iPoint(point.Position3);
				pointList1.Add(newPoint);
			}

			//create new lines for the new plane.
			foreach (AbstractLineSegment line in lineList0)
			{
				if (line.GetComponent<InteractableLineSegment>() != null)
				{
					int point1Index = pointList0.IndexOf(line.transform.GetComponent<InteractableLineSegment>().point1);
					int point2Index = pointList0.IndexOf(line.transform.GetComponent<InteractableLineSegment>().point2);

					AbstractLineSegment newLine = iLineSegment(pointList1[point1Index], pointList1[point2Index]);
					lineList1.Add(newLine);
					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point1Index]);
					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point2Index]);
				}
				else if (line.GetComponent<DependentLineSegment>() != null)
				{
					int point1Index = pointList0.IndexOf(line.transform.GetComponent<DependentLineSegment>().point1);
					int point2Index = pointList0.IndexOf(line.transform.GetComponent<DependentLineSegment>().point2);

					AbstractLineSegment newLine = iLineSegment(pointList1[point1Index], pointList1[point2Index]);
					lineList1.Add(newLine);

					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point1Index]);
					HW_GeoSolver.ins.AddDependence(newLine, pointList1[point2Index]);
				}
			}

			AbstractPolygon plane1 = iPolygon(lineList1, pointList1);


			plane1.InitializeFigure();
			List<AbstractPolygon> sideList = new List<AbstractPolygon>();
			
			foreach (AbstractLineSegment l in lineList0)
			{

				int lineIndex = lineList0.IndexOf(l);

				sideList.Add(makePlaneWall(lineIndex, lineList0, lineList1));
			}

			InteractablePrism prism = InteractablePrism.Constructor();
			List<AbstractPolygon> baseList = new List<AbstractPolygon>();
			baseList.Add(polygon);
			baseList.Add(plane1);

			prism.bases = baseList;
			prism.sides = sideList;

			prism.vertexPoints.ForEach(p => HW_GeoSolver.ins.AddDependence(prism, p));
			prism.lineSegments.ForEach(l => HW_GeoSolver.ins.AddDependence(prism, l));
			prism.bases.ForEach(b => HW_GeoSolver.ins.AddDependence(prism, b));
			prism.sides.ForEach(b => HW_GeoSolver.ins.AddDependence(prism, b));
			prism.InitializeFigure();

			return prism;
		}

		public static AbstractPolygon makePlaneWall(int lineIndex, List<AbstractLineSegment> lineList0, List<AbstractLineSegment> lineList1)
		{
			AbstractLineSegment line0 = lineList0[lineIndex];
			AbstractLineSegment line1 = lineList1[lineIndex];
			AbstractPoint point01;
			AbstractPoint point02;
			AbstractPoint point11;
			AbstractPoint point12;

			if (line0.GetComponent<InteractableLineSegment>() != null)
			{
				point01 = line0.transform.GetComponent<InteractableLineSegment>().point1;
				point02 = line0.transform.GetComponent<InteractableLineSegment>().point2;
			}
			else//(line0.GetComponent<DependentLineSegment>() != null)
			{ 
				point01 = line0.transform.GetComponent<DependentLineSegment>().point1;
				point02 = line0.transform.GetComponent<DependentLineSegment>().point2;
			}


			if (line1.GetComponent<InteractableLineSegment>() != null)
			{
				point11 = line1.transform.GetComponent<InteractableLineSegment>().point1;
				point12 = line1.transform.GetComponent<InteractableLineSegment>().point2;
			}
			else //(line1.GetComponent<DependentLineSegment>() != null)
			{
				point11 = line1.transform.GetComponent<DependentLineSegment>().point1;
				point12 = line1.transform.GetComponent<DependentLineSegment>().point2;
			}


			AbstractLineSegment line2 = iLineSegment(point01, point11);
			AbstractLineSegment line3 = iLineSegment(point02, point12);

			HW_GeoSolver.ins.AddDependence(line2, point01);
			HW_GeoSolver.ins.AddDependence(line2, point11);
			HW_GeoSolver.ins.AddDependence(line3, point02);
			HW_GeoSolver.ins.AddDependence(line3, point12);

			line2.transform.GetComponent<AbstractLineSegment>().InitializeFigure();
			line3.transform.GetComponent<AbstractLineSegment>().InitializeFigure();

			List<AbstractLineSegment> lineList = new List<AbstractLineSegment>();
			lineList.Add(line0);
			lineList.Add(line1);
			lineList.Add(line2);
			lineList.Add(line3);

			List<AbstractPoint> pointList = new List<AbstractPoint>();
			pointList.Add(point01);
			pointList.Add(point02);
			pointList.Add(point12);
			pointList.Add(point11);

            return iPolygon(lineList,pointList);
		}

        public static DependentPyramid dPyramid(AbstractPolygon basePoly, AbstractPoint apex)
		{
            DependentPyramid pyramid = DependentPyramid.Constructor();
            pyramid.basePolygon = basePoly;
            pyramid.apex = apex;

            List<AbstractPolygon> sideList = new List<AbstractPolygon>();

            foreach(AbstractLineSegment baseLine in basePoly.lineList)
            {
				if (baseLine.GetComponent<InteractableLineSegment>() != null)
				{
					List<AbstractPoint> vertexList = new List<AbstractPoint>();
					List<AbstractLineSegment> lineList = new List<AbstractLineSegment>();

					vertexList.Add(apex);
					vertexList.Add(baseLine.GetComponent<InteractableLineSegment>().point1);
					vertexList.Add(baseLine.GetComponent<InteractableLineSegment>().point2);

					lineList.Add(baseLine);
					lineList.Add(iLineSegment(apex, baseLine.GetComponent<InteractableLineSegment>().point1));
					lineList.Add(iLineSegment(apex, baseLine.GetComponent<InteractableLineSegment>().point2));

					sideList.Add(dPolygon(lineList, vertexList));
				}
				else
				{

					List<AbstractPoint> vertexList = new List<AbstractPoint>();
					List<AbstractLineSegment> lineList = new List<AbstractLineSegment>();

					vertexList.Add(apex);
					vertexList.Add(baseLine.GetComponent<DependentLineSegment>().point1);
					vertexList.Add(baseLine.GetComponent<DependentLineSegment>().point2);

					lineList.Add(baseLine);
					lineList.Add(iLineSegment(apex, baseLine.GetComponent<DependentLineSegment>().point1));
					lineList.Add(iLineSegment(apex, baseLine.GetComponent<DependentLineSegment>().point2));

					sideList.Add(dPolygon(lineList, vertexList));
				}
            }

            pyramid.sides = sideList;

            HW_GeoSolver.ins.AddDependence(pyramid, basePoly);//This dependency must be first on the list for XMLManager Loading to operate correctly

            foreach (AbstractPolygon poly in sideList)
            {
				HW_GeoSolver.ins.AddDependence(pyramid, poly);
            }

            return pyramid;
        }

		public static alphabetLabel label(MasterGeoObj obj, string labelText)
		{
			alphabetLabel label = alphabetLabel.Constructor();
			label.spawnOnMGO(obj, labelText);
			return label;
		}

	}
}
