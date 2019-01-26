
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	/// <summary>
	/// Gesture definition for an open palm parallel to the floor, facing down
	/// Overridden by OSVR touchpad press
	/// </summary>
	public abstract class OpenDownwardPalmGesture : OneHandedGesture
	{

		public float angleTolerance = 15f;


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
			return ((hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && Vector3.Angle(hand.PalmNormal.ToVector3(), Vector3.down) < angleTolerance);
		}
		protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
		{
			switch (whichHand)
			{
				case Leap.Unity.Chirality.Left:
					//Button ID 8 is Left controller trackpad being pressed
                    //check downward facing?
					return Input.GetButtonDown("8") && Input.GetAxis("2") < 0 && Quaternion.Angle(interactionController.rotation,Quaternion.identity) < angleTolerance;
				case Leap.Unity.Chirality.Right:
					//Button id 9 is right controller trackpad being pressed
					return Input.GetButtonDown("9") && Input.GetAxis("5") < 0 && Quaternion.Angle(interactionController.rotation, Quaternion.identity) < angleTolerance;
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
					return Input.GetButtonUp("8") || Input.GetAxis("2") > 0 || Quaternion.Angle(interactionController.rotation, Quaternion.identity) > angleTolerance;
				case Leap.Unity.Chirality.Right:
					//Button id 9 is right controller trackpad being pressed
					return Input.GetButtonUp("9") || Input.GetAxis("5") > 0 || Quaternion.Angle(interactionController.rotation, Quaternion.identity) > angleTolerance;
				default:
					return false;
			}
		}
	}
}
