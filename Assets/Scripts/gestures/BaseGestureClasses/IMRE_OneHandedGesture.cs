using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR

namespace IMRE.Gestures
{
public class OneHandedGesture : Leap.Unity.OneHandedGesture
{
  //Everything from LeapMotion's One Handed Gesture Passes Through.
  // We add support for:
      //OSVR Overrides
      //Audio, Tactile and Visual Feedback
      //Pun RPC Calls for gesture start/stop
      //Pun RPC Calls for feedback
      
  //OSVR
  //setting this input device allows us to use the inputs described by Unity as overrides:  https://docs.unity3d.com/Manual/xr_input.html
  interal InputDevice osvrDevice{
  get{
    switch(Chirality)
      case left:
        return  InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
      default: //right
        return InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }
  }
  
  //Feedback System
  
  abstract void visualFeedbackActivated();
  abstract void tactileFeedbackActivated();
  //we need to require an audioPlayer component.
  abstract void audioFeedbackActivated();
  
    abstract void visualFeedbackDeactivated();
  abstract void tactileFeedbackDeactivated();
  //we need to require an audioPlayer component.
  abstract void audioFeedbackDeactivated();
  
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
      visualFeedbackDeactivated();
      tactileFeedbackDeactivated();
      audioFeedbackDeactivated();
    }
    
    internal bool ShouldGestureActivate(Hand hand)
    {
        return ActivationConditionsHand(hand) && ActivationConditionsOSVR(osvrDevice);
    }
    
    internal bool ShouldGestureDeactivateShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason);
    {
        if (DeactivationConditionsActionComplete()){
          deactivationReason = DeactivationReason.Complete;
          return true;
        }
        else if (DeactivationConditionsHand(hand) || DeactivationConditionsOSVR(osvrDevice)
        {
          deactivationReason = DeactivationReason.Canceled;
          return true;
        }else
        {
        return true;
        }
  
    }
    
    //Note that one could assign deactivation conditions to be !activationConditions.
    public abstract bool ActivationConditionsHand(Hand hand);
    public abstract bool ActivationConditionsOSVR(InputDevice osvrController);
    public abstract bool DeactivationConditionsHand(Hand hand);
    public abstract bool DeactivationConditionsOSVR(InputDevice osvrController);
    public abstract bool DeactivationConditionsActionComplete();
  
  
}
}
