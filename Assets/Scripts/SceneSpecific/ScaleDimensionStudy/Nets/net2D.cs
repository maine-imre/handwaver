namespace IMRE.HandWaver.ScaleStudy
{
    public abstract class net2D : UnityEngine.MonoBehaviour, ISliderInput
    {
        private float _percentFolded;

        public System.Collections.Generic.List<UnityEngine.GameObject> foldPoints =
            new System.Collections.Generic.List<UnityEngine.GameObject>();

        public bool sliderOverride;

        /// <summary>
        /// float to set fold percentage to position vertices
        /// </summary>
        public float PercentFolded
        {
            get => _percentFolded;

            set
            {
                //set vertices using vert function
                _percentFolded = value;
                GetComponent<UnityEngine.LineRenderer>().SetPositions(verts(_percentFolded));
            }
        }

        /// <summary>
        /// slider to control fold percent
        /// </summary>
        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

        /// <summary>
        /// abstract function for positioning vertices based on how folded the 2d net is
        /// </summary>
        /// <param name="percentFolded"></param>
        /// <returns></returns>
	public abstract UnityEngine.Vector3[] verts(float percentFolded);
    }
}
