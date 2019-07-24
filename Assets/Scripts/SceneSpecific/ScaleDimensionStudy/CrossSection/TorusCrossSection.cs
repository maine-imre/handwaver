using Enumerable = System.Linq.Enumerable;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// cross section of torus, which could result in one of several different cross-sections depending on the height
    /// which are all covered by "spiric sections"
    /// </summary>
    public class TorusCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
        //radius of 2d circle for torus
        public float circleRadius = .5f;
        public UnityEngine.Material crossSectionMaterial;

        public bool debugRenderer = SpencerStudyControl.debugRendererXC;
        public int n;

        //distance from axis that circle will be rotated on
        public float revolveRadius = 1f;

        public UnityEngine.Material torusMaterial;

        private UnityEngine.Mesh torusRenderer => GetComponent<UnityEngine.MeshFilter>().mesh;
        private UnityEngine.LineRenderer[] crossSectionRenderer => GetComponentsInChildren<UnityEngine.LineRenderer>();

        public float slider
        {
            //scale value from 0 to 1 to -1 to 1
            set => crossSectTorus(-1 + (value * 2));
        }

        // Start is called before the first frame update
        private void Start()
        {
            gameObject.AddComponent<UnityEngine.MeshRenderer>();
            gameObject.AddComponent<UnityEngine.MeshFilter>();
            GetComponent<UnityEngine.MeshRenderer>().material = torusMaterial;
            gameObject.GetComponent<UnityEngine.MeshRenderer>().enabled = debugRenderer;
            renderTorus();

            UnityEngine.GameObject child = new UnityEngine.GameObject();
            child.transform.parent = transform;
            child.AddComponent<UnityEngine.LineRenderer>();

            UnityEngine.GameObject child2 = new UnityEngine.GameObject();
            child2.transform.parent = transform;
            child2.AddComponent<UnityEngine.LineRenderer>();

            UnityEngine.GameObject child3 = new UnityEngine.GameObject();
            child3.transform.parent = transform;
            child3.AddComponent<UnityEngine.LineRenderer>();

            UnityEngine.GameObject child4 = new UnityEngine.GameObject();
            child4.transform.parent = transform;
            child4.AddComponent<UnityEngine.LineRenderer>();

            Enumerable.ToList(crossSectionRenderer).ForEach(r => r.material = crossSectionMaterial);
            Enumerable.ToList(crossSectionRenderer).ForEach(r => r.startWidth = .05f);
            Enumerable.ToList(crossSectionRenderer).ForEach(r => r.endWidth = .05f);
            Enumerable.ToList(crossSectionRenderer).ForEach(r => r.loop = false);
            Enumerable.ToList(crossSectionRenderer).ForEach(r => r.positionCount = n);
        }

        /// <summary>
        /// Function to calculate break toric cross-section into different cases depending on the height it occurs
        /// </summary>
        /// <param name="height"></param>
        public void crossSectTorus(float height)
        {
            //this function is broken into cases where each function is well behaved.
            if (Unity.Mathematics.math.abs(height) < (revolveRadius - circleRadius))
            {
                float oneNth = 1f / n;

                for (int i = 0; i < n; i++)
                {
                    crossSectionRenderer[0].SetPosition(i, spiricMath(i * oneNth, height, 0f, 0f, 0));
                    crossSectionRenderer[1].SetPosition(i, spiricMath(i * oneNth, height, 0f, 0f, 1));
                    crossSectionRenderer[2].SetPosition(i, spiricMath(i * oneNth, height, 0f, 0f, 2));
                    crossSectionRenderer[3].SetPosition(i, spiricMath(i * oneNth, height, 05f, 0f, 3));
                    //may need to use reflection.
                }

                Enumerable.ToList(crossSectionRenderer).ForEach(r => r.enabled = true);
            }
            else if (Unity.Mathematics.math.abs(height) < (revolveRadius + circleRadius))
            {
                //there is only one spiric
                for (int i = 0; i < n; i++)
                {
                    float theta = i * (1f / n) * UnityEngine.Mathf.PI * 2;
                    crossSectionRenderer[0].SetPosition(i, spiricOutsideMath(theta, height));
                }

                Enumerable.ToList(crossSectionRenderer).ForEach(r => r.enabled = false);
                crossSectionRenderer[0].enabled = true;
            }
            else
            {
                Enumerable.ToList(crossSectionRenderer).ForEach(r => r.enabled = false);
            }
        }

        /// <summary>
        /// Function that calculates the math for the cross-section of torus
        /// using the distance from the center of the torus, 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="height"></param>
        /// <param name="alpha"></param>
        /// <param name="phi"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private Unity.Mathematics.float3 spiricMath(float v, float height, float alpha, float phi, int idx)
        {
            //uses method described here: arXiv:1708.00803v2 [math.GM] 6 Aug 2017
            float p = Unity.Mathematics.math.abs(height);
            float x_q = p * Unity.Mathematics.math.sin(alpha) * Unity.Mathematics.math.cos(phi);
            float y_q = p * Unity.Mathematics.math.sin(alpha) * Unity.Mathematics.math.cos(phi);
            float z_q = p * Unity.Mathematics.math.sin(phi);
            float R = revolveRadius;
            float r = circleRadius;
            float w = .25f * v;

            //v ranges from -1 to 1
            //make two values of 2 to accommodate different solution constraints
            //TODO fix these bounds.  the bounds assume phi = 0 and alpha = 0

            //need to project onto different ranges for each solution path.
            float t_0, t_1, t_2, t_3;

            float dist0 = Unity.Mathematics.math.sqrt(Unity.Mathematics.math.pow(r, 2) -
                                                      Unity.Mathematics.math.pow(
                                                          (w * Unity.Mathematics.math.cos(phi)) +
                                                          (p * Unity.Mathematics.math.sin(phi)), 2));
            float dist1 = Unity.Mathematics.math.sqrt(Unity.Mathematics.math.pow(r, 2) -
                                                      Unity.Mathematics.math.pow(
                                                          (w * Unity.Mathematics.math.cos(phi)) +
                                                          (p * Unity.Mathematics.math.sin(phi)), 2));
            t_0 = Unity.Mathematics.math.sqrt(
                -Unity.Mathematics.math.pow(
                    (p * Unity.Mathematics.math.cos(phi)) - (w * Unity.Mathematics.math.sin(phi)), 2) +
                Unity.Mathematics.math.pow(R + dist0, 2));
            t_1 = -t_0;
            t_2 = Unity.Mathematics.math.sqrt(
                -Unity.Mathematics.math.pow(
                    (p * Unity.Mathematics.math.cos(phi)) - (w * Unity.Mathematics.math.sin(phi)), 2) +
                Unity.Mathematics.math.pow(R - dist1, 2));
            t_3 = -t_2;

            Unity.Mathematics.float3 c0 = new Unity.Mathematics.float3(
                (x_q + (t_0 * Unity.Mathematics.math.sin(alpha))) -
                (w * Unity.Mathematics.math.cos(alpha) * Unity.Mathematics.math.sin(phi)),
                y_q - (t_0 * Unity.Mathematics.math.cos(alpha)) -
                (w * Unity.Mathematics.math.sin(alpha) * Unity.Mathematics.math.sin(phi)),
                z_q + (w * Unity.Mathematics.math.cos(phi)));
            Unity.Mathematics.float3 c1 = new Unity.Mathematics.float3(
                (x_q + (t_1 * Unity.Mathematics.math.sin(alpha))) -
                (w * Unity.Mathematics.math.cos(alpha) * Unity.Mathematics.math.sin(phi)),
                y_q - (t_1 * Unity.Mathematics.math.cos(alpha)) -
                (w * Unity.Mathematics.math.sin(alpha) * Unity.Mathematics.math.sin(phi)),
                z_q + (w * Unity.Mathematics.math.cos(phi)));
            Unity.Mathematics.float3 c2 = new Unity.Mathematics.float3(
                (x_q + (t_2 * Unity.Mathematics.math.sin(alpha))) -
                (w * Unity.Mathematics.math.cos(alpha) * Unity.Mathematics.math.sin(phi)),
                y_q - (t_2 * Unity.Mathematics.math.cos(alpha)) -
                (w * Unity.Mathematics.math.sin(alpha) * Unity.Mathematics.math.sin(phi)),
                z_q + (w * Unity.Mathematics.math.cos(phi)));
            Unity.Mathematics.float3 c3 = new Unity.Mathematics.float3(
                (x_q + (t_3 * Unity.Mathematics.math.sin(alpha))) -
                (w * Unity.Mathematics.math.cos(alpha) * Unity.Mathematics.math.sin(phi)),
                y_q - (t_3 * Unity.Mathematics.math.cos(alpha)) -
                (w * Unity.Mathematics.math.sin(alpha) * Unity.Mathematics.math.sin(phi)),
                z_q + (w * Unity.Mathematics.math.cos(phi)));

            switch (idx)
            {
                case 0: return c0;
                case 1: return c1;
                case 2: return c2;
                case 3: return c3;
                default: return new Unity.Mathematics.float3();
            }
        }

        /// <summary>
        /// renders a torus by rotating a 1x1 square around two different axes
        /// </summary>
        private void renderTorus()
        {
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[((n + 1) * (n - 1)) + 1];

            //Array of 2D vectors for UV map of vertices
            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[verts.Length];
            float oneNth = 1f / n;

            //loop through n-1 times, since edges wrap around
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //index value computation
                    int idx = i + (n * j);

                    //find radian value of length of curve
                    float alpha = i * oneNth * 2 * UnityEngine.Mathf.PI;
                    float beta = j * oneNth * 2 * UnityEngine.Mathf.PI;

                    //map vertices from 2 dimensions to 3
                    verts[idx] = torusPosition(alpha, beta);

                    //uv mapping 
                    uvs[idx] = new UnityEngine.Vector2(j * oneNth, i * oneNth);
                }
            }

            //integer array for number of triangle vertices-3 verts x 2 triangles per square x n^2
            int[] triangles = new int[6 * n * n];

            //for each triangle
            for (int k = 0; k < (2 * n * n); k++)
            {
                //for each vertex on each triangle
                for (int m = 0; m < 3; m++)
                    triangles[(3 * k) + m] = IMRE.HandWaver.ScaleDimension.RenderMethods.ToricTriangle(k, m, n);
            }

            torusRenderer.vertices = verts;
            torusRenderer.triangles = triangles;
            torusRenderer.uv = uvs;
            torusRenderer.RecalculateNormals();
            torusRenderer.Optimize();
        }

        /// <summary>
        /// Take alpha and beta to be angles describing turns around the primary and secondary revolutions of a torus.
        /// Take r1 and r2 to be the radii of those revolutions
        /// Find the position on the surface of the torus
        /// </summary>
        /// <param name="alpha">x</param>
        /// <param name="beta">y</param>
        /// <returns></returns>
        private UnityEngine.Vector3 torusPosition(float alpha, float beta)
        {
            //3D vectors for describing positions on the circle
            //the center of a cricle (which could be revolved to create the torus
            UnityEngine.Vector3 firstPosition = new UnityEngine.Vector3(revolveRadius * UnityEngine.Mathf.Cos(alpha),
                revolveRadius * UnityEngine.Mathf.Sin(alpha), 0f);
            //the position of a vertex with a circle centered at Vector3.right*rotateRadius
            UnityEngine.Vector3 secondPosition = new UnityEngine.Vector3(circleRadius * UnityEngine.Mathf.Cos(beta), 0f,
                                                     circleRadius * UnityEngine.Mathf.Sin(beta)) +
                                                 (UnityEngine.Vector3.right * revolveRadius);

            //mapping of rotation
            UnityEngine.Vector3 result = firstPosition +
                                         (UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right,
                                              firstPosition) * secondPosition);
            return result;
        }

        /// <summary>
        /// Math for calculating intersection of torus and plane
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private Unity.Mathematics.float3 spiricOutsideMath(float theta, float height)
        {
            //convert values to variables for equation
            float d = 2f * ((UnityEngine.Mathf.Pow(circleRadius, 2) + UnityEngine.Mathf.Pow(revolveRadius, 2)) -
                            UnityEngine.Mathf.Pow(height, 2));
            float e = 2f * (UnityEngine.Mathf.Pow(circleRadius, 2) - UnityEngine.Mathf.Pow(revolveRadius, 2) -
                            UnityEngine.Mathf.Pow(height, 2));
            float f = -(circleRadius + revolveRadius + height) *
                      ((circleRadius + revolveRadius) - height) *
                      ((circleRadius - revolveRadius) + height) *
                      (circleRadius - revolveRadius - height);

            //distance results 
            float r0;
            float r1;

            r0 = UnityEngine.Mathf.Sqrt(
                     UnityEngine.Mathf.Sqrt(
                         UnityEngine.Mathf.Pow(
                             (-d * UnityEngine.Mathf.Cos(theta) * UnityEngine.Mathf.Cos(theta)) -
                             (e * UnityEngine.Mathf.Sin(theta) * UnityEngine.Mathf.Sin(theta)),
                             2) +
                         (4 * f)) + (d * UnityEngine.Mathf.Cos(theta) * UnityEngine.Mathf.Cos(theta)) +
                     (e * UnityEngine.Mathf.Sin(theta) * UnityEngine.Mathf.Sin(theta))) /
                 UnityEngine.Mathf.Sqrt(2);

            r1 = UnityEngine.Mathf.Sqrt(
                     -UnityEngine.Mathf.Sqrt(
                         UnityEngine.Mathf.Pow(
                             (-d * UnityEngine.Mathf.Cos(theta) * UnityEngine.Mathf.Cos(theta)) -
                             (e * UnityEngine.Mathf.Sin(theta) * UnityEngine.Mathf.Sin(theta)),
                             2) +
                         (4 * f)) + (d * UnityEngine.Mathf.Cos(theta) * UnityEngine.Mathf.Cos(theta)) +
                     (e * UnityEngine.Mathf.Sin(theta) * UnityEngine.Mathf.Sin(theta))) /
                 UnityEngine.Mathf.Sqrt(2);

            Unity.Mathematics.float3x2 result = new Unity.Mathematics.float3x2();

            //distance results converted to theta

            return (r0 * ((UnityEngine.Mathf.Cos(theta) * UnityEngine.Vector3.right) +
                          (UnityEngine.Mathf.Sin(theta) * UnityEngine.Vector3.forward))) +
                   (UnityEngine.Vector3.up * height);
            //result.c1 = -r0 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward)+ Vector3.up*height;
            //Debug.Log(height + " : " + d + " : " + e + " : " + f +" : " + r0+" : " +r1);
            //return result;
        }
    }
}
