using System;
using UnityEngine;
public class VerletV3ControlThread : AbstractThread {
	//Information for the next thread
	public int multiThreadFlag = 0;
	public VerletV2Thread[] multithreadedJobs;
	public double masterTimeCounter;
	public float[] masses;
	public Vector3d[] positions;
	public Vector3d[] previousPositions;
	public string[] names;
	public string[] massObjectNames;
	public float previousTimeStep;
	public bool frameCompleted = false;

	//Information specifically for this script
	public float timestep;
	public float steps;
	public int stepCounter = 0;

	public Vector3d returnvalue;	//Define all variables needed for the function here
	private VerletV2Thread multithreadedJob = new VerletV2Thread(); //List of all of the active threads
	
	protected override void ThreadedFunction() {
		testfunction();			//Calls this function
	}
	protected override Vector3d OnFinished() {
		return returnvalue;
	}

	public void testfunction() {
		if(multiThreadFlag == multithreadedJobs.Length && timestep != 0) {
			DateTime start = DateTime.Now.AddMilliseconds(6);
			while(DateTime.Now < start || stepCounter < steps) {
				stepCounter ++;
				masterTimeCounter += timestep;									//Problematic
				multiThreadFlag = 0;
				multithreadedJobs = new VerletV2Thread[names.Length];
				for(int i = 0; i<names.Length; i++) {
					multithreadedJobs[i] = new VerletV2Thread();
					VerletV2Thread thisJob = multithreadedJobs[i];
					thisJob.ThreadName = names[i]+i;							//Assign name
					thisJob.thisIndex = i;										//Assign index (Unused I think)
					thisJob.m = masses[i];										//Assign mass
					thisJob.p = positions[i];									//Assign position of object
					thisJob.pp = previousPositions[i];
					thisJob.m2 = masses;											//Assign mass of every object
					thisJob.p2 = positions;											//Assign psoition of every object
					thisJob.ts = timestep;											//Assign timestep for the simulation
					thisJob.pts = previousTimeStep;									//Assign previous timestep

					multithreadedJobs[i].Start();									//Start thread
				}
				bool allFinished = false;
				while(!allFinished) {
					int counter = 0;
					for(int i = 0; i<multithreadedJobs.Length; i++) {
						if(multithreadedJobs[i].IsDone) {
							counter += 1;
						}
					}
					if(counter == multithreadedJobs.Length) {
						allFinished = true;
					}
				}
				//code for starting next step
				for(int i = 0; i<multithreadedJobs.Length; i++) {
					previousPositions[i] = positions[i];
					positions[i] = multithreadedJobs[i].outputPosition;
					previousTimeStep = timestep;
				}
			}
			if(stepCounter == steps) {
				frameCompleted = true;
			}
		}
	}
}
