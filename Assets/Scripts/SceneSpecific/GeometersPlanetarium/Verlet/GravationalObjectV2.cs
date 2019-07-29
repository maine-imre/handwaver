using UnityEngine;

namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class GravationalObjectV2 : MonoBehaviour
    {
        public float mass;

        [HideInInspector] public double scale = 1;

        public Vector3 VelocityVector; //Used for initial velocity, not calculation
        public Vector3d VVec;
        public double x;
        public double y;

        public double z;

        // Use this for initialization
        private void Start()
        {
            VVec.x = VelocityVector.x;
            VVec.y = VelocityVector.y;
            VVec.z = VelocityVector.z;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position = new Vector3((float) (x * scale), (float) (y * scale), (float) (z * scale));
        }
    }
}