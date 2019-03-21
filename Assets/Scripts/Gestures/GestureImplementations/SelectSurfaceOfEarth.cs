using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver;
using IMRE.HandWaver.Space;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace IMRE.Gestures
{
	public class SelectSurfaceOfEarth : PointAtGesture
	{
		public float tol = .005f;
		private bool isComplete;
		protected override bool DeactivationConditionsActionComplete()
		{
			if (isComplete)
			{
				isComplete = false;
				return true;
			}

			return false;
		}

		protected override void WhileGestureActive(BodyInput bodyInput, Chirality chirality)
		{
			Hand hand = getHand(bodyInput,chirality);
			Vector3 fingerTip = hand.Fingers[1].Joints[3].Position;
			if (Mathf.Abs((fingerTip - RSDESManager.earthPos).magnitude - RSDESManager.EarthRadius) < tol)
			{
				RSDESPin pin = PrefabManager.Spawn("RSDES_Pin").GetComponent<RSDESPin>();
				pin.Latlong = GeoPlanetMaths.latlong((fingerTip - RSDESManager.earthPos).normalized);
				isComplete = true;
			}
		}
	}
}
