/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Leap.Unity;

namespace IMRE.HandWaver
{
	[RequireComponent(typeof(ExtendedFingerDetector))]

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class FingerPointingSelector : MonoBehaviour
	{

#pragma warning disable 0649, 0169

		public bool lHand;
		[Range(0, 10)]
		public float rayLength = .05f;

		[Range(-1, 1)]
		public float fingerAdjust;

		public Transform fingerTip;
		private bool highlighted;
		private bool cast = true;
		private bool _enabled;
#pragma warning disable 0649, 0169

		public bool castEnabled
		{
			get
			{
				return _enabled;
			}

			set
			{
				_enabled = value;
			}
		}

		public void castToggle()
		{
			castEnabled = !castEnabled;
		}

		void Update()
		{
			if(castEnabled)
			{
				onUpdate();
			}
		}

		private void onUpdate()
		{
			RaycastHit hit;
			float directionMod = -1;
			if (!lHand)
			{
				directionMod = 1;
			}
			Ray fingerRay = new Ray(fingerTip.transform.position - fingerAdjust * fingerTip.transform.right, directionMod * fingerTip.transform.right);


			if (cast && Physics.Raycast(fingerRay, out hit, rayLength))
			{

				if (hit.collider.GetComponent<AbstractGeoObj>() != null)
				{
					StartCoroutine(castCoolDown(0.5f));
					hit.collider.GetComponent<AbstractGeoObj>().IsSelected = !hit.collider.GetComponent<AbstractGeoObj>().IsSelected;
				}
			}
		}

		private IEnumerator castCoolDown(float cooldownTime)
		{
			cast = false;
			yield return new WaitForSecondsRealtime(cooldownTime);
			cast = true;
		}
	}
}