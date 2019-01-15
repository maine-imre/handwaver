using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR

namespace IMRE.Gestures
{
public class SwipeGesture : OneHandedGesture {

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

    public  bool ActivationConditionsHand(Hand hand);
    {
      return false;
    }
    public  bool ActivationConditionsOSVR(InputDevice osvrController);
    {
      return false;
    }
    public  bool DeactivationConditionsHand(Hand hand);
    {
      return false;
    }
    public  bool DeactivationConditionsOSVR(InputDevice osvrController);
    {
      return false;
    }
}
}
