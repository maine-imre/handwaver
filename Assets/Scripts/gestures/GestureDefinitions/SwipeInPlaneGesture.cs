using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
public abstract class SwipeInPlaneGesture : OneHandedGesture {

		/// <summary>
		/// A point on the plane of detection.
		/// </summary>
		public Vector3 pointOnPlane = Vector3.zero;

		/// <summary>
		/// The normal direction for the plane of detection.
		/// </summary>
		public Vector3 planeNormal = Vector3.up;

		/// <summary>
		/// The angle away from normal that is tolerated.
		/// </summary>
		public float angleTolerance = .15f;

		/// <summary>
		/// The distance out of plane that is tolerated.
		/// </summary>
		public float distanceTolerance = .15f;

		protected override void visualFeedbackActivated()
		{
			throw new NotImplementedException();

		}
		protected override void tactileFeedbackActivated()
		{
			throw new NotImplementedException();

		}
		//we need to require an audioPlayer component.
		protected override void audioFeedbackActivated()
		{
			throw new NotImplementedException();

		}

		protected override void visualFeedbackDeactivated(DeactivationReason reason)
		{
			throw new NotImplementedException();

		}
		protected override void tactileFeedbackDeactivated(DeactivationReason reason)
		{
			throw new NotImplementedException();

		}
		//we need to require an audioPlayer component.
		protected override void audioFeedbackDeactivated(DeactivationReason reason)
		{
			throw new NotImplementedException();

		}

		protected override bool ActivationConditionsHand(Leap.Hand hand)
		{
			throw new NotImplementedException();
		}
		protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
		{
			throw new NotImplementedException();
		}
		protected override bool DeactivationConditionsHand(Leap.Hand hand)
		{
			throw new NotImplementedException();
		}
		protected override bool DeactivationConditionsOSVR(InputDevice inputDevice)
		{
			throw new NotImplementedException();
		}
	}
}
