using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// This script, when applied to an object in a scene, will turn to face towards the player.
	/// The main contributor(s) to this script is CE
	/// </summary>
	public class LookAtPlayer : MonoBehaviour
	{
		private Transform player;

		// Use this for initialization
		void Start()
		{
			player = Camera.main.transform;
		}

		// Update is called once per frame
		void Update()
		{
            if(player != null)
			transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
		}
	}

}
