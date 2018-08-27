/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class MarkupTool : HandWaverTools
	{
#pragma warning disable 0649

		public enum markupType {lineCongruance, lineParallel, lineOrthagonal, planeCongruant, planeParallel, planeOrthagonal, angleCongruant};
		public markupType thisMarkupType;


		internal List<MasterGeoObj> objList;

		private bool prevSet = false;
		private bool currSet = false;

		private MasterGeoObj _prevObj;
		private MasterGeoObj _currObj;

		public Transform test;
#pragma warning restore 0649

		private void Start()
		{
			objList = new List<MasterGeoObj>();
		}

		internal MasterGeoObj prevObj
		{
			get
			{
				return _prevObj;
			}

			set
			{
				_prevObj = value;
				prevSet = (value != null);
				//set parameter Objects();
			}
		}

		internal MasterGeoObj currObj
		{
			get
			{
				return _currObj;
			}

			set
			{
				_currObj = value;
				currSet = (value != null);
				if (_currObj != null)
				{
					objList.Add(_currObj);
				}
				setParameters();
				_prevObj = _currObj;
			}
		}


		private void setParameters()
		{
			if (prevSet && currSet && _prevObj != _currObj)
			{
				switch (thisMarkupType)
				{
					case markupType.lineCongruance:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
					case markupType.lineParallel:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
					case markupType.lineOrthagonal:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
					case markupType.planeCongruant:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
					case markupType.planeParallel:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
					case markupType.planeOrthagonal:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
					case markupType.angleCongruant:
						foreach (MasterGeoObj obj in objList)
						{
							Transform model = GameObject.Instantiate(test);
							model.transform.parent = obj.transform;
							model.transform.localPosition = Vector3.zero;
						}
						break;
				}
			}
		}

	}

}