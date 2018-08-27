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
using System.Linq;

namespace IMRE.HandWaver.Solver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class congruencyManager : MonoBehaviour
	{
		private HW_GeoSolver.InteractionMode toSet;

		public HW_GeoSolver.InteractionMode ToSet
		{
			get
			{
				return toSet;
			}

			set
			{
				toSet = value;
				HW_GeoSolver.ins.thisInteractionMode = toSet;
			}
		}
		#region Obselete
		///// <summary>
		///// Used Internally to track objects that should be made congruent next pass. 
		///// </summary>
		//public List<MasterGeoOBj> objectsToMakeCongruent;//TODO: Make internal after testing

		///// <summary>
		///// Objects that have been sucessfully made congruent and have been removed from objectsToMakeCongruent list.
		///// </summary>
		//public List<MasterGeoOBj> congruentObjects;

		//void Start()
		//{
		//	objectsToMakeCongruent = new List<MasterGeoOBj>();
		//	congruentObjects = new List<MasterGeoOBj>();



		//}

		////public void addToCongruencyManager()
		////{
		////	//Looks for objects not already on the list that are selected
		////	objectsToMakeCongruent = FindObjectsOfType<MasterGeoOBj>().Where(geoObj => (geoObj.IsSelected && !objectsToMakeCongruent.Contains(geoObj))).ToList();
		////	objectsToMakeCongruent.ForEach(geoObj => geoObj.geoManager.geomanager.neighborsOfNode(geoObj.figName)leapInteraction = false);



		////}
		#endregion




	}
}
