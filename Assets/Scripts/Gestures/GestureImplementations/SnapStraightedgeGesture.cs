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
			//we should have a more efficient method here.
			

			//want to snap to floor if the other hand is holding a line (straightedge),
			// first we need to have a grasping method out of our gesture system.
			// consider having an "is eligible override" for this method, that turns on when it the straightedge is being
			// held but only for the opposite hand.
						
			
			//if (iHand.isGraspingObject && iHand.graspedObject.gameObject.GetComponent<straightEdgeBehave>() != null
			//	&& iHand.graspedObject.gameObject.GetComponent<straightEdgeBehave>() == this.GetComponent<straightEdgeBehave>())
			//{
			//	this.GetComponent<straightEdgeBehave>().snapToFloor();
			//}
		}
	}
}