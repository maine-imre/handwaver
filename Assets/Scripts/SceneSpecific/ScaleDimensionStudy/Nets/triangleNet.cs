using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A net of a triangle that folds into a triangle.
    /// Used in study of scale and dimension
    /// not integrated with kernel.
    /// </summary>
    public class triangleNet : MonoBehaviour, ISliderInput
    {

        private float _percentFolded = 0f;
        
        public List<GameObject> foldPoints = new List<GameObject>();

        public float PercentFolded
        {
            get { return _percentFolded; }

            set
            {
                //set vertices using verts function
                _percentFolded = value;
                GetComponent<LineRenderer>().SetPositions(verts(_percentFolded));
            }
        }
        public bool sliderOverride;

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

        private void Start()
        {
            //intitial 4 vertices (one extra point that merges with another)
            GetComponent<LineRenderer>().positionCount = 4;
            GetComponent<LineRenderer>().useWorldSpace = false;
            //start and end width of line
            GetComponent<LineRenderer>().startWidth = SpencerStudyControl.lineRendererWidth;
            GetComponent<LineRenderer>().endWidth = SpencerStudyControl.lineRendererWidth;
            
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.ForEach(p => p.transform.SetParent(transform));
        }

        /// <summary>
        /// fold line segment of 4 points by degree t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Vector3[] verts(float percentFolded)
        {
            float t = percentFolded * 120f;
            //matrix of vertices 
            Vector3[] result = new Vector3[4];
            //initial vertices
            result[2] = Vector3.zero;
            result[1] = Vector3.right;
            //rotate vertex by t or -t around (0, 1, 0) with appropriate vector manipulation to connect triangle
            result[0] = result[1] + Quaternion.AngleAxis(t, Vector3.up) * Vector3.right;
            result[3] = result[2] + Quaternion.AngleAxis(-t, Vector3.up) * Vector3.left;

            if (percentFolded == 1)
            {
                foldPoints[0].transform.localPosition = result[0];
                foldPoints[1].transform.localPosition = result[1];
                foldPoints[2].transform.localPosition = result[2];
                foldPoints[3].transform.localPosition = result[0];
            }
            else
            {
                foldPoints[0].transform.localPosition = result[0];
                foldPoints[1].transform.localPosition = result[1];
                foldPoints[2].transform.localPosition = result[2];
                foldPoints[3].transform.localPosition = result[3];
            }
            
            foldPoints[0].SetActive(true);
            foldPoints[1].SetActive(true);
            foldPoints[2].SetActive(true);
            foldPoints[3].SetActive(true);

            return result;
        }
    }
}