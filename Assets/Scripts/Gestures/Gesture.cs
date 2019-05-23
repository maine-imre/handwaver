using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures {
  
    /// <summary>
    /// A thin layer of general abstraction for one-handed and two-handed gestures.
    /// Inspired by LeapPaint https://github.com/leapmotion/Paint
    /// </summary>
    public abstract class Gesture : MonoBehaviour {

        public abstract bool wasActivated { get; }

        public abstract bool isActive { get; }

        public abstract bool wasDeactivated { get; }

        public abstract bool wasFinished { get; }

        public abstract bool wasCancelled { get; }

        /// <summary>
        /// Optionally override this property to specify to systems that take gestures as
        /// input whether or not the gesture "could be activated" given its current state.
        /// </summary>
        public virtual bool isEligible => true;

        protected enum DeactivationReason {
            FinishedGesture,
            CancelledGesture,
        }

        internal Hand getHand(BodyInput bodyInput, Chirality chirality)
        {
            Hand hand = new Hand();
            switch (chirality)
            {
                case Chirality.Left:
                    hand = bodyInput.LeftHand;
                    break;
                case Chirality.Right:
                    hand = bodyInput.RightHand;
                    break;
            }

            return hand;
        }

    }

}