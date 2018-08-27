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

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class paintbucketBehave : MonoBehaviour
	{
#pragma warning disable 0649

		public MeshRenderer paintWater;
        private ColorChooser colorChooserMenu;

        public Color newColor;
		private AnchorableBehaviour thisABehave;
		private Vector3 initialScale;
#pragma warning restore 0649

		private void Start()
        {
			/*  REMOVE WHEN FEATURE IS FULLY FUNCTIONAL */
			/**/gameObject.SetActive(!playMode.demo);/* */
			/*  REMOVE WHEN FEATURE IS FULLY FUNCTIONAL */

			colorChooserMenu = GameObject.FindObjectOfType<ColorChooser>();
            this.newColor = colorChooserMenu.thisColor;
			thisABehave = GetComponent<AnchorableBehaviour>();
			initialScale = this.transform.localScale;
			this.transform.localScale = .25f * initialScale;

			thisABehave.OnAttachedToAnchor += attach;
			thisABehave.OnDetachedFromAnchor += detach;
		}


		private void detach()
		{
			this.transform.localScale = initialScale;
		}

		private void attach()
		{
			this.transform.localScale = .25f * initialScale;
		}

		private void OnTriggerEnter(Collider other)
        {
			if(other.GetComponent<MasterGeoObj>()!=null)
            {
                switch (other.transform.GetComponent<MasterGeoObj>().figType)
                {
                    case GeoObjType.point:
                        other.transform.GetComponent<MeshRenderer>().material.color = newColor;
                        break;
                    case GeoObjType.line:
					case GeoObjType.circle:
						newColor.a = 1.0f;
						LineRenderer otherLR = other.transform.GetComponent<LineRenderer>();
                        otherLR.startColor = newColor;
                        otherLR.endColor = newColor;
						Debug.Log("Other Start " + otherLR.startColor.ToString() + " OtherEnd " + otherLR.endColor.ToString());
                        break;
                    case GeoObjType.polygon:
					case GeoObjType.sphere:
					case GeoObjType.revolvedsurface:
                    case GeoObjType.torus:
                        other.transform.GetComponent<MeshRenderer>().material.color = newColor;
                        break;
                }
            }
        }
    }
}
