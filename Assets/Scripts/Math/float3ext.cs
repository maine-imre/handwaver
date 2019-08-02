using Unity.Mathematics;
using UnityEngine;

namespace IMRE.Math
{
    /// <summary>
    /// Extensions of Float 3
    /// </summary>
    public static class float3ext
    {
        /// <summary>
        /// Converts a float3 to Vector3
        /// </summary>
        /// <returns>Vector3 object of the float3 given.</returns>
        public static Vector3 ToVector3( this float3 n) => new UnityEngine.Vector3(n.x, n.y, n.z);
    }

}