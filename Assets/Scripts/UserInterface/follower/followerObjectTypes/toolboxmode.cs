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

namespace IMRE.HandWaver
{
    [RequireComponent(typeof(Light))]
    /// <summary>
    /// Determines how the toolbox approaches and follows the user.
    /// </summary>
    public class toolboxmode : MonoBehaviour
    {
        //public leapControls[] hands;
        public string mode;

        public float createDist = 2.25f;
        [Range(0f, 3f)]
        public float tolerance = 0.25f;

        private Vector3 initPos;
        private Quaternion initRot;
        private Vector3 initLocalPos;
        private Quaternion initLocalRot;
        private Vector3 initScale;

        private Vector3 currentPos;
        private float currentDist;

        private bool firstSpawn = true;

        public bool itemPooled;
        public string poolName;
        public string objName;

        private InteractionBehaviour thisInteract;

        private void Start()
        {
            firstSpawn = true;
            initPos = gameObject.transform.position;
            initRot = gameObject.transform.rotation;
            initLocalPos = gameObject.transform.localPosition;
            initLocalRot = gameObject.transform.localRotation;
            initScale = gameObject.transform.localScale;
            thisInteract = gameObject.GetComponent<InteractionBehaviour>();
        }

        private void Update()
        {
            currentPos = gameObject.transform.localPosition;
            currentDist = Vector3.Distance(initLocalPos, currentPos);
            gameObject.transform.localScale = (1 + (currentDist / createDist)) * initScale;
            if (gameObject.transform.localPosition == initLocalPos)
            {
                firstSpawn = true;
                //thisInteract.enabled = true;
            }


            {
                if (!thisInteract.isGrasped && currentPos != initPos)
                {
                    returnToInit(null);
                }
                if ((Mathf.Abs(currentDist - createDist)) <= tolerance && firstSpawn)
                {

                    SetMode(mode);
                    firstSpawn = false;
                }
            }
        }

        private void returnToInit(InteractionController iControl)
        {
            if (iControl != null)
            {
                iControl.ReleaseGrasp();
            }

            gameObject.transform.localPosition = initLocalPos;
            gameObject.transform.localRotation = initLocalRot;
        }

        public void SetMode(string value)
        {
			Debug.LogWarning("We Need to rework leapcontrols/setmode");


			//foreach (leapControls hand in hands)
   //         {

   //             hand.setMode(value);
   //         }

            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if(transform.parent.GetChild(i).GetComponent<Light>() != null)
                    transform.parent.GetChild(i).GetComponent<Light>().enabled = false;
            }
            transform.GetComponent<Light>().enabled = true;

            returnToInit(thisInteract.graspingController);
        }

    }
}
