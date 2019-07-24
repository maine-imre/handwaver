namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// cross section of a sphere, resulting in either a circle, a point, or nothing
    /// </summary>
    public class SphereCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
        #region variables/components

        public int n;
        private UnityEngine.Mesh sphereRenderer => GetComponent<UnityEngine.MeshFilter>().mesh;

        private UnityEngine.LineRenderer crossSectionRenderer =>
            transform.GetChild(0).GetComponent<UnityEngine.LineRenderer>();

        public UnityEngine.Material sphereMaterial;
        public UnityEngine.Material crossSectionMaterial;

        public float radius = 1f;
        public UnityEngine.Vector3 center = UnityEngine.Vector3.zero;
        public UnityEngine.Vector3 normal = UnityEngine.Vector3.up;

        public bool debugRenderer = SpencerStudyControl.debugRendererXC;

        #endregion

        //slider to determine cross section height
        public float slider
        {
            //scale value from 0 to 1 range to -1 to 1 range.
            set => crossSectSphere(-1 + (value * 2));
        }

        // Start is called before the first frame update
        private void Start()
        {
            #region Render Cross-section

            gameObject.AddComponent<UnityEngine.MeshRenderer>();
            gameObject.AddComponent<UnityEngine.MeshFilter>();
            GetComponent<UnityEngine.MeshRenderer>().material = sphereMaterial;
            gameObject.GetComponent<UnityEngine.MeshRenderer>().enabled = debugRenderer;
            IMRE.HandWaver.ScaleDimension.RenderMethods.RenderSphere(radius, new Unity.Mathematics.float3(0f,0f,0f), sphereRenderer, n);

            UnityEngine.GameObject child = new UnityEngine.GameObject();
            child.transform.parent = transform;
            child.transform.localPosition = UnityEngine.Vector3.zero;
            child.AddComponent<UnityEngine.LineRenderer>();
            crossSectionRenderer.material = crossSectionMaterial;
            crossSectionRenderer.useWorldSpace = false;

            crossSectionRenderer.startWidth = SpencerStudyControl.lineRendererWidth;
            crossSectionRenderer.endWidth = SpencerStudyControl.lineRendererWidth;
            crossSectionRenderer.loop = true;
                           
            #endregion  
        }

        /// <summary>
        /// Function to calculate cross section of sphere
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public void crossSectSphere(float height)
        {
            //endpoints for line segment if intersection passes through circle

            //if cross section only hits the edge of the circle
            if (Unity.Mathematics.math.abs(height) == radius)
            {
                //if top of sphere, create point at intersection
                if (height == radius)
                {
                    UnityEngine.Vector3 segmentEndPoint0 = UnityEngine.Vector3.up * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.positionCount = 2;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);
                }

                //if bottom of circle, create point at intersection
                else if (height == -radius)
                {
                    UnityEngine.Vector3 segmentEndPoint0 = UnityEngine.Vector3.down * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.positionCount = 2;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);
                }
            }

            //cross section is a circle
            else if (Unity.Mathematics.math.abs(height) < radius)
            {
                //horizontal distance from center of circle to point on line segment

                renderCircle(
                    UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(radius, 2) - UnityEngine.Mathf.Pow(height, 2)),
                    height * UnityEngine.Vector3.up);
            }

            //height for cross section is outside of circle 
            else if (Unity.Mathematics.math.abs(height) > radius)
            {
                UnityEngine.Debug.Log("Height is out of range of object.");
                crossSectionRenderer.enabled = false;
            }
        }

        /// <summary>
        /// Function that renders a circle using a centerpoint coordinate and a radius
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="center"></param>
        private void renderCircle(float radius, UnityEngine.Vector3 center)
        {
            //worldspace rendering of the circle

            //normal vectors
            UnityEngine.Vector3 norm1 = UnityEngine.Vector3.forward;
            UnityEngine.Vector3 norm2 = UnityEngine.Vector3.right;

            //array of vector3s for vertices
            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[n];

            //math for rendering circle
            for (int i = 0; i < n; i++)
            {
                vertices[i] = (radius * ((UnityEngine.Mathf.Sin((i * UnityEngine.Mathf.PI * 2) / (n - 1)) * norm1) +
                                         (UnityEngine.Mathf.Cos((i * UnityEngine.Mathf.PI * 2) / (n - 1)) * norm2))) +
                              center;
            }

            //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            crossSectionRenderer.positionCount = n;
            crossSectionRenderer.SetPositions(vertices);
        }
    }
}
