namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A net of a triangle that folds into a triangle.
    /// Used in study of scale and dimension
    /// not integrated with kernel.
    /// </summary>
    public class triangleNet : net2D
    {
        private void Start()
        {
            //intitial 4 vertices (one extra point that merges with another)
            GetComponent<UnityEngine.LineRenderer>().positionCount = 4;
            GetComponent<UnityEngine.LineRenderer>().useWorldSpace = false;
            //start and end width of line
            GetComponent<UnityEngine.LineRenderer>().startWidth = .008f;
            GetComponent<UnityEngine.LineRenderer>().endWidth = .008f;

            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.ForEach(p => p.transform.SetParent(transform));
        }

        /// <summary>
        /// fold outer two line segments up around stationary middle line segment by angle t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override UnityEngine.Vector3[] verts(float percentFolded)
        {
            float t = percentFolded * 120f;
            //matrix of vertices 
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[4];
            //initial vertices
            result[2] = UnityEngine.Vector3.zero;
            result[1] = UnityEngine.Vector3.right;
            //rotate vertex by t or -t around (0, 1, 0) with appropriate vector manipulation to connect triangle
            result[0] = result[1] + (UnityEngine.Quaternion.AngleAxis(t, UnityEngine.Vector3.up) *
                                     UnityEngine.Vector3.right);
            result[3] = result[2] + (UnityEngine.Quaternion.AngleAxis(-t, UnityEngine.Vector3.up) *
                                     UnityEngine.Vector3.left);

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
