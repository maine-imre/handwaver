using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        protected override bool ActivationConditions(BodyInput bodyInput, Chirality chirality)
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
        protected override bool DeactivationConditions(BodyInput bodyInput, Chirality chirality)
        {
            return !ActivationConditionsHand(bodyInput, chirality);
        }

    }
}
