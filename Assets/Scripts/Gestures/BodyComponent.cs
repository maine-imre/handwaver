using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures
{
    /// <summary>
    /// The generic tracking data for a generic body component. 
    /// </summary>
    [System.Serializable]
    public struct BodyComponent
    {
        /// <summary>
        /// The position of a given joint
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The direction of a joint.
        /// Unless otherwise noted, this is a assumeed to be the radial direction
        /// of the previous connecting bone
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// The average velocity of the component over the last 10 frames.
        /// This is done on seperate calculation, and it is not needed to be updated outside of <see cref="GestureInput"/>
        /// </summary>
        public Vector3 Velocity;
    }
}