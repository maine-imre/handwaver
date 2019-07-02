using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A net of a square that folds from a sequence of parallel line segments.
    /// Not integrated with kernel.
    /// used in study of scale and dimension
    /// </summary>
    public class squareNet : MonoBehaviour, ISliderInput
    {

        private float _percentFolded;
        public bool sliderOverride;
        public List<GameObject> foldPoints = new List<GameObject>();
        public float PercentFolded
        {
            get { return _percentFolded; }

            set
            {
                //set vertices using vert function
                _percentFolded = value;
                GetComponent<LineRenderer>().SetPositions(verts(_percentFolded));
            }
        }

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

        private void Start()
        {
            //intial line segment with 5 points (2 vertices merge)
            GetComponent<LineRenderer>().positionCount = 5;
            //
            GetComponent<LineRenderer>().useWorldSpace = false;
            //start and end width of line
            GetComponent<LineRenderer>().startWidth = SpencerStudyControl.lineRendererWidth;
            GetComponent<LineRenderer>().endWidth = SpencerStudyControl.lineRendererWidth;
            
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(GameObject.Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.ForEach(p => p.transform.SetParent(transform));
        }

        /// <summary>
        /// generate vertices for square
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Vector3[] verts(float percentFolded)
        {
            float angle = percentFolded * 90f;
            //matrix of vertices
            Vector3[] result = new Vector3[5];
            //initial vertices that don't need to move/are pivot points
            result[2] = Vector3.zero;
            result[1] = Vector3.right;
            //rotate vertice by t or -t around (0, 1, 0) 
            result[0] = result[1] + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
            result[3] = result[2] + Quaternion.AngleAxis(-angle, Vector3.up) * Vector3.left;
            //rotate vertice by -2t around (0, 1, 0)
            result[4] = result[3] + Quaternion.AngleAxis(-2 * angle, Vector3.up) * Vector3.left;

            if (percentFolded == 1)
            {
                foldPoints[0].transform.localPosition = result[0];
                foldPoints[1].transform.localPosition = result[1];
                foldPoints[2].transform.localPosition = result[2];
                foldPoints[3].transform.localPosition = result[3];
                foldPoints[4].transform.localPosition = result[4];
            }
            else
            {
                foldPoints[0].transform.localPosition = result[0];
                foldPoints[1].transform.localPosition = result[1];
                foldPoints[2].transform.localPosition = result[2];
                foldPoints[3].transform.localPosition = result[3];
                foldPoints[4].transform.localPosition = result[4];
            }
            
            foldPoints[0].SetActive(true);
            foldPoints[1].SetActive(true);
            foldPoints[2].SetActive(true);
            foldPoints[3].SetActive(true);
            foldPoints[4].SetActive(true);
            return result;
        }
    }
}