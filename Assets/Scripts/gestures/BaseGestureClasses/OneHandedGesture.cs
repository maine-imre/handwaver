using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

namespace IMRE.Gestures {
    public abstract class OneHandedGesture : Leap.Unity.Gestures.OneHandedGesture
    {
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
    switch(whichHand){
      case Leap.Unity.Chirality.Left:
      return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.whichHand = Chirality.Left).First();
      default: //right
      return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.whichHand = Chirality.Right).First();
    } 
    }
  }

  //Feedback System - these functions are implemented at the Gesture Definition Level

  abstract void visualFeedbackActivated();
  abstract void tactileFeedbackActivated();
  //we need to require an audioPlayer component.
  abstract void audioFeedbackActivated();

    abstract void visualFeedbackDeactivated(DeactivationReason reason);
  abstract void tactileFeedbackDeactivated(DeactivationReason reason);
  //we need to require an audioPlayer component.
  abstract void audioFeedbackDeactivated(DeactivationReason reason);

  //PUN
  //leave this for later.


    /// <summary>
    /// Called when the gesture has just been activated. The hand is guaranteed to
    /// be non-null.
    /// </summary>
    void WhenGestureActivated(Hand hand)
    {
      visualFeedbackActivated();
      tactileFeedbackActivated();
      audioFeedbackActivated();
    }

    /// <summary>
    /// Called when the gesture has just been deactivated. The hand might be null; this
    /// will be true if the hand loses tracking while the gesture is active.
    /// </summary>
    void WhenGestureDeactivated(Hand maybeNullHand, DeactivationReason reason)
    {
      visualFeedbackDeactivated(reason);
      tactileFeedbackDeactivated(reason);
      audioFeedbackDeactivated(reason);
    }

    internal bool ShouldGestureActivate(Hand hand)
    {
        return ActivationConditionsHand(hand) || ActivationConditionsOSVR(osvrDevice);
    }

    void WhileGestureActive(Hand hand)
    {
      WhileGestureActive(hand, osvrDevice);
    }

    internal bool ShouldGestureDeactivateShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason);
    {
        if (DeactivationConditionsActionComplete()){
          deactivationReason = DeactivationReason.Complete;
          return true;
        }
        else if (DeactivationConditionsHand(hand) && DeactivationConditionsOSVR(osvrDevice)
        {
          deactivationReason = DeactivationReason.Canceled;
          return true;
        }else
        {
        return true;
        }

    }

     //Note that one could assign deactivation conditions to be !activationConditions.
     //These functions are implemented at the Gesture Defintion Level
    public abstract bool ActivationConditionsHand(Hand hand);
    public abstract bool ActivationConditionsOSVR(InputDevice osvrController);
    public abstract bool DeactivationConditionsHand(Hand hand);
    public abstract bool DeactivationConditionsOSVR(InputDevice osvrController);

    //These functions are implemented for each use case.
    public abstract bool DeactivationConditionsActionComplete();

    public abstract bool WhileGestureActive(Hand hand,InputDevice osvrController);

}
}
