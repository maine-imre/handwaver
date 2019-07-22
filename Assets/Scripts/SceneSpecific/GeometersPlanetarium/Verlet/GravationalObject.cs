namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class GravationalObject : UnityEngine.MonoBehaviour
    {
        public UnityEngine.Vector3d ForceVec; //Force Vector

        [UnityEngine.HideInInspector] public UnityEngine.Vector3d LastVector;

        public float mass;
        public UnityEngine.Vector3 position; //UI position vector
        public UnityEngine.Vector3d positionVector; //position vector

        public float scale = 1;
        public UnityEngine.Vector3 VelocityVector; //UI velocity vector
        public UnityEngine.Vector3d VelVec; //Velocity Vector

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
            gameObject.transform.position = new UnityEngine.Vector3((float) positionVector.x * scale,
                (float) positionVector.y * scale, (float) positionVector.z * scale);
        }

        public void step1(float dt)
        {
            ForceVec = VelVec * (mass / dt);
            LastVector = positionVector;
            UnityEngine.Debug.Log(ForceVec);
        }

        public void step2(GravationalObject gravitationObject, float dt)
        {
            ForceVec = ForceVec + gravityVector(gravitationObject);
        }

        public void step3(float dt)
        {
            VelVec = VelVec + ((ForceVec / mass) * (float) System.Math.Pow(dt, 2));
        }

        private UnityEngine.Vector3d gravityVector(GravationalObject otherObject)
        {
            double distance = UnityEngine.Vector3d.Distance(positionVector, otherObject.LastVector);
            float F = (float) (6.67408 * System.Math.Pow(10, -11)) *
                      (float) ((mass * otherObject.mass) / System.Math.Pow(distance, 2)); //meters, kg, seconds
            UnityEngine.Vector3d vectorn = otherObject.LastVector - positionVector;
            vectorn.Normalize();
            vectorn = vectorn * F;
            return vectorn;
        }
    }
}