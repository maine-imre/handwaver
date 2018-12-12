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
/// This script controls the icon for the ships wheel that can be taken from the toolbox and "placed onto the striaghtedge"
/// Needs to be repalced with UX overhaul.
/// </summary>
	class shipWheelOffStraightedge : HandWaverTools
	{
#pragma warning disable 0649

		public enum wheelType {rotate,revolve,hoist,off};
        public wheelType thisWheelType;
#pragma warning restore 0649

		private AnchorableBehaviour thisABehave;
		private Vector3 initialScale;
		InteractionBehaviour thisIbehave;

		private void Start()
		{
			thisIbehave = gameObject.GetComponent<InteractionBehaviour>();
			thisABehave = gameObject.GetComponent<AnchorableBehaviour>();

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
    }
}
