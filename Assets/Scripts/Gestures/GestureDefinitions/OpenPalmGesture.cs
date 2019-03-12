
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
            switch (whichHand)
            {
                case Leap.Unity.Chirality.Left:
                    //Button ID 8 is Left controller trackpad being pressed
                    return Input.GetButtonDown("8") && Input.GetAxis("2") < 0;
                case Leap.Unity.Chirality.Right:
                    //Button id 9 is right controller trackpad being pressed
                    return Input.GetButtonDown("9") && Input.GetAxis("5") < 0;
                default:
                    return false;
            }
        }
		protected override bool DeactivationConditionsHand(Leap.Hand hand)
		{
			return !ActivationConditionsHand(hand);
		}
		protected override bool DeactivationConditionsOSVR(InputDevice inputDevice)
		{
            switch (whichHand)
            {
                case Leap.Unity.Chirality.Left:
                    //Button ID 8 is Left controller trackpad being pressed
                    return Input.GetButtonUp("8") || Input.GetAxis("2") > 0;
                case Leap.Unity.Chirality.Right:
                    //Button id 9 is right controller trackpad being pressed
                    return Input.GetButtonUp("9") || Input.GetAxis("5") > 0;
                default:
                    return false;
            }
        }
	}
}
