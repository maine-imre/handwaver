/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueprintReality.MixCast;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// Sets MixCast to display on primary or secondary monitor.
	/// </summary>
	public class HWMixcastIO : MonoBehaviour
	{
		private mixCastTargetMode _currMode;
		public Canvas mixCastCanvas;
		public mixCastTargetMode currMode
		{
			get
			{
				return _currMode;
			}

			set
			{
				_currMode = value;
				displayUpdate(value);
			}
		}

		private void displayUpdate(mixCastTargetMode newMode)
		{
			if(mixCastCanvas == null)
			{
				Debug.Log("MixCastCanvas not set in HWMixcastIO!");
				return;
			}
			else
			{
				switch (currMode)
				{
					case mixCastTargetMode.primaryMonitor:
						mixCastCanvas.targetDisplay = 0;
						break;
					case mixCastTargetMode.secondaryMonitor:
						mixCastCanvas.targetDisplay = 1;
						break;
					case mixCastTargetMode.primaryAlt:
						mixCastCanvas.targetDisplay = 0;
						Camera.main.targetDisplay = 1;
						break;
					default:
						mixCastCanvas.targetDisplay = 0;//should anything go wrong fallback on main display
						break;
				}
			}
		}

		void Start()
		{
			//activates diplays based on commandline arguments
			for (int i = 1; i < commandLineArgumentParse.monitorCountArgument(); i++)
			{
				Display.displays[i].Activate();
			}
			//This sets it to target primary monitor unless -mixcast2 is one of the command line arguments
			currMode = commandLineArgumentParse.mixCastTarget();
		}


	}
}