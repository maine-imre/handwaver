using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Leap.Unity;
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

        protected override bool ActivationConditionsHand(Leap.Hand hand)
        {
            //want movement in plane of palm within tolerance.
            Vector3 move = hand.PalmVelocity.ToVector3();
            Vector3 plane = hand.PalmNormal.ToVector3();

            //we want velocity to be nonzero.
            float speed = move.magnitude;
            //we want to have close to zero angle between movement and palm.
            float angle = Mathf.Abs(Vector3.Angle(move, plane));

            float planeAngle = Mathf.Abs(Vector3.Angle(plane, planeNormal));
            float distToPlane = Vector3.Project(hand.PalmPosition.ToVector3() - pointOnPlane, planeNormal).magnitude;

            return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && speed > speedTol && angle < angleTolerance && planeAngle < angleTolerance && distToPlane < distanceTolerance;
        }
        protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
        {
            Vector3 move = interactionController.velocity;
            Vector3 plane = interactionController.rotation * Vector3.down;

            //we want velocity to be nonzero.
            float speed = move.magnitude;
            //we want to have close to zero angle between movement
            float angle = Mathf.Abs(Vector3.Angle(move, plane));
            float planeAngle = Mathf.Abs(Vector3.Angle(plane, planeNormal));
            float distToPlane = Vector3.Project(interactionController.position - pointOnPlane, planeNormal).magnitude;


            //open palm plus motion
            switch (whichHand)
            {
                case Leap.Unity.Chirality.Left:
                    //Button ID 8 is Left controller trackpad being pressed
                    return Input.GetButtonDown("8") && Input.GetAxis("2") < 0 && speed > speedTol && angle < angleTolerance && planeAngle < angleTolerance && distToPlane < distanceTolerance;
                case Leap.Unity.Chirality.Right:
                    //Button id 9 is right controller trackpad being pressed
                    return Input.GetButtonDown("9") && Input.GetAxis("5") < 0 && speed > speedTol && angle < angleTolerance && planeAngle < angleTolerance && distToPlane < distanceTolerance;
                default:
                    return false;
            }
        }
        protected override bool DeactivationConditionsHand(Leap.Hand hand)
        {
            return !ActivationConditionsHand(hand);
        }
        protected override bool DeactivationConditionsOSVR(InputDevice inputDevice)
        {
            Vector3 move = interactionController.velocity;
            Vector3 plane = interactionController.rotation * Vector3.down;

            //we want velocity to be nonzero.
            float speed = move.magnitude;
            //we want to have close to zero angle between movement
            float angle = Mathf.Abs(Vector3.Angle(move, plane));

            switch (whichHand)
            {
                case Leap.Unity.Chirality.Left:
                    //Button ID 8 is Left controller trackpad being pressed
                    return Input.GetButtonUp("8") || Input.GetAxis("2") > 0 || speed < speedTol || angle > angleTolerance;
                case Leap.Unity.Chirality.Right:
                    //Button id 9 is right controller trackpad being pressed
                    return Input.GetButtonUp("9") || Input.GetAxis("5") > 0 || speed < speedTol || angle > angleTolerance;
                default:
                    return false;
            }
        }

    }
}
