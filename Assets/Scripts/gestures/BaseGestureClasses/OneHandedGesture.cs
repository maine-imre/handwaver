using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

namespace IMRE.Gestures
{
	/// <summary>
	/// Base class for leap motion and OSVR gesture controls
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public abstract class OneHandedGesture : Leap.Unity.Gestures.OneHandedGesture
	{
		public bool GestureEnabled = true;

		//Everything from LeapMotion's One Handed Gesture Passes Through.
		// We add support for:
		//OSVR Overrides
		//Audio, Tactile and Visual Feedback
		//Pun RPC Calls for gesture start/stop
		//Pun RPC Calls for feedback

		//OSVR
		//setting this input device allows us to use the inputs described by Unity as overrides
		internal InputDevice osvrDevice
		{
			get
			{
				switch (whichHand)
				{
					case Leap.Unity.Chirality.Left:
						return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
					default: //right
						return InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
				}

			}
		}

		//Leap Motion Hands Module Connection

		internal Leap.Unity.HandModel handModel
		{
			get
			{
				switch (whichHand)
				{
					case Leap.Unity.Chirality.Left:
						return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.Handedness == Leap.Unity.Chirality.Left).First();
					default: //right
						return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.Handedness == Leap.Unity.Chirality.Right).First();
				}
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
		protected override void WhenGestureActivated(Leap.Hand hand)
		{
			visualFeedbackActivated();
			tactileFeedbackActivated();
			audioFeedbackActivated();
		}

		/// <summary>
		/// Called when the gesture has just been deactivated. The hand might be null; this
		/// will be true if the hand loses tracking while the gesture is active.
		/// </summary>
		protected override void WhenGestureDeactivated(Leap.Hand maybeNullHand, DeactivationReason reason)
		{
			visualFeedbackDeactivated(reason);
			tactileFeedbackDeactivated(reason);
			audioFeedbackDeactivated(reason);
		}

		protected override bool ShouldGestureActivate(Leap.Hand hand)
		{
			return GestureEnabled && (ActivationConditionsHand(hand) || ActivationConditionsOSVR(osvrDevice));
		}

		protected override void WhileGestureActive(Leap.Hand hand)
		{
			WhileGestureActive(hand, osvrDevice);
		}

		protected override bool ShouldGestureDeactivate(Leap.Hand hand, out DeactivationReason? deactivationReason)
		{
			if (DeactivationConditionsActionComplete())
			{
				deactivationReason = DeactivationReason.FinishedGesture;
				return true;
			}
			else if ((DeactivationConditionsHand(hand) && DeactivationConditionsOSVR(osvrDevice)) || !GestureEnabled)
			{
				deactivationReason = DeactivationReason.CancelledGesture;
				return true;
			}
			else
			{
				deactivationReason = DeactivationReason.CancelledGesture;
				return false;
			}
		}

		//Note that one could assign deactivation conditions to be !activationConditions.
		//These functions are implemented at the Gesture Defintion Level
		protected abstract bool ActivationConditionsHand(Leap.Hand hand);
		protected abstract bool ActivationConditionsOSVR(InputDevice osvrController);
		protected abstract bool DeactivationConditionsHand(Leap.Hand hand);
		protected abstract bool DeactivationConditionsOSVR(InputDevice osvrController);

		//These functions are implemented for each use case.
		protected abstract bool DeactivationConditionsActionComplete();

		protected abstract void WhileGestureActive(Leap.Hand hand, InputDevice osvrController);


	}
}

