/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;
using UnityEngine;

namespace IMRE.HandWaver
{
    [RequireComponent(typeof(LineRenderer))]

/// <summary>
/// Circles that aren't dynamic
/// </summary>
	class StaticCircle : AbstractCircle, StaticFigure
    {
        internal override bool RMotion(NodeList<string> inputNodeList)
        {
            return false;
        }

        public override void Stretch(InteractionController obj)
        {
            throw new NotImplementedException();
        }

        internal override void GlueToFigure(AbstractGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void SnapToFigure(AbstractGeoObj toObj)
		{
			//do nothing
		}
	}
}