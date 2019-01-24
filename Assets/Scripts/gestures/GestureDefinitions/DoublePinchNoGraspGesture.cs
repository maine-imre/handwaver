using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	public abstract class DoublePinchNoGraspGesture : TwoHandedGesture
	{

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

		protected override bool ActivationConditionsHand(Leap.Hand leftHand, Leap.Hand rightHand)
		{
            //if both the left hand and right hand are detected to be pinching,
            //return true.
			return leftHand.IsPinching() && rightHand.IsPinching();
		}
		protected override bool ActivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController)
		{
			throw new NotImplementedException();
		}
		protected override  bool DeactivationConditionsHand(Leap.Hand leftHand, Leap.Hand rightHand)
		{
			return !this.ActivationConditionsHand;
		}
		protected override bool DeactivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController)
		{
			throw new NotImplementedException();
		}
	}
}

