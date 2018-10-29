using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IMRE.HandWaver.Solver;
using Leap.Unity.Interaction;
using System;

using TMPro;

namespace IMRE.HandWaver.Shearing
{
    public class constructCutoutRectangularPrism : MonoBehaviour
    {
        private AbstractPoint controllPoint;
        public float sideLength1 = .3f;
        public float sideLength2 = .5f;

        private List<AbstractPoint> prismBase = new List<AbstractPoint>();
        private List<AbstractPoint> cutoutRectangle = new List<AbstractPoint>();
        private InteractablePrism prism;
        private AbstractPolygon prismBasePoly;
		private AbstractPolygon prismTopPoly;

        private AbstractPolygon cutoutRectanglePoly;

        private float xLen;

        private LineRenderer volumeLineRenderer;
        private int numFrames = 0;

		private TextMeshPro TMPro;

        // Use this for initialization
        void Start()
        {
			volumeLineRenderer = GetComponentInChildren<LineRenderer>();
			TMPro = GetComponentInChildren<TextMeshPro>();

            xLen = .1f;
            controllPoint = GeoObjConstruction.iPoint(this.transform.position);
			prismBase.Add(controllPoint);
            prismBase.Add(GeoObjConstruction.dPoint(controllPoint.Position3 + Vector3.right * (sideLength1 - 2 * xLen)));
            prismBase.Add(GeoObjConstruction.dPoint(controllPoint.Position3 + Vector3.right * (sideLength1 - 2 * xLen) + Vector3.forward * (sideLength2 - 2 * xLen)));
            prismBase.Add(GeoObjConstruction.dPoint(controllPoint.Position3 + Vector3.forward * (sideLength2 - 2 * xLen)));

			List<AbstractLineSegment> prismBaseLines = new List<AbstractLineSegment>();
			for (int i = 0; i < 3; i++)
			{
				prismBaseLines.Add(GeoObjConstruction.dLineSegment(prismBase[i], prismBase[i + 1]));
			}
			prismBaseLines.Add(GeoObjConstruction.dLineSegment(prismBase[prismBase.Count - 1], prismBase[0]));

            prismBasePoly = GeoObjConstruction.dPolygon(prismBaseLines, prismBase);
            prism = GeoObjConstruction.dPrism(prismBasePoly, prismBasePoly.center + Vector3.up * xLen);
			prismTopPoly = prism.bases[1];

            cutoutRectangle.Add(prismBase[0]);
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[0].Position3 + Vector3.back * xLen));
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[1].Position3 + Vector3.back * xLen));
            cutoutRectangle.Add(prismBase[1]);
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[1].Position3 + Vector3.right * xLen));
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[2].Position3 + Vector3.right * xLen));
            cutoutRectangle.Add(prismBase[2]);
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[2].Position3 + Vector3.forward * xLen));
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[3].Position3 + Vector3.forward * xLen));
            cutoutRectangle.Add(prismBase[3]);
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[3].Position3 + Vector3.left * xLen));
            cutoutRectangle.Add(GeoObjConstruction.dPoint(prismBase[0].Position3 + Vector3.left * xLen));

            List<AbstractLineSegment> cutoutLines = new List<AbstractLineSegment>();
            for (int i = 0; i < cutoutRectangle.Count - 1; i++)
            {
                cutoutLines.Add(GeoObjConstruction.dLineSegment(cutoutRectangle[i], cutoutRectangle[i + 1]));
            }
            cutoutLines.Add(GeoObjConstruction.dLineSegment(cutoutRectangle[0], cutoutRectangle[cutoutRectangle.Count-1]));


            cutoutRectanglePoly = GeoObjConstruction.dPolygon(cutoutLines,cutoutRectangle);

			foreach (MasterGeoObj mgo in prism.transform.parent.GetComponentsInChildren<MasterGeoObj>())
			{
				if (mgo != controllPoint)
				{
					mgo.leapInteraction = false;
				}
			}

            controllPoint.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().OnGraspedMovement += updateDiagram;
			controllPoint.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().OnGraspEnd += updateDiagram;
			volumeLineRenderer.useWorldSpace = false;

			Color tmp = Color.blue;
			tmp.a = .5f;

			cutoutRectanglePoly.figColor = tmp;
			foreach (AbstractPolygon poly in prism.allfaces)
			{
				tmp = Color.red;
				tmp.a = .5f;
				poly.figColor = tmp;
			}
			prism.bases[0].figColor = Color.clear;
		}

		private void updateDiagram(Vector3 oldPosition, Quaternion oldRotation, Vector3 newPosition, Quaternion newRotation, List<InteractionController> graspingControllers)
		{
			updateDiagram();
		}

		private void updateDiagram()
		{
			controllPoint.Position3 = Vector3.Project(controllPoint.Position3 - this.transform.position, (Vector3.right + Vector3.forward).normalized) + this.transform.position;

			xLen = Vector3.Distance(controllPoint.Position3, cutoutRectangle[1].Position3);

			//each corner moves in.
			prismBase[1].Position3 = (controllPoint.Position3 + Vector3.right * (sideLength1 - 2 * xLen));
			prismBase[2].Position3 = (controllPoint.Position3 + Vector3.right * (sideLength1 - 2 * xLen) + Vector3.forward * (sideLength2 - 2 * xLen));
			prismBase[3].Position3 = (controllPoint.Position3 + Vector3.forward * (sideLength2 - 2 * xLen));

			cutoutRectangle[1].Position3 = prismBase[0].Position3 + Vector3.back * xLen;
			cutoutRectangle[2].Position3 = prismBase[1].Position3 + Vector3.back * xLen;

			cutoutRectangle[4].Position3 = prismBase[1].Position3 + Vector3.right * xLen;
			cutoutRectangle[5].Position3 = prismBase[2].Position3 + Vector3.right * xLen;

			cutoutRectangle[7].Position3 = prismBase[2].Position3 + Vector3.forward * xLen;
			cutoutRectangle[8].Position3 = prismBase[3].Position3 + Vector3.forward * xLen;

			cutoutRectangle[10].Position3 = prismBase[3].Position3 + Vector3.left * xLen;
			cutoutRectangle[11].Position3 = prismBase[0].Position3 + Vector3.left * xLen;

			//the prism moves to reflect this...
			for (int i = 0; i < 4; i++)
			{
				prismTopPoly.pointList[i].Position3 = prismBasePoly.pointList[i].Position3 + Vector3.up * xLen;
			}

			foreach (MasterGeoObj mgo in prism.vertexPoints)
			{
				mgo.addToRManager();
			}

			//plot the volume on the line renderer
			volumeLineRenderer.positionCount = (numFrames + 1);
			volumeLineRenderer.SetPosition(numFrames, volumeForLineRenderer);
			numFrames++;

			volumeLineRenderer.transform.GetChild(0).transform.localPosition = volumeForLineRenderer;

			TMPro.SetText("x = " + Math.Round(volumeForLineRenderer.x, 2) + " cm                        V = " + Math.Round(volumeForLineRenderer.y, 2) + " cm^3");


        }

        private Vector3 volumeForLineRenderer
        {
            get
            {
                float volume = xLen * (sideLength1 - 2 * xLen) * (sideLength2 - 2 * xLen);
                return new Vector3(xLen*100f, volume * 100f * 100f * 100f, 0);
            }
        }
    }
}
