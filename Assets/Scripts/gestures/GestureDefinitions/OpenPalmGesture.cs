
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

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

		protected override bool ActivationConditionsHand(Leap.Hand hand)
		{
			return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5);
		}
		protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
		{
			return false;
		}
		protected override bool DeactivationConditionsHand(Leap.Hand hand)
		{
			return false;
		}
		protected override bool DeactivationConditionsOSVR(InputDevice inputDevice)
		{
			return false;
		}
	}
}
