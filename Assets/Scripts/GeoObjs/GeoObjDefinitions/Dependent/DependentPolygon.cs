/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using UnityEngine;
using System;
using IMRE.HandWaver.Solver;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IMRE.HandWaver
{
/// <summary>
/// will be depreciated with new geometery kernel.
/// </summary>
	class DependentPolygon : AbstractPolygon, DependentFigure
    {
		#region Constructors
			public static  DependentPolygon Constructor()
			{
				return GameObject.Instantiate(PrefabManager.Spawn("DepenDependentPolygon")).GetComponent<DependentPolygon>();
			}
		#endregion

		internal override bool RMotion(NodeList<string> inputNodeList)
        {
            if (inputNodeList.checkForMGOmatch(pointListMGO))
            {
                int pointNum = pointList.Count;

                MeshFilter mf = GetComponent<MeshFilter>();
                Mesh mesh = mf.mesh;

                int i = 0;

				Quaternion angle = Quaternion.identity;

				if (!skewable && CheckSkewPolygon())
				{
					Debug.LogWarning("This polygon " + figName + " is a skew polygon.");
					MasterGeoObj firstHit = inputNodeList.findMGOmatch(pointListMGO);

					switch (firstHit.figType)
					{
						case GeoObjType.point:
							//need to rotate every other point based ont the angle from the old point position and the center.
							angle = angleAroundCenter(firstHit.Position3, vertices[pointList.FindIndex(x => x == firstHit.GetComponent<AbstractPoint>())]);
							break;
						case GeoObjType.line:
							//need to rotate every other point based on the angle from the midpoint of the line segment and the center.
							//need to address this case in the future.
							break;
						default:
							break;
					}
				}

				this.Position3 = center;

				foreach (AbstractPoint point in pointList)
				{
					//move points with a rotation around the center.
					if (inputNodeList.checkForMGOmatch(point))
					{
						vertices[i] = angle * (point.Position3 - center);
					}
					else
					{
						vertices[i] = (point.Position3 - center);
						//don't rotate the point around the angle that has already moved.
					}
					i++;
				}

				bool isTrue = mesh.vertices == vertices;
				mesh.vertices = vertices;
				return isTrue;
			}
            else
            {
                return false;
            }

        }

		public override void Stretch(InteractionController iControll)
		{
            if (stretchEnabled && thisIBehave.graspingControllers.Count > 1)
            {
                iControll.ReleaseGrasp();

				InteractablePrism prism = GeoObjConstruction.iPrism(this);

				if (HW_GeoSolver.ins.thisInteractionMode == HW_GeoSolver.InteractionMode.rigid)
				{
					prism.lineSegments.ForEach(p => p.LeapInteraction = false);
					prism.vertexPoints.ForEach(p => p.LeapInteraction = false);
				}
				StartCoroutine(waitForStretch);
            }
        }

		private Quaternion angleAroundCenter(Vector3 pos0, Vector3 pos1)
		{
			return Quaternion.FromToRotation(pos0 - center, pos1 - center);
		}



		internal override void SnapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}

		internal override void GlueToFigure(MasterGeoObj toObj)
        {
            throw new NotImplementedException();
        }
    }
}
