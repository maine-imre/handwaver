using Leap.Unity;
using Leap.Unity.Attachments;
using Leap.Unity.Interaction;
/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace IMRE.HandWaver
{

    [System.Serializable]

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public struct leapHands
    {
        //these are public so that we can set them in the editor.
        public PolyHand lRhand;
        public PolyHand rRhand;
        public InteractionHand lIhand;
        public InteractionHand rIhand;
        public AttachmentHand lAhand;
        public AttachmentHand rAhand;
		public InteractionXRController lOVR;
		public InteractionXRController rOVR;


		public PolyHand Lhand_rigged
        {
            get
            {
                return lRhand;
            }

            set
            {
                lRhand = value;
            }
        }

        public PolyHand RHand_rigged
        {
            get
            {
                return rRhand;
            }

            set
            {
                rRhand = value;
            }
        }

        public InteractionHand LHand_interaction
        {
            get
            {
                return lIhand;
            }

            set
            {
                lIhand = value;
            }
        }

        public InteractionHand RHand_interaction
        {
            get
            {
                return rIhand;
            }

            set
            {
                rIhand = value;
            }
        }

        public AttachmentHand LHand_attachment
        {
            get
            {
                return lAhand;
            }

            set
            {
                lAhand = value;
            }
        }

        public AttachmentHand RHand_attachment
        {
            get
            {
                return rAhand;
            }

            set
            {
                rAhand = value;
            }
        }

		public InteractionXRController LOVR
		{
			get
			{
				return lOVR;
			}

			set
			{
				lOVR = value;
			}
		}

		public InteractionXRController ROVR
		{
			get
			{
				return rOVR;
			}

			set
			{
				rOVR = value;
			}
		}
	}

    [System.Serializable]
    public struct leapHandsData
    {

        public System.DateTime currTime;

        //left hand related data
        public fingerData[] LHandData;
        public Vector3 LHandPalmPos;
        public Quaternion LHandPalmRot;
        /// <summary>
        /// value between 0 and 1. represents percentage the fingers are pinched.
        /// </summary>
        public float LHandPinchvalue;

        //right hand related data
        public fingerData[] RHandData;
        public Vector3 RHandPalmPos;
        public Quaternion RHandPalmRot;
        /// <summary>
        /// value between 0 and 1. represents percentage the fingers are pinched.
        /// </summary>
        public float RHandPinchvalue;

        //head related data
        public Vector3 headPos;
        public Quaternion headRot;
    }

    public class leapHandDataLogger : MonoBehaviour
    {

        public static leapHandDataLogger ins;
		private string sessionID = IMRE.HandWaver.HWIO.XMLManager.sessionID;

		public leapHands currHands;
        public leapHandsData currHandsData;

		private void Awake()
        {
            ins = this;

            if (currHands.Lhand_rigged == null || currHands.RHand_rigged == null || currHands.LHand_interaction == null || currHands.RHand_interaction == null || currHands.LHand_attachment == null || currHands.RHand_attachment == null)
            {
                PolyHand[] rhands = FindObjectsOfType<PolyHand>();
                InteractionHand[] ihands = FindObjectsOfType<InteractionHand>();
                AttachmentHand[] ahands = FindObjectsOfType<AttachmentHand>();

                foreach (PolyHand hand in rhands)
                {
                    switch (hand.Handedness)
                    {
                        case Chirality.Left:
                            currHands.Lhand_rigged = hand;
                            break;
                        case Chirality.Right:
                            currHands.RHand_rigged = hand;
                            break;
                        default:
                            break;
                    }
                }

                foreach (InteractionHand hand in ihands)
                {
                    switch (hand.handDataMode)
                    {
                        case HandDataMode.PlayerLeft:
                            currHands.LHand_interaction = hand;
                            break;
                        case HandDataMode.PlayerRight:
                            currHands.RHand_interaction = hand;
                            break;
                        case HandDataMode.Custom:
                            break;
                        default:
                            break;
                    }
                }
                foreach (AttachmentHand hand in ahands)
                {
                    switch (hand.chirality)
                    {
                        case Chirality.Left:
                            currHands.LHand_attachment = hand;
                            break;
                        case Chirality.Right:
                            currHands.RHand_attachment = hand;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (currHands.Lhand_rigged == null || currHands.RHand_rigged == null || currHands.LHand_interaction == null || currHands.RHand_interaction == null || currHands.LHand_attachment == null || currHands.RHand_attachment == null)
            {
                Debug.LogWarning("Hand References improperly set within leapHandDataLogger");
                this.enabled = false;
            }
            initHandsData();


			checkLog();

			commandLineArgumentParse.logStateChange.AddListener(checkLog);
			
        }

		private void checkLog() {
#if !UNITY_EDITOR
			if (commandLineArgumentParse.logCheck())
			{
				StartCoroutine(logHandData());
			}
#endif

		}


		private void initHandsData()
        {
            currHandsData = new leapHandsData();                //Instanciates a new one

            currHandsData.LHandData = new fingerData[5];        //Sets up array for left hand finger data structs
            currHandsData.RHandData = new fingerData[5];        //right hand finger data structs

        }


        private IEnumerator logHandData()
        {
            if (!Directory.Exists(Application.dataPath + @"/../dataCollection/"))
            {
                Directory.CreateDirectory(Application.dataPath + @" /../dataCollection/");
            }

            while (true)
            {

                yield return new WaitForSecondsRealtime(0.33f);
                LogLeapHandData(Application.dataPath + @"/../dataCollection/LeapHandDataLog" + sessionID + @".hwlog");
            }
        }

        private void LogLeapHandData(string path)
        {
            logData();
            using (FileStream stream = File.Open(path, FileMode.Append))
            {

                XmlSerializer serializer = new XmlSerializer(typeof(leapHandsData));
                serializer.Serialize(stream, leapHandDataLogger.ins.currHandsData);
            }
        }

        public void logData()
        {

            if (!(currHands.Lhand_rigged.fingers.Any(f => ((int)f.fingerType == -1)) || currHands.RHand_rigged.fingers.Any(f => ((int)f.fingerType == -1))))
            //if tracking all fingers properly
            //this is due to cases attempting to access array element 0
            {
                if (currHands.Lhand_rigged.IsTracked)
                {
                    foreach (RiggedFinger riggedFinger in currHands.Lhand_rigged.fingers)
                    {
                        currHandsData.LHandData[(int)riggedFinger.fingerType] = new fingerData(riggedFinger/*, currHands.LHand.GetLeapHand().Finger((int)riggedFinger.fingerType)*/);

                    }
                    currHandsData.LHandPinchvalue = currHands.Lhand_rigged.GetLeapHand().PinchStrength;
                    currHandsData.LHandPalmPos = currHands.Lhand_rigged.palm.position;
                    currHandsData.LHandPalmRot = currHands.Lhand_rigged.palm.rotation;
                }

                if (currHands.RHand_rigged.IsTracked)
                {
                    foreach (RiggedFinger riggedFinger in currHands.RHand_rigged.fingers)
                    {
                        currHandsData.RHandData[(int)riggedFinger.fingerType] = new fingerData(riggedFinger);

                    }
                    currHandsData.RHandPinchvalue = currHands.RHand_rigged.GetLeapHand().PinchStrength;
                    currHandsData.RHandPalmPos = currHands.RHand_rigged.palm.position;
                    currHandsData.RHandPalmRot = currHands.RHand_rigged.palm.rotation;
                }

                //head related
                currHandsData.headPos = Camera.main.gameObject.transform.position;
                currHandsData.headRot = Camera.main.gameObject.transform.rotation;

                //metadata collection
                currHandsData.currTime = System.DateTime.Now;
            }
        }
    }



    [System.Serializable]
    public class fingerData
    {
        public fingerData() { }

        private Ray pointRay;
        [XmlAttribute]
        public Leap.Finger.FingerType type;
        public Vector3 tipPosition;
        public Vector3 pointDirection;
        //public bool isExtended; bRoke for now

        public fingerData(RiggedFinger riggedFinger/*, Finger finger*/)
        {
            this.type = riggedFinger.fingerType;
            this.pointRay = riggedFinger.GetRay();
            this.tipPosition = pointRay.origin;
            this.pointDirection = pointRay.direction;
            //this.isExtended = finger.IsExtended;
        }


        /// <summary>
        /// 	TYPE_UNKNOWN = -1,
        /// 	TYPE_THUMB = 0,
        ///		TYPE_INDEX = 1,
        ///		TYPE_MIDDLE = 2,
        ///		TYPE_RING = 3,
        ///		TYPE_PINKY = 4
        ///		As discribed from comments in "<see cref="Leap.Finger.Type"/>" comments.
        /// </summary>
        public int fingerIdx
        {
            get
            {
                return (int)type;
            }
        }
    }
}