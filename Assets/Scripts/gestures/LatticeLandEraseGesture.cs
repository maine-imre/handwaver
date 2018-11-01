using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity.Gestures;
using UnityEngine;
using System.Linq;
using Leap.Unity;
using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver.Interface
{
	public class LatticeLandEraseGesture : Leap.Unity.Gestures.OneHandedGesture
	{
		private float maximumRangeToSelect = .05f;
		private MasterGeoObj closestObj;

		protected override bool ShouldGestureActivate(Hand hand)
		{
			return ((hand.Fingers.Where(finger => finger.IsExtended).Count() == 5));
		}

		protected override bool ShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason)
		{
			deactivationReason = DeactivationReason.CancelledGesture;

			if (!((hand.Fingers.Where(finger => finger.IsExtended).Count() == 5)))
			{
				deactivationReason = DeactivationReason.CancelledGesture;

				return true;
			}
			else if (foundMGO(hand) || cancelLatticeLandPoint(hand))
			{
				deactivationReason = DeactivationReason.FinishedGesture;

				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void WhileGestureActive(Hand hand)
		{
			base.WhileGestureActive(hand);
			//change hand color

			Chirality chirality = Chirality.Right;
			if (hand.IsLeft)
			{
				chirality = Chirality.Left;
			}
			handColourManager.setHandColorMode(chirality, handColourManager.handModes.snappingPalm);

		}

		protected override void WhenGestureDeactivated(Hand maybeNullHand, DeactivationReason reason)
		{
			base.WhenGestureDeactivated(maybeNullHand, reason);

			Debug.Log(reason);

			Chirality chirality = Chirality.Right;
			if (maybeNullHand.IsLeft)
			{
				chirality = Chirality.Left;
			}
			handColourManager.setHandColorMode(chirality, handColourManager.handModes.none);
			//erase.
			switch (reason)
			{
				case DeactivationReason.FinishedGesture:
					if (cancelLatticeLandPoint(maybeNullHand))
					{
						foreach (LatticeLandPoint lp in GameObject.FindObjectsOfType<LatticeLandPoint>())
						{
							float distance = (Vector3.Project(maybeNullHand.PalmPosition.ToVector3() - (lp.GetComponent<LineRenderer>().GetPosition(0) - lp.GetComponent<LineRenderer>().GetPosition(1)) / 2f, lp.GetComponent<LineRenderer>().GetPosition(0) - lp.GetComponent<LineRenderer>().GetPosition(1)) + lp.transform.position - maybeNullHand.PalmPosition.ToVector3()).magnitude;
							if(distance < maximumRangeToSelect)
							{
								Debug.Log("Try to cancel gesture " + lp.name);
								lp.endInteraction();
							}
						}
					}
					else if (foundMGO(maybeNullHand))
					{
						Debug.Log("Try to delete " + closestObj.name);
						closestObj.deleteGeoObj();
					}
					break;
				case DeactivationReason.CancelledGesture:
					break;
				default:
					break;
			}
		}

		private bool cancelLatticeLandPoint(Hand hand)
		{
			bool result = false;
			foreach (LatticeLandPoint lp in GameObject.FindObjectsOfType<LatticeLandPoint>())
			{
				float distance = (Vector3.Project(hand.PalmPosition.ToVector3() - (lp.GetComponent<LineRenderer>().GetPosition(0) - lp.GetComponent<LineRenderer>().GetPosition(1))/2f, lp.GetComponent<LineRenderer>().GetPosition(0) - lp.GetComponent<LineRenderer>().GetPosition(1)) + lp.transform.position - hand.PalmPosition.ToVector3()).magnitude;
				result = result || distance < maximumRangeToSelect;
			}
			return result;
		}

		public bool foundMGO(Hand hand)
		{
			float shortestDist = Mathf.Infinity;
			closestObj = null;

			foreach (MasterGeoObj mgo in FindObjectsOfType<MasterGeoObj>().Where(g => (g.GetComponent<AnchorableBehaviour>() == null || (g.GetComponent<AnchorableBehaviour>() != null && !g.GetComponent<AnchorableBehaviour>().isAttached))).Where(g => g.GetComponent<MasterGeoObj>().figType != GeoObjType.point))
			{
				float distance = mgo.LocalDistanceToClosestPoint(hand.PalmPosition.ToVector3());

				if (Mathf.Abs(distance) < shortestDist)
				{
					if (distance < shortestDist)
					{
						closestObj = mgo;
						shortestDist = distance;
					}
				}
			}
			return shortestDist < maximumRangeToSelect;
		}
	}
}
