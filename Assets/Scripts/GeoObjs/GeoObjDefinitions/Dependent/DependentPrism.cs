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
	class DependentPrism : AbstractSolid, DependentFigure
    {
#pragma warning disable 0649
		/// <summary>
		/// The bases that can be moved on the figure (they have a handle and interaction behaviour)
		/// </summary>
		public List<AbstractPolygon> bases;
        /// <summary>
        /// The sides that are dependent on the bases (they don't have a handle or interactoinbehaviour.
        /// </summary>
        public List<AbstractPolygon> sides;

#pragma warning restore 0649


		internal override List<AbstractPolygon> allfaces
		{
			get
			{
				List<AbstractPolygon> result = new List<AbstractPolygon>();
				result.AddRange(bases);
				result.AddRange(sides);
				return result;
			}
		}

		internal override float volume
        {
            get
            {
				//volume of a prism, assumes parallel bases.
				//need a more complicated method to resolve this.

				float result = float.NaN;
				//if (Vector3.Magnitude(Vector3.Cross(bases[0].normDir(), bases[1].normDir())) > 0.001)
				//{
				//	Debug.LogWarning("VOLUME FORMULA DOESNT ACCOUNT FOR NON PARALLEL BASES OR BASES OF DIFFERENT SIZES!");
				//}else
				if(bases[0].area != bases[1].area)
				{
					//one of the bases has been changed but they're still parallel.
					// solved by integrating between bases.
					float b1 = bases[0].area;
					float b2 = bases[1].area;
					float a = Vector3.Distance(bases[0].Position3, bases[1].Position3);

					//technically this is plus minus sqrt, but positive should be the result.
					float c = 2f + Mathf.Sqrt(4f - 4f * (a * a + b2 / b1)) / 2f;

					result = b1 * (Mathf.Pow(a, 3) / 3f + c * a * a + c * c * a);
				}
				else
				{
					//formula for the volume of a prism.
					result = bases[0].area * Vector3.Distance(bases[0].Position3, bases[1].Position3);
				}
				return result;
            }
        }

        public List<AbstractLineSegment> lineSegments
        {
            get
            {
                List<AbstractLineSegment> edges = new List<AbstractLineSegment>();
                edges.AddRange(bases[0].lineList);
                edges.AddRange(bases[1].lineList);
                foreach (AbstractPolygon side in sides)
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
                points.AddRange(bases[0].pointList);
                points.AddRange(bases[1].pointList);
                return points;
            }
        }

        internal override AbstractPolygon primaryBase
        {
            get
            {
                return bases[0];
            }
        }

        public override void initializefigure()
        {
            //not applicable.
        }

        internal override bool rMotion(NodeList<string> inputNodeList)
        {
            return true;
        }

        public override void Stretch(InteractionController obj)
        {
            //not applicable.
        }

        public override void updateFigure()
        {
            //do nothing???
        }

        internal override void glueToFigure(MasterGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void snapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}
	}
}
