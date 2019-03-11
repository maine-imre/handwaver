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
/// <summary>
/// Point to select gesture in the sandbox
/// </summary>
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
			(debugkeyboardinput.PointToSelectEnabled &&
			(fingerExtentionState.pointerFingerExtended == hand.Fingers[1].IsExtended) &&
			!(fingerExtentionState.middleFingerExtended == hand.Fingers[2].IsExtended) &&
			!(fingerExtentionState.ringFingerExtended == hand.Fingers[3].IsExtended) &&
			!(fingerExtentionState.pinkyFingerExtended == hand.Fingers[4].IsExtended) &&
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
			if (!isHandTracked || !debugkeyboardinput.PointToSelectEnabled ||
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
				handColourManager.setHandColorMode(whichHand, handColourManager.handModes.none);
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
				float distance = mgo.LocalDistanceToClosestPoint(hand.Fingers[1].TipPosition.ToVector3());
				float angle = mgo.PointingAngleDiff(hand.Fingers[1].TipPosition.ToVector3(), hand.Fingers[1].Direction.ToVector3());

				if (Mathf.Abs(distance) < shortestDist)
				{
					if (distance < shortestDist && angle < angleTolerance)
					{
						closestObj = mgo;
						shortestDist = distance;
					}
				}
				else
				{
					//check to see if any higher priority objectes lie within epsilon
					bool v = (Mathf.Abs(distance) - shortestDist <= maximumRangeToSelect) && (
					  ((closestObj.figType == GeoObjType.line || closestObj.figType == GeoObjType.polygon) && mgo.figType == GeoObjType.point)
					  || (closestObj.figType == GeoObjType.polygon && mgo.figType == GeoObjType.point)
					  );
					if (v)
					{
						closestObj = mgo;
						shortestDist = distance;
					}
				}
			}


			handColourManager.setHandColorMode(whichHand, handColourManager.handModes.select);

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

		private void playSuccessSound()
		{
			myAudioSource.clip = (successSound);
			if (myAudioSource.isPlaying)
				myAudioSource.Stop();
			myAudioSource.Play();
		}

	}
}
