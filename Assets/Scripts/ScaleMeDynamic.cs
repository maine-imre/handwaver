using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace IMRE.HandWaver.Interface
{
	[RequireComponent(typeof(Leap.Unity.Interaction.InteractionBehaviour))]
	/// <summary>
	/// Add to an object in a content layer to enable scaling, where the scalling effect is turned off when placed back on the anchor.
	/// Event-based system for objects to find the gesture control and scale themselves.
	/// in the future this will be integrated into the geosolver kernel.
	/// </summary>
	public class ScaleMeDynamic : Leap.Unity.Interaction.AnchorableBehaviour
	{
		private bool scaleControlled;
		private InteractionBehaviour thisIBehave;
		private AnchorableBehaviour thisABehave;
		public Anchor preferedAnchor;
		private Transform defaultParent;

		private void Start()
		{
			thisIBehave = GetComponent<InteractionBehaviour>();

			this.GetComponent<MeshRenderer>().materials[0].color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
			OnAttachedToAnchor += resetScale;
			OnDetachedFromAnchor += startScaleControl;
			anchor = preferedAnchor;
			TryAttach(true);
		}


		private void startScaleControl()
		{
			scaleControlled = true;
			this.transform.parent = defaultParent;
		}

		private void resetScale()
		{
			scaleControlled = false;
			this.transform.localScale = Vector3.one*.6f;
			this.transform.position = anchor.transform.position;
			this.transform.parent = anchor.transform.parent;
		}

		private void Update()
		{
			if (scaleControlled)
			{
				this.transform.localScale = Vector3.one * worldScaleModifier.ins.AbsoluteScale;
			}
		}

	}
}