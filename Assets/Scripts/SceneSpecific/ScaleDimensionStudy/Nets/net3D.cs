namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    ///     A net of a cube that folds into a cube
    ///     The main contributor(s) to this script is __
    ///     Status: ???
    /// </summary>
    public class cubeNet : UnityEngine.MonoBehaviour, ISliderInput
    {
        private float _percentFolded;

        public bool sliderOverride;

        //public DateTime startTime;
        public UnityEngine.Mesh mesh => GetComponent<UnityEngine.MeshFilter>().mesh;

        public UnityEngine.LineRenderer lineRenderer => GetComponent<UnityEngine.LineRenderer>();

        public float PercentFolded
        {
            get => _percentFolded;
            //set positions for linerenderer and vertices for mesh
            set
            {
                //set vertices on line segment
                _percentFolded = value;
                lineRenderer.SetPositions(lineRendererVerts(_percentFolded));
                //array of vertices converted to list
                mesh.SetVertices(Enumerable.ToList(meshVerts(_percentFolded)));
            }
        }

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

	public abstract static UnityEngine.Vector3[] meshVerts(float percentFolded);
        public abstract static UnityEngine.Vector3[] lineRendererVerts(float percentFolded)
    }
}
