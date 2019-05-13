/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using IMRE.HandWaver.Solver;



namespace IMRE.HandWaver
{
	/// <summary>
	/// enumerator to help select child type
	/// </summary>
	public enum GeoObjType {none, point, line, polygon, prism, pyramid, circle, sphere, revolvedsurface, torus, flatface, straightedge};

/// <summary>
/// enumerator to help select child type
/// </summary>
	public enum GeoObjDef { Abstract, Dependent, Interactable, Static, none};

/// <summary>
/// The most abstract class for all geo objs.
/// Basic features that llow geo objs to interact wil controls
/// Will be refactored in new geometery kernel.
/// </summary>
	internal abstract class MasterGeoObj : MonoBehaviour, UpdatableFigure
	{

        #region Local Positions
        private Vector3 _position3;
		private Vector3 actualPos;
        private Quaternion _rotation3;
        private float _scale;

        internal static Vector3 LocalPosition(Vector3 systemPosition)
        {
            return HW_GeoSolver.ins.localPosition(systemPosition);
        }

        internal static Vector3 systemPosition(Vector3 localPosition)
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
				if (HW_GeoSolver.ins.thisInteractionMode == HW_GeoSolver.InteractionMode.lattice)
				{
					_position3.x = Mathf.RoundToInt(value.x);
					_position3.y = Mathf.RoundToInt(value.y);
					_position3.z = Mathf.RoundToInt(value.z);
				}
                this.transform.position = LocalPosition(_position3);
				actualPos = transform.position;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="localPos"></param>
		/// <returns></returns>
		internal Vector3 ClosestLocalPosition(Vector3 localPos)
		{
			//do we need a try-catch to handle not implemented exceptions?
			try
			{
				return LocalPosition(ClosestSystemPosition(systemPosition(localPos)));
			}
			catch(NotImplementedException e)
			{
				//if an object doesn't have a closest point say that it is infinitely far away.
				Debug.LogWarning(e);
				//this compalins a lot when this is at infinity, so that itsn't an option.  need a real fix soon.
				return 100f * Vector3.one;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="abstractPosition"></param>
		/// <returns></returns>
		internal abstract Vector3 ClosestSystemPosition(Vector3 abstractPosition);

		/// <summary>
		///
		/// </summary>
		/// <param name="localPos"></param>
		/// <param name="localDir"></param>
		/// <returns></returns>
		internal float PointingAngleDiff(Vector3 localPos, Vector3 localDir)
		{
			//do we need a try-catch to handle not implemented exceptions?
			return Vector3.Angle(ClosestLocalPosition(localPos) - localPos, localDir);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="localPos"></param>
		/// <returns></returns>
		internal float LocalDistanceToClosestPoint(Vector3 localPos)
		{
			//do we need a try-catch to handle not implemented exceptions?
			return Vector3.Distance(ClosestLocalPosition(localPos), localPos);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sysPos"></param>
		/// <returns></returns>
		internal float SystemDistanceToClosestPoint(Vector3 sysPos)
		{
			//do we need a try-catch to handle not implemented exceptions?
			try
			{
				return Vector3.Distance(ClosestSystemPosition(sysPos), sysPos);
			}
			catch
			{
				//if an object doesn't have a closest point say that it is infinitely far away.
				return Mathf.Infinity;
			}
		}
        #endregion

		/// <summary>
		///
		/// </summary>
        public bool allowDelete = true;

		/// <summary>
		///
		/// </summary>
		public GeoObjType figType;

		/// <summary>
		///
		/// </summary>
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
//depreciated
            }
        }
		/// <summary>
		/// Used to save/load and keep geoObj definition type
		/// </summary>
		///
		public enum SelectionStatus { none, selected, active, canidate }
		internal SelectionStatus thisSelectStatus
		{
			get
			{
				return _thisSelectStatus;
			}

			set
			{
				switch (value)
				{
					case SelectionStatus.selected:
						foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
						{
							meshRenderer.material.SetColor("_Color",HW_GeoSolver.ins.selectedColor);
							Debug.Log("Set Material to Selected");
						}
						foreach (LineRenderer lineRenderer in GetComponentsInChildren<LineRenderer>())
						{
							lineRenderer.material.SetColor("_Color",HW_GeoSolver.ins.selectedColor);
							Debug.Log("Set Material to Selected");
						}
						break;
					case SelectionStatus.active:
						foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
						{
							meshRenderer.material.SetColor("_Color",HW_GeoSolver.ins.activeColor);
						}
						foreach (LineRenderer lineRenderer in GetComponentsInChildren<LineRenderer>())
						{
							lineRenderer.material.SetColor("_Color",HW_GeoSolver.ins.activeColor);

						}
						break;
					case SelectionStatus.canidate:
						foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
						{
							meshRenderer.material.SetColor("_Color",HW_GeoSolver.ins.canidateColor);
						}
						foreach (LineRenderer lineRenderer in GetComponentsInChildren<LineRenderer>())
						{
							lineRenderer.material.SetColor("_Color",HW_GeoSolver.ins.canidateColor);

						}
						break;
					case SelectionStatus.none:
						switch (myAbility)
						{
							case updateCapability.dependent:
								foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
								{
									meshRenderer.material.SetColor("_Color",HW_GeoSolver.ins.dependentColor);
									Debug.Log("Set Material to Default");
								}
								foreach (LineRenderer lineRenderer in GetComponentsInChildren<LineRenderer>())
								{
									lineRenderer.material.SetColor("_Color",HW_GeoSolver.ins.dependentColor);

									Debug.Log("Set Material to Default");
								}
								break;
							case updateCapability.interactable:
								foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
								{
									meshRenderer.material.SetColor("_Color",HW_GeoSolver.ins.defaultColor);
									Debug.Log("Set Material to Default");
								}
								foreach (LineRenderer lineRenderer in GetComponentsInChildren<LineRenderer>())
								{
									lineRenderer.material.SetColor("_Color",HW_GeoSolver.ins.defaultColor);

									Debug.Log("Set Material to Default");
								}
								break;
							case updateCapability.geoStatic:
								foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
								{
									meshRenderer.material.SetColor("_Color",HW_GeoSolver.ins.staticColor);
									Debug.Log("Set Material to Default");
								}
								foreach (LineRenderer lineRenderer in GetComponentsInChildren<LineRenderer>())
								{
									lineRenderer.material.SetColor("_Color",HW_GeoSolver.ins.staticColor);

									Debug.Log("Set Material to Default");
								}
								break;
						}

						break;
				}
				_thisSelectStatus = value;
			}
		}

		public enum updateCapability {interactable,dependent,geoStatic}
		internal updateCapability myAbility
		{
			get
			{
				if (this.GetComponent<InteractiveFigure>() != null)
					return updateCapability.interactable;
				if (this.GetComponent<DependentFigure>() != null)
					return updateCapability.dependent;
				return updateCapability.geoStatic;
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
		internal bool stretchEnabled = true;

		private bool _leapInteraction;
#pragma warning disable 0169

		private bool isSelected;

		private Component halo;

		internal bool interesectionFigure;
		private string _label;
#pragma warning disable 0169



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

		public bool LeapInteraction
		{
			get
			{
				return _leapInteraction;
			}

			set
			{
				if (value)
				{
					StartCoroutine(EnableLeapInteraction());
				}
				else
				{
					StartCoroutine(DisableLeapInteraction());
				}
				_leapInteraction = value;
			}
		}

		public string Label
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

		public virtual void InitializeFigure()
		{
			thisIBehave.OnGraspBegin += StartInteraction;
			thisIBehave.OnPerControllerGraspBegin += Stretch;
			thisIBehave.OnGraspEnd += EndInteraction;
			
			//attempt to fix colors.
			thisSelectStatus = thisSelectStatus;

			cUpdateRMan = UpdateRMan();
			waitForStretch = WaitForStretch();
			HW_GeoSolver.ins.addComponent(this);
			transform.SetParent(masterParentObj);
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

		private IEnumerator UpdateRMan()
		{
			while (true)
			{
				AddToRManager();
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator WaitForStretch()
		{
			stretchEnabled = false;
			yield return new WaitForSeconds(0.35f);
			if(!strechOverride)
				stretchEnabled = true;
		}

		public void AddToRManager()
		{
			if (transform != null)
				HW_GeoSolver.ins.addToReactionManager(FindGraphNode());
		}

        public void DeleteGeoObj()
        {
			if (allowDelete)
			{
				Destroy(gameObject);
			}
        }

		private Node<string> myGraphNode;
		internal static Transform masterParentObj;
		/// <summary>
		/// Set true to disable streching for all objects of this type
		/// </summary>
		public bool strechOverride = false;

		public Node<string> FindGraphNode()
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
                        SnapToFigure(other.GetComponent<MasterGeoObj>());
                //        break;
                //}

            }
        }

        internal abstract void SnapToFigure(MasterGeoObj toObj);
        internal abstract void GlueToFigure(MasterGeoObj toObj);

        public bool reactMotion(NodeList<string> inputNodeList)
        {
            if (intersectionFigure)
            {
                intersectionManager.ins.updateIntersectionProduct(this);
            }
			return RMotion(inputNodeList);
        }
		internal abstract bool RMotion(NodeList<string> inputNodeList);
		public abstract void updateFigure();

		public abstract void Stretch(InteractionController obj);

		private IEnumerator DisableLeapInteraction()
		{
            yield return new WaitForEndOfFrame();
            GetComponent<InteractionBehaviour>().ignoreContact = true;
			GetComponent<InteractionBehaviour>().ignoreGrasping = true;
			GetComponent<InteractionBehaviour>().ignoreHoverMode = IgnoreHoverMode.Both;
			GetComponent<InteractionBehaviour>().ignorePrimaryHover = true;
            yield return new WaitForEndOfFrame();
		}

		private IEnumerator EnableLeapInteraction()
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
		public void ListBiDirDependencies()
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
		public void ListDependencies()
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
		public void DebugPoisiton3()
		{
			Debug.Log("The stored position of " + name + " is " + Position3+".");
		}

		//public override bool Equals(object obj)
		//{
		//	return Equals(obj as MasterGeoObj);
		//}

		//public bool Equals(MasterGeoObj other)
		//{
		//	return other != null &&
		//		   base.Equals(other) &&
		//		   figType == other.figType &&
		//		   intersectionMultipleIDX == other.intersectionMultipleIDX &&
		//		   intersectionFigure == other.intersectionFigure &&
		//		   figName == other.figName &&
		//		   figIndex == other.figIndex &&
		//		   EqualityComparer<Node<string>>.Default.Equals(myGraphNode, other.myGraphNode);
		//}
	}

}
