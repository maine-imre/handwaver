using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletObjectV2 : MonoBehaviour {
	#region Constructors
	public static VerletObjectV2 Constructor(){
		GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RSDES/VerletV2"));
		return go.GetComponent<VerletObjectV2>();
	}
	#endregion
	public Vector3 inputPosition;
	public Vector3d position;				//The position vector of the body
	public Vector3d previousPosition;		//The previous position of the body
	public Vector3 inputVelocity;
	public Vector3d initialVelocity;			//The initial velocity of the body (note: doesn't change once the calculation starts)
	public float mass;						//The mass of the body
	public float radius;					//The radius of the body
	public float rotation = 0;
	private float scale = 1;
	private double timePassed = 0;
	private float bodyScale = 1;
	private Vector3d CenterBodyOffset;
	public float sunscale = 1;
	private double oldSecondCounter = 0;
	GameObject sceneController;
	VerletV2 controlScript;
	// Use this for initialization
	void Start () {
		sceneController = GameObject.Find("SceneController");				//To keep various variables up to date
		controlScript = (VerletV2) sceneController.GetComponent("VerletV2");
		position = V3toV3D(inputPosition);									//Convert input floats to doubles
		initialVelocity = V3toV3D(inputVelocity);							//Ditto above
		initialVelocity = initialVelocity * 0.00001157407407 * controlScript.timeStep;	//Unit conversion of Velocity from AU/Day to AU/Sec then multiplys by timestep
		previousPosition = position - initialVelocity;						//Calculates previous position based on initial velocity
	}
	
	// Update is called once per frame
	void Update () {
		double time = controlScript.masterTimeCounter;
		scale = controlScript.scale;
		if(rotation != 0){
			this.transform.rotation *= Quaternion.AngleAxis((float)(time-oldSecondCounter)*(360/(rotation*86400)), Vector3.up);
			oldSecondCounter = time;
		}
		bodyScale = controlScript.bodyScale * controlScript.scale;
		CenterBodyOffset = controlScript.CenterBodyOffset;

		Vector3 outputPosition = V3DtoV3(position - CenterBodyOffset);//For displaying position in Unity editor
		transform.position = outputPosition * scale;                        //Updates the position of the unity object

		float diameter = 1;
		if(gameObject.name != "Sol") {
			diameter = 0.0000000067f * 2 * bodyScale * radius;
		} else {
			diameter = 0.0000000067f * 2 * bodyScale / sunscale * radius;
		}
		transform.localScale = new Vector3(diameter, diameter, diameter);
	}

	public void newPos (Vector3d np) {
		previousPosition = position;									//Saves the position for next calculation
		position = np;
	}

	private Vector3 V3DtoV3(Vector3d value) {	//Convert from Vector3d to Vector3
		float xVal = (float) value.x;
		float yVal = (float) value.y;
		float zVal = (float) value.z;
		return(new Vector3(xVal,yVal,zVal));
	}
	private Vector3d V3toV3D(Vector3 value) {	//Convert from Vector3 to Vector3d
		double xVal = value.x;
		double yVal = value.y;
		double zVal = value.z;
		return(new Vector3d(xVal,yVal,zVal));
	}
}
