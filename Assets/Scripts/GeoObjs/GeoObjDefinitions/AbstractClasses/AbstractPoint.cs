/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace IMRE.HandWaver
{
/// <summary>
/// Point in space. zero dim figure.
/// Will be refactored in new geometery kernel.
/// </summary>
	abstract class AbstractPoint : MasterGeoObj
    {
        public Vector3 initialScale;
        public bool changedNeighborNum;

		public override Vector3 ClosestSystemPosition(Vector3 abstractPosition)
		{
			return this.Position3;
		}
	}
}
