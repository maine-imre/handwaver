using System;
using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class HorizonsExampleRunner : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
			planetData sol = HorizonsV4.Planets.Find(p => p.name.ToLower() == "sol");						//planetData is a class with datat access method shown below
			planetData moon = HorizonsV4.Planets.Find(p => p.name.ToLower() == "moon");						//suffs in meters just for you
																											//Use these names, and the DateTime that you want
			Debug.Log(sol.name);
			Debug.Log(sol.time);
			Debug.Log(sol.position);
			Debug.Log(sol.velocity);

			Debug.Log(moon.name);
			Debug.Log(moon.time);
			Debug.Log(moon.position);
			Debug.Log(moon.velocity);
		}

	}
}