using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IMRE.HandWaver.Space.BigBertha
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is TB
	/// Status: WORKING
	/// </summary>
	public class VerletV2StressTestControl : MonoBehaviour
	{
		public Transform massiveBody;
		VerletObjectV1 vO;
		Vector3 generatePosition;
		private int internalNumber = 0;
		public int numberOfBodies = 0;
		public Text text;

		// Use this for initialization
		void Start()
		{
			vO = massiveBody.GetComponent<VerletObjectV1>();
		}

		// Update is called once per frame
		void Update()
		{
			text.text = (numberOfBodies / 5).ToString();
			numberOfBodies += 1;
			for (int i = 0; i < (numberOfBodies - internalNumber); i++)
			{
				((VerletObjectV2)massiveBody.GetComponent("VerletObjectV2")).mass = UnityEngine.Random.Range(100000, 1000000000000);
				massiveBody.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
				Instantiate(massiveBody);
				internalNumber += 10;
			}
		}
	}
}