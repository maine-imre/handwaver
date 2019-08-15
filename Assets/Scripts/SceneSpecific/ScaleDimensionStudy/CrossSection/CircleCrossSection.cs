namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// cross-section of a circle represented by two points where intersection occurs in circle
    /// or one point if it only hits an edge 
    /// </summary>
    public class CircleCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
        //slider to control the cross section
        public float slider
        {
            //value ranges from 0 to 1, scale to -1 to 1
            set => crossSectCirc(-1 + (value * 2));
        }

        // Start is called before the first frame update
        private void Start()
        {
            #region Render

            gameObject.AddComponent<UnityEngine.LineRenderer>();
            circleRenderer.material = circleMaterial;
            circleRenderer.startWidth = .005f;
            circleRenderer.endWidth = .005f;
            circleRenderer.enabled = debugRenderer;
            renderCircle();

            UnityEngine.GameObject child = new UnityEngine.GameObject();
            child.transform.parent = transform;
            child.AddComponent<UnityEngine.LineRenderer>();
            crossSectionRenderer.material = crossSectionMaterial;
            crossSectionRenderer.startWidth = SpencerStudyControl.lineRendererWidth;
            crossSectionRenderer.endWidth = SpencerStudyControl.lineRendererWidth;
            
            crossSectionRenderer.enabled = debugRenderer;

            //generate four points to show crossSections.
            crossSectionPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            crossSectionPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            crossSectionPoints.ForEach(p => p.transform.SetParent(transform));

            #endregion
        }

        /// <summary>
        /// Function to calculate cross section of circle by connecting points of intersection
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public void crossSectCirc(float height)
        {
            //endpoints for line segment if intersection passes through circle
            UnityEngine.Vector3 segmentEndPoint0;
            UnityEngine.Vector3 segmentEndPoint1;

            //if cross section only hits the edge of the circle
            if (Unity.Mathematics.math.abs(height) == radius)
            {
                //if top of circle, create point at intersection
                if (height == radius)
                {
                    segmentEndPoint0 = UnityEngine.Vector3.up * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);

                    crossSectionPoints[0].transform.localPosition = segmentEndPoint0;
                    crossSectionPoints[0].SetActive(true);
                    crossSectionPoints[1].SetActive(false);
                }

                //if bottom of circle, create point at intersection
                else if (height == -radius)
                {
                    segmentEndPoint0 = UnityEngine.Vector3.down * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);

                    crossSectionPoints[0].transform.localPosition = segmentEndPoint0;
                    crossSectionPoints[0].SetActive(true);
                    crossSectionPoints[1].SetActive(false);
                }

                //TODO update rendering
            }

            //cross section is a line that hits two points on the circle (height smaller than radius of circle)
            else if (Unity.Mathematics.math.abs(height) < radius)
            {
                //horizontal distance from center of circle to point on line segment
                float segmentLength = UnityEngine.Mathf.Sqrt(1f - UnityEngine.Mathf.Pow(height, 2));

                //calculations for endpoint coordinates of line segment
                segmentEndPoint0 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.left * segmentLength);
                segmentEndPoint1 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.right * segmentLength);

                crossSectionRenderer.enabled = true;
                crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                crossSectionRenderer.SetPosition(1, segmentEndPoint1);

                crossSectionPoints[0].transform.localPosition = segmentEndPoint0;
                crossSectionPoints[0].SetActive(true);
                crossSectionPoints[1].transform.localPosition = segmentEndPoint1;
                crossSectionPoints[1].SetActive(true);
            }

            //height for cross section is outside of circle 
            else if (Unity.Mathematics.math.abs(height) > radius)
            {
                UnityEngine.Debug.Log("Height is out of range of object.");
                //TODO update rendering
                crossSectionRenderer.enabled = false;

                crossSectionPoints[0].SetActive(false);
                crossSectionPoints[1].SetActive(false);
            }
            crossSectionRenderer.enabled = debugRenderer;

        }

        /// <summary>
        /// Function to render the outline of a circle
        /// </summary>
        public void renderCircle()
        {
            //normal vectors
            UnityEngine.Vector3 norm1 = UnityEngine.Vector3.up;
            UnityEngine.Vector3 norm2 = UnityEngine.Vector3.right;

            //array of vector3s for vertices
            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[n];

            //math for rendering circle
            for (int i = 0; i < n; i++)
            {
                vertices[i] = (radius * (UnityEngine.Mathf.Sin((i * UnityEngine.Mathf.PI * 2) / (n - 1)) * norm1)) +
                              (radius * (UnityEngine.Mathf.Cos((i * UnityEngine.Mathf.PI * 2) / (n - 1)) * norm2));
            }

            //Render circle
            UnityEngine.LineRenderer lineRenderer = GetComponent<UnityEngine.LineRenderer>();
            //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.startColor = UnityEngine.Color.blue;
            lineRenderer.endColor = UnityEngine.Color.blue;
            lineRenderer.startWidth = SpencerStudyControl.lineRendererWidth;
            lineRenderer.endWidth = SpencerStudyControl.lineRendererWidth;
            lineRenderer.positionCount = n;
            lineRenderer.SetPositions(vertices);
        }

        #region Variables/Components

        public int n = 5;
        public float radius = 1f;

        private UnityEngine.LineRenderer circleRenderer => GetComponent<UnityEngine.LineRenderer>();

        private UnityEngine.LineRenderer crossSectionRenderer =>
            transform.GetChild(0).GetComponent<UnityEngine.LineRenderer>();

        public UnityEngine.Material circleMaterial;
        public UnityEngine.Material crossSectionMaterial;
        public bool debugRenderer = SpencerStudyControl.debugRendererXC;

        public System.Collections.Generic.List<UnityEngine.GameObject> crossSectionPoints =
            new System.Collections.Generic.List<UnityEngine.GameObject>();

        #endregion
    }
}