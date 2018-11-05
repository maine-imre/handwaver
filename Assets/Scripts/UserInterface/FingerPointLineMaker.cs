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
using UnityEngine.SceneManagement;
using Leap.Unity;
using UnityEngine.Events;
using PathologicalGames;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver.Lattice
{

	//NEVER PUT ON AN OBJECT THAT YOU WANT TO PERSIST THROUGH LATTICELAND UNLOAD

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class FingerPointLineMaker : MonoBehaviour
	{
        [Range(0,5)]
        public float eraserColliderSize;
        public bool eraserEnabled = true;

		public Leap.Unity.Chirality handedness;

#pragma warning disable 0649
		[System.Serializable]
		public enum pinType { polymaker, wireframe, solidmaker, polycut, none, eraser };

		private LineRenderer thisLR;
		private Transform endofPin;
		private pinType _thisPinType = pinType.none;
		public pinType thisPinType
		{
			get
			{
				return _thisPinType;
			}
			set
			{
				_thisPinType = value;
			}
		}
		internal List<AbstractPoint> pointList;
		internal List<AbstractLineSegment> lineList;

		private bool prevSet = false;
		private bool currSet = false;

		private AbstractPoint _prevPoint;
		private AbstractPoint _currPoint;
		private bool updateLine;
		private bool successfullyMade = false;

		private List<ExtendedFingerDetector> fingerDetectors = new List<ExtendedFingerDetector>();
#pragma warning restore 0649
		private eraserBehave eraser;

		public Transform palmAttachment;

		public Transform indexAttachment;

		private fingerPointLineMakerHandle lineMaker;

		public static FingerPointLineMaker left;
		public static FingerPointLineMaker right;


		private void Start()
		{
			if (this.GetComponent<LatticeLandOpenVR>() == null)
			{
				//fingerDetectors = GetComponents<>().ToList();
				setupDetectors();

				switch (handedness)
				{
					case Chirality.Left:
						indexAttachment = leapHandDataLogger.ins.currHands.Lhand_rigged.fingers[1].bones[3].transform.GetChild(0).transform;
						palmAttachment = leapHandDataLogger.ins.currHands.LHand_attachment.palm.transform;
						left = this;
						break;
					case Chirality.Right:
						indexAttachment = leapHandDataLogger.ins.currHands.RHand_rigged.fingers[1].bones[3].transform.GetChild(0).transform;
						palmAttachment = leapHandDataLogger.ins.currHands.RHand_attachment.palm.transform;
						right = this;
						break;
					default:
						break;
				}
			}
		}

		public void setupDetectors()
		{

            //setup line maker detectors
            for (int idx = 0; idx < 3; idx++)
			{
				ExtendedFingerDetector fingerDetector = fingerDetectors[idx];

				if (fingerDetector == null)
				{
					return;
				}

				float period = .25f;

				switch (handedness)
				{
					case Chirality.Left:
						fingerDetector.HandModel = leapHandDataLogger.ins.currHands.Lhand_rigged;
						break;
					case Chirality.Right:
						fingerDetector.HandModel = leapHandDataLogger.ins.currHands.RHand_rigged;
						break;
					default:
						break;
				}
				fingerDetector.Period = period;

				switch (idx)
				{
					case 0:
						fingerDetector.OnActivate.AddListener(setActiveOneFinger);
						fingerDetector.Thumb = PointingState.NotExtended;
						fingerDetector.Index = PointingState.Extended;
						fingerDetector.Middle = PointingState.NotExtended;
						fingerDetector.Ring = PointingState.NotExtended;
						fingerDetector.Pinky = PointingState.NotExtended;
						fingerDetector.MinimumExtendedCount = 1;
						fingerDetector.MaximumExtendedCount = 1;
						break;
					case 1:
						fingerDetector.OnActivate.AddListener(setActiveTwoFinger);
						fingerDetector.Thumb = PointingState.Extended;
						fingerDetector.Index = PointingState.Extended;
						fingerDetector.Middle = PointingState.Either;
						fingerDetector.Ring = PointingState.NotExtended;
						fingerDetector.Pinky = PointingState.NotExtended;
						fingerDetector.MinimumExtendedCount = 2;
						fingerDetector.MaximumExtendedCount = 3;
						break;
					case 2:
                        if (eraserEnabled)
                        {
                            fingerDetector.OnActivate.AddListener(setActiveFourFinger);
                        }
						fingerDetector.Thumb = PointingState.Either;
						fingerDetector.Index = PointingState.Extended;
						fingerDetector.Middle = PointingState.Extended;
						fingerDetector.Ring = PointingState.Extended;
						fingerDetector.Pinky = PointingState.Extended;
						fingerDetector.MinimumExtendedCount = 4;
						fingerDetector.MaximumExtendedCount = 5;
						break;
					default:
						break;
				}
				//why does this have an error?
				fingerDetector.OnDeactivate.AddListener(setUnactive);
			}
		}

		private void setUnactive()
		{
			thisPinType = pinType.none;
			endInteraction();
			eraserDetach();
		}

		internal void eraserDetach()
		{
			if (eraser != null)
			{
				eraser.follow(false);
				eraser.transform.position = Vector3.down;
				PoolManager.Pools["Tools"].Despawn(eraser.transform);
				eraser = null;
			}
		}

		internal void eraserAttach()
		{
			eraser = PoolManager.Pools["Tools"].Spawn("eraser2Prefab").GetComponent<eraserBehave>();
			eraser.transform.position = palmAttachment.position;
			eraser.transform.localPosition = new Vector3(0, 1, 0);
			eraser.transform.parent = palmAttachment;
			eraser.follow(palmAttachment);
			eraser.hideMesh();
        }

		internal void setActiveFourFinger()
		{
			Debug.Log("eraser mode");
			thisPinType = pinType.eraser;
			eraserAttach();
		}

		internal void setActiveTwoFinger()
		{
			thisPinType = pinType.polymaker;
		}

		internal void setActiveOneFinger()
		{
			thisPinType = pinType.wireframe;
		}

		private void changeActiveStateByScene(Scene arg0)
		{
			if(arg0.name == "LatticeLand" || arg0.name == "ShearingLab" )
			{
				gameObject.SetActive(false);
			}
		}

		public void endInteraction()
		{
			thisLR.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
			updateLine = false;
			pointList.Clear();
			lineList.Clear();
			prevSet = false;
			currSet = false;
			successfullyMade = false;
			lineMaker.delaySearch();
			if (!successfullyMade)
			{
				lineList.ForEach(l => l.deleteGeoObj());
			}
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
						//if (_currPoint == pointList[0] && lineList.Count > 1 && pointsAreCoPlanar(pointList,lineList))
						if (_currPoint == pointList[0] && lineList.Count > 1)
						{
							GeoObjConstruction.iPolygon(lineList, pointList);
							endInteraction();
						}
						successfullyMade = true;
						break;
					case pinType.wireframe:
						lineList.Add(GeoObjConstruction.dLineSegment(currPoint, prevPoint));
						successfullyMade = true;
						polyCut();
						break;
					case pinType.solidmaker:
						Debug.Log("This isn't setup yet do not use! Object: " + gameObject.name);
						Destroy(gameObject, Time.fixedDeltaTime);
						break;
					default:
						break;
					
				}
			}
		}

		private void polyCut()
		{
			List<AbstractPolygon> prevPointPolygons = new List<AbstractPolygon>();
			List<AbstractPolygon> currPointPolygons = new List<AbstractPolygon>();
			HW_GeoSolver.ins.geomanager.bidirectionalNeighborsOfNode(_prevPoint.figName)//all bidirectional neighbors
				.Where(d => HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPolygon>() != null).ToList()// list of all abstractpolygons in prev list
				.ForEach(d => prevPointPolygons.Add(HW_GeoSolver.ins.geomanager.findGraphNode(d.Value).mytransform.GetComponent<AbstractPolygon>()));   //foreach adds the polygon to final list
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
							HW_GeoSolver.ins.addDependence(newPoly1.transform, cPrism.transform);
							HW_GeoSolver.ins.addDependence(newPoly2.transform, cPrism.transform);
						}
						HW_GeoSolver.ins.removeComponent(p);
					}
				}

			}
		}

		private bool pointsAreCoPlanar(List<AbstractPoint> pointList, List<AbstractLineSegment> lineList)
		{
			Vector3[] pointPos = new Vector3[pointList.Count];
			foreach(AbstractPoint point in pointList)
			{
				pointPos[pointList.IndexOf(point)] = point.transform.position;
			}

			int idx = 0;
			Vector3 basis0 = pointPos[0] - pointPos[1];
			while (basis0.magnitude == 0)
			{
				if (idx == pointList.Count-1)
				{
					return false;
				}
				basis0 = pointPos[idx] - pointPos[idx + 1];
				idx++;
			}

			Vector3 basis1 = pointPos[idx] - pointPos[idx + 1];
			while(basis1.magnitude == 0 || Vector3.Cross(basis1,basis0).magnitude == 0)
			{
				if (idx == pointList.Count - 1)
				{
					return false;
				}
				basis1 = pointPos[idx] - pointPos[idx + 1];
				idx++;
			}

			Vector3 normDir = Vector3.Cross(basis0, basis1).normalized;

			foreach(AbstractLineSegment line in lineList)
			{
				Vector3 linedir = line.vertex0 - line.vertex1;
				if(Vector3.Project(linedir,normDir).magnitude > 0f)
				{
					return false;
				}
			}

			//you made it!
			return true;
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
				if (i == currPointList.Count - 1)//if last in list
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