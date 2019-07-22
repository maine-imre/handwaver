using Enumerable = System.Linq.Enumerable;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    ///     A net of a tetrahedron that folds into a tetrahedron.
    ///     used in study of scale and dimension
    ///     not integrated with kernel.
    /// </summary>
    public class tetrahedronNet : UnityEngine.MonoBehaviour, ISliderInput
    {
        private static readonly float COMPLETEDFOLD = 180f - UnityEngine.Vector3.Angle(
                                                          UnityEngine.Vector3.one -
                                                          ((new UnityEngine.Vector3(1, -1, -1) +
                                                            new UnityEngine.Vector3(-1, 1, -1)) /
                                                           2f),
                                                          new UnityEngine.Vector3(-1, -1, 1) -
                                                          ((new UnityEngine.Vector3(1, -1, -1) +
                                                            new UnityEngine.Vector3(-1, 1, -1)) / 2f));

        private float _percentFolded;
        public bool sliderOverride;

        public UnityEngine.Mesh mesh => GetComponent<UnityEngine.MeshFilter>().mesh;

        public UnityEngine.LineRenderer lineRenderer => GetComponent<UnityEngine.LineRenderer>();

        public float PercentFolded
        {
            get => _percentFolded;

            set
            {
                _percentFolded = value;
                lineRenderer.SetPositions(lineRendererVerts(_percentFolded));
                mesh.SetVertices(Enumerable.ToList(meshVerts(_percentFolded)));
            }
        }

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

        private void Start()
        {
            //unfolded shape(degree of fold = 0)
            mesh.vertices = meshVerts(0);
            //triangles for unfolded shape
            mesh.triangles = meshTris();
            //11 vertices on trace of unfolded shape 
            lineRenderer.positionCount = 11;
            lineRenderer.useWorldSpace = false;
            lineRenderer.startWidth = .01f;
            lineRenderer.endWidth = .01f;
            lineRenderer.SetPositions(lineRendererVerts(0));
        }

        /// <summary>
        ///     fold tetrahedron net up by angle t
        /// </summary>
        /// <param name="percentfolded"></param>
        /// <returns></returns>
        private static UnityEngine.Vector3[] meshVerts(float percentfolded)
        {
            float degreefolded = (percentfolded * COMPLETEDFOLD) + 180f;
            //6 vertices on tetrahedron
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[6];

            //inner 3 vertices
            result[0] = (UnityEngine.Vector3.right * (UnityEngine.Mathf.Sqrt(3f) / 2f)) +
                        (UnityEngine.Vector3.forward * .5f);
            result[1] = (UnityEngine.Vector3.right * (UnityEngine.Mathf.Sqrt(3f) / 2f)) +
                        (UnityEngine.Vector3.back * .5f);
            result[2] = UnityEngine.Vector3.zero;

            //vertex between 0 and 1
            //use trivert() to fold outer vertices up relative to inner vertices
            result[3] = triVert(result[0], result[1], result[2], degreefolded);

            //vertex between 1 and 2
            result[4] = triVert(result[1], result[2], result[0], degreefolded);
            //vertex between 0 and 2
            result[5] = triVert(result[2], result[0], result[1], degreefolded);
            return result;
        }

        /// <summary>
        ///     function to calculate outer vertices position relative to inner vertices
        /// </summary>
        /// <param name="nSegmentA"></param>
        /// <param name="nSegmentB"></param>
        /// <param name="oppositePoint"></param>
        /// <param name="degreeFolded"></param>
        /// <returns></returns>
        private static UnityEngine.Vector3 triVert(UnityEngine.Vector3 nSegmentA, UnityEngine.Vector3 nSegmentB,
            UnityEngine.Vector3 oppositePoint, float degreeFolded)
        {
            return (UnityEngine.Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) *
                    (oppositePoint - ((nSegmentA + nSegmentB) / 2f))) + ((nSegmentA + nSegmentB) / 2f);
        }

        /// <summary>
        ///     return array of vertices for the 4 triangles in the unfolded tetrahedron
        /// </summary>
        /// <returns></returns>
        private static int[] meshTris()
        {
            return new[]
            {
                0, 1, 2,
                0, 3, 1,
                2, 1, 4,
                2, 5, 0
            };
        }

        /// <summary>
        ///     trace edges of mesh
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static UnityEngine.Vector3[] lineRendererVerts(float t)
        {
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[11];

            UnityEngine.Vector3[] tmp = meshVerts(t);
            //map vertices on line segment(s) to vertices on tetrahedron net
            result[0] = tmp[0];
            result[1] = tmp[3];
            result[2] = tmp[1];
            result[3] = tmp[0];
            result[4] = tmp[5];
            result[5] = tmp[2];
            result[6] = tmp[0];
            result[7] = tmp[1];
            result[8] = tmp[4];
            result[9] = tmp[2];
            result[10] = tmp[1];
            return result;
        }
    }
}