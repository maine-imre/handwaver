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
	public class GravationalObjectV2 : MonoBehaviour
	{
		public float mass;
		public double x = 0;
		public double y = 0;
		public double z = 0;
		public Vector3 VelocityVector = new Vector3(); //Used for initial velocity, not calculation
		public Vector3d VVec;
		[HideInInspector]
		public double scale = 1;
		// Use this for initialization
		void Start()
		{
			VVec.x = (double)VelocityVector.x;
			VVec.y = (double)VelocityVector.y;
			VVec.z = (double)VelocityVector.z;
		}

		// Update is called once per frame
		void Update()
		{
			transform.position = new Vector3((float)(x * scale), (float)(y * scale), (float)(z * scale));
		}
	}
}
