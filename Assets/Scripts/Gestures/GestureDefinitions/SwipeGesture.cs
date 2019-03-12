using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Leap.Unity;

namespace IMRE.Gestures
{
    public abstract class SwipeGesture : OneHandedGesture
    {
        public float speedTol = .5f;
        public float angleTol = 30f;

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
            //consider using running average of hand movement
            Vector3 move = hand.PalmVelocity.ToVector3();
            Vector3 plane = hand.PalmNormal.ToVector3();

            //we want velocity to be nonzero.
            float speed = move.magnitude;
            //we want to have close to zero angle between movement and palm.
            float angle = 90 - Mathf.Abs(Vector3.Angle(move, plane));

            return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && speed > speedTol && angle < angleTol;
        }
        protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
        {
            Vector3 move = interactionController.velocity;
            Vector3 plane = interactionController.rotation * Vector3.down;

            //we want velocity to be nonzero.
            float speed = move.magnitude;
            //we want to have close to zero angle between movement
            float angle = 90- Mathf.Abs(Vector3.Angle(move, plane));

            //open palm plus motion
            switch (whichHand)
            {
                case Leap.Unity.Chirality.Left:
                    //Button ID 8 is Left controller trackpad being pressed
                    return Input.GetButtonDown("8") && Input.GetAxis("2") < 0 && speed > speedTol && angle < angleTol; ;
                case Leap.Unity.Chirality.Right:
                    //Button id 9 is right controller trackpad being pressed
                    return Input.GetButtonDown("9") && Input.GetAxis("5") < 0 && speed > speedTol && angle < angleTol; ;
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
            float angle = 90- Mathf.Abs(Vector3.Angle(move, plane));

            switch (whichHand)
            {
                case Leap.Unity.Chirality.Left:
                    //Button ID 8 is Left controller trackpad being pressed
                    return Input.GetButtonUp("8") || Input.GetAxis("2") > 0 || speed < speedTol || angle > angleTol;
                case Leap.Unity.Chirality.Right:
                    //Button id 9 is right controller trackpad being pressed
                    return Input.GetButtonUp("9") || Input.GetAxis("5") > 0 || speed < speedTol || angle > angleTol;
                default:
                    return false;
            }
        }
    }
}
