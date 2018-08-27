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
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class DependentPoint : AbstractPoint, DependentFigure
    {
		private Vector3 oldPos;

		public override void initializefigure()
        {
			//   do nothing    
		}

        internal override bool rMotion(NodeList<string> inputNodeList)
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

		internal override void glueToFigure(MasterGeoObj toObj)
        {
			//   do nothing    
		}

		internal override void snapToFigure(MasterGeoObj toObj)
        {
			//   do nothing    
		}
	}

}
