using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script controls 
/// The main contributor(s) to this script is NG
/// </summary>

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class RSDESAnchorSpawn : MonoBehaviour
	{
		public GameObject GMTClock;
		public GameObject PinAnchor;
		public GameObject EraserAnchor;
		// Use this for initialization
		void Start()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		private void OnSceneUnloaded(Scene arg0)
		{
			if (arg0.name == "RSDES")
			{
				PinAnchor.SetActive(false);
				EraserAnchor.SetActive(false);
				GMTClock.SetActive(false);

			}
		}

		private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			if (arg0.name == "RSDES")
			{
				PinAnchor.SetActive(true);
				EraserAnchor.SetActive(true);
				GMTClock.SetActive(true);

			}

		}

	}
}