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
/// will be depreciated with new geometery kernel.
/// </summary>
	class DependentPoint : AbstractPoint, DependentFigure
    {
		#region Constructors
		public static DependentPoint Constructor()
		{
									return GameObject.Instantiate(PrefabManager.Spawn("DependentPoint")).GetComponent<DependentPoint>();
		}
		#endregion
		private Vector3 oldPos;

        internal override bool RMotion(NodeList<string> inputNodeList)
        {
			return hasMoved();
		}

		public bool hasMoved()
		{
			bool result = (this.Position3 == oldPos);
			oldPos = this.Position3;
			return result;
		}

		public override void Stretch(InteractionController obj)
		{
			//   do nothing
		}

		public override void updateFigure()
        {
			//   do nothing
		}

		internal override void GlueToFigure(MasterGeoObj toObj)
        {
			//   do nothing
		}

		internal override void SnapToFigure(MasterGeoObj toObj)
        {
			//   do nothing
		}
	}

}
