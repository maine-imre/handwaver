using System;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity.Gestures;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
	//notes that we might need to pick a gesture other than openPalm to do this.
	public class CallToolbox : OpenPalmGesture
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
