/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Examples;
using System;
using System.Linq;
using TMPro;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class selectionMenu : MonoBehaviour
	{
#pragma warning disable 0649
		public InteractionButton left;
		public InteractionButton right;
		public InteractionButton check;
		public InteractionButton POSSpawner;
		public InteractionButton deselect;
		public List<FingerPointingSelector> fingerPointerList;

		public TextMeshPro warningText;

		private WorkstationBehaviourExample thisWS;
		private AnchorableBehaviour thisABehave;
		private InteractionBehaviour iBehave;

		private List<MasterGeoObj> mgoCanidates;
		public MasterGeoObj activeMGO;
		private int canidateIDX;


		[Range(0f, 2f)]
		public float selectionRadius = 0.5f;
		public AudioClip successClip;
		private AudioSource thisAS;
		public AudioClip failClip;

#pragma warning restore 0649
		void Start()
		{
			thisABehave = GetComponent<AnchorableBehaviour>();
			thisWS = GetComponent<WorkstationBehaviourExample>();
			iBehave = GetComponent<InteractionBehaviour>();
			thisABehave.OnDetachedFromAnchor += detach;
			thisABehave.OnAttachedToAnchor += attach;
			iBehave.OnGraspEnd += interactionEnd;
			left.OnPress += (prevActive);
			right.OnPress += (nextActive);
			check.OnPress += (selectMGO);
			POSSpawner.OnPress += (spawnPOS);
			deselect.OnPress += (deselectFunc);
			thisAS = GetComponent<AudioSource>();
		}

		private void detach()
		{
			fingerPointerList.ForEach(fp => fp.castEnabled = false);
		}

		private void attach()
		{
			fingerPointerList.ForEach(fp => fp.castEnabled= true);
		}

		private void deselectFunc()
		{
			foreach (MasterGeoObj obj in GameObject.FindObjectsOfType<MasterGeoObj>().Where(obj => obj.IsSelected))
			{
				obj.IsSelected = false;
			}
		}

		private void spawnPOS()
		{
			GeoObjConstruction.snapPoint(activeMGO, this.transform.position);
		}

		private void selectMGO()
		{
			if (activeMGO.thisSelectStatus == MasterGeoObj.SelectionStatus.active)
			{
				activeMGO.thisSelectStatus = MasterGeoObj.SelectionStatus.selected;
			}
			else
			{
				activeMGO.thisSelectStatus = MasterGeoObj.SelectionStatus.active;
			}
		}

		private void nextActive()
		{
			activeMGO.thisSelectStatus = MasterGeoObj.SelectionStatus.canidate;
			canidateIDX++;
			if (canidateIDX > mgoCanidates.Count)
			{
				canidateIDX = 0;
			}
			else if (canidateIDX < 0)
			{
				canidateIDX = mgoCanidates.Count;
			}

			activeMGO = mgoCanidates[canidateIDX];
			activeMGO.thisSelectStatus = MasterGeoObj.SelectionStatus.active;
		}

		private void prevActive()
		{
			activeMGO.thisSelectStatus = MasterGeoObj.SelectionStatus.canidate;

			canidateIDX--;
			if (canidateIDX > mgoCanidates.Count)
			{
				canidateIDX = 0;
			}
			else if (canidateIDX < 0)
			{
				canidateIDX = mgoCanidates.Count;
			}

			activeMGO = mgoCanidates[canidateIDX];
			activeMGO.thisSelectStatus = MasterGeoObj.SelectionStatus.active;
		}

		private void interactionEnd()
		{
			mgoCanidates = new List<MasterGeoObj>();
			if (this.GetComponent<AnchorableBehaviour>().isAttached == false)
			{

				foreach (MasterGeoObj mgo in FindObjectsOfType<MasterGeoObj>().Where(g => (g.GetComponent<AnchorableBehaviour>() == null || (g.GetComponent<AnchorableBehaviour>() != null && !g.GetComponent<AnchorableBehaviour>().isAttached))))
				{
					float distance = 15;
					switch (mgo.figType)
					{
						case GeoObjType.point:
							distance = Vector3.Magnitude(this.transform.position - mgo.transform.position);
							break;
						case GeoObjType.line:
							distance = Vector3.Magnitude(Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<AbstractLineSegment>().vertex0 - mgo.GetComponent<AbstractLineSegment>().vertex1) + mgo.transform.position - this.transform.position);
							break;
						case GeoObjType.polygon:
							Vector3 positionOnPlane = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractPolygon>().normDir) + mgo.transform.position;
							distance = Vector3.Magnitude(positionOnPlane - this.transform.position);
							Debug.LogWarning("Polygon doesn't check boundariers");
							break;
						case GeoObjType.prism:
							distance = Vector3.Magnitude(mgo.transform.position - this.transform.position);
							break;
						case GeoObjType.pyramid:
							Debug.LogWarning("Pyramids not yet supported");
							break;
						case GeoObjType.circle:
							Vector3 positionOnPlane2 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<AbstractCircle>().normalDir) + mgo.transform.position;
							Vector3 positionOnCircle = Vector3.Normalize(positionOnPlane2 - mgo.GetComponent<AbstractCircle>().centerPos) * mgo.GetComponent<AbstractCircle>().Radius + mgo.GetComponent<AbstractCircle>().centerPos;
							distance = Vector3.Magnitude(this.transform.position - positionOnCircle);
							break;
						case GeoObjType.sphere:
							Vector3 lineDir = Vector3.Normalize(transform.position - mgo.transform.position);
							Vector3 positionOnSphere1 = mgo.GetComponent<AbstractSphere>().radius * lineDir + mgo.transform.position;
							distance = Vector3.Magnitude(positionOnSphere1 - this.transform.position);
							break;
						case GeoObjType.revolvedsurface:
							Debug.LogWarning("RevoledSurface not yet supported");
							break;
						case GeoObjType.torus:
							Debug.LogWarning("Torus not yet supported");
							break;
						case GeoObjType.flatface:
							Vector3 positionOnPlane3 = Vector3.ProjectOnPlane(transform.position - mgo.transform.position, mgo.GetComponent<flatfaceBehave>().normalDir) + mgo.transform.position;
							distance = Vector3.Magnitude(positionOnPlane3 - this.transform.position);
							break;
						case GeoObjType.straightedge:
							Vector3 positionOnStraightedge = Vector3.Project(transform.position - mgo.transform.position, mgo.GetComponent<straightEdgeBehave>().normalDir) + mgo.transform.position;
							distance = Vector3.Magnitude(positionOnStraightedge - this.transform.position);
							break;
						default:
							Debug.LogWarning("Something went wrong in the selectionmanager.... :(");
							break;
					}
					if (Mathf.Abs(distance) < selectionRadius && mgo.thisSelectStatus != MasterGeoObj.SelectionStatus.selected)
					{
						mgo.thisSelectStatus = MasterGeoObj.SelectionStatus.canidate;
						mgoCanidates.Add(mgo);
					}
					else if (Mathf.Abs(distance) < selectionRadius)
					{
						mgoCanidates.Add(mgo);
					}
					else if (mgo.thisSelectStatus == MasterGeoObj.SelectionStatus.canidate || mgo.thisSelectStatus == MasterGeoObj.SelectionStatus.active)
					{
						mgo.thisSelectStatus = MasterGeoObj.SelectionStatus.none;
					}
				}
			}
			if (mgoCanidates.Count >= 1)
			{
				activeMGO = mgoCanidates[0];
				canidateIDX = 0;
			}
			setbuttonState(mgoCanidates.Count >= 1);
		}

		private void setbuttonState(bool v)
		{
			if (v)
			{
				thisAS.clip = successClip;
				thisAS.Play();
			}else
			{
				thisAS.clip = failClip;
				thisAS.Play();
			}

			left.gameObject.SetActive(v);
			right.gameObject.SetActive(v);
			check.gameObject.SetActive(v);
			POSSpawner.gameObject.SetActive(v);
			warningText.gameObject.SetActive(!v);
		}
	}
}
