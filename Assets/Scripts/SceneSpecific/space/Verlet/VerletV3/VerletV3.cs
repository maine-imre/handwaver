using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletV3 : MonoBehaviour {
	public float timeStep = 1;					//The length of the timestep in seconds
	public double masterTimeCounter = 0;
	public double masterDaysCounter = 0;
	private float previousTimeStep;				//The previous timestep for time corrected verlet
	public GameObject[] massObject;				//A list of the rest of the bodies (Note to self: Bring down to nLog(n) using control script)
	public float scale = 1;						//Numerical scale of the distance between objects
	public float bodyScale = 1; 				//Multipliciative scale of the size of the bodies in the system
	private VerletV2Thread[] multithreadedJobs = new VerletV2Thread[0]; //List of all of the active threads
	private VerletV3ControlThread simulationControlThread;
	private List<string> threadIdent = new List<string>();
	private int multiThreadFlag = 0;			//This flag indicates how many threads have been completed
	public string CenterBody = "Sol";
	public Vector3d CenterBodyOffset = new Vector3d(0,0,0);

	//stuff for controlling the timestep
	public bool lastFrameCompleted = true;
	public int reducing = 0;				//0 has not been reduced, 1 is reducing and 2 is reduced
	public float multiple = 10;
	public int stepsPerFrame = 1;
	public float stepTimeStep = 10;
	public float stepTimeStepP = 10;
	private int mode1FlipCounter = 0;

	//Below are DEBUG variables
	private int counter = 0;					//Counter of the number of thread loops
	private int minicounter = 0;				//Counter of the number of update calls

	// Use this for initialization
	void Start () {
		previousTimeStep = timeStep;			//Initial condition
		runThreads();
	}
	
	// Update is called once per frame
	void Update () {
		VerletObjectV3 center = (VerletObjectV3)GameObject.Find(CenterBody).GetComponent("VerletObjectV3");
		CenterBodyOffset = center.position;
		minicounter++;
		masterDaysCounter = masterTimeCounter/86400;
		//Debug.Log("On frame : "+minicounter+", "+counter+" loops have completed.");
		runThreads();
	}

	void runThreads() {
		if(multiThreadFlag == multithreadedJobs.Length && timeStep != 0) {		//If the threads have completed
			if(timeStep != 0 && timeStep == previousTimeStep){
				Debug.Log(lastFrameCompleted + "	" + stepTimeStep + "	" + timeStep + "	" + stepTimeStep/timeStep + "	" + (int)Mathf.CeilToInt(stepTimeStep/timeStep));
				//Debug.Log(lastFrameCompleted + "	" + multiple + "	" + stepTimeStep + "	" + (multiple - Mathf.Log(Mathf.Pow(multiple, 1/6))));
				if(lastFrameCompleted && multiple >= 1.05) {
					stepTimeStep = stepTimeStep	/ multiple;
					stepsPerFrame = Mathd.CeilToInt(timeStep / stepTimeStep);
				} if (!lastFrameCompleted && multiple >= 1.05) {
					stepTimeStep = stepTimeStep * multiple;
					stepsPerFrame = Mathd.CeilToInt(timeStep / stepTimeStep);
					multiple = multiple - Mathf.Log(Mathf.Pow(multiple, 1/6));
				} if ((lastFrameCompleted && multiple < 1.05) || (Mathf.Abs(stepTimeStep) < 0.05)) {
					Debug.Log("Ping");
					multiple = 1;
				} if (!lastFrameCompleted && multiple < 1.05) {
					multiple = 5;
				}
			} else if (previousTimeStep != timeStep) {
				multiple = 10;
				stepTimeStep = 10;
			}
			multiThreadFlag = 0;
			massObject = GameObject.FindGameObjectsWithTag("massObject");		//Gathers all of the massObjects for calculation
			float[] masses = new float[massObject.Length];						//Gathers all of the masses in order
			Vector3d[] positions = new Vector3d[massObject.Length];				//Gathers all of the vectors of the objects in order
			string[] names = new string[massObject.Length];
			Vector3d[] previousPositions = new Vector3d[massObject.Length];
			for(int i = 0; i < massObject.Length; i++) {						//For every object, get data for mass and position
				VerletObjectV3 obj = (VerletObjectV3)massObject[i].GetComponent("VerletObjectV3");
				masses[i] = obj.mass;
				positions[i] = obj.position;
				names[i] = obj.name;
				previousPositions[i] = obj.previousPosition;//Assign previous position
			}
			simulationControlThread = new VerletV3ControlThread();
			simulationControlThread.masses = masses;
			simulationControlThread.massObjectNames = names;
			simulationControlThread.masterTimeCounter = masterTimeCounter;
			simulationControlThread.positions = positions;
			simulationControlThread.timestep = stepTimeStep;
			simulationControlThread.steps = stepsPerFrame;
			simulationControlThread.names = names;
			simulationControlThread.previousPositions = previousPositions;
			simulationControlThread.previousTimeStep = stepTimeStepP;

			simulationControlThread.Start();									//Start thread
			StartCoroutine ("finished" , simulationControlThread);				//Start end coroutine

			previousTimeStep = timeStep;
			stepTimeStepP = stepTimeStep;
		}
	}
	IEnumerator finished (VerletV3ControlThread myJob) {
		yield return StartCoroutine(myJob.WaitFor());
		//Debug.Log("finished "+myJob.ThreadName+"	it was: "+myJob.p);
		for(int i = 0; i < myJob.names.Length; i++) {
			VerletObjectV3 thisObject = ((VerletObjectV3)massObject[i].GetComponent("VerletObjectV3"));
			thisObject.newPos(myJob.positions[i]);
			thisObject.previousPosition = myJob.previousPositions[i];
		}
		masterTimeCounter = myJob.masterTimeCounter;
		lastFrameCompleted = myJob.frameCompleted;
		multiThreadFlag += 1;
		runThreads();
	}
}
