namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// cross section of hypersphere and hyperplane, resulting in a sphere with varying size
    /// </summary>
    public class HyperSphereCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
        //slider to control cross section height
        public float slider
        {
            set
            {
                //using solution from here:  https://math.stackexchange.com/questions/1159613/would-the-cross-section-of-a-hypersphere-be-a-sphere
                float sliderval = -1 + (2 * value);

                IMRE.HandWaver.ScaleDimension.RenderMethods.RenderSphere(UnityEngine.Mathf.Sqrt((radius * radius) - (sliderval * sliderval)), new Unity.Mathematics.float3(0f,0f,0f), crossSectionRenderer, n);
            }
        }

        private void Start()
        {
            gameObject.AddComponent<UnityEngine.MeshRenderer>();
            gameObject.AddComponent<UnityEngine.MeshFilter>();
            GetComponent<UnityEngine.MeshRenderer>().material = sphereMaterial;
        }

        #region variables/components

        public int n;
        private UnityEngine.Mesh crossSectionRenderer => GetComponent<UnityEngine.MeshFilter>().mesh;
        public UnityEngine.Material sphereMaterial;
        public float radius = 1f;

        #endregion
    }
}
