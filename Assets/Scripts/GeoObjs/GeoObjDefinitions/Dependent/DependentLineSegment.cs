/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

 using Leap.Unity.Interaction;
using System;
using UnityEngine;

namespace IMRE.HandWaver
{
/// <summary>
/// will be depreciated with new geometery kernel.
/// </summary>
	class DependentLineSegment : AbstractLineSegment, DependentFigure
    {
				#region Constructors
            public static DependentLineSegment Constructor()
						{
							return PrefabManager.Spawn("DependentLineSegment").GetComponent<DependentLineSegment>();
						}
        #endregion

        //can these be used this way???
        public AbstractPoint point1;
        public AbstractPoint point2;

        internal override bool RMotion(NodeList<string> inputNodeList)
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
