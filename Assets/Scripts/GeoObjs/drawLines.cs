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

namespace IMRE.HandWaver
{

/// <summary>
/// draws a dependent line segment between all abstract points that are selected in the scene.
/// will be depreciated in new geometery kernel.
/// </summary>
	public class drawLines : MonoBehaviour
	{
		public void drawAllLines() {
			foreach (AbstractPoint point1 in GameObject.FindObjectsOfType<AbstractPoint>()) {
				foreach(AbstractPoint point2 in GameObject.FindObjectsOfType<AbstractPoint>())
				{
					if (point1 != point2 && point1.IsSelected && point2.IsSelected)
					{
						GeoObjConstruction.dLineSegment(point1, point2);
					}
				}

			}
		}
	}
}
