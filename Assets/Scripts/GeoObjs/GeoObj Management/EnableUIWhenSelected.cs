/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

//using System;
//using UnityEngine;
//using UnityEngine.Events;
//using System.Collections;
//using System.Collections.Generic;
//using Leap.Unity;
//using Leap.Unity.Attributes;


//namespace IMRE.HandWaver
//{
//	[RequireComponent (typeof(ProximityDetector))]
//	[RequireComponent (typeof(ExtendedFingerDetector))]
//	[RequireComponent (typeof(FingerDirectionDetector))]
//	[RequireComponent (typeof(UnityStandardAssets.Utility.FollowTarget))]

//	/** 
//		 * This class handles dynamic pointUI activation/deactivation by
//		 * spawning UIs only on points where the UI is about to be activated
//		 * and despawning UIs that are not activated.
//		 * 
//		 * 
//		 * When a the specified finger on the specified handmodel
//		 * is extended an event is triggered which this script listens for and
//		 * when recieved activates the proximity sensor which triggers an event
//		 * when the extended finger is near a point.  When that event is recieved
//		 * a finger direction detector is activated which triggers an event when the
//		 * finger is pointed at the point that triggered the proximity event.
//		 * When an extended finger event is triggered a pointUI is spawned on that point
//		 * When any detector in the series is deactivated (detectors call Deactivae()
//		 * when their activation conditions are no longer met) it deactivates all
//		 * downstream detectors.  This means that retracting the extended finger will 
//		 * deactivate the proximity and direction detectors.  Manipulating the detection
//		 * period in the editor has a large effect on the feel of this feature.
//		 */

/// <summary>
/// This script does ___.
/// The main contributor(s) to this script is __
/// Status: ???
/// </summary>
//	public class EnableUIWhenSelected : MonoBehaviour
//	{



//		//instance variables for event handlers
//		private UnityAction efActivateHandler;
//		private UnityAction pdActivateHandler;
//		private UnityAction fdActivateHandler;
//		private UnityAction efDeactivateHandler;
//		private UnityAction pdDeactivateHandler;
//		private UnityAction fdDeactivateHandler;

//		//instance variables for detectors
//		private ExtendedFingerDetector efd;
//		private ProximityDetector pd;
//		private FingerDirectionDetector fdd;

//		private UnityStandardAssets.Utility.FollowTarget follow;
//        [Tooltip("logs debug messages to console")]
//        public bool debug;

//        [Header ("Target Object Settings")]
//		[Tooltip ("tags for items to target")]
//		public List<string> targetTags;
//		//	[Tooltip ("the spawn pool for the UI object")]
//		//	public string spawnPoolName = "UI";
//		//	[Tooltip ("the prefab pool for the UI object")]
//		//	public string prefabPoolName = "PointUI";

//		//[Tooltip ("the UI to place on target objects")]
//		//public Transform tUI;
//		//ui for objects of the specified tag


//		/*optionss used for child detector objects*/
//		[Header ("Global Detector Settings")]
//		[Tooltip ("The finger to watch.")]
//		public Transform Finger;
//		[Tooltip ("The hand model to watch.")]
//		public IHandModel HandModel = null;
//		[Header ("Proximity Detector Settings")]
//		[Units ("seconds")]
//		[MinValue (0)]
//		[Tooltip ("The interval in seconds at which to check this detector's proximity conditions.")]
//		public float ProximityPeriod = .1f;
//		[Header ("Distance Settings")]
//		[Tooltip ("The target distance in meters to activate the detector.")]
//		[MinValue (0)]
//		public float OnDistance = .01f;
//		[Tooltip ("The distance in meters at which to deactivate the detector.")]
//		public float OffDistance = .015f;
//		[Header ("Extended Finger Detector Settings")]
//		[Tooltip ("The interval in seconds at which to check this detector's conditions.")]
//		[Units ("seconds")]
//		[MinValue (0)]
//		public float ExtendedFingerPeriod = .1f;
//		[Header ("Finger States")]
//		[Tooltip ("Required state of the thumb.")]
//		public PointingState Thumb = PointingState.Either;
//		[Tooltip ("Required state of the index finger.")]
//		public PointingState Index = PointingState.Extended;
//		[Tooltip ("Required state of the middle finger.")]
//		public PointingState Middle = PointingState.NotExtended;
//		[Tooltip ("Required state of the ring finger.")]
//		public PointingState Ring = PointingState.NotExtended;
//		[Tooltip ("Required state of the little finger.")]
//		public PointingState Pinky = PointingState.NotExtended;
//		[Header ("Min and Max Finger Counts")]
//		[Range (0, 5)]
//		[Tooltip ("The minimum number of fingers extended.")]
//		public int MinimumExtendedCount = 0;
//		[Range (0, 5)]
//		[Tooltip ("The maximum number of fingers extended.")]
//		public int MaximumExtendedCount = 5;
//		[Header ("Finger Direction Detector Settings")]
//		[Units ("seconds")]
//		[Tooltip ("The interval in seconds at which to check this detector's conditions.")]
//		[MinValue (0)]
//		public float DirectionPeriod = .1f;
//		[Tooltip ("The finger to observe.")]
//		public Leap.Finger.FingerType FingerName = Leap.Finger.FingerType.TYPE_INDEX;
//		[Tooltip ("The angle in degrees from the target direction at which to turn on.")]
//		[Range (0, 180)]
//		public float OnAngle = 15f;
//		[Tooltip ("The angle in degrees from the target direction at which to turn off.")]
//		[Range (0, 180)]
//		public float OffAngle = 25f;

//		public EnableUIWhenSelected ()
//		{
//			//nothing to do in constructor
//		}

//        /// <summary>
//        /// applies settings fron editor to detectors
//        /// </summary>
//        public void Awake ()
//		{


//			this.follow = this.gameObject.GetComponent<UnityStandardAssets.Utility.FollowTarget> ();
//			this.efd = this.gameObject.GetComponent<ExtendedFingerDetector> ();
//			this.pd = this.gameObject.GetComponent<ProximityDetector> ();
//			this.fdd = this.gameObject.GetComponent<FingerDirectionDetector> ();
//			this.efd.enabled = false;
//			this.pd.enabled = false;
//			this.fdd.enabled = false;

//			this.efActivateHandler = new UnityAction (this.extendedFingerActivationEvent);
//			this.pdActivateHandler = new UnityAction (this.proximityActivationEvent);
//			this.fdActivateHandler = new UnityAction (this.directionActivationEvent);
//			this.efDeactivateHandler = new UnityAction (this.extendedFingerDeactivationEvent);
//			this.pdDeactivateHandler = new UnityAction (this.proximityDeactivationEvent);
//			this.fdDeactivateHandler = new UnityAction (this.directionDeactivationEvent);
//		}

//		public void Start ()
//		{
//			this.follow.target = this.Finger;
//			this.follow.offset = new Vector3 (0f, 0f, 0f);
//			this.follow.enabled = true;
//			this.applyExtendedFingerDetectorSettings ();
//			this.applyProximityDetectorSettings ();
//			this.applyFingerDirectionDetectorSettings ();
//			this.efd.enabled = true;
//		}


//		private void applyProximityDetectorSettings ()
//		{

//			//this.pd.OnActivate = null; //todo
//			//this.pd.OnDeactivate = null;//todo 
//			this.pd.OnDistance = this.OnDistance;
//			this.pd.OffDistance = this.OffDistance;
//			this.refreshProximityTargets ();//apply target settings
//			this.pd.OnActivate.AddListener (this.pdActivateHandler);
//			this.pd.OnDeactivate.AddListener (this.pdDeactivateHandler);
//		}


//		private void applyExtendedFingerDetectorSettings ()
//		{
//			this.efd.HandModel = this.HandModel;
//			this.efd.Period = this.ExtendedFingerPeriod;
//			this.efd.Thumb = this.Thumb;
//			this.efd.Index = this.Index;
//			this.efd.Middle = this.Middle;
//			this.efd.Ring = this.Ring;
//			this.efd.Pinky = this.Pinky;
//			this.efd.MinimumExtendedCount = this.MinimumExtendedCount;
//			this.efd.MaximumExtendedCount = this.MaximumExtendedCount;
//			this.efd.OnActivate.AddListener (this.efActivateHandler);
//			this.efd.OnDeactivate.AddListener (this.efDeactivateHandler);
//		}

//		private void applyFingerDirectionDetectorSettings ()
//		{
//			this.fdd.Period = this.DirectionPeriod;
//			this.fdd.HandModel = this.HandModel;  
//			this.fdd.FingerName = this.FingerName;
//			this.fdd.PointingType = PointingType.AtTarget;
//			this.fdd.TargetObject = null; //this gets set at runtime via ProximityEvent
//			this.fdd.OnAngle = this.OnAngle;
//			this.fdd.OffAngle = this.OffAngle;
//			this.fdd.OnActivate.AddListener (this.fdActivateHandler);
//			this.fdd.OnDeactivate.AddListener (this.fdDeactivateHandler);

//		}
//        /// <summary>
//        /// refresh the list of targets the proximity detector is looking for
//        ///per ProximityDetecor 4.1.3 docs objects are not added dynamically via tag so we have to get objects and update the list manually rather than specify by tag
//        /// </summary>
//        private void refreshProximityTargets ()
//		{
//			if (this.targetTags != null) {
//				List<GameObject> targets = new List<GameObject> ();
//				foreach (string t in this.targetTags) {
//					targets.AddRange (GameObject.FindGameObjectsWithTag (t));
//				}
//				//per ProximityDetecor 4.1.3 docs objects are not added dynamically via tag
//				//so we have to get objects and update the list manually rather than specify by tag
//				this.pd.TargetObjects = targets.ToArray ();
//			}
//		}


//        /// <summary>
//        /// enables proximity detector
//        /// </summary>
//		public void extendedFingerActivationEvent ()
//		{
//			this.refreshProximityTargets ();
//			this.pd.enabled = true;
//            if (this.debug) { Debug.Log("finger extended, enabling proximity detector with " + this.pd.TargetObjects.Length + " targets"); }
//		}

//        /// <summary>
//        /// deacticates proximity detector
//        /// </summary>
//		public void extendedFingerDeactivationEvent ()
//		{
//            if (this.debug)
//            { Debug.Log("finger retracted, disabling proximity detector"); }
//			if (this.pd.IsActive) {
//				this.pd.Deactivate ();
//			}
//			this.pd.enabled = false;
//		}

//        /// <summary>
//        /// activates finger direction detector
//        /// </summary>
//		public void proximityActivationEvent ()
//		{
//            if (this.debug)
//            { Debug.Log("a point is nearby"); }
//			this.fdd.TargetObject = this.pd.CurrentObject.transform; 
//			this.fdd.enabled = true;
//		}
//        /// <summary>
//        /// deactivates direction detector 
//        /// </summary>
//		public void proximityDeactivationEvent ()
//		{
//            if (this.debug)
//            { Debug.Log("a point is no longer nearby"); }
//			if (this.fdd.IsActive) {//deactivate the direction detector if active
//				this.fdd.Deactivate ();
//			}
//			this.fdd.enabled = false;
//		}

//        /// <summary>
//        /// spawns ui on point
//        /// </summary>
//		public void directionActivationEvent ()
//		{
//            if (this.debug)
//            { Debug.Log("an extended finger is pointing at a neaby point"); }
//			//(this.fdd.TargetObject.GetComponent<AbstractPoint> ()).spawnUI ();
//		}

//        /// <summary>
//        /// despawns ui on point
//        /// </summary>
//		public void directionDeactivationEvent ()
//		{
//            if (this.debug)
//            { Debug.Log("an extended finger is no longer pointing at a neaby point"); }
//			//(this.fdd.TargetObject.GetComponent<AbstractPoint> ()).despawnUI ();
//		}




//	}

//}
