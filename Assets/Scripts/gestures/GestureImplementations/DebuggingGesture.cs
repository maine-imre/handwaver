using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;
using UnityEngine.XR;
using Hand = Leap.Hand;

namespace IMRE.Gestures
{
		//For debugging scripts the DebuggingGesture inherits from the PointAtGesture which allows this to test the implementation of the point at gesture

	public class DebuggingGesture : DoublePinchNoGraspGesture
	{
		protected override bool DeactivationConditionsActionComplete()
		{
			return false;
		}

		protected override void WhileGestureActive(Hand leftHand, Hand rightHand, InputDevice leftOSVRController, InputDevice rightOSVRController)
		{
			Debug.Log("Gesture Active");
		}
	}
}