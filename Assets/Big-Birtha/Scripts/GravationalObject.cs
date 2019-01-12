using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver.Space.BigBertha
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is TB
	/// Status: WORKING
	/// </summary>
	public class GravationalObject : MonoBehaviour
	{
		public float mass;
		public Vector3d positionVector = new Vector3d();    //position vector

		public float scale = 1;

		[HideInInspector]
		public Vector3d LastVector;
		public Vector3d VelVec = new Vector3d(); //Velocity Vector
		public Vector3 VelocityVector = new Vector3(); //UI velocity vector
		public Vector3d ForceVec = new Vector3d(); //Force Vector
		public Vector3 position;    //UI position vector

		// Use this for initialization
		void Start()
		{
			VelVec.x = (double)VelocityVector.x;
			VelVec.y = (double)VelocityVector.y;
			VelVec.z = (double)VelocityVector.z;

			positionVector.x = position.x;
			positionVector.y = position.y;
			positionVector.z = position.z;
		}

		// Update is called once per frame
		void Update()
		{
			gameObject.transform.position = new Vector3((float)positionVector.x * scale, (float)positionVector.y * scale, (float)positionVector.z * scale);
		}
		public void step1(float dt)
		{
			ForceVec = VelVec * (mass / dt);
			LastVector = positionVector;
			Debug.Log(ForceVec);
		}
		public void step2(GravationalObject gravitationObject, float dt)
		{
			ForceVec = ForceVec + gravityVector(gravitationObject);
		}
		public void step3(float dt)
		{
			VelVec = VelVec + ((ForceVec / mass) * (float)Math.Pow(dt, 2));
		}
		Vector3d gravityVector(GravationalObject otherObject)
		{
			Double distance = Vector3d.Distance(positionVector, otherObject.LastVector);
			float F = (float)(6.67408 * Math.Pow(10, -11)) * (float)((mass * otherObject.mass) / Math.Pow(distance, 2)); //meters, kg, seconds
			Vector3d vectorn = otherObject.LastVector - positionVector;
			vectorn.Normalize();
			vectorn = vectorn * F;
			return vectorn;
		}
	}
}