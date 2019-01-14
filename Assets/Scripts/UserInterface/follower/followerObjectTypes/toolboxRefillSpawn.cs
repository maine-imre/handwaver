/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

using System;

namespace IMRE.HandWaver {
	[RequireComponent(typeof(InteractionBehaviour), typeof(AnchorableBehaviour))]
	/// <summary>
	/// Refillable spanwer for disposable tools on the toolbox.
	/// </summary>
	public class toolboxRefillSpawn : MonoBehaviour
	{
		InteractionBehaviour thisIbehave;
		AnchorableBehaviour thisABehave;
		Vector3 initPos;

		public GameObject item;

		void Start()
		{
			thisIbehave = GetComponent<InteractionBehaviour>();
			thisABehave = GetComponent<AnchorableBehaviour>();
			thisABehave.OnDetachedFromAnchor += detach;
			initPos = gameObject.transform.position;
		}

		private void detach()
		{
			Debug.Log(1);
			thisIbehave.graspingController.ReleaseGrasp();

			Transform newObj;

				if (item != null)
				{
					newObj = Instantiate(item,initPos, Quaternion.identity).transform;
					newObj.GetComponent<AnchorableBehaviour>().TryAttachToNearestAnchor();
								thisIbehave.graspingController.ReleaseGrasp();
			newObj.transform.position = thisIbehave.graspingController.transform.position;
				}
				else
				{
					Debug.Log("item is not set. please set item or fix pool. Object: "+gameObject.name);
				}

			wait();
		}
		/// <summary>
		/// Waits for end of frame before allowing grasp again.
		/// </summary>
		/// <returns></returns>
		IEnumerator wait()
		{
			thisIbehave.ignoreGrasping = true;
			yield return new WaitForEndOfFrame();
			thisIbehave.ignoreGrasping = false;
		}
	}
}
