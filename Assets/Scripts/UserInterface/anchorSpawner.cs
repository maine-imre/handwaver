/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class anchorSpawner : MonoBehaviour
	{
		[Range(0f, 1f)]

		public float anchorScale;
		public bool pointSpawner;

		public GameObject item;


		public GameObject prevItem;

		private Anchor thisAnchor;

		void Start()
		{
			thisAnchor = GetComponent<Anchor>();
			thisAnchor.OnNoAnchorablesAttached += detached;
			spawnItem(this.transform);
		}

		private void OnEnable()
		{
			if(prevItem!= null)
				prevItem.SetActive(true);	
		}


		private void OnDisable()
		{
			if (prevItem != null)
				prevItem.SetActive(false);

		}

		private void detached()
		{
			spawnItem(this.transform);
		}

		internal virtual void spawnItem(Transform spawnPoint)
		{
			if(prevItem != null)
			{
				prevItem.transform.localScale /= anchorScale;
			}
			Transform newObj;
			if (!pointSpawner)
			{
				newObj = Instantiate(item, transform.position, Quaternion.identity).transform;
			}
			else
			{
				newObj = GeoObjConstruction.iPoint(HW_GeoSolver.ins.systemPosition(transform.position)).transform;
			}
			if(newObj == null)
			{
				Debug.LogError("No object spawning.");
				return;
			}
			newObj.transform.localScale *= anchorScale;
			newObj.GetComponent<AnchorableBehaviour>().anchor = thisAnchor;
			if (!newObj.GetComponent<AnchorableBehaviour>().TryAttach(true))
			{
				Debug.Log("Didnt attach to anchor. Whyyy");
			}
			prevItem = newObj.gameObject;

		}
	}
}
