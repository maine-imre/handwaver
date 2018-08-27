/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Leap.Unity.Interaction;

namespace IMRE.HandWaver.Lattice
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class PinBoard3D : MonoBehaviour
    {
        /// <summary>
        /// Used to size the pin board
        /// </summary>
        [Range(2,100)]
        public int size = 10;
		[Range(0, 4)]
		public float scaleFactor = 1;

		public enum basisType { rectangular, tetrahedronEquidistant}

		public basisType thisBasisType = basisType.rectangular;

		public Vector3 translation = Vector3.zero;

		public GameObject showobject;

		Vector3[] basisSystem
		{
			get
			{
				switch (thisBasisType)
				{
					case basisType.rectangular:
						return new Vector3[] { Vector3.right, Vector3.up, Vector3.forward };
					case basisType.tetrahedronEquidistant:
						Vector3 v1 = new Vector3(Mathf.Sqrt(8f / 9f), 0, -1f / 3f);
						Vector3 v2 = new Vector3(-Mathf.Sqrt(2f / 9f), Mathf.Sqrt(2f / 3f), -1f / 3f);
						Vector3 v3 = new Vector3(-Mathf.Sqrt(2f / 9f), -Mathf.Sqrt(2f / 3f), -1f / 3f);
						Vector3 v4 = Vector3.forward;

						Vector3 basis0 = (v4 - v1).normalized;
						Vector3 basis1 = (v4 - v2).normalized;
						Vector3 basis2 = (v4 - v3).normalized;
						return new Vector3[] { basis0, basis1, basis2 };
					default:
						Debug.LogWarning("NO BASIS SET!");
						return new Vector3[] { Vector3.right, Vector3.up, Vector3.forward };
				}
			}
		}

        private void Awake()
        {
			Resources.FindObjectsOfTypeAll<FingerPointLineMaker>().ToList().ForEach(f => f.gameObject.SetActive(true));
        }

		/// <summary>
		/// just for now
		/// </summary>
		private void Start()
		{
			constructLatticeLand();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				showobject.SetActive(true);
			}
		}
		float waitTime = 0;

		private void LateUpdate()
		{
			while(waitTime < 30)
			{
				waitTime++;
			}
			if (waitTime == 20)
			{
				IMRE.Wrappers.Achievements.ins.setAchievement("OpenedLatticeLand", true);
				IMRE.Wrappers.Achievements.ins.incrementStat("OpenLatticeLandCount");
				Debug.Log("VIVEPORT STATS :" + IMRE.Wrappers.Achievements.ins.getAchievement("OpenedLatticeLand") + IMRE.Wrappers.Achievements.ins.getStat("OpenLatticeLandCount"));
			}
		}

		public void constructLatticeLand()
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					for (int k = 0; k < size; k++)
					{
						StaticPoint curr = GeoObjConstruction.sPoint(scaleFactor * ((i - (size / 2)) * basisSystem[0] + j * basisSystem[1] + (k - (size / 2)) * basisSystem[2]) + translation);
						curr.transform.SetParent(transform);
						//curr.GetComponent<InteractionBehaviour>().enabled = false;
						curr.tag = "NoSave";

						curr.GetComponent<MasterGeoObj>().allowDelete = false;
					}
				}
			}
		}
    }
}