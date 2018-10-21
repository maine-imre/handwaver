using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;



public enum mode
{
	none,
	select,
	colour
}

namespace IMRE.HandWaver
{
	public class handColourManager : MonoBehaviour
	{
		public HandModelManager handMan;

		public Dictionary<mode, Color> handColours = new Dictionary<mode, Color>();

		public void changeHandColour(mode mode)
		{
			//hands.material.color(handColours[mode]);
		}

	}
}