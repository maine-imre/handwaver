using System;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity.Gestures;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	public class ScalingGesture : DoublePinchNoGraspGesture
	{
		protected override bool DeactivationConditionsActionComplete()
		{
			throw new NotImplementedException();
		}

		protected override bool WhileGestureActive(Leap.Hand leftHand, Leap.Hand rightHand, InputDevice leftOSVRController, InputDevice rightOSVRController)
		{
			throw new NotImplementedException();
		}
	}
}
