/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using IMRE.HandWaver.Solver;


namespace IMRE.HandWaver
{
	public enum GeoObjType { point, line, polygon, prism, pyramid, circle, sphere, revolvedsurface, torus, flatface, straightedge, none };

	public enum GeoObjDef { Abstract, Dependent, Interactable, Static, none};

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	abstract class MasterGeoObj : MonoBehaviour, UpdatableFigure
	{

        #region Local Positions
        private Vector3 _position3;
		private Vector3 actualPos;
        private Quaternion _rotation3;
        private float _scale;

        internal Vector3 LocalPosition(Vector3 systemPosition)
        {
            return HW_GeoSolver.ins.localPosition(systemPosition);
        }

        internal Vector3 systemPosition(Vector3 localPosition)
        {
            return HW_GeoSolver.ins.systemPosition(localPosition);
        }

        public Vector3 Position3
        {
            get
            {
                if(_position3 == Vector3.zero || actualPos != transform.position)
                {
                    Position3 = systemPosition(this.transform.position);
					actualPos = transform.position;
                }

                return _position3;
            }

            set
            {
                _position3 = value;
                this.transform.position = LocalPosition(_position3);
				actualPos = transform.position;
			}
		}
        #endregion

        public bool allowDelete = true;

		public GeoObjType figType;

        internal int intersectionMultipleIDX;

        public bool intersectionFigure = false;

        public MasterGeoObj setIntersectionFigure(int value)
        {
            intersectionMultipleIDX = value;
            intersectionFigure = true;
            return this;
        }

        internal Color figColor
        {
            set
            {
                switch (figType)
                {
                    case GeoObjType.point:
                        GetComponent<MeshRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.line:
                        GetComponent<LineRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.polygon:
                        GetComponent<MeshRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.prism:
                        break;
                    case GeoObjType.pyramid:
                        break;
                    case GeoObjType.circle:
                        GetComponent<LineRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.sphere:
                        GetComponent<MeshRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.revolvedsurface:
                        GetComponent<MeshRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.torus:
                        GetComponent<MeshRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.flatface:
                        GetComponent<MeshRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.straightedge:
                        GetComponent<LineRenderer>().materials[0].color = value;
                        break;
                    case GeoObjType.none:
                        break;
                }
            }
        }
		/// <summary>
		/// Used to save/load and keep geoObj definition type
		/// </summary>
		/// 
		public enum SelectionStatus { selected, active, canidate, none }
		internal SelectionStatus thisSelectStatus
		{
			get
			{
				return _thisSelectStatus;
			}

			set
			{
				Material mat = null;
				switch (value)
				{
					case SelectionStatus.selected:
						mat = HW_GeoSolver.ins.selectedMaterial;
						break;
					case SelectionStatus.active:
						mat = HW_GeoSolver.ins.activeMaterial;
						break;
					case SelectionStatus.canidate:
						mat = HW_GeoSolver.ins.canidateMaterial;
						break;
					case SelectionStatus.none:
						mat = StandardMaterial;
						break;
				}
				if (GetComponent<MeshRenderer>() != null)
				{
					GetComponent<MeshRenderer>().material = mat;
				}
				if (GetComponent<LineRenderer>() != null)
				{
					GetComponent<LineRenderer>().material = mat;
				}
				_thisSelectStatus = value;
			}
		}

		private SelectionStatus _thisSelectStatus;

		public string figName;
		public int figIndex;

		private InteractionBehaviour _thisIBehave;
        internal InteractionBehaviour thisIBehave {
			get
			{
				if(_thisIBehave == null)
				{
					_thisIBehave = GetComponent<InteractionBehaviour>();
				}
				return _thisIBehave;
			}
		}
		private IEnumerator cUpdateRMan;
		public IEnumerator waitForStretch;
		public bool stretchEnabled = true;

		private bool _leapInteraction;
#pragma warning disable 0169

		private bool isSelected;

		private Component halo;

		internal bool interesectionFigure;
		private Material _standardMaterial;
		private string _label;
#pragma warning disable 0169

		internal Material StandardMaterial
		{
			get
			{
				return _standardMaterial;
			}

			set
			{
				_standardMaterial = value;
				thisSelectStatus = thisSelectStatus;
			}
		}

		[ContextMenu("Display Selection Status")]
		public void displaySelectionStatus()
		{
			Debug.Log(gameObject.name+"'s selection status is set to "+thisSelectStatus);
		}

		public bool IsSelected
		{
			get
			{
				return thisSelectStatus == MasterGeoObj.SelectionStatus.selected;
			}

			set
			{
				if (value)
				{
					thisSelectStatus = MasterGeoObj.SelectionStatus.selected;
				}
				else
				{
					thisSelectStatus = MasterGeoObj.SelectionStatus.none;
				}
			}
		}

		public bool leapInteraction
		{
			get
			{
				return _leapInteraction;
			}

			set
			{
				if (value)
				{
					StartCoroutine(enableLeapInteraction());
				}
				else
				{
					StartCoroutine(disableLeapInteraction());
				}
				_leapInteraction = value;
			}
		}

		public string label
		{
			get
			{
				return _label;
			}

			set
			{
				_label = value;
			}
		}

        public void OnSpawned()
		{
			HW_GeoSolver.ins.addComponent(this);
		}

		public void Start()
		{
			if (this.GetComponent<Renderer>() != null)
			{
				_standardMaterial = GetComponent<Renderer>().material;
			}
			_thisSelectStatus = MasterGeoObj.SelectionStatus.none;

			if (this.GetComponent("Halo")!= null)
			{
				halo = this.GetComponent("Halo");
			}


			//if (this.GetComponent<InteractionBehaviour>() != null)
			//{
				//this.thisIBehave = this.GetComponent<InteractionBehaviour>();
				thisIBehave.OnGraspBegin += StartInteraction;
				thisIBehave.OnPerControllerGraspBegin += Stretch;
				thisIBehave.OnGraspEnd += EndInteraction;
			//}
			cUpdateRMan = updateRMan();
			waitForStretch = WaitForStretch();
		}

        void LateUpdate()
        {
            //consider moving this function to the reaction system instead of LateUpdate.

            //check for the existance of the dependicies of this object
            if (figType != GeoObjType.point && figType != GeoObjType.none &&  figType != GeoObjType.straightedge && figType != GeoObjType.flatface)
            {
                HW_GeoSolver.ins.checkLifeRequirements(this);
            }
            //if the object has been ordered to be destroyed by user or dependent objects have been destroyed, destroy this gameObject.
        }

        void toggleHighlight(bool isEnabled) //This allows for highlighting points by pointing at them
		{
			halo.GetType().GetProperty("enabled").SetValue(halo, isEnabled, null);
		}

		private void EndInteraction()
		{
			StopCoroutine(cUpdateRMan);
			//addToRManager();
		}

		private void StartInteraction()
		{
			StartCoroutine(cUpdateRMan);
		}

		private IEnumerator updateRMan()
		{
			while (true)
			{
				addToRManager();
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator WaitForStretch()
		{
			stretchEnabled = false;
			yield return new WaitForSeconds(0.35f);
			stretchEnabled = true;
		}

		public void addToRManager()
		{
			if (transform != null)
				HW_GeoSolver.ins.addToReactionManager(findGraphNode());
		}

        public void deleteGeoObj()
        {
			if (allowDelete)
			{
				HW_GeoSolver.ins.removeComponent(this);
			}
        }

		private Node<string> myGraphNode;

		public Node<string> findGraphNode()
		{
			if(myGraphNode == null)
			{
				myGraphNode = HW_GeoSolver.ins.geomanager.findGraphNode(figName);
			}
			return myGraphNode;
		}

        public void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<MasterGeoObj>() != null)
            {

                //switch (geoManager.thisMode)
                //{
                //    case ObjManHelper.IntersectionMode.glue:
                //        if(other.GetComponent<MasterGeoOBj>().figType == this.figType)
                //        {
                //            snapToFigure(other.GetComponent<MasterGeoOBj>());
                //        }
                //        break;                    
				//		case ObjManHelper.IntersectionMode.intersect:
                //         geoManager.GetComponent<intersectionManager>().checkIntersection(this, other.GetComponent<MasterGeoOBj>());
                //         break;
				//		case ObjManHelper.IntersectionMode.snap:
                        snapToFigure(other.GetComponent<MasterGeoObj>());
                //        break;
                //}

            }
        }

        internal abstract void snapToFigure(MasterGeoObj toObj);
        internal abstract void glueToFigure(MasterGeoObj toObj);

		[ContextMenu("Initialize Figure")]
		public abstract void initializefigure();
        public bool reactMotion(NodeList<string> inputNodeList)
        {
            if (intersectionFigure)
            {
                intersectionManager.ins.updateIntersectionProduct(this);
            }
			return rMotion(inputNodeList);
        }
		internal abstract bool rMotion(NodeList<string> inputNodeList);
		public abstract void updateFigure();

		public abstract void Stretch(InteractionController obj);

		private IEnumerator disableLeapInteraction()
		{
            yield return new WaitForEndOfFrame();
            GetComponent<InteractionBehaviour>().ignoreContact = true;
			GetComponent<InteractionBehaviour>().ignoreGrasping = true;
			GetComponent<InteractionBehaviour>().ignoreHoverMode = IgnoreHoverMode.Both;
			GetComponent<InteractionBehaviour>().ignorePrimaryHover = true;
            yield return new WaitForEndOfFrame();
		}

		private IEnumerator enableLeapInteraction()
		{
			while (GetComponent<InteractionBehaviour>().isGrasped)
				yield return new WaitForEndOfFrame();
			GetComponent<InteractionBehaviour>().ignoreContact = false;
			GetComponent<InteractionBehaviour>().ignoreGrasping = false;
			GetComponent<InteractionBehaviour>().ignoreHoverMode = IgnoreHoverMode.None;
			GetComponent<InteractionBehaviour>().ignorePrimaryHover = false;
            yield return new WaitForEndOfFrame();

        }

        [ContextMenu("List Bidir neighbors")]
		public void listBiDirDependencies()
		{

			Debug.Log("********");
			Debug.Log(gameObject.name);
			Debug.Log("______");

			foreach (Node<string> cNeighbor in HW_GeoSolver.ins.geomanager.findGraphNode(figName).BidirectionalNeighbors)
			{

				Debug.Log(cNeighbor.Value);
			}
			Debug.Log("********");

		}

		[ContextMenu("List Dependencies")]
		public void listDependencies()
		{

			Debug.Log("********");
			Debug.Log(gameObject.name);
			Debug.Log("______");

			foreach (Node<string> cNeighbor in HW_GeoSolver.ins.geomanager.findGraphNode(figName).Neighbors)
			{

				Debug.Log(cNeighbor.Value);
			}
			Debug.Log("********");

		}

		[ContextMenu("Output position3 variable")]
		public void debugPoisiton3()
		{
			Debug.Log("The stored position of " + name + " is " + Position3+".");
		}
	}

}