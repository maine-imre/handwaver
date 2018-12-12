/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver
{
/// <summary>
/// Sets the color of figures to match based on area equality. Allows for approximations with gradients.!--
/// Attempt at measurement tool.
/// Currently deprecaited.
/// will be depreciated with new geometery kernel.
/// </summary>
	public class AreaEqualityCheck : MonoBehaviour
    {
		public bool verbose = false;

        public int iterationCap = 10;
        public int clusterCount = 3;
        public float epsilon = .10f;
		//public bool checkOn = false;

		public void Start()
		{
			polygonDict = new Dictionary<AbstractPolygon, float>();
			_polygonEqualityClasses = new Dictionary<AbstractPolygon, int>();
		}

		// Update is called once per second
		//[ExecuteInEditMode]
		//IEnumerator checkArea()

		/// <summary>
		/// This is a stored cache to the polygons found within the scene and their area. To update use <code>updateAreaDict(FindObjectsOfType<AbstractPolygon>())</code>
		/// </summary>
		private static Dictionary<AbstractPolygon, float> polygonDict;
        private static Dictionary<AbstractPolygon, Color> polygonColors;

		public void CheckEquality()
		{
			//if (Input.GetKey(KeyCode.A)) {

                updateAreaDict(FindObjectsOfType<AbstractPolygon>());

                //updateClasses();
                gradientScheme();

                foreach(AbstractPolygon poly in polygonColors.Keys)
                {
                    poly.GetComponent<MeshRenderer>().material.color = polygonColors[poly];
                }
			//}
		}

        private void gradientScheme()
        {
            float minArea = polygonDict.Values.Min();
            float maxArea = polygonDict.Values.Max();
            polygonColors = new Dictionary<AbstractPolygon, Color>();

            foreach (AbstractPolygon poly in polygonDict.Keys)
            {
                float value = (polygonDict[poly] - minArea) / (maxArea - minArea);
                polygonColors[poly] = Color.HSVToRGB(value, 1, 1);
            }
        }

        private Dictionary<AbstractPolygon, int> _polygonEqualityClasses = null;
        public float[] clusterMeans;


        internal void updateClasses()
        {
            //this uses k-means clustering with k=1.  See (Bock, 2016) @ Bates Scarab for more info & MATLAB implementation.
            //fix null refs
            if (clusterMeans == null || clusterCount > polygonDict.Count)
            {
                clusterMeans = new float[Mathf.Min(clusterCount, polygonDict.Count / 2)];
            }

            int[] clusterInteralCount = new int[clusterMeans.Length];

            //random assignment to clusters.
            System.Random rdm = new System.Random();
            foreach (AbstractPolygon poly in _polygonEqualityClasses.Keys)
            {
                _polygonEqualityClasses[poly] = Mathf.RoundToInt(rdm.Next(0, clusterMeans.Length-1));
            }

                for (int i = 0; i < iterationCap; i++)
            {
                //calculate cluster means

                for (int k = 0; k < clusterMeans.Length; k++)
                {
                    clusterMeans[k] = 0f;
                }

                clusterInteralCount = new int[clusterMeans.Length];

                foreach (AbstractPolygon poly in _polygonEqualityClasses.Keys)
                {
                    if (_polygonEqualityClasses[poly] >0) {
						Debug.Log(poly.name + " IDX :  " + _polygonEqualityClasses[poly]);
                        clusterMeans[_polygonEqualityClasses[poly]] += polygonDict[poly];
                        clusterInteralCount[_polygonEqualityClasses[poly]] += 1;
                    }
                }

                for (int k = 0; k < clusterMeans.Length; k++)
                {
                    if (clusterInteralCount[k] != 0)
                    {
                        clusterMeans[k] = clusterMeans[k] / clusterInteralCount[k];
                    }
                }

                //reassign clusters

                foreach (AbstractPolygon poly in polygonDict.Keys)
                {
                    _polygonEqualityClasses[poly] = minClusterDistIDX(polygonDict[poly]);
                }
                //rinse and repeat.
                //add condition to stop if equilibrium has been reached.

            }

            //filter out values with dist < epsilon;

            foreach (AbstractPolygon poly in polygonDict.Keys)
            {
                float closestMean = minClusterDistIDX(polygonDict[poly]);
                float closestMeanDist = closestMean - polygonDict[poly];
                float percentDiff = closestMeanDist / closestMean;
                if (Mathf.Abs(percentDiff) > epsilon)
                {
                    //put anyone that doesn't get within epsilon of the mean in the 'Reject' class
                    _polygonEqualityClasses[poly] = -1;
                }
            }

            //calculate cluster means after pulling out outliers

            for (int k = 0; k < clusterMeans.Length; k++)
            {
                clusterMeans[k] = 0f;
            }

            clusterInteralCount = new int[clusterMeans.Length];

            foreach (AbstractPolygon poly in polygonDict.Keys)
            {
                if (_polygonEqualityClasses[poly] != -1)
                {
                    clusterMeans[_polygonEqualityClasses[poly]] += polygonDict[poly];
                    clusterInteralCount[_polygonEqualityClasses[poly]] += 1;
                }
            }

            for (int k = 0; k < clusterMeans.Length; k++)
            {
                if (clusterInteralCount[k] != 0)
                {
                    clusterMeans[k] = clusterMeans[k] / clusterInteralCount[k];
                }
            }

            //set colors to match

            foreach (AbstractPolygon poly in polygonDict.Keys)
            {
                float closestMean = minClusterDistIDX(polygonDict[poly]);
                float closestMeanDist = closestMean - polygonDict[poly];
                float percentDiff = closestMeanDist / closestMean;
                if (Mathf.Abs(percentDiff) < epsilon)
                {
                    poly.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(closestMean / clusterCount, percentDiff, 1);
                }
                else
                {
                    //add condition that dumps equivalence classes of 1 here.
                    poly.GetComponent<MeshRenderer>().material.color = Color.grey;
                }
            }
        }

        private int minClusterDistIDX(float v)
        {
            float dist = Mathf.Infinity;
            int idx = 0;
            for (int k = 0; k < clusterMeans.Length; k++)
            {
                if(Mathf.Abs(clusterMeans[k] - v) < dist)
                {
                    dist = Mathf.Abs(clusterMeans[k] - v);
                    idx = k;
                }
            }
            return idx;
        }

        /// <summary>
        /// Updates <paramref name="polygonDict"/> to have all polygon and area values stored.
        /// </summary>
        /// <param name="abstractPolygons">Polygons to update/add to the <paramref name="polyDict"/></param>
        private void updateAreaDict(AbstractPolygon[] abstractPolygons)
		{
			foreach (AbstractPolygon poly in abstractPolygons)
			{
				if (polygonDict.ContainsKey(poly))                  //if already contains
					polygonDict[poly] = poly.area;                  //update value
				else
				{
					polygonDict.Add(poly, poly.area);               //else add as a new Key Value
					_polygonEqualityClasses.Add(poly, -1);
				}
			}
			polygonDict.OrderBy(k => k.Value);

		}

		private void equalArea(AbstractPolygon abstractPolygon1, AbstractPolygon abstractPolygon2)
        {
			if (verbose)
			{
				Debug.Log(abstractPolygon1.figName + " and " + abstractPolygon2 + " have equal areas.");
			}
            abstractPolygon1.GetComponent<MeshRenderer>().material.color = Color.green;
            abstractPolygon2.GetComponent<MeshRenderer>().material.color = Color.green;

            //also need to change textures.
        }

        private void approxEqualArea(KeyValuePair<AbstractPolygon, float> abstractPolygon1, KeyValuePair<AbstractPolygon, float> abstractPolygon2)
        {
			float area1 = abstractPolygon1.Value;
			float area2 = abstractPolygon2.Value;

			if (verbose)
			{
				Debug.Log(abstractPolygon1.Key.figName + " and " + abstractPolygon2 + " have approximately equal areas.");
			}

            float percentDiff1 = (area1 - area2) / (2f*Mathf.Max(area1, area2));
            float percentDiff2 = -percentDiff1;

            abstractPolygon1.Key.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(33 + percentDiff1,1,1);
            abstractPolygon2.Key.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(33 + percentDiff2, 1, 1);

            //don't change texture.
        }
    }
}
