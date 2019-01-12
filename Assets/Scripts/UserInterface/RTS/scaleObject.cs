/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Leap.Unity;

//namespace IMRE.HandWaver
//{

//    public class scaleObject : MonoBehaviour
//    {

//        public Transform handWorldMenu;

//        public Transform leftHand;
//        public Transform rightHand;

//        public Transform leftPinch;
//        public Transform rightPinch;

//        public Transform line;
//        private Transform head;

//        private float dist = 0;
//        private float oldDist = 0;
//        private float scale = 0;
//        private Vector3 vectorScale;
//        private float distanceFromObject = 0.0f;


//        public bool maxSizeLimit = true;
//        public bool scaleMode = false;
//        public bool rotateMode = false;
//        public bool translateMode = true;
//        public bool inverseRotate = true;
//        public bool leapMode = false;

//        private Quaternion oldRotation;
//        private Quaternion newRotation;

//        private Vector3 newPosDiff;
//        private Vector3 oldPosDiff;

//        private Vector3 newMeanPos;
//        private Vector3 oldMeanPos;


//        void Awake()
//        {
//            head = GameObject.Find("CenterEyeAnchor").transform;

//            GameObject lineGO = Instantiate(Resources.Load("prefabs/ScalingLine", typeof(GameObject))) as GameObject;
//            line = lineGO.transform;

//            this.GetComponent<LeapRTS>().PinchDetectorA = leftPinch.GetComponent<PinchDetector>();	//LeapRTS (use riggedHands.cs), Pinch Detector (now built into riggedHands)are no longer used 
//            this.GetComponent<LeapRTS>().PinchDetectorB = rightPinch.GetComponent<PinchDetector>();

//            //handWorldMenu = GameObject.Find ("WorldRow").transform;
//        }

//        void Start()
//        {
//            setRestraints();

//            //handWorldMenu.GetComponent<worldManipulationManager>().addTransform(this.transform);
//        }

//        public void setRestraints()
//        {
//            if (leapMode)
//            {
//                this.GetComponent<LeapRTS>().enabled = true;
//                maxSizeLimit = false;
//                scaleMode = false;
//                rotateMode = false;
//                translateMode = false;
//                inverseRotate = false;
//            }
//            else
//            {
//                this.GetComponent<LeapRTS>().enabled = false;
//            }
//        }

//        void Update()
//        {
//            linePosition();
//            getInput();
//        }

//        void getInput()
//        {
//            if (leftHand.GetComponent<ControlBehave>().gripPressDown ^ rightHand.GetComponent<ControlBehave>().gripPressDown)					//icontroller is the replacement for controlbvehave
//            {
//                oldDist = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
//                oldRotation = this.transform.rotation;
//                oldPosDiff = leftHand.GetComponent<Transform>().position - rightHand.GetComponent<Transform>().position;
//                oldMeanPos = .5f * (leftHand.GetComponent<Transform>().position + rightHand.GetComponent<Transform>().position);
//                vectorScale = this.transform.localScale;
//            }
//            else
//            {
//                line.transform.GetComponent<LineRenderer>().enabled = false;
//            }

//            if (leftHand.GetComponent<ControlBehave>().gripPressDown && rightHand.GetComponent<ControlBehave>().gripPressDown)
//            {
//                dist = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
//                newRotation = quatFromVec(oldPosDiff, newPosDiff);
//                scaleObjectFuction();
//                rotateObjectFuction();
//                translateObjectFunction();
//            }

//        }

//        void linePosition()
//        {
//            line.GetComponent<LineRenderer>().SetPosition(0, leftHand.transform.position);
//            line.GetComponent<LineRenderer>().SetPosition(1, rightHand.transform.position);
//            newPosDiff = leftHand.GetComponent<Transform>().position - rightHand.GetComponent<Transform>().position;
//            calcDistance();
//        }

//        void calcDistance()
//        {
//            scale = (dist / oldDist);
//        }

//        void scaleObjectFuction()
//        {
//            bool distanceFromObject = (3.0f * Vector3.Distance(this.transform.position, head.transform.position)) > Vector3.Magnitude(this.transform.localScale);

//            if ((distanceFromObject || scale < 1) || !maxSizeLimit)
//            {

//                line.transform.GetComponent<LineRenderer>().enabled = true;
//                if (scaleMode && !rotateMode && !translateMode)
//                {
//                    this.transform.localScale = scale * vectorScale;
//                }

//            }
//        }

//        void rotateObjectFuction()
//        {
//            if (rotateMode && !scaleMode && !translateMode)
//            {
//                if (inverseRotate)
//                {
//                    this.transform.rotation = oldRotation * Quaternion.Inverse(Quaternion.FromToRotation(oldPosDiff, newPosDiff));
//                }
//                else
//                {
//                    this.transform.rotation = oldRotation * Quaternion.FromToRotation(oldPosDiff, newPosDiff);
//                }

//            }
//        }

//        void translateObjectFunction()
//        {
//            if (translateMode && !scaleMode && !rotateMode)
//            {
//                newMeanPos = .5f * (leftHand.GetComponent<Transform>().position + rightHand.GetComponent<Transform>().position);
//                Vector3 translateDelta = newMeanPos - oldMeanPos;
//                translateDelta = translateDelta.x * Vector3.left + translateDelta.y * Vector3.up + translateDelta.z * Vector3.down;
//                this.transform.Translate(translateDelta);
//            }
//        }

//        static Quaternion quatFromVec(Vector3 u, Vector3 v)
//        {
//            float m = Mathf.Sqrt(2.0f + 2.0f * Vector3.Dot(u, v));
//            Vector3 w = (1.0f / m) * Vector3.Cross(u, v);
//            Quaternion quat = new Quaternion(0.5f * m, w.x, w.y, w.z);
//            return quat;
//        }
//    }
//}