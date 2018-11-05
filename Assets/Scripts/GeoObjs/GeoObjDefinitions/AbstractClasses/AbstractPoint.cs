/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	abstract class AbstractPoint : MasterGeoObj
    {
        public Vector3 initialScale;
        public bool changedNeighborNum;

		internal override Vector3 ClosestSystemPosition(Vector3 abstractPosition)
		{
			return this.Position3;
		}
	}
}
