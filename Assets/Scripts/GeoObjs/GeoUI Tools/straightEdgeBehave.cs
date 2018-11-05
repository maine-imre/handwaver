/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using PathologicalGames;
using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class straightEdgeBehave : MasterGeoObj
	{
#pragma warning disable 0649

		public shipWheelOffStraightedge.wheelType wheelType;


        public Transform seHandle1;
        public Transform seHandle2;

        public Transform shipsWheel_rotate;
		public Transform shipsWheel_revolve;
		public Transform shipsWheel_hoist;
        //public HingeJoint shipWC;
		//public SphereCollider shipWCollider;

        private CapsuleCollider capsule;

        public bool spindle = false;

		private AnchorableBehaviour thisAbehave;
		private InteractionBehaviour thisIbehave;
#pragma warning restore 0649

		new void Start()
		{
			base.Start();
			thisAbehave = GetComponent<AnchorableBehaviour>();
			thisAbehave.OnDetachedFromAnchor += detach;
			thisAbehave.OnAttachedToAnchor += attach;
		}

		private void detach()
		{
			capsule.radius = this.GetComponent<LineRenderer>().startWidth * 2.0f;
		}

		private void attach()
		{
			capsule.radius = 65;
		}

		public shipWheelOffStraightedge.wheelType WheelType
        {
            get
            {
                return wheelType;
            }

            set
            {
                wheelType = value;
                spindleToggle(value);
            }
        }

		internal void snapToFloor()
		{
			normalDir = Vector3.up;
		}

		//private int initLayer;

		// Use this for initialization
		internal new void OnSpawned()
        {
			base.OnSpawned();

			capsule = GetComponent<CapsuleCollider>();
            this.GetComponent<LineRenderer>().positionCount = 4;

            Vector3[] positions = new Vector3[4];
            positions[1] = seHandle1.transform.position;
            positions[2] = seHandle2.transform.position;

            positions[0] = positions[2] - 1000 * (positions[2] - positions[1]);
            positions[3] = positions[2] + 1000 * (positions[2] - positions[1]);

            this.GetComponent<LineRenderer>().SetPositions(positions);


            //capsule.isTrigger = false;
            capsule.radius = this.GetComponent<LineRenderer>().startWidth * 2.0f;
            capsule.height = 100f;
            capsule.direction = 2;

			thisIbehave = gameObject.GetComponent<InteractionBehaviour>();
			//initLayer = gameObject.layer;

			shipsWheel_rotate.gameObject.SetActive(false);
			shipsWheel_revolve.gameObject.SetActive(false);
			shipsWheel_hoist.gameObject.SetActive(false);

			gameObject.GetComponent<CapsuleCollider>().enabled = true;

			//thisIbehave.OnGraspEndEvent += onGraspEnd;
		}

		// Update is called once per frame
		void Update()
        {
            if (spindle)
            {
                shipsWheel_hoist.transform.rotation = Quaternion.FromToRotation(Vector3.right, normalDir);
				shipsWheel_revolve.transform.rotation = Quaternion.FromToRotation(Vector3.right, normalDir);
				shipsWheel_rotate.transform.rotation = Quaternion.FromToRotation(Vector3.right, normalDir);
			}
        }

		private void OnTriggerEnter(Collider other)
		{
			//base.OnTriggerEnter(other);
			if (other.GetComponent<shipWheelOffStraightedge>() != null && !(other.GetComponent<AnchorableBehaviour>().isAttached))
				WheelType = other.GetComponent<shipWheelOffStraightedge>().thisWheelType;
		}

		private void spindleToggle(shipWheelOffStraightedge.wheelType wheelType)
        {
			Transform shipsWheel = null;
			Transform[] wheelsArray = new Transform[3] { shipsWheel_revolve, shipsWheel_hoist, shipsWheel_rotate };
			switch (wheelType)
			{
				case shipWheelOffStraightedge.wheelType.revolve:
					shipsWheel = shipsWheel_revolve;
					break;
				//the hoist case should be depreciated.
				case shipWheelOffStraightedge.wheelType.hoist:
					shipsWheel = shipsWheel_hoist;
					break;
                case shipWheelOffStraightedge.wheelType.rotate:
                    shipsWheel = shipsWheel_rotate;
					break;
                case shipWheelOffStraightedge.wheelType.off:
                    spindleOff();
                    return;
            }
			foreach(Transform wheel in wheelsArray)
			{
				wheel.gameObject.SetActive(false);
			}
			shipsWheel.gameObject.SetActive(true);
			//shipWCollider.enabled = true;
			//gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spindle = true;

            shipsWheel.transform.position = center;
            shipsWheel.transform.rotation = Quaternion.FromToRotation(Vector3.forward, normalDir);
        }

		void spindleOff()
		{
			shipsWheel_hoist.gameObject.SetActive(false);
			shipsWheel_revolve.gameObject.SetActive(false);
			shipsWheel_rotate.gameObject.SetActive(false);
			//shipWCollider.enabled = true;
			gameObject.GetComponent<CapsuleCollider>().enabled = true;

			spindle = false;
		}

        /// <summary>
        /// The direction vector
        /// </summary>
        /// <returns></returns>
        public Vector3 normalDir
        {
            get
            {
                return (seHandle1.transform.position - seHandle2.transform.position);
            }
			set
			{
				this.transform.rotation *= Quaternion.FromToRotation(normalDir,value);
			}
        }

        public Vector3 center
        {
            get
            {
                Vector3 result = (seHandle1.transform.position + seHandle2.transform.position) / 2f;
                return result;
            }
        }

        internal override void snapToFigure(MasterGeoObj toObj)
		{
			//throw new NotImplementedException();
		}

		internal override void glueToFigure(MasterGeoObj toObj)
		{
			throw new NotImplementedException();
		}

		public override void initializefigure()
		{
			//do nothing
		}

		internal override bool rMotion(NodeList<string> inputNodeList)
		{
			return false;
		}

		public override void updateFigure()
		{
			//do nothing
		}

		public override void Stretch(InteractionController obj)
		{
			//do nothing
		}
	}
}