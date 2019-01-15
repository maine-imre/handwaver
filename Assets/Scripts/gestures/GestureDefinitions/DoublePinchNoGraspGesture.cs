using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR

namespace IMRE.Gestures
{
public abstract class DoublePinchNoGraspGesture : OneHandedGesture {

    internal void visualFeedbackActivated(){
    }
    internal void tactileFeedbackActivated(){
    }
    //we need to require an audioPlayer component.
    internal void audioFeedbackActivated(){
    }

    internal void visualFeedbackDeactivated(DeactivationReason reason){
    }
    internal void tactileFeedbackDeactivated(DeactivationReason reason){
    }
      //we need to require an audioPlayer component.
    internal void audioFeedbackDeactivated(DeactivationReason reason){
    }

    public  bool ActivationConditionsHand(Hand leftHand, Hand rightHand);
    {
      return false;
    }
    public  bool ActivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController);
    {
      return false;
    }
    public  bool DeactivationConditionsHand(Hand leftHand, Hand rightHand);
    {
      return false;
    }
    public  bool DeactivationConditionsOSVR(InputDevice leftOSVRController, InputDevice rightOSVRController);
    {
      return false;
    }


}
}
