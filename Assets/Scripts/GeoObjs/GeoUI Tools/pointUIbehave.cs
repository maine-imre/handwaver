/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Leap.Unity;
//using Leap.Unity.DetectionExamples;
//using Leap.Unity.Interaction;
//using PathologicalGames;

//namespace IMRE.HandWaver
//{
/// <summary>
/// This script does ___.
/// The main contributor(s) to this script is __
/// Status: ???
/// </summary>
//    public class pointUIbehave : MonoBehaviour
//    {

//        public Transform point;

//        public Transform switchLabel;

//        public Transform xSwitch;
//        public Transform ySwitch;
//        public Transform zSwitch;

//        public Transform xRotSwitch;
//        public Transform yRotSwitch;
//        public Transform zRotSwitch;

//        public Transform hueSlider;
//        public Transform scaleSlider;

//        public Transform follower1;  // Camden, these need a more descriptive name
//        public Transform follower2; // I think this is the pointUI menu

//        void Update()
//        {
//            if ((point == null) || (point.gameObject.activeSelf == false))
//            {
//                try
//                {
//                    PoolManager.Pools["UI"].Despawn(this.transform);
//                }
//                catch
//                {
//                    this.gameObject.SetActive(false);
//                }
//            }
//            else
//            {
//                switchLabel.GetComponent<HoverItemDataText>().Label = point.transform.position.ToString();
//            }
//        }
//		/** returns if the pointUI menu is active (calls follower2.IsAnimating) */
//		public bool MenuIsAnimating()
//		{
//			return this.follower2.GetComponent<VertexMenuScale> ().IsAnimating;

//		}
//        // Use this for initialization
//        public void Init()
//        {
//            follower1.GetComponent<FollowTransform>().FollowTx = point.transform;
//            follower1.GetComponent<FollowTransform>().enabled = true;
//            follower1.GetComponent<LookAtTransform>().enabled = true;
//            follower2.GetComponent<FollowTransform>().FollowTx = point.transform;
//            follower2.GetComponent<FollowTransform>().enabled = true;
//            follower2.GetComponent<LookAtTransform>().enabled = true;
//            follower2.GetComponent<SimVertexEventHandler>().interact = point.GetComponent<InteractionBehaviour>();

//            point.GetComponent<InteractablePoint>().xSwitch = xSwitch;
//            point.GetComponent<InteractablePoint>().ySwitch = ySwitch;
//            point.GetComponent<InteractablePoint>().zSwitch = zSwitch;

//            point.GetComponent<InteractablePoint>().xRotSwitch = xRotSwitch;
//            point.GetComponent<InteractablePoint>().yRotSwitch = yRotSwitch;
//            point.GetComponent<InteractablePoint>().zRotSwitch = zRotSwitch;

//            point.GetComponent<InteractablePoint>().hueSlider = hueSlider;
//            point.GetComponent<InteractablePoint>().scaleSlider = scaleSlider;

//            moveRestrict();
//        }

//        public void moveRestrict()
//        {
//            point.GetComponent<InteractablePoint>().moveRestrict();
//        }

//    }
//}
