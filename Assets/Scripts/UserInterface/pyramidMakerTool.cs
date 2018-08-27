/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver
{
	[RequireComponent(typeof(InteractionBehaviour),typeof(BoxCollider), typeof(AnchorableBehaviour))]

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	internal class pyramidMakerTool : HandWaverTools
	{
		private InteractionBehaviour thisIbehave;
		private AbstractPolygon baseOfPyramid;
		private AbstractPoint apexOfPyramid;

		private bool apexFollow = false;
		private bool madePyramid = false;

		private void Start()
		{
			thisIbehave = GetComponent<InteractionBehaviour>();
			thisIbehave.OnGraspEnd += endGrasp;

		}

		private void endGrasp()
		{
			StopCoroutine(Hold());
			if(madePyramid)
				Destroy(gameObject);

		}

		private void OnTriggerEnter(Collider other)
		{
			if(!madePyramid && other.GetComponent<AbstractPolygon>() != null)
			{
				apexOfPyramid = GeoObjConstruction.iPoint(transform.position);
				apexFollow = true;
				StartCoroutine(Hold());
				GeoObjConstruction.dPyramid(other.GetComponent<AbstractPolygon>(), apexOfPyramid);
				madePyramid = true;
			}
            else if(!madePyramid && other.GetComponent<InteractableLineSegment>() != null)
            {
                apexOfPyramid = GeoObjConstruction.iPoint(transform.position);
                apexFollow = true;
                StartCoroutine(Hold());
                GeoObjConstruction.iTriangle(other.GetComponent<InteractableLineSegment>(), apexOfPyramid);
                madePyramid = true;
            }
		}

		private IEnumerator Hold()
		{
			while (apexFollow)
			{
				apexOfPyramid.transform.position = transform.position;
				apexOfPyramid.addToRManager();
				yield return new WaitForEndOfFrame();
			}
		}

	}
}