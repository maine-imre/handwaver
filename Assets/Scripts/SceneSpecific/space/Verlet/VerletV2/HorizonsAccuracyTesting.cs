using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;


public class HorizonsAccuracyTesting : MonoBehaviour {
	//public GameObject defaultBody;		//The default version of a massive object
	private string bodyName = "";		//The given name of the object
	private float radius = 1;			//The radius of the object (if the object is close to circular)
	private float mass = 1;				//The mass of the object
	private float x;					//The X position of the object
	private float y;					//The Y position of the object
	private float z;					//The Z position of the object
	private float vx;					//The X Velocity of the object
	private float vy;					//The Y Velocity of the object
	private float vz;					//The Z Velocity of the object
	private string date;				//The current date (for getting the current to the hour data)
	private string time;				//The current time (assists above)
	private float rotation;
	private bool flag = false;			//If the parse process failed
	private DateTime STARTTIME;
	private List<DateTime> UpdateTimes = new List<DateTime>();
	public String forBody;
	private bool timePassedFlag = false;
	public VerletV2 controlScript;

	// Use this for initialization
	void Start () { 
		STARTTIME = DateTime.Now;
		for(int i = 0; i < 29; i++) {
			UpdateTimes.Add(STARTTIME.AddDays(i));
		}
	}
	void TestTime(DateTime currenttime, String body) {
		XmlDocument dataFile = new XmlDocument();								//The file of data about the bodies
		dataFile.Load(@"Assets/Big-Birtha/PlanetaryData/filename.xml");
		XmlDocument orbitFile = new XmlDocument();								//The file of the orbit data for each body
		orbitFile.Load(@"Assets/Big-Birtha/PlanetaryData/orbitData.xml");
		foreach(XmlNode node in dataFile.DocumentElement.ChildNodes){			//For every body
   			bodyName = node.Attributes["name"].Value;							//Name the body
			foreach(XmlNode subnode in node.ChildNodes){						//For all accociated data
				if(subnode.Name == "mass"){										//Get the mass
					//mass = float.Parse(subnode.InnerText);
					bool successfullyParsed = float.TryParse(subnode.InnerText, out mass);
					if(!successfullyParsed) {
						flag = true;
						break;
					}
				} else if (subnode.Name == "radius"){							//Get the radius (if available)
					radius = float.Parse(subnode.InnerText);
				} else if (subnode.Name == "rotation"){
					rotation = float.Parse(subnode.InnerText);
				}
			}
			string currentMonth = "";
			switch(currenttime.Month){											//Getting the month correct
				case 1:
					currentMonth = "Jan";
					break;
				case 2:
					currentMonth = "Feb";
					break;
				case 3:
					currentMonth = "Mar";
					break;
				case 4:
					currentMonth = "Apr";
					break;
				case 5:
					currentMonth = "May";
					break;
				case 6:
					currentMonth = "Jun";
					break;
				case 7:
					currentMonth = "Jul";
					break;
				case 8:
					currentMonth = "Aug";
					break;
				case 9:
					currentMonth = "Sep";
					break;
				case 10:
					currentMonth = "Oct";
					break;
				case 11:
					currentMonth = "Nov";
					break;
				case 12:
					currentMonth = "Dec";
					break;
			}
			string targetDateTime = currenttime.Year.ToString() + "-" + currentMonth + "-";
			if(currenttime.Day < 10) {
				targetDateTime += "0";
			}
			targetDateTime += currenttime.Day.ToString() + " ";
			if(currenttime.Hour < 10) {
				targetDateTime +="0";
			}
			targetDateTime += currenttime.Hour + ":00:00.0000";

			foreach(XmlNode orbitNode in orbitFile.DocumentElement.ChildNodes){	//For every planet
				if(orbitNode.Attributes["name"].Value == bodyName){
					foreach(XmlNode orbitsubnode in orbitNode.ChildNodes) {		//For every orbit datapoint
						if(orbitsubnode.Attributes["timeStamp"].Value == targetDateTime){
							x = float.Parse(orbitsubnode["X"].InnerText);		//(above) if the timestamp is the reqested one
							y = float.Parse(orbitsubnode["Y"].InnerText);		//		Get the data
							z = float.Parse(orbitsubnode["Z"].InnerText);
							vx = float.Parse(orbitsubnode["VX"].InnerText);
							vy = float.Parse(orbitsubnode["VY"].InnerText);
							vz = float.Parse(orbitsubnode["VZ"].InnerText);
						}
					}
				}
			}
			if(!flag) {                                                             //Flag prevents bad input
																					//defaultBody.name = bodyName;										//This code sets the default body values to the given data and instantiates it
				if(bodyName == body) {
					VerletObjectV2 theBody = (VerletObjectV2)GameObject.Find(body).GetComponent("VerletObjectV2");
					Double drift = dist(theBody.position, new Vector3d(x,y,z));
					Debug.Log(drift);
				}
			} else {
				flag = false;
			}
		}
	}
	
	Double dist (Vector3d p, Vector3d mO) {
		return Mathd.Sqrt(Mathd.Pow(p.x - mO.x,2) + Mathd.Pow(p.y - mO.y,2) +Mathd.Pow(p.z - mO.z,2));
	}
	// Update is called once per frame
	void Update () {
		TimeSpan duration = UpdateTimes.ToArray()[0] - STARTTIME;
		if(controlScript.masterTimeCounter > duration.TotalSeconds) {
			TestTime(UpdateTimes.ToArray()[0], bodyName);
			UpdateTimes.RemoveAt(0);
		}
	}
}