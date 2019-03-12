using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Gestures;
using Leap;
using System;
using System.Linq;
using Leap.Unity.Interaction;
using Leap.Unity;

namespace IMRE.HandWaver
{
	/// <summary>
	/// snapping gesture for straightedges (lines) orthagonal to the floor.
	/// </summary>
	public class AxisSnapGesture : OneHandedGesture
	{
		/// <summary>
		/// Toggles the debugging out of closest object when it toggles selection state.
		/// </summary>
		public bool debugSelect = false;
		public float angleTolerance = 15f;

		internal straightEdgeBehave myStraightEdge
		{
			get
			{
				return this.transform.GetComponent<straightEdgeBehave>();
			}
		}

		protected override bool ShouldGestureActivate(Hand hand)
		{
			return ((hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && Vector3.Angle(hand.PalmNormal.ToVector3(),Vector3.down) < angleTolerance);
		}

		protected override bool ShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason)
		{
			deactivationReason = DeactivationReason.CancelledGesture;

			if (!((hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && Vector3.Angle(hand.PalmNormal.ToVector3(), Vector3.down) < angleTolerance))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void WhileGestureActive(Hand hand)
		{
			base.WhileGestureActive(hand);

			Chirality chirality = Chirality.Right;
			//choose opposite hand for iHand;
			InteractionHand iHand = leapHandDataLogger.ins.currHands.rIhand;
			if (hand.IsLeft)
			{
				iHand = leapHandDataLogger.ins.currHands.rIhand;
				chirality = Chirality.Left;
			}

			handColourManager.setHandColorMode(chirality,handColourManager.handModes.snappingPalm);

			if (iHand.isGraspingObject && iHand.graspedObject.gameObject.GetComponent<straightEdgeBehave>()  == myStraightEdge)
			{
				Debug.Log("Trying to snap to floor");
				//iHand.ReleaseObject(myStraightEdge.GetComponent<InteractionBehaviour>());

				myStraightEdge.snapToFloor();
			}

		}
	}
}