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
    public class toolboxMenuCreator : MonoBehaviour
    {
        public GameObject menuPanel;
        public float createDist = 2.25f;
        [Range(0f, 3f)]
        public float tolerance = 0.25f;

        private Vector3 initPos;
        private Quaternion initRot;
        private Vector3 initLocalPos;
        private Quaternion initLocalRot;
        private Vector3 initScale;

        private Vector3 menuSpawnPosition;


		private Vector3 currentPos;
        private float currentDist;

        private bool firstSpawn = true;


        private InteractionBehaviour thisInteract;

        private toolboxfollower Parent;

        private void Start()
        {
            firstSpawn = true;
            initPos = gameObject.transform.position;
            initRot = gameObject.transform.rotation;
            initLocalPos = gameObject.transform.localPosition;
            initLocalRot = gameObject.transform.localRotation;
            initScale = gameObject.transform.localScale;
            menuSpawnPosition = new Vector3(-1.305f, 1.3f, -0.14f);
			thisInteract = gameObject.GetComponent<InteractionBehaviour>();
            Parent = gameObject.GetComponentInParent<toolboxfollower>();
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
                if ((Mathf.Abs(currentDist - createDist)) <= tolerance && firstSpawn && Parent.spawnable())
                {
                    GameObject loc = new GameObject();
                    loc.transform.position = menuSpawnPosition;
					loc.transform.rotation = Quaternion.AngleAxis(90, Vector3.down);
                    spawnPanel(loc.transform);
                    firstSpawn = false;
                }
            }
        }

        private void spawnPanel(Transform spawnPoint)
        {
                if (menuPanel != null)
                {
                    Instantiate(menuPanel, spawnPoint.transform.position, spawnPoint.rotation);
                }
                else
                {
                    Debug.Log("item is not set. please set item or fix pool.");
                }

            returnToInit(thisInteract.graspingController);
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
    }
}