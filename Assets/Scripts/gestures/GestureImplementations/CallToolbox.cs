using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
  //notes that we might need to pick a gesture other than openPalm to do this.
  public class PushEarthGesture : OpenPalmGesture {
    public bool DeactivationConditionsActionComplete(){
      return false;
    }

    public bool WhileGestureActive(Hand hand,InputDevice osvrController){
      return false;
    }
  }
}
