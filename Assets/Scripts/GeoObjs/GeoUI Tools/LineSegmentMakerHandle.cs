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

namespace IMRE.HandWaver
{
/// <summary>
/// Draws lines segements bewteen points.
/// Developed for lattice land.
/// Moved to Sandbox.
/// Currently Depreciated.
/// </summary>
	public class LineSegmentMakerHandle : MonoBehaviour
    {
        private LineSegmentMaker lineSegmentMaker;
		private bool first = true;
		private AudioSource thisAudioSource;
		private InteractionBehaviour thisIBehave;

		private void Start()
		{
			thisAudioSource = GetComponent<AudioSource>();
			lineSegmentMaker = GetComponent<LineSegmentMaker>();
			thisIBehave = GetComponent<InteractionBehaviour>();

		}


        private void OnTriggerEnter(Collider other)
        {
			if ((GetComponent<AnchorableBehaviour>() != null && !GetComponent<AnchorableBehaviour>().isAttached) ^ GetComponent<AnchorableBehaviour>() == null) {
				if (other.GetComponent<AbstractPoint>() == null)
					return;
				thisAudioSource.Play();
				if (first)
				{
					first = false;
					lineSegmentMaker.prevPoint = other.gameObject.GetComponent<AbstractPoint>();
					lineSegmentMaker.pointList.Add(other.gameObject.GetComponent<AbstractPoint>());
				}
				else
				{
					lineSegmentMaker.currPoint = other.gameObject.GetComponent<AbstractPoint>();
				}
			}
        }
    }
}