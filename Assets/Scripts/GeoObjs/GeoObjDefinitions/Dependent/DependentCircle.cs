/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class DependentCircle : AbstractCircle, DependentFigure
    {
        public AbstractPoint center;
        public AbstractPoint edge;

		public bool freeAxis = false;

        internal override bool RMotion(NodeList<string> inputNodeList)
        {
            if (checkForPointsInNodeList(inputNodeList))
            {
				if (freeAxis)
				{
					centerPos = center.Position3;
					edgePos = edge.Position3;

					if (Vector3.Dot(centerPos - edgePos, this.normalDir) != 0)
					{
						//eventually add wrist-based motion in leapControls. HA
						Vector3 tmpVec = centerPos - edgePos;
						Vector3.OrthoNormalize(ref tmpVec, ref this.normalDir);
					}
				}
				else
				{
					centerPos = center.Position3;
					edgePos = edge.Position3;

					Vector3 change = Vector3.Project(edgePos - centerPos, normalDir);
					centerPos += change;
					center.Position3 = centerPos;
				}
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

        internal override void GlueToFigure(MasterGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void SnapToFigure(MasterGeoObj toObj)
        {
			//do nothing
		}

        private bool checkForPointsInNodeList(NodeList<string> nodeList)
        {
            if (nodeList.Contains(center.FindGraphNode()) && (center.Position3 != centerPos)) { return true; }
            if (nodeList.Contains(edge.FindGraphNode()) && (edge.Position3 != edgePos)) { return true; }
            return false;
        }
    }
}