namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A net of a square that folds from a sequence of parallel line segments.
    /// Not integrated with kernel.
    /// used in study of scale and dimension
    /// </summary>
    public class squareNet : net2D
    {
        private void Start()
        {
            //intial line segment with 5 points (2 vertices merge)
            GetComponent<UnityEngine.LineRenderer>().positionCount = 5;
            //
            GetComponent<UnityEngine.LineRenderer>().useWorldSpace = false;
            //start and end width of line
            GetComponent<UnityEngine.LineRenderer>().startWidth = .008f;
            GetComponent<UnityEngine.LineRenderer>().endWidth = .008f;

            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            foldPoints.ForEach(p => p.transform.SetParent(transform));
        }

        /// <summary>
        /// calculate position of vertices for folding of square 
        /// by rotating three line segments around one stationary line segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override UnityEngine.Vector3[] verts(float percentFolded)
        {
            float angle = percentFolded * 90f;
            //matrix of vertices
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[5];
            //initial vertices that don't need to move/are pivot points
            result[2] = UnityEngine.Vector3.zero;
            result[1] = UnityEngine.Vector3.right;
            //rotate vertice by t or -t around (0, 1, 0) 
            result[0] = result[1] + (UnityEngine.Quaternion.AngleAxis(angle, UnityEngine.Vector3.up) *
                                     UnityEngine.Vector3.right);
            result[3] = result[2] + (UnityEngine.Quaternion.AngleAxis(-angle, UnityEngine.Vector3.up) *
                                     UnityEngine.Vector3.left);
            //rotate vertice by -2t around (0, 1, 0)
            result[4] = result[3] + (UnityEngine.Quaternion.AngleAxis(-2 * angle, UnityEngine.Vector3.up) *
                                     UnityEngine.Vector3.left);

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
