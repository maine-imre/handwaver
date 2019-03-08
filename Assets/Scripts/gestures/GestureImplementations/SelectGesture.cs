using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IMRE.HandWaver.Space;
using Leap;
using UnityEngine;
using UnityEngine.XR;\
	using IMRE.HandWaver.Space;
using Leap.Unity;

namespace IMRE.Gestures
{
	public class SelectGesture : PointAtGesture
	{
		public float tol = .02f;
		private bool isComplete = false;
		protected override bool DeactivationConditionsActionComplete()
		{
			return isComplete;
		}

		protected override void WhileGestureActive(Leap.Hand hand, InputDevice osvrController)
		{
			if ((hand.PalmPosition.ToVector3() - RSDESManager.earthPos).magnitude - RSDESManager.EarthRadius < tol)
			{
				RSDESPin pin = PrefabManager.Spawn("RSDESpushPinPreFab").GetComponent<RSDESPin>();
				pin.Latlong = GeoPlanetMaths.latlong(hand.PalmPosition.ToVector3(), RSDESManager.earthPos);
				isComplete = true;
			}
			//Add method for OSVR
			else
			{
				isComplete = false;
			}
		}
	}
}
