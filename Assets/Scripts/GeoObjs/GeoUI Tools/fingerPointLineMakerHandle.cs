/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Lattice
{
	[RequireComponent(typeof(AudioSource))]

/// <summary>
/// Depreciated. Point to draw with line segments.
/// </summary>
	public class fingerPointLineMakerHandle : MonoBehaviour
	{
		internal FingerPointLineMaker lineSegmentMaker;
		private bool first = true;
		private AudioSource thisAudioSource;
		private InteractionBehaviour thisIBehave;
		//public bool enableSearch = false;

		private void Start()
		{
			thisAudioSource = GetComponent<AudioSource>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (lineSegmentMaker.thisPinType != FingerPointLineMaker.pinType.none)
			{
				if ((GetComponent<AnchorableBehaviour>() != null && !GetComponent<AnchorableBehaviour>().isAttached) ^ GetComponent<AnchorableBehaviour>() == null)
				{
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

		public IEnumerator delaySearch()
		{
			//enableSearch = false;
			yield return new WaitForSeconds(0.5f);
			//enableSearch = true;
		}
	}
}