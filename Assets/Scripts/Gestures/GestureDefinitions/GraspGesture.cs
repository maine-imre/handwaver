using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures
{
public abstract class GraspGesture : OneHandedGesture {
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
             Hand hand = getHand(bodyInput, chirality);
             return (hand.PinchStrength > .8f);
		}
		
		protected override bool DeactivationConditions(BodyInput bodyInput, Chirality chirality)
		{
			return !ActivationConditions(bodyInput,chirality);
		}
	}
}
