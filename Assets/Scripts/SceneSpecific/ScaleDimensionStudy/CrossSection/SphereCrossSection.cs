namespace IMRE.HandWaver.ScaleStudy
{
    public class SphereCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
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
            renderSphere();

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
        ///     Function to calculate cross section of circle
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

        private void renderSphere()
        {
            sphereRenderer.Clear();
            int nbLong = n;
            int nbLat = n;

            #region Vertices

            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[((nbLong + 1) * nbLat) + 2];
            float pi = UnityEngine.Mathf.PI;
            float _2pi = pi * 2f;

            vertices[0] = UnityEngine.Vector3.up * radius;
            for (int lat = 0; lat < nbLat; lat++)
            {
                float a1 = (pi * (lat + 1)) / (nbLat + 1);
                float sin1 = UnityEngine.Mathf.Sin(a1);
                float cos1 = UnityEngine.Mathf.Cos(a1);

                for (int lon = 0; lon <= nbLong; lon++)
                {
                    float a2 = (_2pi * (lon == nbLong ? 0 : lon)) / nbLong;
                    float sin2 = UnityEngine.Mathf.Sin(a2);
                    float cos2 = UnityEngine.Mathf.Cos(a2);

                    vertices[lon + (lat * (nbLong + 1)) + 1] =
                        new UnityEngine.Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }

            vertices[vertices.Length - 1] = UnityEngine.Vector3.up * -radius;

            #endregion

            #region Normals		

            UnityEngine.Vector3[] normales = new UnityEngine.Vector3[vertices.Length];
            for (int n = 0; n < vertices.Length; n++)
                normales[n] = vertices[n].normalized;

            #endregion

            #region UVs

            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[vertices.Length];
            uvs[0] = UnityEngine.Vector2.up;
            uvs[uvs.Length - 1] = UnityEngine.Vector2.zero;
            for (int lat = 0; lat < nbLat; lat++)
            for (int lon = 0; lon <= nbLong; lon++)
            {
                uvs[lon + (lat * (nbLong + 1)) + 1] =
                    new UnityEngine.Vector2((float) lon / nbLong, 1f - ((float) (lat + 1) / (nbLat + 1)));
            }

            #endregion

            #region Triangles

            int nbFaces = vertices.Length;
            int nbTriangles = nbFaces * 2;
            int nbIndexes = nbTriangles * 3;
            int[] triangles = new int[nbIndexes];

            //Top Cap
            int i = 0;
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = lon + 2;
                triangles[i++] = lon + 1;
                triangles[i++] = 0;
            }

            //Middle
            for (int lat = 0; lat < (nbLat - 1); lat++)
            {
                for (int lon = 0; lon < nbLong; lon++)
                {
                    int current = lon + (lat * (nbLong + 1)) + 1;
                    int next = current + nbLong + 1;

                    triangles[i++] = current;
                    triangles[i++] = current + 1;
                    triangles[i++] = next + 1;

                    triangles[i++] = current;
                    triangles[i++] = next + 1;
                    triangles[i++] = next;
                }
            }

            //Bottom Cap
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = vertices.Length - 1;
                triangles[i++] = vertices.Length - (lon + 2) - 1;
                triangles[i++] = vertices.Length - (lon + 1) - 1;
            }

            #endregion

            sphereRenderer.vertices = vertices;
            sphereRenderer.normals = normales;
            sphereRenderer.uv = uvs;
            sphereRenderer.triangles = triangles;

            sphereRenderer.RecalculateBounds();
        }

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
    }
}