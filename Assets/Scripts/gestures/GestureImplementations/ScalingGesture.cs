using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
  public class ScalingGesture : DoublePinchNoGraspGesture {
    public bool DeactivationConditionsActionComplete(){
      return false;
    }

        //this does the action of the gesture.
         //These functions are implemented for each use case.
    public bool WhileGestureActive(Hand leftHand, Hand rightHand,InputDevice leftOSVRController, InputDevice rightOSVRController){[
      return false;
    }
  }
}
