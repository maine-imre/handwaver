/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver
{
/// <summary>
/// An interactable solid for prisms.
/// Mostly useful for spawning objects.
/// Allows prisms to be moved by their center points.
/// Will be depreciated with new Geometery kernel
/// </summary>
	class InteractablePrism : AbstractSolid, InteractiveFigure
	{
		private List<AbstractPoint> _vertexPoints = new List<AbstractPoint>();
		private List<AbstractLineSegment> _lineSegments = new List<AbstractLineSegment>();

		/// <summary>
		/// The bases that can be moved on the figure (they have a handle and interaction behaviour)
		/// </summary>
		public List<AbstractPolygon> bases;
		/// <summary>
		/// The sides that are dependent on the bases (they don't have a handle or interactoinbehaviour.
		/// </summary>
		public List<AbstractPolygon> sides;

        internal override float volume
        {
            get
			{
				//volume of a prism, assumes parallel bases.
				//need a more complicated method to resolve this.

				float result = float.NaN;
				//if (Vector3.Magnitude(Vector3.Cross(bases[0].normDir(), bases[1].normDir())) > 0.001)
				//{
				//	Debug.LogWarning("VOLUME FORMULA DOESNT ACCOUNT FOR NON PARALLEL BASES OR BASES OF DIFFERENT SIZES!");
				//}
				//else
				if (bases[0].area != bases[1].area)
				{
					//one of the bases has been changed but they're still parallel.
					// solved by integrating between bases.
					float b1 = bases[0].area;
					float b2 = bases[1].area;
					float a = Vector3.Distance(bases[0].Position3, bases[1].Position3);

					//technically this is plus minus sqrt, but positive should be the result.
					float c = 2f + Mathf.Sqrt(4f - 4f * (a * a + b2 / b1)) / 2f;

					result = b1 * (Mathf.Pow(a, 3) / 3f + c * a * a + c * c * a);
				}
				else
				{
					//formula for the volume of a prism.
					result = bases[0].area * Vector3.Distance(bases[0].Position3, bases[1].Position3);
				}
				return result;
			}
		}

        public List<AbstractLineSegment> lineSegments
		{
			get
			{
				return _lineSegments;
			}
			set
			{
				if (bases.Count != 2)
				_lineSegments = value;
			}
		}

		public List<AbstractPoint> vertexPoints
		{
			get
			{
				if(bases.Count == 2)
				{
					List<AbstractPoint> tmpVert = new List<AbstractPoint>();
					 bases.ForEach(b => tmpVert.AddRange(b.pointList));
					return tmpVert;
				}
				else
				return _vertexPoints;
			}
		}

		internal Vector3 center
		{
			get
			{
				Vector3 tempCenter = Vector3.zero;
				if (bases.Count == 2)
				{
					foreach (AbstractPoint point in vertexPoints)
					{
						tempCenter += point.Position3 / vertexPoints.Count;
					}
				}
				else
				{
					foreach (AbstractPoint point in _vertexPoints)
					{
						tempCenter += point.Position3 / vertexPoints.Count;
					}
				}
				return tempCenter;
			}
			set
			{
				Vector3 tempCenter = Vector3.zero;
				foreach (AbstractPoint point in vertexPoints)
				{
					tempCenter += point.Position3 / vertexPoints.Count;
				}
				if (bases.Count != 0 && sides.Count != 0)
				{
					bases.ForEach(p => p.Position3 += value - tempCenter);
					sides.ForEach(p => p.Position3 += value - tempCenter);
				}
				else
				{
					vertexPoints.ForEach(point => point.Position3 += value - tempCenter);
				}
			}
		}

        internal override AbstractPolygon primaryBase
        {
            get
            {
                return bases[0];
            }
        }

		internal override List<AbstractPolygon> allfaces
		{
			get
			{
				List<AbstractPolygon> result = new List<AbstractPolygon>();
				result.AddRange(bases);
				result.AddRange(sides);
				return result;
			}
		}

		public override void initializefigure()
		{
			if (bases.Count == 2)
			{
				this.Position3 = center;
				bases.ForEach(b => _vertexPoints.AddRange(b.pointList));
				bases.ForEach(b => _lineSegments.AddRange(b.lineList));
				foreach (AbstractPolygon side in sides)
				{
					foreach (AbstractLineSegment line in side.lineList)
					{
						if (!_lineSegments.Contains(line))
						{
							_lineSegments.Add(line);
						}
					}
				}
			}
			else
			{
				center = center;
				this.Position3 = center;
			}
		}

		internal override bool RMotion(NodeList<string> inputNodeList)
		{
			checkActive(bases);
			checkActive(sides);
			if (thisIBehave.isGrasped)
			{
				Vector3 tempCenter = center;
				bases.ForEach(p => p.Position3 += this.Position3 - tempCenter);
				sides.ForEach(p => p.Position3 += this.Position3 - tempCenter);
				_vertexPoints.ForEach(p => p.Position3 += this.Position3 - tempCenter);
			}
			else if(checkForPointsInNodeList(inputNodeList))
			{ 
				this.Position3 = center;
			}
			return true;
		}

		/// <summary>
		/// Removes nonenabled polygons from the lits
		/// </summary>
		/// <param name="polys">List to remove all nonenabled from</param>
		private void checkActive(List<AbstractPolygon> polys)
		{
			polys.RemoveAll(p => !p.gameObject.activeSelf);
			//List<AbstractPolygon> polyToRemove = polys.Where(p => !p.gameObject.activeSelf).ToList();	optimized above
			//polyToRemove.ForEach(p => polys.Remove(p));
		}

		private bool checkForPointsInNodeList(NodeList<string> nodeList)
		{
			List<MasterGeoObj> masterList = new List<MasterGeoObj>();
			masterList.AddRange(bases.Cast<MasterGeoObj>().ToList());
			masterList.AddRange(vertexPoints.Cast<MasterGeoObj>().ToList());
			masterList.AddRange(sides.Cast<MasterGeoObj>().ToList());

			return nodeList.checkForMGOmatch(masterList);
		}

		public override void Stretch(InteractionController obj)
		{
			if (stretchEnabled)
			{
				Debug.Log("Can't Stretch To 4D yet...");
				InteractablePrism thisPrism = this;

				List<AbstractLineSegment> lineList = new List<AbstractLineSegment>();
				List<AbstractPoint> pointList = new List<AbstractPoint>();

				thisPrism.bases[0].pointList.ForEach(p => pointList.Add(GeoObjConstruction.iPoint(p.Position3)));
				lineList.Add(GeoObjConstruction.iLineSegment(pointList[0], pointList[1]));
				lineList.Add(GeoObjConstruction.iLineSegment(pointList[1], pointList[2]));
				lineList.Add(GeoObjConstruction.iLineSegment(pointList[3], pointList[4]));
				lineList.Add(GeoObjConstruction.iLineSegment(pointList[1], pointList[0]));

				InteractablePrism nextPrism = GeoObjConstruction.iPrism(GeoObjConstruction.iPolygon(lineList, pointList));

				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < thisPrism.bases.Count; j++)
					{
						GeoObjConstruction.makePlaneWall(i, thisPrism.bases[j].lineList, nextPrism.bases[j].lineList);
					}

					for (int k = 0; k < thisPrism.sides.Count; k++)
					{
						GeoObjConstruction.makePlaneWall(i, thisPrism.sides[k].lineList, nextPrism.sides[k].lineList);
					}
				}

			}
		}

		public override void updateFigure()
		{
			//do nothing, there are no graphical changes.
		}

		internal override void GlueToFigure(MasterGeoObj toObj)
		{
			throw new NotImplementedException();
		}

		internal override void SnapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}
	}
}
