using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	public class DebuggingGesture : PointAtGesture
	{
		protected override bool DeactivationConditionsActionComplete()
		{
			return false;
		}

		protected override void WhileGestureActive(Leap.Hand hand, InputDevice osvrController)
		{
			Debug.Log("GESTURE ACTIVE");
		}
	}
}
