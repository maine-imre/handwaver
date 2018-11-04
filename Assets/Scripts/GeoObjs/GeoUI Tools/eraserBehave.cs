/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;
using IMRE.HandWaver.Lattice;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class eraserBehave : HandWaverTools
	{
        private InteractionBehaviour thisIBehave;
		private Vector3 initialScale;
		private AnchorableBehaviour thisABehave;
		bool isFollowing;

		//private bool graspedBefore = false;

		private void Start()
        {
			thisIBehave = GetComponent<InteractionBehaviour>();
			thisABehave = GetComponent<AnchorableBehaviour>();
			thisIBehave.OnGraspBegin += StartInteraction;

			initialScale = this.transform.localScale;
			this.transform.localScale = .25f * initialScale;

			thisABehave.OnAttachedToAnchor += attach;
			thisABehave.OnDetachedFromAnchor += detach;
		}

		private void detach()
		{
			this.transform.localScale = initialScale;
			if (parentToAnchor)
				transform.SetParent(null);

		}

		private void attach()
		{
			this.transform.localScale = .45f * initialScale;
			if (parentToAnchor)
				transform.SetParent(thisABehave.anchor.transform);
		}


		private void StartInteraction()
		{
			//called when it is grasped
		}

		private void OnTriggerEnter(Collider other)
        {
            if (thisIBehave != null && thisIBehave.isGrasped || isFollowing)
            {

				if (other.GetComponent<AnchorableBehaviour>() != null && other.GetComponent<AnchorableBehaviour>().isAttached)
					return;
				if (other.GetComponent<HandWaverTools>() != null)
				{
					//if (other.GetComponent<CrossSectionBehave>() != null)					//if a cross section
					//	other.GetComponent<CrossSectionBehave>().holsterCrossSection();		//return to holster
					Destroy(other.gameObject);
				}
				else if (other.GetComponent<MasterGeoObj>() != null)
				{
					if(other.GetComponent<flatfaceBehave>() != null)
					{
						Destroy(other);
						return;
					}
					other.GetComponent<MasterGeoObj>().DeleteGeoObj();
					Debug.Log("Trying to erase GeoObject");
				}
				else if(other.GetComponent<Space.RSDESPin>() != null)
				{
					other.GetComponent<Space.RSDESPin>().despawn();
				}
            }
        }

		private IEnumerator eraserFollowingBehaviour;
		public bool parentToAnchor = false;

		private IEnumerator eraserfollowing(Transform target)
		{
			while (true)
			{
				this.transform.position = target.position;
				this.transform.rotation = target.rotation;
				yield return new WaitForEndOfFrame();
			}
		}

		internal void follow(Transform fingerPointLineMaker)
		{
			eraserFollowingBehaviour = eraserfollowing(fingerPointLineMaker.transform);
			StartCoroutine(eraserFollowingBehaviour);
			isFollowing = true;
		}

		internal void hideMesh()
		{
			foreach(MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
			{
				mesh.enabled = false;
			}
		}

		internal void follow(bool value)
		{
			isFollowing = value;
			if (value)
			{
				StartCoroutine(eraserFollowingBehaviour);
			}
			else
			{
				StopCoroutine(eraserFollowingBehaviour);
			}
			
		}
	}
}
