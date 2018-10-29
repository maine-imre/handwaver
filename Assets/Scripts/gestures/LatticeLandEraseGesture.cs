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
								lp.endInteraction();
							}
						}
					}
					else if (foundMGO(maybeNullHand))
					{
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
				float distance = distHandToMGO(hand, mgo);

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


		private float distHandToMGO(Hand hand, MasterGeoObj mgo)
		{
			float distance = 15;
			switch (mgo.figType)
			{
				case GeoObjType.point:
					distance = Vector3.Magnitude(fingerTip(hand) - mgo.transform.position);
					break;
				case GeoObjType.line:
					distance = Vector3.Magnitude(Vector3.Project(fingerTip(hand) - mgo.transform.position, mgo.GetComponent<AbstractLineSegment>().vertex0 - mgo.GetComponent<AbstractLineSegment>().vertex1) + mgo.transform.position - fingerTip(hand));
					break;
				case GeoObjType.polygon:
					Vector3 positionOnPlane = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractPolygon>().normDir) + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnPlane - fingerTip(hand));
					Debug.LogWarning("Polygon doesn't check boundariers");
					break;
				case GeoObjType.prism:
					distance = Vector3.Magnitude(mgo.transform.position - fingerTip(hand));
					break;
				case GeoObjType.pyramid:
					Debug.LogWarning("Pyramids not yet supported");
					break;
				case GeoObjType.circle:
					Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractCircle>().normalDir) + mgo.transform.position;
					Vector3 positionOnCircle = Vector3.Normalize(positionOnPlane2 - mgo.GetComponent<AbstractCircle>().centerPos) * mgo.GetComponent<AbstractCircle>().Radius + mgo.GetComponent<AbstractCircle>().centerPos;
					distance = Vector3.Magnitude(fingerTip(hand) - positionOnCircle);
					break;
				case GeoObjType.sphere:
					Vector3 lineDir = Vector3.Normalize(transform.position - mgo.transform.position);
					Vector3 positionOnSphere1 = mgo.GetComponent<AbstractSphere>().radius * lineDir + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnSphere1 - fingerTip(hand));
					break;
				case GeoObjType.revolvedsurface:
					Debug.LogWarning("RevoledSurface not yet supported");
					break;
				case GeoObjType.torus:
					Debug.LogWarning("Torus not yet supported");
					break;
				case GeoObjType.flatface:
					Vector3 positionOnPlane3 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<flatfaceBehave>().normalDir) + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnPlane3 - fingerTip(hand));
					break;
				case GeoObjType.straightedge:
					Vector3 positionOnStraightedge = Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<straightEdgeBehave>().normalDir) + mgo.transform.position;
					distance = Vector3.Magnitude(positionOnStraightedge - fingerTip(hand));
					break;
				default:
					Debug.LogWarning("Something went wrong in the selection.... :(");
					break;
			}

			return distance;
		}


		public Vector3 fingerTip(Hand hand)
		{
			return hand.Fingers[1].TipPosition.ToVector3();
		}

		public Vector3 fingerDirection(Hand hand)
		{
			return hand.Fingers[1].Direction.ToVector3();
		}
	}
}
