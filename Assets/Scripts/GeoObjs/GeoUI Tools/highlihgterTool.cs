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
	class highlihgterTool : HandWaverTools
	{
		private void OnCollisionEnter(Collision collision)
		{
			GameObject other = collision.gameObject;
			if (other.GetComponent<MasterGeoObj>() != null)
			{
				other.GetComponent<MasterGeoObj>().IsSelected = !other.GetComponent<MasterGeoObj>().IsSelected;
			}
		}
	}
}