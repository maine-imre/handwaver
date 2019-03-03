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

namespace IMRE.HandWaver
{
	[RequireComponent(typeof(AudioSource))]

/// <summary>
/// Handle for  arctus tool
/// Needs some refactoring in UX overhaul.
/// </summary>
	class arctusHandle : HandWaverTools
	{
#pragma warning disable 0649

		internal arctusBehaveV3 thisArctus;
		public bool center;
		private arctusHandle edge;
		private AudioSource thisAudioSource;
		public AudioClip snapToPointSound;

#pragma warning restore 0649

		private void Start()
		{
			thisAudioSource = GetComponent<AudioSource>();
			if(snapToPointSound != null)
			{
				thisAudioSource.clip = snapToPointSound;
			}
			else
			{
				Debug.LogError("Snap to Point Sound is not set within "+name);
			}


			if (center)
			{
				thisArctus = GetComponent<arctusBehaveV3>();
			}
			
		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<AbstractPoint>() != null)
			{
				snapToPoint(other);
				if (center)
				{
					thisArctus.Center = other.gameObject.GetComponent<AbstractPoint>();
					if (thisArctus.edgeHandle == null)
					{
						edge = thisArctus.spawnEdge(other.transform.position);
					}
				}
				else
				{
					if (thisArctus != null)
					{
						thisArctus.Edge = other.gameObject.GetComponent<AbstractPoint>();
						GetComponent<InteractionBehaviour>().enabled = false;
						this.transform.parent = thisArctus.Edge.transform;
					}
					else
						Debug.Log("This handle wants a reference to the arctus behave. " + name);
				}
			}
		}

		private void snapToPoint(Collider pointCollider)
		{
			transform.position = pointCollider.transform.position;
			GetComponent<InteractionBehaviour>().ReleaseFromGrasp();
			if(thisAudioSource.clip != null)
			{
				thisAudioSource.Play();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.GetComponent<AbstractPoint>() != null)
			{
				if (center)
					thisArctus.Center = null;
				else
					thisArctus.Edge = null;
			}
		}

    }
}
