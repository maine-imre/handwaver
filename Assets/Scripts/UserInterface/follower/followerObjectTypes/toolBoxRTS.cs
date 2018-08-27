/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PathologicalGames;
//using Leap.Unity.Interaction;



//namespace IMRE.HandWaver
//{
//    public class toolBoxRTS : MonoBehaviour
//    {
//        public float createDist = 2.25f;
//        [Range(0f, 3f)]
//        public float tolerance = 0.25f;

//        private Vector3 initPos;
//        private Quaternion initRot;
//        private Vector3 initLocalPos;
//        private Quaternion initLocalRot;
//        private Vector3 initScale;

//        private Vector3 currentPos;
//        private float currentDist;

//        private bool firstSpawn = true;

//        public bool rotateToggle = false;
//        public bool translateToggle = false;
//        public bool scaleToggle = false;

//        private InteractionBehaviour thisInteract;

//        private toolboxfollower Parent;
//        //private LeapRTSManager RTSManager;

//        private void Start()
//        {
//            firstSpawn = true;
//            initPos = gameObject.transform.position;
//            initRot = gameObject.transform.rotation;
//            initLocalPos = gameObject.transform.localPosition;
//            initLocalRot = gameObject.transform.localRotation;
//            initScale = gameObject.transform.localScale;
//            thisInteract = gameObject.GetComponent<InteractionBehaviour>();
//            Parent = gameObject.GetComponentInParent<toolboxfollower>();
//            RTSManager = GameObject.Find("RTS Anchor").GetComponent<LeapRTSManager>();
//        }

//        private void Update()
//        {
//            currentPos = gameObject.transform.localPosition;
//            currentDist = Vector3.Distance(initLocalPos, currentPos);
//            gameObject.transform.localScale = (1 + (currentDist / createDist)) * initScale;
//            if (gameObject.transform.localPosition == initLocalPos)
//            {
//                firstSpawn = true;
//                //thisInteract.enabled = true;
//            }


//            {
//                if (!thisInteract.isGrasped && currentPos != initPos)
//                {
//                    returnToInit();
//                }
//                if ((Mathf.Abs(currentDist - createDist)) <= tolerance && firstSpawn && Parent.spawnable())
//                {

//                    swapBool();
//                    firstSpawn = false;
//                    thisInteract.NotifyTeleported();
//                }
//            }
//        }

//        private void swapBool()
//        {
//            if (rotateToggle)
//            {
//                RTSManager.RotateModeSwap();
//            }
//            if (scaleToggle)
//            {
//                RTSManager.ScaleModeSwap();
//            }
//            returnToInit();
//        }

//        private void returnToInit()
//        {
//            gameObject.transform.localPosition = initLocalPos;
//            gameObject.transform.localRotation = initLocalRot;
//        }
//    }
//}