using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        private float speedTol;

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

        protected override bool ActivationConditions(BodyInput bodyInput, Chirality chirality)
        {
	        Hand hand = getHand(bodyInput, chirality);

            //want movement in plane of palm within tolerance.
            Vector3 move = hand.Palm.Velocity;
            Vector3 plane = hand.Palm.Direction;

            //we want velocity to be nonzero.
            float speed = move.magnitude;
            //we want to have close to zero angle between movement and palm.
            float angle = 90-Mathf.Abs(Vector3.Angle(move, plane));

            float planeAngle = Mathf.Abs(Vector3.Angle(plane, planeNormal));
            float distToPlane = Vector3.Project(hand.Palm.Position - pointOnPlane, planeNormal).magnitude;

            return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && speed > speedTol && 
                   angle < angleTolerance && planeAngle < angleTolerance && distToPlane < distanceTolerance;
        }
        protected override bool DeactivationConditions(BodyInput bodyInput, Chirality chirality)
        {
            return !ActivationConditions(bodyInput, chirality);
        }


    }
}
