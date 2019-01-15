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
        //Everything from LeapMotion's One Handed Gesture Passes Through.
        // We add support for:
        //OSVR Overrides
        //Audio, Tactile and Visual Feedback
        //Pun RPC Calls for gesture start/stop
        //Pun RPC Calls for feedback

        //OSVR
        //setting this input device allows us to use the inputs described by Unity as overrides:  https://docs.unity3d.com/Manual/xr_input.html

  internal InputDevice leftOSVRDevice{
    get{
     return  InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
   }
  }

  internal InputDevice rightOSVRDevice{
  get{
     return  InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
  }
}

    //Leap Motion Hands Module Connection

  internal Leap.Unity.HandModel leftHandModel{
    get{
      return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.whichHand = Chirality.Left).First();
    }
  }

    internal Leap.Unity.HandModel rightHandModel{
      get{
      return GameObject.FindObjectsOfType<Leap.Unity.HandModel>().Where(h => h.whichHand = Chirality.Right).First();
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
    public void WhenGestureActivated(Hand leftHand, Hand rightHand)
    {
      visualFeedbackActivated();
      tactileFeedbackActivated();
      audioFeedbackActivated();
    }


    public void WhenGestureDeactivated(Hand maybeNullLeftHand, Hand
     MaybeNullRightHand, DeactivationReason reason)
    {
      visualFeedbackDeactivated(reason);
      tactileFeedbackDeactivated(reason);
      audioFeedbackDeactivated(reason);
    }

    public bool ShouldGestureActivate(Hand leftHand, Hand rightHand)
    {
        return ActivationConditionsHand(leftHand,rightHand) || ActivationConditionsOSVR(leftOSVRDevice,rightOSVRDevice);
    }

    public bool ShouldGestureDeactivateShouldGestureDeactivate(Hand leftHand,
     Hand rightHand, out DeactivationReason? deactivationReason);
    {
        if (DeactivationConditionsActionComplete()){
          deactivationReason = DeactivationReason.Complete;
          return true;
        }
        else if (DeactivationConditionsHand(leftHand, rightHand) &&
         DeactivationConditionsOSVR(leftOSVRController, rightOSVRController)
        {
          deactivationReason = DeactivationReason.Canceled;
          return true;
        }else
        {
        return true;
        }

    }

    void WhileGestureActive(Hand leftHand, Hand rightHand)
    {
      WhileGestureActive(leftHand, rightHand, leftOSVRController, rightOSVRController);
    }

    //Note that one could assign deactivation conditions to be !activationConditions.
     //These functions are implemented at the Gesture Defintion Level
    public abstract bool ActivationConditionsHand(Hand leftHand, Hand rightHand);
    public abstract bool ActivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController);
    public abstract bool DeactivationConditionsHand(Hand leftHand, Hand rightHand);
    public abstract bool DeactivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController);
    public abstract bool DeactivationConditionsActionComplete();

    //this does the action of the gesture.
     //These functions are implemented for each use case.
    public abstract bool WhileGestureActive(Hand leftHand, Hand rightHand,InputDevice leftOSVRController, InputDevice rightOSVRController);
}
}
