using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

namespace IMRE.Gestures
{
	public abstract class DoublePinchNoGraspGesture : TwoHandedGesture
	{
        public float pinchTol = .8f;

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

		protected override bool ActivationConditions(BodyInput bodyInput)		
		{
            //if both the left hand and right hand are detected to be pinching,
            //return true.
            
            //we need to move grasp to this gesture system to have an idea of no grasp. 
            return (bodyInput.LeftHand.PinchStrength > pinchTol && bodyInput.RightHand.PinchStrength > pinchTol);
		}
		protected override  bool DeactivationConditions(BodyInput bodyInput)
		{
			return !ActivationConditions(bodyInput);
		}
	}
}

