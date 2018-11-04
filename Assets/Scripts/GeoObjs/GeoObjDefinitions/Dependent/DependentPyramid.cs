/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class DependentPyramid : AbstractSolid
    {
        public AbstractPoint apex;
        /// <summary>
        /// The bases that can be moved on the figure (they have a handle and interaction behaviour)
        /// </summary>
        public AbstractPolygon basePolygon;
        /// <summary>
        /// The sides that are dependent on the bases (they don't have a handle or interactoinbehaviour.
        /// </summary>
        public List<AbstractPolygon> sides;


		internal override List<AbstractPolygon> allfaces
		{
			get
			{
				List<AbstractPolygon> result = new List<AbstractPolygon>();
				result.Add(basePolygon);
				result.AddRange(sides);
				return result;
			}
		}

		internal override float volume
        {
            get
            {
                //volume of a pyramid, assumes parallel bases.

                return basePolygon.area * Vector3.Project(basePolygon.Position3- apex.Position3,basePolygon.normDir).magnitude;
            }
        }

        public List<AbstractLineSegment> lineSegments
        {
            get
            {
                List<AbstractLineSegment> edges = new List<AbstractLineSegment>();
                edges.AddRange(basePolygon.lineList);
                foreach (DependentPolygon side in sides)
                {
                    foreach (AbstractLineSegment line in side.lineList)
                    {
                        if (!edges.Contains(line))
                        {
                            edges.Add(line);
                        }
                    }
                }
                return edges;
            }
        }

        public List<AbstractPoint> vertexPoints
        {
            get
            {
                List<AbstractPoint> points = new List<AbstractPoint>();
                points.AddRange(basePolygon.pointList);
                points.Add(apex);
                return points;
            }
        }

        internal override AbstractPolygon primaryBase
        {
            get
            {
                return basePolygon;
            }
        }

        public override void initializefigure()
        {
            //not applicable
        }

        internal override bool RMotion(NodeList<string> inputNodeList)
        {
            if (inputNodeList.Contains(apex.FindGraphNode()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Stretch(InteractionController obj)
        {
            //not applicable
        }

        public override void updateFigure()
        {
            //not applicable
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