using System;
using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	public class LatticeLandTraceGesture : PointAtGesture
	{
		protected override bool DeactivationConditionsActionComplete()
		{
			throw new NotImplementedException();
		}

		protected override void WhileGestureActive(Leap.Hand hand, InputDevice osvrController)
		{
			throw new NotImplementedException();
		}
	}
}
