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
/// <summary>
/// Points that aren't dynamic
/// </summary>
	class StaticPoint : AbstractPoint, StaticFigure
    {
        #region Constructors
            public static StaticPoint Constructor()
						{
							return GameObject.Instantiate(PrefabManager.Spawn("StaticPoint")).GetComponent<StaticPoint>();
						}
        #endregion

        public override void initializefigure()
        {
			//do nothing.
		}

		internal override bool RMotion(NodeList<string> inputNodeList)
        {
			return false;
        }

	public override void Stretch(InteractionController obj)
		{
			//do nothing.
		}

		public override void updateFigure()
        {
			//do nothing.
        }

        internal override void GlueToFigure(MasterGeoObj toObj)
        {
			//do nothing.
		}

		internal override void SnapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}
	}
}
