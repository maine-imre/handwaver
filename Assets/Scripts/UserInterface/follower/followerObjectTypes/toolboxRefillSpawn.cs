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
using PathologicalGames;
using System;

namespace IMRE.HandWaver {
	[RequireComponent(typeof(InteractionBehaviour), typeof(AnchorableBehaviour))]
	public class toolboxRefillSpawn : MonoBehaviour
	{
		InteractionBehaviour thisIbehave;
		AnchorableBehaviour thisABehave;
		Vector3 initPos;


		public string poolName;
		public string objName;
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

			Transform newObj = PoolManager.Pools[poolName].Spawn(objName);
			thisIbehave.graspingController.ReleaseGrasp();
			newObj.transform.position = thisIbehave.graspingController.transform.position;
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
