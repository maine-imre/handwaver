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
	class DependentLineSegment : AbstractLineSegment, DependentFigure
    {
        //can these be used this way???
        public AbstractPoint point1;
        public AbstractPoint point2;

        internal override bool rMotion(NodeList<string> inputNodeList)
        {
			bool hasChanged = false;

			if (!thisIBehave.isGrasped)
			{
				if (vertex0 != point1.Position3)
				{
					vertex0 = point1.Position3;
					hasChanged = true;
				}

				if (vertex1 != point1.Position3)
				{
					vertex1 = point2.Position3;
					hasChanged = true;
				}

				if (this.Position3 != (vertex0 + vertex1) / 2f)
				{
					this.Position3 = (vertex0 + vertex1) / 2f;
					hasChanged = true;
				}

			}
			else
			{
				vertex0 = point1.Position3;
				vertex1 = point2.Position3;
				Vector3 center = (vertex0 + vertex1) / 2f;

				if (this.Position3 != center)
				{
					//allows two handed grasp on capsule collider.
					point1.Position3 = this.Position3 + (vertex0 - center);
					point2.Position3 = this.Position3 + (vertex1 - center);

					vertex0 = point1.Position3;
					vertex1 = point2.Position3;

					hasChanged = true;
				}
			}
			return hasChanged;
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
		}
	}