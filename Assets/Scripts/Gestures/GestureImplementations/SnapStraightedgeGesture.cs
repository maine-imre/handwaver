using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver;
using UnityEngine;

namespace IMRE.Gestures
{
	/// <summary>
	/// 
	/// </summary>
	[RequireComponent(typeof(straightEdgeBehave))]
	public class SnapStraightedgeGesture : OpenDownwardPalmGesture
	{
		//we could consider adding functionality that only enables this when a straightedge is being held.


		protected override bool DeactivationConditionsActionComplete()
		{
			//we want this to be a continuous action, so the action never finishes.
			return false;
		}

		protected override void WhileGestureActive(BodyInput bodyInput, Chirality chirality)
		{
			InteractionHand iHand;
			switch (chirality)
			{
				case Chirality.Left:
					iHand = leapHandDataLogger.ins.currHands.lIhand;
					break;
				case Chirality.Right:
					iHand = leapHandDataLogger.ins.currHands.rIhand;
					break;
				default:
					Debug.LogError("Hand Chirality Undefined");
					return;
			}

			if (iHand.isGraspingObject && iHand.graspedObject.gameObject.GetComponent<straightEdgeBehave>() != null
				&& iHand.graspedObject.gameObject.GetComponent<straightEdgeBehave>() == this.GetComponent<straightEdgeBehave>())
			{
				this.GetComponent<straightEdgeBehave>().snapToFloor();
			}
		}
	}
}
