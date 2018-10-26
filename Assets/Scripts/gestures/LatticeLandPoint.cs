using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Gestures;
using Leap;
using System;
using System.Linq;
using Leap.Unity.Interaction;
using Leap.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using PathologicalGames;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver.Lattice
{
	public class LatticeLandPoint : OneHandedGesture
	{

		[Space]
		[Header("LatticeLand Point Gesture Properties")]
		[Space]

		/// <summary>
		/// Desired State for extended fingers.
		/// </summary>
		public fingerExtentionBools fingerExtentionState;

		/// <summary>
		/// Whether you have selected something previously with the same instance of a gesture.
		/// </summary>
		public bool completeBool = false;

		/// <summary>
		/// Toggles the debugging out of closest object when it toggles selection state.
		/// </summary>
		public bool debugSelect = false;


		/// <summary>
		/// How close you have to be to ativate a selection.
		/// </summary>
		public float maximumRangeToSelect = 0.1f;

		public float angleTolerance = 65f;


		private MasterGeoObj closestObj;

		public enum pinType { polymaker, wireframe, solidmaker, polycut, none, eraser };
		private LineRenderer thisLR;
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

		private eraserBehave eraser;
		[Space]

		public AudioClip successSound;
		//public AudioClip errorSound;

		private AudioSource _myAudioSource;

		private AudioSource myAudioSource
		{
			get
			{
				if (_myAudioSource == null)
					_myAudioSource = GetComponent<AudioSource>();
				return _myAudioSource;
			}
		}

		private void Start()
		{

			if (this.GetComponent<LatticeLandOpenVR>() == null)
			{
				//fingerDetectors = GetComponents<>().ToList();
				//setupDetectors();
			}

			pointList = new List<AbstractPoint>();
			lineList = new List<AbstractLineSegment>();

			thisLR = GetComponent<LineRenderer>();
			thisLR.useWorldSpace = true;
			thisLR.positionCount = 2;
			thisLR.enabled = false;
			thisLR.startWidth = .005f;
			thisLR.endWidth = .005f;
			SceneManager.sceneUnloaded += changeActiveStateByScene;

			//lineMaker = GetComponentInChildren<fingerPointLineMakerHandle>();
			//if (lineMaker != null)
			//{
			//	lineMaker.transform.parent = indexAttachment;
			//	lineMaker.transform.localPosition = Vector3.zero;
			//	lineMaker.lineSegmentMaker = this;
			//}
			//else
			//{
			//	Debug.LogWarning("Requires lineMaker to function " + name);
			//}

			//endofPin = fingerTip(hand);
		}

		/// <summary>
		/// Returns true if hand has a pointing gesture
		/// </summary>
		/// <param name="hand">hand that is checked for gesture</param>
		/// <returns>desired gesture activation state</returns>
		protected override bool ShouldGestureActivate(Hand hand)
		{
			bool tmp =
			(/*(fingerExtentionState.thumbExtended && hand.Fingers[0].IsExtended) &&*/
			(fingerExtentionState.pointerFingerExtended && hand.Fingers[1].IsExtended) &&
			!(fingerExtentionState.middleFingerExtended && hand.Fingers[2].IsExtended) &&
			!(fingerExtentionState.ringFingerExtended && hand.Fingers[3].IsExtended) &&
			!(fingerExtentionState.pinkyFingerExtended && hand.Fingers[4].IsExtended) &&
			!completeBool &&
			!hand.IsPinching()
			);
			return tmp;
		}

		/// <summary>
		/// Returns if you want a gesture to deactivate and for what reason.
		/// </summary>
		/// <param name="hand">Hand to test gesture</param>
		/// <param name="deactivationReason">reason for deactivation</param>
		/// <returns>desired gesture completion state</returns>
		protected override bool ShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason)
		{
			if (!isHandTracked ||
					!(/*(fingerExtentionState.thumbExtended && hand.Fingers[0].IsExtended) &&*/
					(fingerExtentionState.pointerFingerExtended && hand.Fingers[1].IsExtended) &&
					!(fingerExtentionState.middleFingerExtended && hand.Fingers[2].IsExtended) &&
					!(fingerExtentionState.ringFingerExtended && hand.Fingers[3].IsExtended) &&
					!(fingerExtentionState.pinkyFingerExtended && hand.Fingers[4].IsExtended) &&
					!completeBool &&
					!hand.IsPinching()
					))
			{
				deactivationReason = DeactivationReason.CancelledGesture;
				return true;
			}
			else if (completeBool)
			{
				deactivationReason = DeactivationReason.FinishedGesture;
				return true;
			}
			else
			{
				deactivationReason = null;
				return false;
			}
		}

		/// <summary>
		/// On Gesture end
		/// </summary>
		/// <param name="maybeNullHand">Hand reference that ended the gesture</param>
		/// <param name="reason">reason the gesture was ended</param>
		protected override void WhenGestureDeactivated(Hand maybeNullHand, DeactivationReason reason)
		{
			//sethandMNodeNone

			switch (reason)
			{
				case DeactivationReason.FinishedGesture:
					break;
				//case DeactivationReason.CancelledGesture:
				//	if (!completeBool)
				//		//playErrorSound();
				//	break;
				default:
					break;
			}
			if (maybeNullHand != null)
			{
				Chirality chirality = Chirality.Right;
				if (maybeNullHand.IsLeft)
				{
					chirality = Chirality.Left;
				}
				handColourManager.setHandColorMode(chirality, handColourManager.handModes.none);
				StartCoroutine(gestureCooldown());
			}


		}

		IEnumerator gestureCooldown()
		{
			yield return new WaitForSeconds(1f);
			completeBool = false;
		}

		/// <summary>
		/// Called each frame the gesture is active
		/// </summary>
		/// <param name="hand">The hand completing the gesture</param>
		protected override void WhileGestureActive(Hand hand)
		{
			//sethandtomodeSelect
			float shortestDist = Mathf.Infinity;
			closestObj = null;

			foreach (MasterGeoObj mgo in FindObjectsOfType<MasterGeoObj>().Where(g => (g.GetComponent<AnchorableBehaviour>() == null || (g.GetComponent<AnchorableBehaviour>() != null && !g.GetComponent<AnchorableBehaviour>().isAttached))))
			{
				float distance = distHandToMGO(hand, mgo);
				float angle = angleHandToMGO(hand, mgo);

				if (Mathf.Abs(distance) < shortestDist)
				{
					if (distance < shortestDist && angle < angleTolerance)
					{
						closestObj = mgo;
						shortestDist = distance;
					}
				}
			}


			Chirality chirality = Chirality.Right;
			if (hand.IsLeft)
			{
				chirality = Chirality.Left;
			}
			handColourManager.setHandColorMode(chirality, handColourManager.handModes.select);

			if (closestObj != null && shortestDist <= maximumRangeToSelect)
			{
				if (debugSelect)
					Debug.Log(closestObj + " is the object toggling selection state.");



				//switch on mode:
				//				Select
				//				Colour
				if (closestObj.IsSelected)
					closestObj.thisSelectStatus = MasterGeoObj.SelectionStatus.none;
				else
					closestObj.thisSelectStatus = MasterGeoObj.SelectionStatus.selected;


				playSuccessSound();

				//This determines if you have to cancel the gesture to select another object
				completeBool = true;
			}

		}

		private float angleHandToMGO(Hand hand, MasterGeoObj mgo)
		{
			float angle = 370;
			switch (mgo.figType)
			{
				case GeoObjType.point:
					angle = Vector3.Angle(fingerTip(hand) - mgo.transform.position, fingerDirection(hand));
					break;
				//case GeoObjType.line:
				//	angle = Vector3.Angle(Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<AbstractLineSegment>().vertex0 - mgo.GetComponent<AbstractLineSegment>().vertex1) + mgo.transform.position - fingerTip(hand), fingerDirection(hand));
				//	break;
				//case GeoObjType.polygon:
				//	Vector3 positionOnPlane = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractPolygon>().normDir) + mgo.transform.position;
				//	angle = Vector3.Angle(positionOnPlane - fingerTip(hand), fingerDirection(hand));
				//	Debug.LogWarning("Polygon doesn't check boundariers");
				//	break;
				//case GeoObjType.prism:
				//	angle = Vector3.Angle(mgo.transform.position - fingerTip(hand), fingerDirection(hand));
				//	break;
				//case GeoObjType.pyramid:
				//	Debug.LogWarning("Pyramids not yet supported");
				//	break;
				//case GeoObjType.circle:
				//	Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractCircle>().normalDir) + mgo.transform.position;
				//	Vector3 positionOnCircle = Vector3.Normalize(positionOnPlane2 - mgo.GetComponent<AbstractCircle>().centerPos) * mgo.GetComponent<AbstractCircle>().Radius + mgo.GetComponent<AbstractCircle>().centerPos;
				//	angle = Vector3.Angle(fingerTip(hand) - positionOnCircle, fingerDirection(hand));
				//	break;
				//case GeoObjType.sphere:
				//	Vector3 lineDir = Vector3.Normalize(transform.position - mgo.transform.position);
				//	Vector3 positionOnSphere1 = mgo.GetComponent<AbstractSphere>().radius * lineDir + mgo.transform.position;
				//	angle = Vector3.Angle(positionOnSphere1 - fingerTip(hand), fingerDirection(hand));
				//	break;
				//case GeoObjType.revolvedsurface:
				//	Debug.LogWarning("RevoledSurface not yet supported");
				//	break;
				//case GeoObjType.torus:
				//	Debug.LogWarning("Torus not yet supported");
				//	break;
				//case GeoObjType.flatface:
				//	Vector3 positionOnPlane3 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<flatfaceBehave>().normalDir) + mgo.transform.position;
				//	angle = Vector3.Angle(positionOnPlane3 - fingerTip(hand), fingerDirection(hand));
				//	break;
				//case GeoObjType.straightedge:
				//	Vector3 positionOnStraightedge = Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<straightEdgeBehave>().normalDir) + mgo.transform.position;
				//	angle = Vector3.Angle(positionOnStraightedge - fingerTip(hand), fingerDirection(hand));
				//	break;
				default:
					Debug.LogWarning("Something went wrong in the selection.... :(");
					break;
			}

			return angle;
		}

		private float distHandToMGO(Hand hand, MasterGeoObj mgo)
		{
			float distance = 15;
			switch (mgo.figType)
			{
				case GeoObjType.point:
					distance = Vector3.Magnitude(fingerTip(hand) - mgo.transform.position);
					break;
				//case GeoObjType.line:
				//	distance = Vector3.Magnitude(Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<AbstractLineSegment>().vertex0 - mgo.GetComponent<AbstractLineSegment>().vertex1) + mgo.transform.position - fingerTip(hand));
				//	break;
				//case GeoObjType.polygon:
				//	Vector3 positionOnPlane = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractPolygon>().normDir) + mgo.transform.position;
				//	distance = Vector3.Magnitude(positionOnPlane - fingerTip(hand));
				//	Debug.LogWarning("Polygon doesn't check boundariers");
				//	break;
				//case GeoObjType.prism:
				//	distance = Vector3.Magnitude(mgo.transform.position - fingerTip(hand));
				//	break;
				//case GeoObjType.pyramid:
				//	Debug.LogWarning("Pyramids not yet supported");
				//	break;
				//case GeoObjType.circle:
				//	Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractCircle>().normalDir) + mgo.transform.position;
				//	Vector3 positionOnCircle = Vector3.Normalize(positionOnPlane2 - mgo.GetComponent<AbstractCircle>().centerPos) * mgo.GetComponent<AbstractCircle>().Radius + mgo.GetComponent<AbstractCircle>().centerPos;
				//	distance = Vector3.Magnitude(fingerTip(hand) - positionOnCircle);
				//	break;
				//case GeoObjType.sphere:
				//	Vector3 lineDir = Vector3.Normalize(transform.position - mgo.transform.position);
				//	Vector3 positionOnSphere1 = mgo.GetComponent<AbstractSphere>().radius * lineDir + mgo.transform.position;
				//	distance = Vector3.Magnitude(positionOnSphere1 - fingerTip(hand));
				//	break;
				//case GeoObjType.revolvedsurface:
				//	Debug.LogWarning("RevoledSurface not yet supported");
				//	break;
				//case GeoObjType.torus:
				//	Debug.LogWarning("Torus not yet supported");
				//	break;
				//case GeoObjType.flatface:
				//	Vector3 positionOnPlane3 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<flatfaceBehave>().normalDir) + mgo.transform.position;
				//	distance = Vector3.Magnitude(positionOnPlane3 - fingerTip(hand));
				//	break;
				//case GeoObjType.straightedge:
				//	Vector3 positionOnStraightedge = Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<straightEdgeBehave>().normalDir) + mgo.transform.position;
				//	distance = Vector3.Magnitude(positionOnStraightedge - fingerTip(hand));
				//	break;
				default:
					Debug.LogWarning("Something went wrong in the selection.... :(");
					break;
			}

			return distance;
		}

		public Vector3 fingerTip(Hand hand)
		{
			return hand.Fingers[1].TipPosition.ToVector3();
		}

		public Vector3 fingerDirection(Hand hand)
		{
			return hand.Fingers[1].Direction.ToVector3();
		}

		private void playSuccessSound()
		{
			myAudioSource.clip = (successSound);
			if (myAudioSource.isPlaying)
				myAudioSource.Stop();
			myAudioSource.Play();
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
			//eraser.transform.position = palmAttachment.position;
			eraser.transform.localPosition = new Vector3(0, 1, 0);
			//eraser.transform.parent = palmAttachment;
			//eraser.follow(palmAttachment);
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
			if (arg0.name == "LatticeLand" || arg0.name == "ShearingLab")
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
			//lineMaker.delaySearch();
			if (!successfullyMade)
			{
				lineList.ForEach(l => l.deleteGeoObj());
			}
		}

		void Update()
		{
			if (updateLine)
			{
				//thisLR.SetPosition(1, endofPin.position);
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
				//thisLR.SetPosition(1, endofPin.position);
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
			foreach (AbstractPoint point in pointList)
			{
				pointPos[pointList.IndexOf(point)] = point.transform.position;
			}

			int idx = 0;
			Vector3 basis0 = pointPos[0] - pointPos[1];
			while (basis0.magnitude == 0)
			{
				if (idx == pointList.Count - 1)
				{
					return false;
				}
				basis0 = pointPos[idx] - pointPos[idx + 1];
				idx++;
			}

			Vector3 basis1 = pointPos[idx] - pointPos[idx + 1];
			while (basis1.magnitude == 0 || Vector3.Cross(basis1, basis0).magnitude == 0)
			{
				if (idx == pointList.Count - 1)
				{
					return false;
				}
				basis1 = pointPos[idx] - pointPos[idx + 1];
				idx++;
			}

			Vector3 normDir = Vector3.Cross(basis0, basis1).normalized;

			foreach (AbstractLineSegment line in lineList)
			{
				Vector3 linedir = line.vertex0 - line.vertex1;
				if (Vector3.Project(linedir, normDir).magnitude > 0f)
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


		//private void playErrorSound()
		//{
		//	myAudioSource.clip = (errorSound);
		//	if (myAudioSource.isPlaying)
		//		myAudioSource.Stop();
		//	myAudioSource.Play();
		//}

	}
}	
