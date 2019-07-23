using UnityEngine;

namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class GravationalObject : MonoBehaviour
    {
        public Vector3d ForceVec; //Force Vector

        [HideInInspector] public Vector3d LastVector;

        public float mass;
        public Vector3 position; //UI position vector
        public Vector3d positionVector; //position vector

        public float scale = 1;
        public Vector3 VelocityVector; //UI velocity vector
        public Vector3d VelVec; //Velocity Vector

        // Use this for initialization
        private void Start()
        {
            VelVec.x = VelocityVector.x;
            VelVec.y = VelocityVector.y;
            VelVec.z = VelocityVector.z;

            positionVector.x = position.x;
            positionVector.y = position.y;
            positionVector.z = position.z;
        }

        // Update is called once per frame
        private void Update()
        {
            gameObject.transform.position = new Vector3((float) positionVector.x * scale,
                (float) positionVector.y * scale, (float) positionVector.z * scale);
        }

        public void step1(float dt)
        {
            ForceVec = VelVec * (mass / dt);
            LastVector = positionVector;
            Debug.Log(ForceVec);
        }

        public void step2(GravationalObject gravitationObject, float dt)
        {
            ForceVec = ForceVec + gravityVector(gravitationObject);
        }

        public void step3(float dt)
        {
            VelVec = VelVec + ForceVec / mass * (float) System.Math.Pow(dt, 2);
        }

        private Vector3d gravityVector(GravationalObject otherObject)
        {
            var distance = Vector3d.Distance(positionVector, otherObject.LastVector);
            var F = (float) (6.67408 * System.Math.Pow(10, -11)) *
                    (float) (mass * otherObject.mass / System.Math.Pow(distance, 2)); //meters, kg, seconds
            var vectorn = otherObject.LastVector - positionVector;
            vectorn.Normalize();
            vectorn = vectorn * F;
            return vectorn;
        }
    }
}