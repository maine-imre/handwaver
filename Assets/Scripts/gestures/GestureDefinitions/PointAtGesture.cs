using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
public abstract class PointAtGesture : OneHandedGesture {
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

		protected override bool ActivationConditionsHand(Leap.Hand hand)
		{
			throw new NotImplementedException();
		}
		protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
		{
			throw new NotImplementedException();
		}
		protected override bool DeactivationConditionsHand(Leap.Hand hand)
		{
			throw new NotImplementedException();
		}
		protected override bool DeactivationConditionsOSVR(InputDevice inputDevice)
		{
			throw new NotImplementedException();
		}
	}
}
