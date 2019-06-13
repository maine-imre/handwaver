using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    /// The generic tracking data for a Hand.
    /// </summary>
    [System.Serializable]
    public struct Hand
    {
        /// <summary>
        /// This controls how pinched a hand must be to qualify for the isPinching boolean
        /// </summary>
        private const float pinchTolerance = 0.8f;
        
        private bool isLeft;
        
        /// <summary>
        /// The handedness of the hand.
        /// </summary>
        public Chirality WhichHand{
            get{
                if(isLeft) 
                    return Chirality.Left;
                return Chirality.Right;
            }
            set{
                isLeft = (value == Chirality.Right);
            }
        }

        /// <summary>
        /// An array of fingers.
        /// [0] = Thumb.
        /// [1] = Index Finger.
        /// [2] = Middle Finger.
        /// [3] = Ring Finger
        /// [4] = Pinky
        /// </summary>
        public fixed Finger Fingers[5];

        /// <summary>
        /// The palm.  Direction is normal to palm.
        /// </summary>
        public BodyComponent Palm;

        /// <summary>
        /// Wrist.  Direction is from wrist to first joint of finger
        /// </summary>
        public BodyComponent Wrist;

        /// <summary>
        /// How close the thumb and pointer finger are as a measure form 0 to 1 where 1 is fully "pinched"
        /// </summary>
        public float PinchStrength;

        /// <summary>
        /// Is the pinch strength sufficient to be pinching?
        /// </summary>
        public bool IsPinching => (PinchStrength >= pinchTolerance);
    }
}
