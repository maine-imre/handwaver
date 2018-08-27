/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver
{
    public class toolboxtoolspawn : MonoBehaviour 
    {
		#pragma warning disable 0649, 0414, 0169

		public toolboxfollower follower;

        public GameObject item;
        public float createDist = 2.25f;
        [Range(0f,3f)]
        public float tolerance = 0.25f;

        private Vector3 initPos;
        private Quaternion initRot;
        private Vector3 initLocalPos;
        private Quaternion initLocalRot;
        private Vector3 initScale;

        private Vector3 currentPos;
        private float currentDist;

		private int count = 1;


        //private bool firstSpawn = true;

        public bool itemPooled;
        public string poolName;
        public string objName;

        private InteractionBehaviour thisInteract;

        private toolboxfollower Parent;

		private AnchorableBehaviour aBehave;

#pragma warning disable 0649, 0414, 0169


		private void Start()
        {
			//firstSpawn = true;
			initPos = gameObject.transform.position;
            initRot = gameObject.transform.rotation;
            initLocalPos = gameObject.transform.localPosition;
            initLocalRot = gameObject.transform.localRotation;
            initScale = gameObject.transform.localScale;
            thisInteract = gameObject.GetComponent<InteractionBehaviour>();
            
			aBehave = this.GetComponent<AnchorableBehaviour>();
			aBehave.OnAttachedToAnchor += attach;
			aBehave.OnDetachedFromAnchor += detach;
        }

		private void detach()
		{
			//spawnItem(gameObject.transform);
		}

		private void attach()
		{
			returnToInit(null);

		}

		private void Update()
		{
			currentDist = Vector3.Magnitude(this.transform.position - follower.transform.position);

			////gameObject.transform.localScale = (1 + (currentDist / createDist)) * initScale;
			//if (gameObject.transform.localPosition == initLocalPos)
			//{
			//	firstSpawn = true;
			//	//thisInteract.enabled = true;
			//}


			{
				if (!thisInteract.isGrasped && currentPos != initPos)
				{
					returnToInit(null);
				}
				if (currentDist > createDist && follower.spawnable())
				{

					spawnItem(gameObject.transform);
					//firstSpawn = false;
				}
			}
		}

		private void spawnItem(Transform spawnPoint)
        {
				Transform newObj;
				if (!itemPooled)
				{
					if (item != null)
					{
						newObj = Instantiate(item, spawnPoint.transform.position, spawnPoint.rotation).transform;
					}
					else
					{
						Debug.Log("item is not set. please set item or fix pool.");
					}
				}
				else
				{
					newObj = PoolManager.Pools[poolName].Spawn(objName).transform;

					newObj.transform.position = gameObject.transform.position;
				}
				returnToInit(thisInteract.graspingController);
		}

        private void returnToInit(InteractionController iControl)
        {
            if (iControl != null)
            {
                iControl.ReleaseGrasp();
            }

			// With new system does not need to be handled
			//gameObject.transform.localPosition = initLocalPos;
			//gameObject.transform.localRotation = initLocalRot;

			//if (aBehave.anchor != null)
			//{
				aBehave.TryAttachToNearestAnchor();
			//}
		}
	}
}