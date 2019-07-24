using Enumerable = System.Linq.Enumerable;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// cross-section of an annulus represented by points where intersection between annulus and line segment occurs 
    /// </summary>
    public class AnnulusCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
        public int n = 5;

        //slider to control cross section
        public float slider
        {
            //value ranges from 0 to 1, scale to -1 to 1
            set => crossSectAnnulus(-1 + (value * 2));
        }

        // Start is called before the first frame update
        private void Start()
        {
            gameObject.AddComponent<UnityEngine.MeshRenderer>();
            gameObject.AddComponent<UnityEngine.MeshFilter>();
            gameObject.GetComponent<UnityEngine.MeshRenderer>().material = annulusMaterial;
            gameObject.GetComponent<UnityEngine.MeshRenderer>().enabled = SpencerStudyControl.debugRendererXC;

            #region Vertices/Triangles

            //number of vertices within pre-rotated square
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[(n + 1) * n];

            //Array of 2d vectors for uv mapping
            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[verts.Length];
            float oneNth = 1f / n;
            float oneNMinusTwoth = 1f / (n - 2);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < (n - 1); j++)
                {
                    //index value computation
                    int idx = i + (n * j);

                    //find radian value of length of curve
                    float alpha = i * oneNth;
                    float beta = j * oneNMinusTwoth;

                    //map vertices from 2 dimensions to 3
                    verts[idx] = annulusPosition(beta, alpha);

                    //uv mapping 
                    uvs[idx] = new UnityEngine.Vector2(alpha, beta);
                }
            }

            //3 vertices x 2 triangles per square x dimension of square
            int[] triangles = new int[6 * n * n];

            for (int k = 0; k < (2 * n * n); k++)
            {
                //for each vertex on each triangle
                for (int m = 0; m < 3; m++)
                    triangles[(3 * k) + m] = triangle(k, m);
            }

            annulusRenderer.vertices = verts;
            annulusRenderer.triangles = triangles;
            annulusRenderer.uv = uvs;
            annulusRenderer.RecalculateNormals();
            annulusRenderer.Optimize();

            #endregion

            #region CrossSections' GameObjects

            UnityEngine.GameObject child = new UnityEngine.GameObject();
            child.transform.parent = transform;
            child.transform.localPosition = UnityEngine.Vector3.zero;
            child.AddComponent<UnityEngine.LineRenderer>();

            UnityEngine.GameObject child2 = new UnityEngine.GameObject();
            child2.transform.parent = transform;
            child2.transform.localPosition = UnityEngine.Vector3.zero;
            child2.AddComponent<UnityEngine.LineRenderer>();

            child.GetComponent<UnityEngine.LineRenderer>().endWidth = SpencerStudyControl.lineRendererWidth;
            child.GetComponent<UnityEngine.LineRenderer>().startWidth = SpencerStudyControl.lineRendererWidth;
            child.GetComponent<UnityEngine.LineRenderer>().enabled = false && SpencerStudyControl.debugRendererXC;
            child2.GetComponent<UnityEngine.LineRenderer>().endWidth = SpencerStudyControl.lineRendererWidth;
            child2.GetComponent<UnityEngine.LineRenderer>().startWidth = SpencerStudyControl.lineRendererWidth;
            child2.GetComponent<UnityEngine.LineRenderer>().enabled = false && SpencerStudyControl.debugRendererXC;

            child2.GetComponent<UnityEngine.LineRenderer>().useWorldSpace = false;
            child.GetComponent<UnityEngine.LineRenderer>().useWorldSpace = false;

            Enumerable.ToList(crossSectionRenderer).ForEach(r => r.material = crossSectionMaterial);

            //generate four points to show crossSections.
            crossSectionPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            crossSectionPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            crossSectionPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            crossSectionPoints.Add(Instantiate(SpencerStudyControl.ins.pointPrefab));
            crossSectionPoints.ForEach(p => p.transform.SetParent(transform));

            #endregion
        }

        /// <summary>
        /// Function to calculate cross section of an annulus by finding points of intersection and connecting them
        /// </summary>
        /// <param name="height"></param>
        public void crossSectAnnulus(float height)
        {
            //point if intersection hits edge, or points for line segment(s) if it passes through annulus
            UnityEngine.Vector3 segmentAEndPoint0, segmentAEndPoint1, segmentBEndPoint0, segmentBEndPoint1;

            //horizontal distance from center of annulus to points on line segment(s)
            float x1;
            float x2;

            //cross-section only hits edge of annulus
            if (Unity.Mathematics.math.abs(height) == outerRadius)
            {
                //if top edge, create point at intersection
                if (height == outerRadius)
                    segmentAEndPoint0 = UnityEngine.Vector3.up * outerRadius;
                //if bottom edge, create point at intersection
                else
                    segmentAEndPoint0 = UnityEngine.Vector3.down * outerRadius;

                crossSectionRenderer[0].enabled = true && SpencerStudyControl.debugRendererXC;
                crossSectionRenderer[0].SetPosition(0, segmentAEndPoint0);
                crossSectionRenderer[0].SetPosition(1, segmentAEndPoint0);
                crossSectionRenderer[1].enabled = false && SpencerStudyControl.debugRendererXC;

                crossSectionPoints[0].transform.localPosition = segmentAEndPoint0;
                crossSectionPoints[0].SetActive(true);
                crossSectionPoints[1].SetActive(false);
                crossSectionPoints[2].SetActive(false);
                crossSectionPoints[3].SetActive(false);
            }
            //cross section is a line segment in between the inner circle and outer circle
            else if ((Unity.Mathematics.math.abs(height) < outerRadius) &&
                     (Unity.Mathematics.math.abs(height) >= innerRadius))
            {
                //horizontal distance from center to point on outer edge of annulus
                x1 = UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(outerRadius, 2) - UnityEngine.Mathf.Pow(height, 2));

                //calculations for coordinates of line segment endpoints
                segmentAEndPoint0 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.right * x1);
                segmentAEndPoint1 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.left * x1);

                crossSectionRenderer[0].enabled = true && SpencerStudyControl.debugRendererXC;
                crossSectionRenderer[0].SetPosition(0, segmentAEndPoint0);
                crossSectionRenderer[0].SetPosition(1, segmentAEndPoint1);
                crossSectionRenderer[1].enabled = false && SpencerStudyControl.debugRendererXC;

                crossSectionPoints[0].transform.localPosition = segmentAEndPoint0;
                crossSectionPoints[1].transform.localPosition = segmentAEndPoint1;
                crossSectionPoints[0].SetActive(true);
                crossSectionPoints[1].SetActive(true);
                crossSectionPoints[2].SetActive(false);
                crossSectionPoints[3].SetActive(false);
            }
            //cross section height is less than the inner radius, resulting in two line segments
            else if (Unity.Mathematics.math.abs(height) < innerRadius)
            {
                //horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
                x1 = UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(outerRadius, 2) - UnityEngine.Mathf.Pow(height, 2));
                x2 = UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(innerRadius, 2) - UnityEngine.Mathf.Pow(height, 2));

                //calculations for inner and outer endpoints for each line segment
                segmentAEndPoint0 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.left * x1);
                segmentAEndPoint1 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.left * x2);

                segmentBEndPoint0 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.right * x2);
                segmentBEndPoint1 = (UnityEngine.Vector3.up * height) + (UnityEngine.Vector3.right * x1);
                crossSectionRenderer[0].enabled = true && SpencerStudyControl.debugRendererXC;
                crossSectionRenderer[1].enabled = true && SpencerStudyControl.debugRendererXC;

                crossSectionRenderer[0].SetPosition(0, segmentAEndPoint0);
                crossSectionRenderer[0].SetPosition(1, segmentAEndPoint1);
                crossSectionRenderer[1].SetPosition(0, segmentBEndPoint0);
                crossSectionRenderer[1].SetPosition(1, segmentBEndPoint1);

                crossSectionPoints[0].transform.localPosition = segmentAEndPoint0;
                crossSectionPoints[1].transform.localPosition = segmentAEndPoint1;
                crossSectionPoints[2].transform.localPosition = segmentBEndPoint0;
                crossSectionPoints[3].transform.localPosition = segmentBEndPoint1;
                crossSectionPoints[0].SetActive(true);
                crossSectionPoints[1].SetActive(true);
                crossSectionPoints[2].SetActive(true);
                crossSectionPoints[3].SetActive(true);
            }
            //cross section height is out of range of annulus
            else if (Unity.Mathematics.math.abs(height) > outerRadius)
            {
                UnityEngine.Debug.Log("Height is out of range of object.");
                //TODO update rendering
                crossSectionRenderer[0].enabled = false && SpencerStudyControl.debugRendererXC;
                crossSectionRenderer[1].enabled = false && SpencerStudyControl.debugRendererXC;

                crossSectionPoints[0].SetActive(false);
                crossSectionPoints[1].SetActive(false);
                crossSectionPoints[2].SetActive(false);
                crossSectionPoints[3].SetActive(false);
            }
        }

        /// <summary>
        /// Take alpha to be the angle describing the rotation around a center axis to form the annulus
        /// and beta as the length to be converted
        /// Find position on annulus
        /// </summary>
        /// <param name="alpha">angle</param>
        /// <param name="beta">length</param>
        /// <returns></returns>
        private UnityEngine.Vector3 annulusPosition(float alpha, float beta)
        {
            //conversion factor
            float length = (beta * (outerRadius - innerRadius)) + innerRadius;
            //rotation mapping
            UnityEngine.Vector3 result =
                ((UnityEngine.Vector3.right * UnityEngine.Mathf.Cos(alpha * 2 * UnityEngine.Mathf.PI)) +
                 (UnityEngine.Vector3.up * UnityEngine.Mathf.Sin(alpha * 2 * UnityEngine.Mathf.PI))) * length;

            return result;
        }

        /// <summary>
        /// Function to map vertices on each triangle
        /// Mapping was copied from torusRevolve script in horizon scene
        /// </summary>
        /// <param name="k">triangle</param>
        /// <param name="m">vertex</param>
        /// <returns>index value of vertex</returns>
        private int triangle(int k, int m)
        {
            if (k < (n * n))
            {
                //lower half triangles first 
                switch (m)
                {
                    //simply the index of the triangle
                    case 0:
                        return k;
                    //2nd vertex-same thing
                    case 1:
                        return ((UnityEngine.Mathf.FloorToInt(k / n) + 1) * n) + ((k + 1) % n);
                    //3rd vertex calculation-wrap around horizontally but not vertically
                    case 2:
                        return (UnityEngine.Mathf.FloorToInt(k / n) * n) + ((k + 1) % n);
                }
            }
            //upper half triangles
            else
            {
                k = k - (n * n);

                switch (m)
                {
                    //same logic as with previous cases
                    case 0:
                        return k;
                    case 1:
                        return ((UnityEngine.Mathf.FloorToInt(k / n) + 1) * n) + (k % n);
                    case 2:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + ((k + 1) % n);
                }
            }

            UnityEngine.Debug.LogError("Invalid parameter.");

            return 0;
        }

        #region Components/Variables

        private UnityEngine.Mesh annulusRenderer => GetComponent<UnityEngine.MeshFilter>().mesh;
        private UnityEngine.LineRenderer[] crossSectionRenderer => GetComponentsInChildren<UnityEngine.LineRenderer>();

        public float innerRadius = 0.75f;
        public float outerRadius = 1f;

        public UnityEngine.Material annulusMaterial;
        public UnityEngine.Material crossSectionMaterial;

        public System.Collections.Generic.List<UnityEngine.GameObject> crossSectionPoints =
            new System.Collections.Generic.List<UnityEngine.GameObject>();

        #endregion
    }
}