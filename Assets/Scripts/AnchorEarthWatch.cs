using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// The anchro earth watch script turns off the anchor when it might intersect the earth.
	/// This prevents anchored items from meeting the earth in error.
	/// Will be depreciated when the Geometer's Planetarium UX design is redone.
	/// </summary>
	public class AnchorEarthWatch : MonoBehaviour
	{
		/// <summary>
		/// The anchor spawner script that manages the anchored item.
		/// </summary>
		public anchorSpawner spawner;

		private void Update()
		{
			if (spawner.gameObject.activeSelf != isValid)
			{
				spawner.gameObject.SetActive(isValid);
			}
		}

		private bool isValid
		{
			get
			{
				return (RSDESManager.ins != null) && (this.transform.position - RSDESManager.earthPos).magnitude > (RSDESManager.EarthRadius + .05f);
			}
		}

	}
}
