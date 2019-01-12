using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Space.BigBertha
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is TB
	/// Status: WORKING
	/// </summary>
	public class pointLightAtObject : MonoBehaviour
	{
		public GameObject target;
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			transform.LookAt(target.transform);
		}
	}
}