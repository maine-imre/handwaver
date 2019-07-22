namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class VerletObjectV1 : UnityEngine.MonoBehaviour
    {
        public UnityEngine.Vector3d
            acceleration; //The Acceleration of the body for verlet (Note to self: Does this need to be public?)

        public double distance;

        public UnityEngine.Vector3d
            initialVelocity; //The initial velocity of the body (note: doesn't change once the calculation starts)

        public UnityEngine.Vector3 inputPosition;
        public UnityEngine.Vector3 inputVelocity;
        public float mass; //The mass of the body

        private UnityEngine.GameObject[]
            massObject; //A list of the rest of the bodies (Note to self: Bring down to nLog(n) using control script)

        public UnityEngine.Vector3d position; //The position vector of the body
        public UnityEngine.Vector3d previousPosition; //The previous position of the body
        public float previousTimeStep; //The previous timestep for time corrected verlet
        public float radius; //The radius of the body
        public float scale = 1;
        public float timeStep; //The length of the timestep in seconds

        // Use this for initialization
        private void Start()
        {
            //ERROR IF TIMESTEP IS CHANGED NEED TO FIX
            position = V3toV3D(inputPosition); //Convert input floats to doubles
            initialVelocity = V3toV3D(inputVelocity); //Ditto above
            initialVelocity =
                initialVelocity * 0.00001157407407 *
                timeStep; //Unit conversion of Velocity from AU/Day to AU/Sec then multiplys by timestep
            previousPosition = position - initialVelocity; //Calculates previous position based on initial velocity
            previousTimeStep = timeStep; //Initial condition
        }

        private void LateUpdate()
        {
            //Late update because time and scale needs to be updated for every object before calculation
            CalculateNextPosition();
        }

        private void CalculateNextPosition()
        {
            massObject =
                UnityEngine.GameObject
                    .FindGameObjectsWithTag("massObject"); //Gathers all of the massObjects for calculation
            verletIntegration(); //Calls the verlet integration function
            UnityEngine.Vector3 outputPosition = V3DtoV3(position); //For displaying position in Unity editor
            transform.position = outputPosition * scale; //Updates the position of the unity object
            inputPosition = outputPosition; //This line is for tracking purposes and should not be included in the build
            inputVelocity = V3DtoV3(position - previousPosition); //For displaying velocity in Unity editor
        }

        public void verletIntegration()
        {
            calculateAcceleration(); //Calculates acceleration
            UnityEngine.Vector3d
                tempPosition = position; //Saves the position for next calculation BELOW:The Verlet Calculation
            position = position + (position - previousPosition) * (timeStep / previousTimeStep) +
                       acceleration * timeStep * timeStep;
            previousTimeStep =
                timeStep; //Saves the timestep for next calculation (ABOVE) The actual time adjusted verlet algorithm
            previousPosition = tempPosition; //Saves the position for next calculation
        }

        public void calculateAcceleration()
        {
            UnityEngine.Vector3d ForceVector = UnityEngine.Vector3d.zero;
            for (int i = 0; i < massObject.Length; i++)
            {
                //For the rest of the objects...
                if (massObject[i] != gameObject)
                {
                    //If the object isn't this object
                    VerletObjectV1 mO = massObject[i].GetComponent<VerletObjectV1>(); //The "O" is a letter not a number
                    distance = UnityEngine.Mathd.Sqrt(UnityEngine.Mathd.Pow(position.x - mO.position.x, 2) +
                                                      UnityEngine.Mathd.Pow(position.y - mO.position.y, 2) +
                                                      UnityEngine.Mathd.Pow(position.z - mO.position.z, 2));
                    double Force = 1.9934976 * UnityEngine.Mathd.Pow(10, -44) * (mass * mO.mass) *
                                   (1 / UnityEngine.Mathd.Pow(distance,
                                        2)); //This and ABOVE calculate force of gravity and distance between bodies respectively

                    UnityEngine.Vector3d
                        thisVector =
                            (mO.position - position).normalized *
                            Force; //Updates the velocity vector based on each body, thus calculating all relationships
                    ForceVector = ForceVector + thisVector;
                }
            }

            acceleration = ForceVector * (1 / mass); //Calculates the actual accleration
        }

        private UnityEngine.Vector3 V3DtoV3(UnityEngine.Vector3d value)
        {
            //Convert from Vector3d to Vector3
            float xVal = (float) value.x;
            float yVal = (float) value.y;
            float zVal = (float) value.z;
            return new UnityEngine.Vector3(xVal, yVal, zVal);
        }

        private UnityEngine.Vector3d V3toV3D(UnityEngine.Vector3 value)
        {
            //Convert from Vector3 to Vector3d
            double xVal = value.x;
            double yVal = value.y;
            double zVal = value.z;
            return new UnityEngine.Vector3d(xVal, yVal, zVal);
        }
    }
}