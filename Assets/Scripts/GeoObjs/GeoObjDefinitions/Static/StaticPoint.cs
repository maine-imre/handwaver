/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class StaticPoint : AbstractPoint, StaticFigure
    {
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
