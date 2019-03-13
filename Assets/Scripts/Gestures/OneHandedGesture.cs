using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.Gestures
{
	/// <summary>
	/// Base class for leap motion and OSVR gesture controls
	/// Inspired by LeapPaint https://github.com/leapmotion/Paint
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public abstract class OneHandedGesture : Gesture
	{
		public bool GestureEnabled = true;
		public bool requireHandTracking;
		public bool requireHeadTracking;
		public bool requireBodyTracking;
		
		public Chirality chirality;
		
		/// <summary>
		/// Whether this gesture is currently active.
		/// </summary>
		public override bool isActive { get { return _isActive; } }

		/// <summary>
		/// Whether this gesture was activated this frame.
		/// </summary>
		public override bool wasActivated { get { return _wasActivated; } }

		/// <summary>
		/// Whether this gesture was deactivated this frame. For some gestures, it may be
		/// necessary to differentiate between a successfully completed gesture and a
		/// cancelled gesture -- for this distinction, refer to wasCancelled and wasFinished.
		/// </summary>
		public override bool wasDeactivated { get { return _wasDeactivated; } }

		/// <summary>
		/// Whether this gesture was deactivated via cancellation this frame.
		/// 
		/// 'Cancelled' implies the gesture was not finished successfully, or was
		/// deliberately halted so as to not enact the result of the gesture. For example,
		/// a currently active gesture is cancelled when a hand performing the gesture
		/// loses tracking. This distinction is important when a gesture is a trigger of some
		/// kind.
		/// </summary>
		public override bool wasCancelled { get { return _wasCancelled; } }

		/// <summary>
		/// Whether this gesture was deactivated by being successfully completed this frame.
		/// 
		/// 'Finished' implies the gesture was successful, as opposed to cancellation. This
		/// distinction is important when a gesture is a trigger for some behavior.
		/// </summary>
		public override bool wasFinished { get { return _wasFinished; } }

		//Everything from LeapMotion's One Handed Gesture Passes Through.
		// We add support for:
		//OSVR Overrides
		//Audio, Tactile and Visual Feedback
		//Pun RPC Calls for gesture start/stop
		//Pun RPC Calls for feedback


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
		protected void WhenGestureActivated(BodyInput bodyInput, Chirality Chirality)
		{
			visualFeedbackActivated();
			tactileFeedbackActivated();
			audioFeedbackActivated();
		}

		/// <summary>
		/// Called when the gesture has just been deactivated. The hand might be null; this
		/// will be true if the hand loses tracking while the gesture is active.
		/// </summary>
		protected void WhenGestureDeactivated(BodyInput bodyInput,Chirality chirality, DeactivationReason reason)
		{
			visualFeedbackDeactivated(reason);
			tactileFeedbackDeactivated(reason);
			audioFeedbackDeactivated(reason);
		}

		protected bool ShouldGestureActivate(BodyInput bodyInput, Chirality chirality)
		{
			return GestureEnabled && (ActivationConditions(bodyInput, chirality));
		}

		protected bool ShouldGestureDeactivate(BodyInput bodyInput,Chirality chirality,
			out DeactivationReason? deactivationReason)
		{
			if (DeactivationConditionsActionComplete())
			{
				deactivationReason = DeactivationReason.FinishedGesture;
				return true;
			}
			else if (DeactivationConditions(bodyInput,chirality) || !GestureEnabled)
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
		protected abstract bool ActivationConditions(BodyInput bodyInput, Chirality chirality);
		protected abstract bool DeactivationConditions(BodyInput bodyInput, Chirality chirality);

		//These functions are implemented for each use case.
		protected abstract bool DeactivationConditionsActionComplete();

		protected abstract void WhileGestureActive(BodyInput bodyInput, Chirality chirality);

 #region Base Implementation (Unity Callbacks)

    private bool _isActive = false;
    
    
    protected bool _wasActivated = false;
    protected bool _wasDeactivated = false;
    protected bool _wasCancelled = false;
    protected bool _wasFinished = false;
    
    public override bool isEligible {
      get { return ((!requireHandTracking || GestureInput.bodyInput.HandTracking) && 
                    (!requireBodyTracking || GestureInput.bodyInput.FullBodyTracking) && 
                    (!requireHeadTracking || GestureInput.bodyInput.HeadTracking) ); }
    }

    protected virtual void OnDisable() {
      if (_isActive) {
        WhenGestureDeactivated(GestureInput.bodyInput, DeactivationReason.CancelledGesture);
        _isActive = false;
      }
    }

    protected virtual void Update() {
      _wasActivated = false;
      _wasDeactivated = false;
      _wasCancelled = false;
      _wasFinished = false;

      // Determine whether or not the gesture should be active or inactive.
      bool shouldGestureBeActive;
      DeactivationReason? deactivationReason = null;
      if (!isEligible) {
        shouldGestureBeActive = false;
      }
      else {
        if (!_isActive) {
          if (ShouldGestureActivate(GestureInput.bodyInput)) {
            shouldGestureBeActive = true;
          }
          else {
            shouldGestureBeActive = false;
          }
        }
        else {
          if (ShouldGestureDeactivate(GestureInput.bodyInput, out deactivationReason)) {
            shouldGestureBeActive = false;
          }
          else {
            shouldGestureBeActive = true;
          }
        }
      }

      // If the deactivation reason is not set, we assume the gesture completed
      // successfully.
      bool wasGestureSuccessful = deactivationReason.GetValueOrDefault()
                                  == DeactivationReason.FinishedGesture;

      // Fire gesture state change events.
      if (shouldGestureBeActive != _isActive) {
        if (shouldGestureBeActive) {
          _wasActivated = true;
          _isActive = true;

          WhenGestureActivated(GestureInput.bodyInput, Chirality);
          OnGestureActivated();
          OnOneHandedGestureActivated(GestureInput.bodyInput);
        }
        else {
          _wasDeactivated = true;
          if (wasGestureSuccessful) {
            _wasFinished = true;
          }
          else {
            _wasCancelled = true;
          }
          _isActive = false;

          WhenGestureDeactivated(GestureInput.bodyInput, deactivationReason.GetValueOrDefault());
          OnGestureDeactivated();
          OnOneHandedGestureDeactivated(GestureInput.bodyInput);
        }
      }

      // Fire per-update events.
      if (_isActive) {
        WhileGestureActive(GestureInput.bodyInput);
      }
      else {
        WhileGestureInactive(GestureInput.bodyInput);
      }
    }

    #endregion
	}
}

