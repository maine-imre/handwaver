namespace IMRE.HandWaver.ScaleStudy
{
    public class net2D : UnityEngine.MonoBehaviour, ISliderInput
    {
        private float _percentFolded;

        public System.Collections.Generic.List<UnityEngine.GameObject> foldPoints =
            new System.Collections.Generic.List<UnityEngine.GameObject>();

        public bool sliderOverride;

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

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

	public abstract UnityEngine.Vector3[] verts(float percentFolded);
    }
}
