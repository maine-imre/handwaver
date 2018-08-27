/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	abstract class AbstractMarker : MonoBehaviour
    {

        internal enum MarkerType { congruant, orthagonal, parallel }
        internal MarkerType myType;

#pragma warning disable 0649
		public int MarkerIDX;

        public MasterGeoObj attachedObj;
#pragma warning restore 0649
		private void Start()
        {
            attachedObj.thisIBehave.OnGraspedMovement += forceCongruance;
        }

        abstract internal bool Comply(AbstractMarker marker);

        private void forceCongruance(Vector3 p1, Quaternion p2, Vector3 p3, Quaternion p4, List<InteractionController> p5)
        {

            foreach (AbstractMarker marker in GameObject.FindObjectsOfType<AbstractMarker>())
            {

                if (marker.myType == myType && marker.MarkerIDX == MarkerIDX)
                {
                    marker.Comply(this);
                }
            }
        }
    }
}