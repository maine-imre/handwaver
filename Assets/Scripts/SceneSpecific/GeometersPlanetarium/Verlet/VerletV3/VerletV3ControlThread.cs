public class VerletV3ControlThread : AbstractThread
{
    public bool frameCompleted;
    public float[] masses;
    public string[] massObjectNames;
    public double masterTimeCounter;
    private VerletV2Thread multithreadedJob = new VerletV2Thread(); //List of all of the active threads

    public VerletV2Thread[] multithreadedJobs;

    //Information for the next thread
    public int multiThreadFlag;
    public string[] names;
    public UnityEngine.Vector3d[] positions;
    public UnityEngine.Vector3d[] previousPositions;
    public float previousTimeStep;

    public UnityEngine.Vector3d returnvalue; //Define all variables needed for the function here
    public int stepCounter;
    public float steps;

    //Information specifically for this script
    public float timestep;

    protected override void ThreadedFunction()
    {
        testfunction(); //Calls this function
    }

    protected override UnityEngine.Vector3d OnFinished()
    {
        return returnvalue;
    }

    public void testfunction()
    {
        if (multiThreadFlag == multithreadedJobs.Length && timestep != 0)
        {
            System.DateTime start = System.DateTime.Now.AddMilliseconds(6);
            while (System.DateTime.Now < start || stepCounter < steps)
            {
                stepCounter++;
                masterTimeCounter += timestep; //Problematic
                multiThreadFlag = 0;
                multithreadedJobs = new VerletV2Thread[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    multithreadedJobs[i] = new VerletV2Thread();
                    VerletV2Thread thisJob = multithreadedJobs[i];
                    thisJob.ThreadName = names[i] + i; //Assign name
                    thisJob.thisIndex = i; //Assign index (Unused I think)
                    thisJob.m = masses[i]; //Assign mass
                    thisJob.p = positions[i]; //Assign position of object
                    thisJob.pp = previousPositions[i];
                    thisJob.m2 = masses; //Assign mass of every object
                    thisJob.p2 = positions; //Assign psoition of every object
                    thisJob.ts = timestep; //Assign timestep for the simulation
                    thisJob.pts = previousTimeStep; //Assign previous timestep

                    multithreadedJobs[i].Start(); //Start thread
                }

                bool allFinished = false;
                while (!allFinished)
                {
                    int counter = 0;
                    for (int i = 0; i < multithreadedJobs.Length; i++)
                    {
                        if (multithreadedJobs[i].IsDone) counter += 1;
                    }

                    if (counter == multithreadedJobs.Length) allFinished = true;
                }

                //code for starting next step
                for (int i = 0; i < multithreadedJobs.Length; i++)
                {
                    previousPositions[i] = positions[i];
                    positions[i] = multithreadedJobs[i].outputPosition;
                    previousTimeStep = timestep;
                }
            }

            if (stepCounter == steps) frameCompleted = true;
        }
    }
}