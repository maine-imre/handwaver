public class VerletV2Thread : AbstractThread
{
    public UnityEngine.Vector3d a;
    public float m;
    public float[] m2;
    public UnityEngine.Vector3d outputPosition;
    public UnityEngine.Vector3d p;
    public UnityEngine.Vector3d[] p2;
    public UnityEngine.Vector3d pp; //The previous position of the body
    public float pts; //The previous timestep for time corrected verlet
    public int thisIndex;
    public float ts; //The length of the timestep in seconds

    protected override void ThreadedFunction()
    {
        verletIntegration();
    }

    protected override UnityEngine.Vector3d OnFinished()
    {
        return p;
    }

    public void verletIntegration()
    {
        calculateAcceleration(); //Calculates acceleration
        UnityEngine.Vector3d tp = p; //Saves the position for next calculation BELOW:The Verlet Calculation
        p = p + (p - pp) * (ts / pts) + a * ts * ts;
        pts = ts; //Saves the timestep for next calculation (ABOVE) The actual time adjusted verlet algorithm
        pp = tp; //Saves the position for next calculation
    }

    public void calculateAcceleration()
    {
        UnityEngine.Vector3d ForceVector = UnityEngine.Vector3d.zero;
        double d = 0;
        for (int i = 0; i < m2.Length; i++)
        {
            //For the rest of the objects...
            if (thisIndex != i)
            {
                //If the object isn't this object
                UnityEngine.Vector3d mO = p2[i];
                float m1 = m2[i];
                d = UnityEngine.Mathd.Sqrt(UnityEngine.Mathd.Pow(p.x - mO.x, 2) + UnityEngine.Mathd.Pow(p.y - mO.y, 2) +
                                           UnityEngine.Mathd.Pow(p.z - mO.z, 2));
                double Force = 1.9934976 * UnityEngine.Mathd.Pow(10, -44) * (m * m1) *
                               (1 / UnityEngine.Mathd.Pow(d,
                                    2)); //This and ABOVE calculate force of gravity and distance between bodies respectively

                UnityEngine.Vector3d
                    thisVector =
                        (mO - p).normalized *
                        Force; //Updates the velocity vector based on each body, thus calculating all relationships
                ForceVector = ForceVector + thisVector;
            }
        }

        a = ForceVector * (1 / m); //Calculates the actual accleration
    }
}