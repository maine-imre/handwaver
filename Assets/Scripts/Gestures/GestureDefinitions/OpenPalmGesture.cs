
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.Gestures
{
    public abstract class OpenPalmGesture : OneHandedGesture {

		protected override void visualFeedbackActivated()
		{

		}
		protected override void tactileFeedbackActivated()
		{

		}
		//we need to require an audioPlayer component.
		protected override void audioFeedbackActivated()
		{

		}

		protected override void visualFeedbackDeactivated(DeactivationReason reason)
		{

		}
		protected override void tactileFeedbackDeactivated(DeactivationReason reason)
		{

		}
		//we need to require an audioPlayer component.
		protected override void audioFeedbackDeactivated(DeactivationReason reason)
		{

		}

		protected override bool ActivationConditions(BodyInput bodyInput, Chirality chirality)
		{
            Hand hand = getHand(bodyInput,chirality);
			return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5);
		}

		protected override bool DeactivationConditions(BodyInput bodyInput, Chirality chirality)
		{
			return !ActivationConditions(bodyInput,chirality);
		}

	}
}
