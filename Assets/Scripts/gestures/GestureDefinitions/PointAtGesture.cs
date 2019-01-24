using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
public abstract class PointAtGesture : OneHandedGesture {
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
            /* Fingers are indexed from thumb to pinky. The thumb is 0, index is 1
             * middle 2, ring 3, pinky 4. Below, we return true if the pinky,
             * ring finger, and middle finger are not extended while the index
             * finger is extended. There is also an additional portion to this 
             * where the hand should not be pointing if there is a pinch happening.
             * This prevents false pointing while trying to pinch something
             * 
             * The thumb is ignored in whether this gesture will activate or not
             */
			return (
                 (hand.Fingers[1].IsExtended) &&
                !(hand.Fingers[2].IsExtended) &&
                !(hand.Fingers[3].IsExtended) &&
                !(hand.Fingers[4].IsExtended) &&
                !hand.IsPinching()
			    );
        }
		protected override bool ActivationConditionsOSVR(InputDevice inputDevice)
		{
			throw new NotImplementedException();
		}
		protected override bool DeactivationConditionsHand(Leap.Hand hand)
		{
			return !this.ActivationConditionsHand();
		}
		protected override bool DeactivationConditionsOSVR(InputDevice inputDevice)
		{
			throw new NotImplementedException();
		}
	}
}
