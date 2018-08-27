/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class DependentSphere : AbstractSphere, DependentFigure
    {
        public AbstractPoint center;
        public AbstractPoint edge;

        internal override bool rMotion(NodeList<string> inputNodeList)
        {
            if (checkForPointsInNodeList(inputNodeList))
            {
                centerPosition = center.Position3;
                edgePosition = edge.Position3;
				this.Position3 = centerPosition;
				return true;
            }
            else
            {
                return false;
            }
        }

        public override void Stretch(InteractionController obj)
		{
			throw new NotImplementedException();
		}

        internal override void glueToFigure(MasterGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void snapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}

		private bool checkForPointsInNodeList(NodeList<string> inputNodeList)
        {
            if ((inputNodeList.Contains(HW_GeoSolver.ins.geomanager.findGraphNode(edge.gameObject.name))) &&(edge.Position3 != edgePosition))
            {
                return true;
            }else if ((inputNodeList.Contains(HW_GeoSolver.ins.geomanager.findGraphNode(center.gameObject.name))) && (center.Position3 != centerPosition))
			{
				return true;
			}
            else
            {
                return false;
            }
        }

    }
}
