using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR

namespace IMRE.Gestures
{
public class TwoHandedGesture : Leap.Unity.TwoHandedGesture
{
  //Everything from LeapMotion's One Handed Gesture Passes Through.
  // We add support for:
      //OSVR Overrides
      //Audio, Tactile and Visual Feedback
      //Pun RPC Calls for gesture start/stop
      //Pun RPC Calls for feedback
      
  //OSVR
  //setting this input device allows us to use the inputs described by Unity as overrides:  https://docs.unity3d.com/Manual/xr_input.html
  interal InputDevice leftOSVRDevice{
  get{
     return  InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
  }
  
    interal InputDevice rightOSVRDevice{
  get{
     return  InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
  }
  
    //Leap Motion Hands Module Connection
  
  internal Leap.Unity.HandModel leftHandModel{
      return //needs to be implemented.
  }
  
    internal Leap.Unity.HandModel rightHandModel{
      return //needs to be implemented.
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
    internal void WhenGestureActivated(Hand leftHand, Hand rightHand) 
    {
      visualFeedbackActivated();
      tactileFeedbackActivated();
      audioFeedbackActivated();
    }


    internal void WhenGestureDeactivated(Hand maybeNullLeftHand, Hand MaybeNullRightHand, DeactivationReason reason) 
    {
      visualFeedbackDeactivated(reason);
      tactileFeedbackDeactivated(reason);
      audioFeedbackDeactivated(reason);
    }
    
    internal bool ShouldGestureActivate(Hand leftHand, Hand rightHand)
    {
        return ActivationConditionsHand(leftHand,rightHand) && ActivationConditionsOSVR(leftOSVRDevice,rightOSVRDevice);
    }
    
    internal bool ShouldGestureDeactivateShouldGestureDeactivate(Hand leftHand, Hand rightHand, out DeactivationReason? deactivationReason);
    {
        if (DeactivationConditionsActionComplete()){
          deactivationReason = DeactivationReason.Complete;
          return true;
        }
        else if (DeactivationConditionsHand(leftHand, rightHand) || DeactivationConditionsOSVR(leftOSVRController, rightOSVRController)
        {
          deactivationReason = DeactivationReason.Canceled;
          return true;
        }else
        {
        return true;
        }
  
    }
    
    void WhenGestureActivated(Hand leftHand, Hand rightHand)
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
