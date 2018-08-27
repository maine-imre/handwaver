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

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class flashlightBehave : HandWaverTools
	{
#pragma warning disable 0649

		public Light thislight;
		public AnchorableBehaviour thisABehave;
		private Vector3 initialScale;
#pragma warning restore 0649

		private void Start()
		{
			thislight.gameObject.SetActive(false);
			thisABehave.OnDetachedFromAnchor += lightOn;
			thisABehave.OnAttachedToAnchor += lightOff;

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

		private void lightOff()
		{
			thislight.gameObject.SetActive(false);
		}

		private void lightOn()
		{
			thislight.gameObject.SetActive(true);
		}
	}
}
