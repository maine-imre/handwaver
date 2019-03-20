/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{

	[RequireComponent(typeof(AudioSource))]

/// <summary>
/// Uses a pin to draw line segments between points.
/// Developed for lattice land
/// Moved to sandbox.
/// Currently depreciated.
/// </summary>
	class LineSegmentMaker : HandWaverTools
    {
#pragma warning disable 0649

		public enum pinType { polymaker,wireframe,solidmaker, polycut};

		private LineRenderer thisLR;
		public Transform endofPin;
		public pinType thisPinType = pinType.polymaker;
		internal List<AbstractPoint> pointList;
		internal List<AbstractLineSegment> lineList;

        private bool prevSet = false;
        private bool currSet = false;

        private AbstractPoint _prevPoint;
        private AbstractPoint _currPoint;
		private InteractionBehaviour thisIBehave;
		private bool updateLine;
        public GameObject scissors;
		private bool successfullyMade = false;
#pragma warning restore 0649

		private void Start()
		{
			pointList = new List<AbstractPoint>();
			lineList = new List<AbstractLineSegment> ();
			
			thisLR = GetComponent<LineRenderer>();
			thisLR.useWorldSpace = true;
			thisLR.positionCount = 2;
			thisLR.enabled = false;
			thisIBehave = GetComponent<InteractionBehaviour>();
			thisIBehave.OnGraspEnd += endGrasp;

		}

		private void endGrasp()
		{
			if (successfullyMade)
				Destroy(gameObject);

		}
		void Update()
		{
			if (updateLine)
			{
				thisLR.SetPosition(1, endofPin.position);
			}
		}

		internal AbstractPoint prevPoint
        {
            get
            {
                return _prevPoint;
            }

            set
            {
				prevLine(value);
                _prevPoint = value;
                prevSet = (value != null);
                //checkTwoPoints();
            }
        }

		private void prevLine(AbstractPoint value)
		{
			if (value == null)
			{
				thisLR.enabled = false;
				updateLine = false;
			}
			else
			{
				thisLR.enabled = true;
				thisLR.SetPosition(0, value.transform.position);
				updateLine = true;
				thisLR.SetPosition(1, endofPin.position);
			}
		}



		internal AbstractPoint currPoint
        {
            get
            {
                return _currPoint;
            }

            set
			{
				_currPoint = value;
				currSet = (value != null);
				if (_currPoint != null)
				{
					pointList.Add(_currPoint);
				}
				checkTwoPoints();
				prevPoint = _currPoint;
			}
		}

		private void checkTwoPoints()
        {
            if (prevSet && currSet && _prevPoint != _currPoint)
            {
				currPoint.tag = "Untagged";
				prevPoint.tag = "Untagged";
				switch (thisPinType)
				{
					case pinType.polymaker:
						lineList.Add(GeoObjConstruction.dLineSegment(currPoint, prevPoint));
						if (_currPoint == pointList[0] && lineList.Count > 1)
						{
							GeoObjConstruction.iPolygon(lineList, pointList);
							Destroy(gameObject, Time.fixedDeltaTime);
						}
						break;
					case pinType.wireframe:
						lineList.Add(GeoObjConstruction.dLineSegment(currPoint, prevPoint));
						successfullyMade = true;
						break;
					case pinType.solidmaker:
						Debug.Log("This isn't setup yet do not use! Object: "+gameObject.name);
						Destroy(gameObject,Time.fixedDeltaTime);
						break;
					case pinType.polycut:
						List<AbstractPolygon> prevPointPolygons = new List<AbstractPolygon>();
						List<AbstractPolygon> currPointPolygons = new List<AbstractPolygon>();
						HW_GeoSolver.ins.geomanager.bidirectionalNeighborsOfNode(_prevPoint.figName)//all bidirectional neighbors
							.Where(d => HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPolygon>() != null).ToList()// list of all abstractpolygons in prev list
							.ForEach(d => prevPointPolygons.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPolygon>()));	//foreach adds the polygon to final list
						HW_GeoSolver.ins.geomanager.bidirectionalNeighborsOfNode(_currPoint.figName)//same as above but with other point
							.Where(d => HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPolygon>() != null).ToList()
							.ForEach(d => currPointPolygons.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPolygon>()));
						//prevPointPolygons.ForEach(p => Debug.Log(_prevPoint.figName + " is in the following: " + p.figName));
						//Debug.Log("_____+_____");
						//currPointPolygons.ForEach(p => Debug.Log(_currPoint.figName + " is in the following: " + p.figName));
						//Debug.Log("_____=_____");
						List<AbstractPolygon> sharedPolygons;
						if (prevPointPolygons.Count > currPointPolygons.Count)
						{
							sharedPolygons = prevPointPolygons.Intersect(currPointPolygons).Where(poly => poly.lineList.Count > 3).ToList();
						}
						else
						{
							sharedPolygons = currPointPolygons.Intersect(prevPointPolygons).Where(poly => poly.lineList.Count > 3).ToList();
						}
						//sharedPolygons.ForEach(p => Debug.Log("Both are in the following: " + p.figName));
						//list created from any duplicates from two prev lists that have more than 3 sides
						//Debug.Log(_prevPoint.figName + " and " + _currPoint.figName + " are both on " + sharedPolygons.Count + " together.");
						if (sharedPolygons.Count > 0)
						{
							DependentLineSegment cutLine = GeoObjConstruction.dLineSegment(_prevPoint, _currPoint);
							foreach (AbstractPolygon p in sharedPolygons)
							{
								List<AbstractPoint> currPointList = p.pointList;
								List<AbstractLineSegment> currLineList = p.lineList;
								if (Mathf.Abs(currPointList.IndexOf(_prevPoint) - currPointList.IndexOf(_currPoint)) > 1)
								{
									List<AbstractPoint> newPointList1 = newPointListGen(currPointList, currPointList.IndexOf(_prevPoint), currPointList.IndexOf(_currPoint));
									List<AbstractPoint> newPointList2 = newPointListGen(currPointList, currPointList.IndexOf(_currPoint), currPointList.IndexOf(_prevPoint));

									List<AbstractLineSegment> newLineList1 = new List<AbstractLineSegment>() { cutLine };//creates list and adds the line created by the cut
									List<AbstractLineSegment> newLineList2 = new List<AbstractLineSegment>() { cutLine };//creates list and adds the line created by the cut
									foreach (AbstractLineSegment currLine in currLineList                        //newLineList1 Generator
										.Where(cL => ((cL.GetComponent<InteractableLineSegment>() != null &&         //is interactable line segment and point1 or point2 is found in newPointList1
											newPointList1.Any(point => point == cL.GetComponent<InteractableLineSegment>().point1 || point == cL.GetComponent<InteractableLineSegment>().point2))
									|| ((cL.GetComponent<DependentLineSegment>() != null &&                      //is dependent line segment and point1 or point2 is found in newPointList1
											newPointList1.Any(point => point == cL.GetComponent<DependentLineSegment>().point1 || point == cL.GetComponent<DependentLineSegment>().point2))))))
									{
										newLineList1.Add(currLine);
									}
									foreach (AbstractLineSegment currLine in currLineList                           //newLineList2 Generator
											.Where(cL => ((cL.GetComponent<InteractableLineSegment>() != null &&    //is interactable line segment and point1 or point2 is found in newPointList1
												newPointList2.Any(point => point == cL.GetComponent<InteractableLineSegment>().point1 || point == cL.GetComponent<InteractableLineSegment>().point2))
										|| ((cL.GetComponent<DependentLineSegment>() != null &&                     //is dependent line segment and point1 or point2 is found in newPointList1
												newPointList2.Any(point => point == cL.GetComponent<DependentLineSegment>().point1 || point == cL.GetComponent<DependentLineSegment>().point2))))))
									{
										newLineList2.Add(currLine);
									}
									AbstractPolygon newPoly1 = GeoObjConstruction.iPolygon(newLineList1, newPointList1);
									AbstractPolygon newPoly2 = GeoObjConstruction.iPolygon(newLineList2, newPointList2);
									List<InteractablePrism> currPrismList = new List<InteractablePrism>();
									HW_GeoSolver.ins.geomanager.bidirectionalNeighborsOfNode(p.figName)//all bidirectional neighbors
										.Where(d => HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<InteractablePrism>() != null).ToList().ForEach(prism => currPrismList.Add(prism.mytransform.GetComponent<InteractablePrism>()));
									foreach (InteractablePrism cPrism in currPrismList)
									{
										HW_GeoSolver.ins.AddDependence(newPoly1, cPrism);
										HW_GeoSolver.ins.AddDependence(newPoly2, cPrism);
									}
									HW_GeoSolver.ins.removeComponent(p);
								}
							}
							//GameObject currScissors = Instantiate(scissors);
							//currScissors.transform.position = _prevPoint.transform.position;														Cuts things
							//currScissors.GetComponent<scissorAnimator>().travel(_prevPoint.transform.position, _currPoint.transform.position);
							Destroy(gameObject, Time.fixedDeltaTime);
						}
						break;
					default:
						break;
				}
            }
        }


        /// <summary>
        /// Use this function to generate a newPointList from a larger polygon.pointList
        /// </summary>
        /// <param name="currPointList"> larger polygon pointList</param>
        /// <param name="index1">Index of the starting point you want to add to list</param>
        /// <param name="index2">Index of the last point you want to add to the list</param>
        /// <returns>List of points from currPointList from index1 to index2</returns>
        private List<AbstractPoint> newPointListGen(List<AbstractPoint> currPointList, int index1, int index2)
		{
			List<AbstractPoint> result = new List<AbstractPoint>();
			for (int i = index1; i != index2;/*incremented below*/)
			{
				result.Add(currPointList[i]);
				if(i == currPointList.Count-1)//if last in list
				{
					i = 0;//wrap around
				}
				else
				{
					i++;
				}
			}
			result.Add(currPointList[index2]);
			return result;
		}
	}

}