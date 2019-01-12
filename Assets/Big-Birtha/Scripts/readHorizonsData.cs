using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;

namespace IMRE.HandWaver.Space.BigBertha
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is TB
	/// Status: WORKING
	/// </summary>
	public class readHorizonsData : MonoBehaviour
	{
		public GameObject defaultBody;      //The default version of a massive object
		private string bodyName = "";       //The given name of the object
		private float radius = 1;           //The radius of the object (if the object is close to circular)
		private float mass = 1;             //The mass of the object
		private float x;                    //The X position of the object
		private float y;                    //The Y position of the object
		private float z;                    //The Z position of the object
		private float vx;                   //The X Velocity of the object
		private float vy;                   //The Y Velocity of the object
		private float vz;                   //The Z Velocity of the object
		private string date;                //The current date (for getting the current to the hour data)
		private string time;                //The current time (assists above)

		// Use this for initialization
		void Start()
		{
			XmlDocument dataFile = new XmlDocument();                               //The file of data about the bodies
			dataFile.Load(@"Assets/PlanetaryData/filename.xml");
			XmlDocument orbitFile = new XmlDocument();                              //The file of the orbit data for each body
			orbitFile.Load(@"Assets/PlanetaryData/orbitData.xml");
			foreach (XmlNode node in dataFile.DocumentElement.ChildNodes)
			{           //For every body
				bodyName = node.Attributes["name"].Value;                           //Name the body
				foreach (XmlNode subnode in node.ChildNodes)
				{                       //For all accociated data
					if (subnode.Name == "mass")
					{                                       //Get the mass
						mass = float.Parse(subnode.InnerText);
					}
					else if (subnode.Name == "radius")
					{                           //Get the radius (if available)
						radius = float.Parse(subnode.InnerText);
					}
				}
				DateTime currenttime = DateTime.Now;                                //The current date/time
				string currentMonth = "";
				switch (currenttime.Month)
				{                                           //Getting the month correct
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
				Debug.Log(currenttime.Hour);
				string targetDateTime = "";
				if (currenttime.Hour == 0)
				{
					targetDateTime = currenttime.Year.ToString() + "-" + currentMonth + "-" + currenttime.Day.ToString() + " 0" + currenttime.Hour + ":00:00.0000";
				}
				else
				{
					targetDateTime = currenttime.Year.ToString() + "-" + currentMonth + "-" + currenttime.Day.ToString() + " " + currenttime.Hour + ":00:00.0000";
				}
				foreach (XmlNode orbitNode in orbitFile.DocumentElement.ChildNodes)
				{   //For every planet
					if (orbitNode.Attributes["name"].Value == bodyName)
					{
						Debug.Log("START");
						Debug.Log(targetDateTime);
						foreach (XmlNode orbitsubnode in orbitNode.ChildNodes)
						{       //For every orbit datapoint
							if (orbitsubnode.Attributes["timeStamp"].Value == targetDateTime)
							{
								x = float.Parse(orbitsubnode["X"].InnerText);       //(above) if the timestamp is the reqested one
								y = float.Parse(orbitsubnode["Y"].InnerText);       //		Get the data
								z = float.Parse(orbitsubnode["Z"].InnerText);
								vx = float.Parse(orbitsubnode["VX"].InnerText);
								vy = float.Parse(orbitsubnode["VY"].InnerText);
								vz = float.Parse(orbitsubnode["VZ"].InnerText);
							}
						}
					}
				}
				//defaultBody.name = bodyName;										//This code sets the default body values to the given data and instantiates it
				VerletObjectV1 df = defaultBody.GetComponent<VerletObjectV1>();
				df.mass = mass;
				df.inputPosition = new Vector3(x, y, z);
				df.inputVelocity = new Vector3(vx, vy, vz);
				df.radius = radius;
				GameObject body = Instantiate(defaultBody);
				body.name = bodyName;
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}