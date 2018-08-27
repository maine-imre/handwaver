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
using System;
using TMPro;
using UnityEngine.Events;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class pyramidGeneratorMenu : MonoBehaviour
	{

		public InteractionButton nSideUp;
		public InteractionButton nSideDown;
		public InteractionToggle prismToggle;
		public InteractionSlider unitSlider;
		public TextMeshPro text;
		public TextMeshPro toggleText;

		//TODO: On Value changed of property match with toggle/slider if not same
		#region Properties
		private int nSides;
		private float unit;
		private bool prism;
		private AnchorableBehaviour aBehave;

		public int NSides
		{
			get
			{
				return nSides;
			}

			set
			{
				nSides = value;
				text.text = value.ToString();
			}
		}

		public float Unit
		{
			get
			{
				return unit;
			}

			set
			{
				unit = value;
			}
		}

		public bool Prism
		{
			get
			{
				return prism;
			}

			set
			{
				if (value)
					toggleText.text = "x";
				else
					toggleText.text = " ";
				prism = value;
			}
		}
#endregion
		void Start()
		{
			NSides = 3;//set defaults here to auto update text
			Unit = 1;
			Prism = false;
			
			aBehave = GetComponent<AnchorableBehaviour>();
			aBehave.OnAttachedToAnchor += attach;
			//aBehave.OnDetachedFromAnchor += detach;
			nSideDown.OnPress += (nSideDownFunc);
			nSideUp.OnPress += (nSideUpFunc);
			prismToggle.OnPress += (prismToggleFunc);
			unitSlider.HorizontalSlideEvent += (unitSliderFunc);
		}

		#region Event Functions
		private void unitSliderFunc(float arg0)
		{
			Unit = arg0;
		}

		private void prismToggleFunc()
		{
			Prism = prismToggle.isToggled;
		}

		private void nSideUpFunc()
		{
			if (NSides < 999)
				NSides++;
		}

		private void nSideDownFunc()
		{
			if (NSides >= 4)
				NSides--;
		}
#endregion

		private void attach()
		{
			//arg2.GetComponent<pyramidSceneConstruction>().OnDocked(NSides, Unit, Prism);
		}

		private void detach()
		{
			//arg2.GetComponent<pyramidSceneConstruction>().onAnchorRemove();
		}
	}
}