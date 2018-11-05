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

namespace IMRE.HandWaver.Interface
{
	[RequireComponent(typeof(LineRenderer))]
	[RequireComponent(typeof(AudioSource))]
	public class LatticeLandPoint : OneHandedGesture
	{

		[Space]
		[Header("LatticeLand Point Gesture Properties")]
		[Space]

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

		private LineRenderer thisLR {
		get
			{
				return this.GetComponent<LineRenderer>();
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

		public AudioClip successSound;
		//public AudioClip errorSound;

		private AudioSource _myAudioSource;

		private AudioSource MyAudioSource
		{
			get
			{
				if (_myAudioSource == null)
					_myAudioSource = GetComponent<AudioSource>();
				return _myAudioSource;
			}
		}

		private void Awake()
		{
			pointList = new List<AbstractPoint>();
			lineList = new List<AbstractLineSegment>();

			if(this.thisLR == null)
			{
				this.gameObject.AddComponent<LineRenderer>();
			}
			thisLR.useWorldSpace = true;
			thisLR.positionCount = 2;
			thisLR.enabled = false;
			thisLR.startWidth = .005f;
			thisLR.endWidth = .005f;
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
			(hand.Fingers[1].IsExtended) &&
			!(hand.Fingers[2].IsExtended) &&
			!(hand.Fingers[3].IsExtended) &&
			!(hand.Fingers[4].IsExtended) &&
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
					(hand.Fingers[1].IsExtended) &&
					!(hand.Fingers[2].IsExtended) &&
					!(hand.Fingers[3].IsExtended) &&
					!(hand.Fingers[4].IsExtended) &&
					!hand.IsPinching()
					))
			{
				deactivationReason = DeactivationReason.CancelledGesture;
				prevSet = false;
				currSet = false;
				return true;
			}
			else if (foundMGO(hand))
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

		public bool foundMGO(Hand hand)
		{
			float shortestDist = Mathf.Infinity;
			closestObj = null;

			foreach (StaticPoint mgo in FindObjectsOfType<StaticPoint>().Where(g => (g.GetComponent<AnchorableBehaviour>() == null || (g.GetComponent<AnchorableBehaviour>() != null && !g.GetComponent<AnchorableBehaviour>().isAttached))))
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
			if(shortestDist < maximumRangeToSelect)
			{
				currPoint = closestObj.GetComponent<AbstractPoint>();
			}
			return shortestDist < maximumRangeToSelect;
		}

		/// <summary>
		/// On Gesture end
		/// </summary>
		/// <param name="maybeNullHand">Hand reference that ended the gesture</param>
		/// <param name="reason">reason the gesture was ended</param>
		protected override void WhenGestureDeactivated(Hand maybeNullHand, DeactivationReason reason)
		{
			this.thisLR.enabled = false;

			if (maybeNullHand != null)
			{

				switch (reason)
				{
					case DeactivationReason.FinishedGesture:
						if (maybeNullHand.Fingers[0].IsExtended)
						{
							//thumb up is draw polygon
							if (prevSet && currSet && _prevPoint != _currPoint)
							{
								lineList.Add(GeoObjConstruction.dLineSegment(currPoint, prevPoint));
								playSuccessSound();
								if (_currPoint == pointList[0] && lineList.Count > 1)
								{
									GeoObjConstruction.iPolygon(lineList, pointList);
									endInteraction();
								}
							}
						}
						else
						{
							//thumb down is draw wireframe
							if (prevSet && currSet && _prevPoint != _currPoint)
							{
								lineList.Add(GeoObjConstruction.dLineSegment(currPoint, prevPoint));
								playSuccessSound();
							}
						}
					break;
					default:
						break;
				}

				Chirality chirality = Chirality.Right;
				if (maybeNullHand.IsLeft)
				{
					chirality = Chirality.Left;
				}
				handColourManager.setHandColorMode(chirality, handColourManager.handModes.none);
			}


		}

		/// <summary>
		/// Draw the line between the last point and the current finger.
		/// </summary>
		/// <param name="hand">The hand completing the gesture</param>
		protected override void WhileGestureActive(Hand hand)
		{
			//needs to draw that line.
			thisLR.SetPosition(1, hand.Fingers[1].TipPosition.ToVector3());
			thisLR.enabled = currPoint != null;

			Chirality chirality = Chirality.Right;
			if (hand.IsLeft)
			{
				chirality = Chirality.Left;
			}
			handColourManager.setHandColorMode(chirality, handColourManager.handModes.select);

		}

		private float angleHandToMGO(Hand hand, StaticPoint mgo)
		{
			return Vector3.Angle(mgo.transform.position-fingerTip(hand), fingerDirection(hand));
		}

		private float distHandToMGO(Hand hand, StaticPoint mgo)
		{
			return Vector3.Magnitude(fingerTip(hand) - mgo.transform.position);
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
			if (MyAudioSource != null)
			{
				MyAudioSource.clip = (successSound);
				if (MyAudioSource.isPlaying)
					MyAudioSource.Stop();
				MyAudioSource.Play();
			}
		}

		public void endInteraction()
		{
			thisLR.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
			thisLR.enabled = false;
			updateLine = false;
			pointList.Clear();
			lineList.Clear();
			prevSet = false;
			currSet = false;
			successfullyMade = false;
				lineList.ForEach(l => l.deleteGeoObj());
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
				if(_currPoint != null)
					prevPoint = _currPoint;

				_currPoint = value;
				currSet = (value != null);
				thisLR.SetPosition(0, _currPoint.transform.position);
				if (_currPoint != null)
				{
					pointList.Add(_currPoint);
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
	}
}	
