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
using UnityEngine.Events;
using TMPro;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class polygonGenerator : MonoBehaviour
    {
#pragma warning disable 0649

		#region regularPolygon Variables
		public InteractionButton sideDown;
		public InteractionButton sideUp;
		public InteractionButton GenPoly;
		public InteractionSlider apothemSlider;
		public TextMeshPro text;

		public GameObject spawnLocation;
#pragma warning restore 0649

		private float sideLength = 1f;

        /// <summary>
        /// Number of sides you want the polygon to have.
        /// </summary>
        [Range(3, 999)]
        public int sideCount;

		/// <summary>
		/// If side count can be increased, adds one to the side count. If it will hit the cap, report error to console.
		/// </summary>
		#endregion

		void Start()
		{
			sideUp.OnPress += (sideCountUp);
			sideDown.OnPress += (sideCountDown);
			GenPoly.OnPress += (generatePolygon);
			apothemSlider.HorizontalSlideEvent += (setApothem);
			sideLength = apothemSlider.horizontalStepValue;
			sideCount = 3;
			text.text = sideCount.ToString();
		}

		private void setApothem(float arg0)
		{
			sideLength = arg0;
		}

		public void sideCountUp()
        {
            if(sideCount < 999) {
				sideCount++;
				text.text = sideCount.ToString();
			} else
			{
				Debug.LogAssertion("Side count cap reached! What are you doing?", this);
			}
        }

        /// <summary>
        /// If side count can be decreased, subtracts one to the side count. If not feasible, reports error to console.
        /// </summary>
        public void sideCountDown()
        {
            if (sideCount > 3)
			{
				sideCount--;
				text.text = sideCount.ToString();
			} else
			{
				Debug.LogAssertion("Polygon must have at least 3 sides!", this);
			}
        }

        /// <summary>
        /// Generates a polygon based on sideCount
        /// </summary>
        public void generatePolygon()
        {
			if (spawnLocation == null)
				Debug.Log("Spawn Location not set!");
			float apothem = 0.5f* sideLength / Mathf.Tan(Mathf.PI / sideCount);
			/*regularPolygon poly =*/ GeoObjConstruction.rPoly(sideCount, apothem, spawnLocation.transform.position);
        }

    }
}
