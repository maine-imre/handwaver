using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

namespace IMRE.Gestures
{
	public abstract class DoublePinchNoGraspGesture : TwoHandedGesture
	{
        public float pinchTol = .8f;

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
			return (leftHand.PinchStrength > pinchTol && rightHand.PinchStrength > pinchTol 
                && !(leftInteractionHand.isGraspingObject || rightInteractionHand.isGraspingObject));
		}
		protected override bool ActivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController)
		{
            //button id 14 is the Left trigger button for HTC controller 15 is the right
            //when the trigger buttons are pressed
            return Input.GetButtonDown("14") && Input.GetButtonDown("15") 
                && !(leftInteractionController.isGraspingObject || rightInteractionController.isGraspingObject);
		}
		protected override  bool DeactivationConditionsHand(Leap.Hand leftHand, Leap.Hand rightHand)
		{
			return !ActivationConditionsHand(leftHand, rightHand);
		}
		protected override bool DeactivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController)
		{
            //button id 14 is the Left trigger button for HTC controller 15 is the right
			return Input.GetButtonUp("14") && Input.GetButtonUp("15") 
                && !(leftInteractionController.isGraspingObject || rightInteractionController.isGraspingObject);
        }
	}
}

