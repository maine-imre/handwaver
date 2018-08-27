using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using PathologicalGames;

namespace IMRE.HandWaver.Shearing {
    class constructPyramidPrismForShearing : MonoBehaviour {

        public List<Color> mgoColor = new List<Color>();

        public int nSides = 4;
        /// <summary>
        /// The apothem to make this a cube.
        /// </summary>
        private float apothem
        {
            get
            {
                float h = Mathf.Abs(height2 - height1);
                return h / 2f;
            }
        }

        private InteractablePrism myPrism;

        private List<AbstractPolygon> sidesForPyramids;
        private List<DependentPyramid> myPyramids;

        private InteractablePoint myApex;
        private flatlandSurface flatface1;

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
            sidesForPyramids = new List<AbstractPolygon>();
            myPyramids = new List<DependentPyramid>();

            myPrism = GeoObjConstruction.iPrism(GeoObjConstruction.rPoly(nSides, apothem, Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height1), Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2);
            myPrism.leapInteraction = false;
            foreach (AbstractLineSegment mgo in myPrism.allEdges)
            {
                mgo.leapInteraction = false;
            }
            foreach (AbstractPolygon mgo in myPrism.allfaces)
            {
                mgo.leapInteraction = false;
                foreach (AbstractPoint mgol2 in mgo.pointList)
                {
                    mgol2.leapInteraction = false;
                }
            }
            foreach(AbstractPolygon side in myPrism.sides)
            {
                sidesForPyramids.Add(side);
            }
            sidesForPyramids.Add(myPrism.bases[0]);

            myApex = GeoObjConstruction.iPoint(Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2);

            //restrict the movement of myApex.
            flatface1 = PoolManager.Pools["Tools"].Spawn("flatlandSurface", Vector3.ProjectOnPlane(this.transform.position, Vector3.up) + Vector3.up * height2, Quaternion.identity, this.transform).GetComponent<flatlandSurface>();
            flatface1.GetComponent<MeshRenderer>().materials[0].color = Color.clear;
            flatface1.attachedObjs.Add(myApex);

            foreach (AbstractPolygon side in sidesForPyramids)
            {
                myPyramids.Add(GeoObjConstruction.dPyramid(side,myApex));
            }

            for (int i = 0; i < myPyramids.Count; i++)
            {
                foreach(AbstractPolygon face in myPyramids[i].allfaces)
                {
                    face.figColor = mgoColor[i];
                    face.leapInteraction = false;
                }
                foreach(AbstractLineSegment line in myPyramids[i].allEdges)
                {
                    line.leapInteraction = false;
                }
                foreach(AbstractPoint point in myPyramids[i].basePolygon.pointList)
                {
                    point.leapInteraction = false;
                }
            }
            foreach (DependentPyramid pyramid in myPyramids)
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

        private void setEmission(MasterGeoObj face, float value)
        {
            Material mat = face.GetComponent<MeshRenderer>().materials[0];

            float emission = value;
            Color baseColor = mat.color;

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

            mat.SetColor("_EmissionColor", finalColor);
        }
    }
}
