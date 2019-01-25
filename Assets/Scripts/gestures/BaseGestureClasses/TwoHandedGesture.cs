using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	public abstract class TwoHandedGesture : Leap.Unity.Gestures.TwoHandedGesture
	{
		public bool GestureEnabled = true;


		//Everything from LeapMotion's One Handed Gesture Passes Through.
		// We add support for:
		//OSVR Overrides
		//Audio, Tactile and Visual Feedback
		//Pun RPC Calls for gesture start/stop
		//Pun RPC Calls for feedback

		//OSVR
		//setting this input device allows us to use the inputs described by Unity as overrides:  https://docs.unity3d.com/Manual/xr_input.html

		internal InputDevice leftOSVRDevice
		{
			get
			{
				return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
			}
		}

		internal InputDevice rightOSVRDevice
		{
			get
			{
				return InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
			}
		}

		//Leap Motion Hands Module Connection

		internal Leap.Unity.HandModel leftHandModel
		{
			get
			{
				return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.Handedness == Leap.Unity.Chirality.Left).First();
			}
		}

		internal Leap.Unity.HandModel rightHandModel
		{
			get
			{
				return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.Handedness == Leap.Unity.Chirality.Right).First();
			}
		}

        internal Leap.Unity.Interaction.InteractionHand leftInteractionHand
        {
            get
            {
                return GameObject.FindObjectsOfType<Leap.Unity.Interaction.InteractionHand>().Where(h => h.isLeft).First();
            }
        }

        internal Leap.Unity.Interaction.InteractionHand rightInteractionHand
        {
            get
            {
                return GameObject.FindObjectsOfType<Leap.Unity.Interaction.InteractionHand>().Where(h => !h.isLeft).First();
            }
        }

        internal Leap.Unity.Interaction.InteractionController rightInteractionController
        {
            get
            {
                return GameObject.FindObjectsOfType<Leap.Unity.Interaction.InteractionController>().Where(h => !h.isLeft).First();
            }
        }

        internal Leap.Unity.Interaction.InteractionController leftInteractionController
        {
            get
            {
                return GameObject.FindObjectsOfType<Leap.Unity.Interaction.InteractionController>().Where(h => !h.isLeft).First();
            }
        }


        //Feedback System - these functions are implemented at the Gesture Definition Level
        protected abstract void visualFeedbackActivated();
		protected abstract void tactileFeedbackActivated();
		//we need to require an audioPlayer component.
		protected abstract void audioFeedbackActivated();

		protected abstract void visualFeedbackDeactivated(DeactivationReason reason);
		protected abstract void tactileFeedbackDeactivated(DeactivationReason reason);
		//we need to require an audioPlayer component.
		protected abstract void audioFeedbackDeactivated(DeactivationReason reason);

		//PUN
		//leave this for later.


		/// <summary>
		/// Called when the gesture has just been activated. The hand is guaranteed to
		/// be non-null.
		/// </summary>
		protected override void WhenGestureActivated(Leap.Hand leftHand, Leap.Hand rightHand)
		{
			visualFeedbackActivated();
			tactileFeedbackActivated();
			audioFeedbackActivated();
		}


		protected override void WhenGestureDeactivated(Leap.Hand maybeNullLeftHand, Leap.Hand
		 MaybeNullRightHand, DeactivationReason reason)
		{
			visualFeedbackDeactivated(reason);
			tactileFeedbackDeactivated(reason);
			audioFeedbackDeactivated(reason);
		}

		protected override bool ShouldGestureActivate(Leap.Hand leftHand, Leap.Hand rightHand)
		{
			return GestureEnabled && (ActivationConditionsHand(leftHand, rightHand) || ActivationConditionsOSVR(leftOSVRDevice, rightOSVRDevice));
		}

		protected override bool ShouldGestureDeactivate(Leap.Hand leftHand,
		 Leap.Hand rightHand, out DeactivationReason? deactivationReason)
		{
			if (DeactivationConditionsActionComplete())
			{
				deactivationReason = DeactivationReason.FinishedGesture;
				return true;
			}
			else if (!GestureEnabled || (DeactivationConditionsHand(leftHand, rightHand) &&
			 DeactivationConditionsOSVR(leftOSVRDevice, rightOSVRDevice)))
			{
				deactivationReason = DeactivationReason.CancelledGesture;
				return true;
			}
			else
			{
				deactivationReason = DeactivationReason.CancelledGesture;

				return true;
			}

		}

		protected override void WhileGestureActive(Leap.Hand leftHand, Leap.Hand rightHand)
		{
			WhileGestureActive(leftHand, rightHand, leftOSVRDevice, rightOSVRDevice);
		}

		//Note that one could assign deactivation conditions to be !activationConditions.
		//These functions are implemented at the Gesture Defintion Level
		protected abstract bool ActivationConditionsHand(Leap.Hand leftHand, Leap.Hand rightHand);
		protected abstract bool ActivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController);
		protected abstract bool DeactivationConditionsHand(Leap.Hand leftHand, Leap.Hand rightHand);
		protected abstract bool DeactivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController);
		protected abstract bool DeactivationConditionsActionComplete();

		//this does the action of the gesture.
		//These functions are implemented for each use case.
		protected abstract bool WhileGestureActive(Leap.Hand leftHand, Leap.Hand rightHand, InputDevice leftOSVRController, InputDevice rightOSVRController);
	}
}
