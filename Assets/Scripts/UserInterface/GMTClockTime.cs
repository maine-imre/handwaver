using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class GMTClockTime : MonoBehaviour
	{
		public pinData sunsubpoint;
		void Start()
		{
	
		}

		void Update()
		{
			GetComponent<TextMeshPro>().SetText(GeoPlanetMaths.timeOfSimulation(sunsubpoint.contactPoint).ToLongTimeString() + " GMT");
		}
	}
}
