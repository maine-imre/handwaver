using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Gestures;
using Leap;
using System;
using System.Linq;
using Leap.Unity.Interaction;
using Leap.Unity;

namespace IMRE.HandWaver
{
	/// <summary>
	/// snapping gesture for straightedges (lines) orthagonal to the floor.
	/// </summary>
	public class AxisControllerSnap : MonoBehaviour
	{
		private bool active;
		public bool isLeft;

		private InteractionXRController _iXR;

		private InteractionXRController iXR
		{
			get
			{
				if (_iXR == null)
					_iXR = FindObjectsOfType<InteractionXRController>()
						.FirstOrDefault(c => c.isLeft == (isLeft));
				return _iXR;
			}
		}

		private straightEdgeBehave MyStraightEdge => GetComponent<straightEdgeBehave>();

		protected bool ShouldGestureActivate()
		{
			float graspDepressedValue = 0.8f;

			if (isLeft)
			{
				return Input.GetAxis("LeftXRGripAxis") > graspDepressedValue;
			}
			else
			{
				return Input.GetAxis("RightXRGripAxis") > graspDepressedValue;
			}
		}

		protected bool ShouldGestureDeactivate()
		{

			float graspDepressedValue = 0.5f;

			if (isLeft)
			{
				return Input.GetAxis("LeftXRGripAxis") < graspDepressedValue;
			}
			else
			{
				return Input.GetAxis("RightXRGripAxis") < graspDepressedValue;
			}
		}

		private void Update()
		{
			if (active)
			{
				active = !ShouldGestureDeactivate();
			}
			else
			{
				active = ShouldGestureActivate();
			}

			if (active && iXR.isGraspingObject &&
			    iXR.graspedObject.gameObject.GetComponent<straightEdgeBehave>() == MyStraightEdge)
			{
				MyStraightEdge.snapToFloor();
			}
		}

	}
}