using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver.Shearing
{
    /// <summary>
    /// Euclids construction of a triangular  prism with three triangular pyramids
    /// https://mathcs.clarku.edu/~djoyce/elements/bookXII/propXII7.html
    /// </summary>
    public class constructPrismsForShearingLab : MonoBehaviour
    {
        public Color mgoColor;

        private int nSides = 3;

        private InteractablePrism myPrism;
        private List<DependentPyramid> myPyramids = new List<DependentPyramid>();
        public float height1
        {
            get
            {
                return shearingLabManager.height1;
            }
        }
        public float height2
        {
            get
            {
                return shearingLabManager.height2;
            }
        }

        private void Start()
        {
            myPrism = GeoObjConstruction.iPrism(GeoObjConstruction.rPoly(nSides, Mathf.Abs(height1 - height2) / 2f, Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height1), Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2);
            myPrism.LeapInteraction = false;
            foreach(AbstractLineSegment mgo in myPrism.allEdges)
            {
                mgo.LeapInteraction = false;
            }
            foreach (AbstractPolygon mgo in myPrism.bases)
            {
                mgo.LeapInteraction = false;
                foreach(AbstractPoint mgol2 in mgo.pointList)
                {
                    mgol2.LeapInteraction = false;
                }
            }

            foreach(AbstractPolygon face in myPrism.allfaces)
            {
                face.figColor = mgoColor;
            }

            //pyramid 1 - base ABC - apex D
            myPyramids.Add(GeoObjConstruction.dPyramid(myPrism.bases[0], myPrism.bases[1].pointList[0]));

            //pyramid 2 - base DEF - apex C
            myPyramids.Add(GeoObjConstruction.dPyramid(myPrism.bases[1], myPrism.bases[0].pointList[2]));

            //pyramid 3 - base CBD - apex E
            myPyramids.Add(GeoObjConstruction.dPyramid(myPyramids[0].sides[1], myPrism.bases[1].pointList[1]));

            foreach(DependentPyramid pyramid in myPyramids)
            {
                pyramid.GetComponent<InteractionBehaviour>().OnHoverBegin += updateGlow;
                pyramid.GetComponent<InteractionBehaviour>().OnHoverEnd += updateGlow;
            }
        }
        private void updateGlow()
        {
            //changes transparency, could change emission.
            float alphaGlow = .6f;
            float alphaNormal = 0f;
            foreach (DependentPyramid pyramid in myPyramids)
            {
                foreach (AbstractPolygon face in pyramid.allfaces)
                {
                    //Color tempColor = face.GetComponent<MeshRenderer>().materials[0].color;

                    if (pyramid.GetComponent<InteractionBehaviour>().isHovered)
                    {
                        //tempColor.a = alphaGlow;
                        setEmission(face, alphaGlow);

                    }
                    else
                    {
                        //tempColor.a = alphaNormal;
                        setEmission(face, alphaNormal);

                    }
                    //face.GetComponent<MeshRenderer>().materials[0].color = tempColor;
                }
            }
        }

		/// <summary>
		/// Sets the value for emission brightness
		/// </summary>
		/// <param name="obj">object to change emission</param>
		/// <param name="value">Between 0 and 1, percent brightness</param>
        private void setEmission(AbstractGeoObj obj, float value)
        {
			if (value > 1 || value < 0)
				return;

			Material mat = obj.GetComponent<MeshRenderer>().materials[0];

			if (mat != null)
				mat.SetColor("_EmissionColor", Vector4.one * value);
			else
				Debug.LogError("Tried to set the emission for object "+obj.name+", but it has no material!");
		}
    }
}
