namespace IMRE.HandWaver.ScaleStudy
{
    public abstract class net3D : UnityEngine.MonoBehaviour, ISliderInput
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
                mesh.SetVertices(System.Linq.Enumerable.ToList(meshVerts(_percentFolded)));
            }
        }

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }
        
        /// <summary>
        /// abstract function for positioning mesh vertices for rendering based on percent that the net is folded
        /// </summary>
        /// <param name="percentFolded"></param>
        /// <returns></returns>
	    public abstract UnityEngine.Vector3[] meshVerts(float percentFolded);
        
        /// <summary>
        /// abstract function for positioning line renderers based on percent that the net is folded
        /// </summary>
        /// <param name="percentFolded"></param>
        /// <returns></returns>
        public abstract UnityEngine.Vector3[] lineRendererVerts(float percentFolded);
    }
}
