using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures
{
    /// <summary>
    /// A generic tracking data from a finger.
    /// </summary>
    public struct Finger
    {
        /// <summary>
        /// A list of joints for each figner.
        /// </summary>
        public BodyComponent[] Joints;

        /// <summary>
        /// the direction a given finger is pointing in.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// Wheter or not a finger is extended (pointing).
        /// </summary>
        public bool IsExtended;
    }
}