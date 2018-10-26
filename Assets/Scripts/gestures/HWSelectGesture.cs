using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Gestures;
using Leap;
using System;
using System.Linq;
using Leap.Unity.Interaction;
using Leap.Unity;

namespace IMRE.HandWaver {
	[System.Serializable]
		public struct fingerExtentionBools
		{
			public bool thumbExtended;
			public bool pointerFingerExtended;
			public bool middleFingerExtended;
			public bool ringFingerExtended;
			public bool pinkyFingerExtended;

		}
	[RequireComponent(typeof(AudioSource))]
	public class HWSelectGesture : OneHandedGesture
	{
		
		[Space]
		[Header("Select Gesture Properties")]
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


		/// <summary>
		/// Returns true if hand has a pointing gesture
		/// </summary>
		/// <param name="hand">hand that is checked for gesture</param>
		/// <returns>desired gesture activation state</returns>
		protected override bool ShouldGestureActivate(Hand hand)
		{
			bool tmp =
			(/*(fingerExtentionState.thumbExtended && hand.Fingers[0].IsExtended) &&*/
			(fingerExtentionState.pointerFingerExtended && hand.Fingers[1].IsExtended)  &&
			!(fingerExtentionState.middleFingerExtended && hand.Fingers[2].IsExtended)  &&
			!(fingerExtentionState.ringFingerExtended && hand.Fingers[3].IsExtended)    &&
			!(fingerExtentionState.pinkyFingerExtended && hand.Fingers[4].IsExtended)   &&
			!completeBool																&&
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

			if (closestObj != null  && shortestDist <= maximumRangeToSelect)
			{
				if(debugSelect)
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
					angle = Vector3.Angle(fingerTip(hand) - mgo.transform.position,fingerDirection(hand));
					break;
				case GeoObjType.line:
					angle = Vector3.Angle(Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<AbstractLineSegment>().vertex0 - mgo.GetComponent<AbstractLineSegment>().vertex1) + mgo.transform.position - fingerTip(hand),fingerDirection(hand));
					break;
				case GeoObjType.polygon:
					Vector3 positionOnPlane = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractPolygon>().normDir) + mgo.transform.position;
					angle = Vector3.Angle(positionOnPlane - fingerTip(hand),fingerDirection(hand));
					Debug.LogWarning("Polygon doesn't check boundariers");
					break;
				case GeoObjType.prism:
					angle = Vector3.Angle(mgo.transform.position - fingerTip(hand), fingerDirection(hand));
					break;
				case GeoObjType.pyramid:
					Debug.LogWarning("Pyramids not yet supported");
					break;
				case GeoObjType.circle:
					Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractCircle>().normalDir) + mgo.transform.position;
					Vector3 positionOnCircle = Vector3.Normalize(positionOnPlane2 - mgo.GetComponent<AbstractCircle>().centerPos) * mgo.GetComponent<AbstractCircle>().Radius + mgo.GetComponent<AbstractCircle>().centerPos;
					angle = Vector3.Angle(fingerTip(hand) - positionOnCircle, fingerDirection(hand));
					break;
				case GeoObjType.sphere:
					Vector3 lineDir = Vector3.Normalize(transform.position - mgo.transform.position);
					Vector3 positionOnSphere1 = mgo.GetComponent<AbstractSphere>().radius * lineDir + mgo.transform.position;
					angle = Vector3.Angle(positionOnSphere1 - fingerTip(hand), fingerDirection(hand));
					break;
				case GeoObjType.revolvedsurface:
					Debug.LogWarning("RevoledSurface not yet supported");
					break;
				case GeoObjType.torus:
					Debug.LogWarning("Torus not yet supported");
					break;
				case GeoObjType.flatface:
					Vector3 positionOnPlane3 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<flatfaceBehave>().normalDir) + mgo.transform.position;
					angle = Vector3.Angle(positionOnPlane3 - fingerTip(hand),fingerDirection(hand));
					break;
				case GeoObjType.straightedge:
					Vector3 positionOnStraightedge = Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<straightEdgeBehave>().normalDir) + mgo.transform.position;
					angle = Vector3.Angle(positionOnStraightedge - fingerTip(hand), fingerDirection(hand));
					break;
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
				case GeoObjType.line:
					distance = Vector3.Magnitude(Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<AbstractLineSegment>().vertex0 - mgo.GetComponent<AbstractLineSegment>().vertex1) + mgo.transform.position - fingerTip(hand));
					break;
				case GeoObjType.polygon:
					Vector3 positionOnPlane = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractPolygon>().normDir) + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnPlane - fingerTip(hand));
					Debug.LogWarning("Polygon doesn't check boundariers");
					break;
				case GeoObjType.prism:
					distance = Vector3.Magnitude(mgo.transform.position - fingerTip(hand));
					break;
				case GeoObjType.pyramid:
					Debug.LogWarning("Pyramids not yet supported");
					break;
				case GeoObjType.circle:
					Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractCircle>().normalDir) + mgo.transform.position;
					Vector3 positionOnCircle = Vector3.Normalize(positionOnPlane2 - mgo.GetComponent<AbstractCircle>().centerPos) * mgo.GetComponent<AbstractCircle>().Radius + mgo.GetComponent<AbstractCircle>().centerPos;
					distance = Vector3.Magnitude(fingerTip(hand) - positionOnCircle);
					break;
				case GeoObjType.sphere:
					Vector3 lineDir = Vector3.Normalize(transform.position - mgo.transform.position);
					Vector3 positionOnSphere1 = mgo.GetComponent<AbstractSphere>().radius * lineDir + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnSphere1 - fingerTip(hand));
					break;
				case GeoObjType.revolvedsurface:
					Debug.LogWarning("RevoledSurface not yet supported");
					break;
				case GeoObjType.torus:
					Debug.LogWarning("Torus not yet supported");
					break;
				case GeoObjType.flatface:
					Vector3 positionOnPlane3 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<flatfaceBehave>().normalDir) + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnPlane3 - fingerTip(hand));
					break;
				case GeoObjType.straightedge:
					Vector3 positionOnStraightedge = Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<straightEdgeBehave>().normalDir) + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnStraightedge - fingerTip(hand));
					break;
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


		//private void playErrorSound()
		//{
		//	myAudioSource.clip = (errorSound);
		//	if (myAudioSource.isPlaying)
		//		myAudioSource.Stop();
		//	myAudioSource.Play();
		//}

	}
}
