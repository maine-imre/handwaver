/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
/// <summary>
/// Interactable line segments, using two abstract points.
/// Note that your line segment can be interactable while the endpoints are dependent.
/// Will be depreciated with new Geometery kernel
/// </summary>
	class InteractableLineSegment : AbstractLineSegment, InteractiveFigure
    {
		#region Constructors
            public static InteractableLineSegment Constructor()
            {
                GameObject go = new GameObject();
				go.AddComponent<LineRenderer>();
				//check if sphere mesh is added.
				go.AddComponent<CapsuleCollider>();
				go.AddComponent<Rigidbody>();
				go.GetComponent<Rigidbody>().useGravity = false;
				go.GetComponent<Rigidbody>().isKinematic = false;
				go.AddComponent<InteractionBehaviour>();
                go.GetComponent<InteractionBehaviour>().ignoreContact = true;
                go.GetComponent<InteractionBehaviour>().ignoreGrasping = true;
				return go.AddComponent<InteractableLineSegment>();
            }
        #endregion

        public AbstractPoint point1;
        public AbstractPoint point2;

        public void glueToCollider(Collider col)
        {
            throw new NotImplementedException();
        }


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

                if (this.Position3 != (vertex0 + vertex1) / 2f){

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

        public void snapToCollider(Collider col)
        {
            throw new NotImplementedException();
        }

		public override void Stretch(InteractionController iControll)
		{

            if (stretchEnabled && thisIBehave.graspingControllers.Count > 1)
            {
                iControll.ReleaseGrasp();

                AbstractPoint newPoint1 = GeoObjConstruction.iPoint(point1.Position3);
                AbstractPoint newPoint2 = GeoObjConstruction.iPoint(point2.Position3);

                AbstractLineSegment newLine1 = GeoObjConstruction.iLineSegment(point1, newPoint1);
                AbstractLineSegment newLine2 = GeoObjConstruction.iLineSegment(newPoint1, newPoint2);
                AbstractLineSegment newLine3 = GeoObjConstruction.iLineSegment(newPoint2, point2);

                List<AbstractPoint> pointList = new List<AbstractPoint>();
                pointList.Add(point1);
                pointList.Add(newPoint1);
                pointList.Add(newPoint2);
                pointList.Add(point2);

                List<AbstractLineSegment> lineList = new List<AbstractLineSegment>();
                lineList.Add(this);
                lineList.Add(newLine1);
                lineList.Add(newLine2);
                lineList.Add(newLine3);
				lineList.ForEach(l => l.updateFigure());

                GeoObjConstruction.iPolygon(lineList, pointList);

				if (HW_GeoSolver.ins.thisInteractionMode == HW_GeoSolver.InteractionMode.rigid)
				{
					lineList.ForEach(l=> l.LeapInteraction = false);
					pointList.ForEach(p => p.LeapInteraction = false);
				}

				StartCoroutine(waitForStretch);

            }
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