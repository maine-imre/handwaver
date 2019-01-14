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
/// Line segments that aren't dynamic.
/// </summary>
	class StaticLineSegment : AbstractLineSegment, StaticFigure
    {
        public override void initializefigure()
        {
            throw new NotImplementedException();
        }

        internal override bool RMotion(NodeList<string> inputNodeList)
        {
            throw new NotImplementedException();
        }

 public override void Stretch(InteractionController obj)
		{
			throw new NotImplementedException();
		}

		public override void updateFigure()
        {
            throw new NotImplementedException();
        }

        internal override void GlueToFigure(MasterGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void SnapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}
	}
}
