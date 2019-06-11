using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    /// A generic tracking data from a finger.
    /// </summary>
    [System.Serializable]
    public struct Finger
    {
        /// <summary>
        /// A list of joints for each figner. (four joints per finger, indexed 0 through 3)
        /// </summary>
        public BodyComponent[] Joints;

        /// <summary>
        /// the direction a given finger is pointing in.
        /// </summary>
        public float3 Direction;

        /// <summary>
        /// Wheter or not a finger is extended (pointing).
        /// </summary>
        public bool IsExtended;
    }
}