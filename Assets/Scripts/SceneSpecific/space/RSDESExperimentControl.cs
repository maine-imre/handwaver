using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This script controls the functionality of toggling various items with keyboard buttons in the RSDES scene.
/// The main contributor(s) to this script is NG
/// </summary>

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class RSDESExperimentControl : MonoBehaviour
	{
		private RSDESManager man
		{
			get
			{
				return RSDESManager.ins;
			}
		}
		private List<pinData> pins
		{
			get
			{
				//Creates list and adds pins that are placed to that list
				return man.PinnedPoints.Where(p => (p.pin.myPintype == RSDESPin.pintype.Star)).ToList();
			}
		}
	}
}