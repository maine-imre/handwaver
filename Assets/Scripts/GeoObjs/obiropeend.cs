using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class obiropeend : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
			AbstractPoint thisPoint = GeoObjConstruction.iPoint(transform.position);
			thisPoint.stretchEnabled = false;
		}


	}
}
