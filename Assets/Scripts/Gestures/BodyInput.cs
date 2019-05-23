using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures
{
    /// <summary>
    /// Generic tracking data for a body and hands
    /// </summary>
    [System.Serializable]
    public struct BodyInput
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
        public BodyComponent[] RightLeg;

        /// <summary>
        /// The left leg.
        /// [0] Hip
        /// [1] Knee
        /// [2] Foot
        /// </summary>
        public BodyComponent[] LeftLeg;

        /// <summary>
        /// The right arm.
        /// [0] Shoulder
        /// [1] Elbow
        /// [2] Wrist
        /// </summary>
        public BodyComponent[] RightArm;

        /// <summary>
        /// The right arm.
        /// [0] Shoulder
        /// [1] Elbow
        /// [2] Wrist
        /// </summary>
        public BodyComponent[] LeftArm;

        /// <summary>
        /// The left hand.
        /// </summary>
        public Hand LeftHand;

        /// <summary>
        /// The right hand.
        /// </summary>
        public Hand RightHand;

        #endregion

    }
}