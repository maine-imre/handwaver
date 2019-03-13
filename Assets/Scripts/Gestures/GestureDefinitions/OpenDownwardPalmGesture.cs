
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.Gestures
{
	/// <summary>
	/// Gesture definition for an open palm parallel to the floor, facing down
	/// Overridden by OSVR touchpad press
	/// </summary>
	public abstract class OpenDownwardPalmGesture : OneHandedGesture
	{

		public float angleTolerance = 20f;


		protected override void visualFeedbackActivated()
		{
			throw new NotImplementedException();
		}
		protected override void tactileFeedbackActivated()
		{
			throw new NotImplementedException();
		}
		//we need to require an audioPlayer component.
		protected override void audioFeedbackActivated()
		{
			throw new NotImplementedException();
		}

		protected override void visualFeedbackDeactivated(DeactivationReason reason)
		{
			throw new NotImplementedException();
		}
		protected override void tactileFeedbackDeactivated(DeactivationReason reason)
		{
			throw new NotImplementedException();
		}
		//we need to require an audioPlayer component.
		protected override void audioFeedbackDeactivated(DeactivationReason reason)
		{
			throw new NotImplementedException();
		}

		protected override bool ActivationConditions(BodyInput bodyInput, Chirality chirality)
		{
			return ((hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && Vector3.Angle(hand.PalmNormal.ToVector3(), Vector3.down) < angleTolerance);
		}
		protected override bool DeactivationConditions(BodyInput bodyInput, Chirality chirality)
		{
			return !ActivationConditionsHand(bodyInput, chirality);
		}
	}
}
