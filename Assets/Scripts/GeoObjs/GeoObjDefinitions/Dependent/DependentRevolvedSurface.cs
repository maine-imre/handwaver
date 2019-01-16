/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;
using IMRE.HandWaver.Solver;
using UnityEngine;

namespace IMRE.HandWaver
{
/// <summary>
/// will be depreciated with new geometery kernel.
/// </summary>
	class DependentRevolvedSurface : AbstractRevolvedSurface, DependentFigure
    {

        #region Constructors
        public static DependentRevolvedSurface Constructor()
				{
					return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GeoObj/DepDependentRevolvedSurface")).GetComponent<DependentRevolvedSurface>();
				}
        #endregion

        public AbstractLineSegment attachedLine;
        public AbstractPoint center;

        internal override bool RMotion(NodeList<string> inputNodeList)
        {
            if (checkForPointsInNodeList(inputNodeList))
                //checkForPointsInNodeList(inputNodeList))
            {
                endpoint1 = attachedLine.vertex0;
                endpoint2 = attachedLine.vertex1;
                return true;
            }
            else { return false; }
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

		private bool checkForPointsInNodeList(NodeList<string> inputNodeList)
        {
            if (inputNodeList.Contains(HW_GeoSolver.ins.geomanager.findGraphNode(attachedLine.gameObject.name)) &&
                (attachedLine.vertex0 != endpoint1 || attachedLine.vertex1 != endpoint2))
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
