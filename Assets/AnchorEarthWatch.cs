using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Space
{
	public class AnchorEarthWatch : MonoBehaviour
	{
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
