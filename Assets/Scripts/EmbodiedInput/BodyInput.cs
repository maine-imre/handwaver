using System;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    ///     Generic tracking data for a body and hands
    /// </summary>
    [Serializable]
    public struct BodyInput
    {
        #region Tracking Modes

        /// <summary>
        ///     Is the body being tracked?
        /// </summary>
        public bool FullBodyTracking;

        /// <summary>
        ///     Are hands being tracked (that is the fingers of each hand)?
        /// </summary>
        public bool HandTracking;

        /// <summary>
        ///     Is hand tracking being simulated?
        /// </summary>
        public bool SimulatedHandTracking;

        /// <summary>
        ///     Is the head being tracked (OSVR etc)
        /// </summary>
        public bool HeadTracking;

        /// <summary>
        ///     Is the head tracking simulated (Stationary Viewer)
        /// </summary>
        public bool SimulatedHeadTracking;

        #endregion

        #region Tracking Data

        /// <summary>
        ///     The head, usually tracked by the HMD
        /// </summary>
        public BodyComponent Head;

        /// <summary>
        ///     The neck (center of shoulders)
        /// </summary>
        public BodyComponent Neck;

        /// <summary>
        ///     The center of the chest
        /// </summary>
        public BodyComponent Chest;

        /// <summary>
        ///     Center of hips at waist.
        /// </summary>
        public BodyComponent Waist;

        /// <summary>
        ///     The right leg.
        ///     [0] Hip
        ///     [1] Knee
        ///     [2] Foot
        /// </summary>
        public BodyComponent[] RightLeg;

        /// <summary>
        ///     The left leg.
        ///     [0] Hip
        ///     [1] Knee
        ///     [2] Foot
        /// </summary>
        public BodyComponent[] LeftLeg;

        /// <summary>
        ///     The right arm.
        ///     [0] Shoulder
        ///     [1] Elbow
        ///     [2] Wrist
        /// </summary>
        public BodyComponent[] RightArm;

        /// <summary>
        ///     The right arm.
        ///     [0] Shoulder
        ///     [1] Elbow
        ///     [2] Wrist
        /// </summary>
        public BodyComponent[] LeftArm;

        /// <summary>
        ///     The left hand.
        /// </summary>
        public Hand LeftHand;

        /// <summary>
        ///     The right hand.
        /// </summary>
        public Hand RightHand;

        #endregion

        /// <summary>
        ///     Generates an initialized value BodyInput.
        ///     This does not affect any settings booleans contained within the BI.
        ///     These must be set after use.
        /// </summary>
        /// <returns>A initialized BodyInput with all boolean settings false.</returns>
        public static BodyInput newInput()
        {
            return new BodyInput
            {
                Head = newBodyComponent(),
                Neck = newBodyComponent(),
                Chest = newBodyComponent(),
                Waist = newBodyComponent(),
                RightLeg = new[] {newBodyComponent(), newBodyComponent(), newBodyComponent()},
                LeftLeg = new[] {newBodyComponent(), newBodyComponent(), newBodyComponent()},
                RightArm = new[] {newBodyComponent(), newBodyComponent(), newBodyComponent()},
                LeftArm = new[] {newBodyComponent(), newBodyComponent(), newBodyComponent()},
                LeftHand = newHand(),
                RightHand = newHand()
            };
        }

        /// <summary>
        ///     Sets up a new hand
        /// </summary>
        /// <returns>initialized hand</returns>
        private static Hand newHand()
        {
            return new Hand
            {
                WhichHand = Chirality.Left,
                Fingers = new[] {newFinger(), newFinger(), newFinger(), newFinger(), newFinger()},
                Palm = newBodyComponent(),
                Wrist = newBodyComponent(),
                PinchStrength = 0f
            };
        }

        /// <summary>
        ///     Sets up a new finger
        /// </summary>
        /// <returns>initialized finger</returns>
        private static Finger newFinger()
        {
            return new Finger
            {
                Joints = new[] {newBodyComponent(), newBodyComponent(), newBodyComponent(), newBodyComponent()},
                Direction = Vector3.zero
            };
        }

        /// <summary>
        ///     Generates an initialized body component
        /// </summary>
        /// <returns>initialized body component</returns>
        private static BodyComponent newBodyComponent()
        {
            return new BodyComponent
            {
                Direction = Vector3.zero,
                Position = Vector3.zero,
                Velocity = Vector3.zero
            };
        }

        public Hand GetHand(Chirality chirality)
        {
            switch (chirality)
            {
                case Chirality.Left:
                    return LeftHand;
                case Chirality.Right:
                    return RightHand;
                default:
                    return new Hand();
            }
        }
    }

    /// <summary>
    ///     The generic tracking data for a generic body component.
    /// </summary>
    [Serializable]
    public struct BodyComponent
    {
        private float3x3 data;

        /// <summary>
        ///     The position of a given joint
        /// </summary>
        public float3 Position
        {
            get => data.c0;
            set => data.c0 = value;
        }

        /// <summary>
        ///     The direction of a joint.
        ///     Unless otherwise noted, this is a assumeed to be the radial direction
        ///     of the previous connecting bone
        /// </summary>
        public float3 Direction
        {
            get => data.c1;
            set => data.c1 = value;
        }

        /// <summary>
        ///     The average velocity of the component over the last 10 frames.
        ///     This is done on seperate calculation, and it is not needed to be updated outside of
        ///     <see cref="BodyInputDataSystem" />
        /// </summary>
        public float3 Velocity
        {
            get => data.c2;
            set => data.c2 = value;
        }
    }

    [Serializable]
    public enum Chirality
    {
        Left,
        Right,
        None
    }

    /// <summary>
    ///     A generic tracking data from a finger.
    /// </summary>
    [Serializable]
    public struct Finger
    {
        /// <summary>
        ///     A list of joints for each figner. (four joints per finger, indexed 0 through 3)
        /// </summary>
        public BodyComponent[] Joints;

        /// <summary>
        ///     the direction a given finger is pointing in.
        /// </summary>
        public float3 Direction;

        /// <summary>
        ///     Wheter or not a finger is extended (pointing).
        /// </summary>
        public bool IsExtended;
    }

    /// <summary>
    ///     The generic tracking data for a Hand.
    /// </summary>
    [Serializable]
    public struct Hand
    {
        /// <summary>
        ///     This controls how pinched a hand must be to qualify for the isPinching boolean
        /// </summary>
        private const float pinchTolerance = 0.8f;

        private bool isLeft;

        /// <summary>
        ///     The handedness of the hand.
        /// </summary>
        public Chirality WhichHand
        {
            get
            {
                if (isLeft)
                    return Chirality.Left;
                return Chirality.Right;
            }
            set => isLeft = value == Chirality.Right;
        }

        /// <summary>
        ///     An array of fingers.
        ///     [0] = Thumb.
        ///     [1] = Index Finger.
        ///     [2] = Middle Finger.
        ///     [3] = Ring Finger
        ///     [4] = Pinky
        /// </summary>
        public Finger[] Fingers;

        /// <summary>
        ///     The palm.  Direction is normal to palm.
        /// </summary>
        public BodyComponent Palm;

        /// <summary>
        ///     Wrist.  Direction is from wrist to first joint of finger
        /// </summary>
        public BodyComponent Wrist;

        /// <summary>
        ///     How close the thumb and pointer finger are as a measure form 0 to 1 where 1 is fully "pinched"
        /// </summary>
        public float PinchStrength;

        /// <summary>
        ///     Is the pinch strength sufficient to be pinching?
        /// </summary>
        public bool IsPinching => PinchStrength >= pinchTolerance;
    }
}