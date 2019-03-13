using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

namespace IMRE.Gestures 
{
	/// <summary>
	/// A gesture for
	/// </summary>
	public class pointingGesture : OneHandedGesture
	{
		protected override bool ShouldGestureActivate(Hand hand)
		{
			throw new System.NotImplementedException();
		}

		protected override bool ShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason)
		{
			throw new System.NotImplementedException();
		}
	}
}
