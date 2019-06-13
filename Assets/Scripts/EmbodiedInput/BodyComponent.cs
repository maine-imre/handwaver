using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    /// The generic tracking data for a generic body component. 
    /// </summary>
    [System.Serializable]
    public struct BodyComponent
    {
        private float3x3 data;
        /// <summary>
        /// The position of a given joint
        /// </summary>
        public float3 Position{
            get {return data.c0;}
            set {data.c0 = value;}
        }

        /// <summary>
        /// The direction of a joint.
        /// Unless otherwise noted, this is a assumeed to be the radial direction
        /// of the previous connecting bone
        /// </summary>
        public float3 Direction{
            get {return data.c1;}
            set {data.c1 = value;}
        }

        /// <summary>
        /// The average velocity of the component over the last 10 frames.
        /// This is done on seperate calculation, and it is not needed to be updated outside of <see cref="BodyInputDataSystem"/>
        /// </summary>
        public float3 Velocity{
            get {return data.c2;}
            set {data.c2 = value;}
        }
    }
}
