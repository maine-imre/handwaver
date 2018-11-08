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
			if (maybeNullHand != null)
			{
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
							foreach (LatticeLandPoint lp in GameObject.FindObjectsOfType<LatticeLandPoint>().Where(l => l.GetComponent<LineRenderer>() != null).ToList())
							{
								//steal this from AbstractLineSegment
								Vector3 a = lp.GetComponent<LineRenderer>().GetPosition(0);
								Vector3 b = lp.GetComponent<LineRenderer>().GetPosition(1);
								Vector3 c = maybeNullHand.PalmPosition.ToVector3();
								Vector3 result = Vector3.Dot(c - a, (b - a).normalized) * (b - a).normalized + a;
								if ((result - a).magnitude + (result - b).magnitude > (a - b).magnitude)
								{
									//the point is outside the endpoints, find closest endpoint instead.
									if ((result - a).magnitude < (result - b).magnitude)
									{
										result = a;
									}
									else
									{
										result = b;
									}
								}

								float distance = (maybeNullHand.PalmPosition.ToVector3() - result).magnitude;
								if (distance < maximumRangeToSelect && lp.whichHand != this.whichHand)
								{
									lp.endInteraction();
								}
							}
						}
						else if (foundMGO(maybeNullHand))
						{
							closestObj.DeleteGeoObj();
						}
						break;
					case DeactivationReason.CancelledGesture:
						break;
					default:
						break;
				}
			}
		}

		private bool cancelLatticeLandPoint(Hand hand)
		{
			bool result = false;
			foreach (LatticeLandPoint lp in GameObject.FindObjectsOfType<LatticeLandPoint>())
			{
				//steal this from AbstractLineSegment
				Vector3 a = lp.GetComponent<LineRenderer>().GetPosition(0);
				Vector3 b = lp.GetComponent<LineRenderer>().GetPosition(1);
				Vector3 c = hand.PalmPosition.ToVector3();
				Vector3 r = Vector3.Dot(c - a, (b - a).normalized) * (b - a).normalized + a;
				if ((r - a).magnitude + (r - b).magnitude > (a - b).magnitude)
				{
					//the point is outside the endpoints, find closest endpoint instead.
					if ((r - a).magnitude < (r - b).magnitude)
					{
						r = a;
					}
					else
					{
						r = b;
					}
				}

				float distance = (hand.PalmPosition.ToVector3() - r).magnitude;

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
