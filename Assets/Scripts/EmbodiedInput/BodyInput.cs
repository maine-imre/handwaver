using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Entities;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    /// Generic tracking data for a body and hands
    /// </summary>
    [System.Serializable]
    public struct BodyInput : IComponentData
    {      
        
        #region Tracking Modes

        /// <summary>
        /// Is the body being tracked?
        /// </summary>
        public bool FullBodyTracking;

        /// <summary>
        /// Are hands being tracked (that is the fingers of each hand)?
        /// </summary>
        public bool HandTracking;

        /// <summary>
        /// Is hand tracking being simulated?
        /// </summary>
        public bool SimulatedHandTracking;

        /// <summary>
        /// Is the head being tracked (OSVR etc)
        /// </summary>
        public bool HeadTracking;

        /// <summary>
        /// Is the head tracking simulated (Stationary Viewer)
        /// </summary>
        public bool SimulatedHeadTracking;

        #endregion

        #region Tracking Data

        /// <summary>
        /// The head, usually tracked by the HMD
        /// </summary>
        public BodyComponent Head;

        /// <summary>
        /// The neck (center of shoulders)
        /// </summary>
        public BodyComponent Neck;

        /// <summary>
        /// The center of the chest
        /// </summary>
        public BodyComponent Chest;

        /// <summary>
        /// Center of hips at waist.
        /// </summary>
        public BodyComponent Waist;

        /// <summary>
        /// The right leg.
        /// [0] Hip
        /// [1] Knee
        /// [2] Foot
        /// </summary>
        public fixed BodyComponent RightLeg[3];

        /// <summary>
        /// The left leg.
        /// [0] Hip
        /// [1] Knee
        /// [2] Foot
        /// </summary>
        public fixed BodyComponent LeftLeg[3];

        /// <summary>
        /// The right arm.
        /// [0] Shoulder
        /// [1] Elbow
        /// [2] Wrist
        /// </summary>
        public fixed BodyComponent RightArm[3];

        /// <summary>
        /// The right arm.
        /// [0] Shoulder
        /// [1] Elbow
        /// [2] Wrist
        /// </summary>
        public fixed BodyComponent LeftArm[3];

        /// <summary>
        /// The left hand.
        /// </summary>
        public Hand LeftHand;

        /// <summary>
        /// The right hand.
        /// </summary>
        public Hand RightHand;

        #endregion

        /// <summary>
        /// Generates an initialized value BodyInput.
        /// This does not affect any settings booleans contained within the BI.
        /// These must be set after use.
        /// </summary>
        /// <returns>A initialized BodyInput with all boolean settings false.</returns>
        public static BodyInput newInput()=>new BodyInput
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

        /// <summary>
        /// Sets up a new hand
        /// </summary>
        /// <returns>initialized hand</returns>
        private static Hand newHand() =>
            new Hand
            {
                WhichHand = Chirality.Left,
                Fingers = new []{ newFinger(), newFinger(), newFinger(), newFinger(), newFinger()},
                Palm = newBodyComponent(),
                Wrist = newBodyComponent(),
                PinchStrength = 0f
                
            };

        /// <summary>
        /// Sets up a new finger
        /// </summary>
        /// <returns>initialized finger</returns>
        private static Finger newFinger() =>
            new Finger
            {
                Joints = new []{ newBodyComponent(),newBodyComponent(),newBodyComponent(),newBodyComponent()},
                Direction = Vector3.zero
            };


        /// <summary>
        /// Generates an initialized body component
        /// </summary>
        /// <returns>initialized body component</returns>
        private static BodyComponent newBodyComponent() =>
            new BodyComponent
            {
                Direction = Vector3.zero, 
                Position = Vector3.zero,
                Velocity = Vector3.zero
            };
        
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
}
