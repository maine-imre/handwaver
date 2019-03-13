using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures
{
	//notes that we might need to pick a gesture other than openPalm to do this.
	public class CallToolbox : OpenPalmGesture
	{
		protected override bool DeactivationConditionsActionComplete()
		{
			throw new NotImplementedException();
		}

		protected override void WhileGestureActive(BodyInput bodyInput, Chirality chirality)
		{
			throw new NotImplementedException();
		}
	}
}
