using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMRE.HandWaver.HWIO;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class TorusCrossSection : MonoBehaviour, ISliderInput
    {
        public int n;

        //radius of 2d circle for torus
        public float circleRadius = .5f;

        //distance from axis that circle will be rotated on
        public float revolveRadius = 1f;



        private Mesh torusRenderer => GetComponent<MeshFilter>().mesh;
        private LineRenderer[] crossSectionRenderer => GetComponentsInChildren<LineRenderer>();

        public Material torusMaterial;
        public Material crossSectionMaterial;

        public bool debugRenderer = SpencerStudyControl.debugRendererXC;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshRenderer>().material = torusMaterial;
            gameObject.GetComponent<MeshRenderer>().enabled = debugRenderer;
            renderTorus();

            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.AddComponent<LineRenderer>();

            GameObject child2 = new GameObject();
            child2.transform.parent = transform;
            child2.AddComponent<LineRenderer>();

            GameObject child3 = new GameObject();
            child3.transform.parent = transform;
            child3.AddComponent<LineRenderer>();

            GameObject child4 = new GameObject();
            child4.transform.parent = transform;
            child4.AddComponent<LineRenderer>();

            crossSectionRenderer.ToList().ForEach(r => r.material = crossSectionMaterial);
            crossSectionRenderer.ToList().ForEach(r => r.startWidth = .005f);
            crossSectionRenderer.ToList().ForEach(r => r.endWidth = .005f);
            crossSectionRenderer.ToList().ForEach(r => r.loop = true);
            crossSectionRenderer.ToList().ForEach(r => r.positionCount = n);

        }

        public float slider
        {
            //scale value from 0 to 1 to -1 to 1
            set => crossSectTorus(-1 + value * 2);
        }

        /// <summary>
        /// Function to calculate cross section of a torus
        /// </summary>
        /// <param name="height"></param>
        public void crossSectTorus(float height)
        {
            float oneNth = 1f / n;
            float wMin = 0f;
            float wMax = 1f;
            for (int i = 0; i < n; i++)
            {
                float w = (i * oneNth * (wMin - wMax)) + wMin;
                float3x4 result = spiricMath(w, height, 0f, 0f);
                crossSectionRenderer[0].SetPosition(i, result.c0);
                crossSectionRenderer[1].SetPosition(i, result.c1);
                crossSectionRenderer[2].SetPosition(i, result.c2);
                crossSectionRenderer[3].SetPosition(i, result.c3);
            }

            crossSectionRenderer.ToList().ForEach(r => r.enabled = true);
        }

         private float3x4 spiricMath(float w, float height, float alpha, float phi)
        {
            //uses method described here: arXiv:1708.00803v2 [math.GM] 6 Aug 2017
            float p = math.abs(height);
            float x_q = p * math.sin(alpha) * math.cos(phi);
            float y_q = p * math.sin(alpha) * math.cos(phi);
            float z_q = p * math.sin(phi);
            float R = revolveRadius;
            float r = circleRadius;
            
            float dist = math.sqrt(math.pow(r,2) - math.pow(w*math.cos(phi)+ p*math.sin(phi),2))
            float t_0 = math.sqrt(-math.pow(p * math.cos(phi) - w * math.sin(phi), 2) + math.pow(R + dist, 2));
            float t_1 =math.sqrt(-math.pow(p * math.cos(phi) - w * math.sin(phi), 2) + math.pow(R + dist, 2));
            float t_2 = -t_0;
            float t_3 = -t_1;

            return new float3x4
            {
                c0 = new float3(x_q + t_0 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
                    y_q - t_0 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi)),
                c1 = new float3(x_q + t_1 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
                    y_q - t_1 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi)),
                c2 = new float3(x_q + t_2 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
                    y_q - t_2 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi)),
                c3 = new float3(x_q + t_3 * math.sin(alpha) - w * math.cos(alpha) * math.sin(phi),
                    y_q - t_3 * math.cos(alpha) - w * math.sin(alpha) * math.sin(phi), z_q + w * math.cos(phi))
            };
        }

        

        private void renderTorus()
        {
            Vector3[] verts = new Vector3[(n + 1) * (n - 1) + 1];

            //Array of 2D vectors for UV map of vertices
            Vector2[] uvs = new Vector2[verts.Length];
            float oneNth = 1f / ((float) n);

            //loop through n-1 times, since edges wrap around
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //index value computation
                    int idx = i + n * j;

                    //find radian value of length of curve
                    float alpha = i * oneNth * 2 * Mathf.PI;
                    float beta = j * oneNth * 2 * Mathf.PI;

                    //map vertices from 2 dimensions to 3
                    verts[idx] = torusPosition(alpha, beta);

                    //uv mapping 
                    uvs[idx] = new Vector2(j * oneNth, i * oneNth);
                }
            }

            //integer array for number of triangle vertices-3 verts x 2 triangles per square x n^2
            int[] triangles = new int[6 * n * n];

            //for each triangle
            for (int k = 0; k < 2 * n * n; k++)
            {
                //for each vertex on each triangle
                for (int m = 0; m < 3; m++)
                    triangles[3 * k + m] = triangle(k, m);
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
        private Vector3 torusPosition(float alpha, float beta)
        {
            //3D vectors for describing positions on the circle
            //the center of a cricle (which could be revolved to create the torus
            Vector3 firstPosition = new Vector3(revolveRadius * Mathf.Cos(alpha), revolveRadius * Mathf.Sin(alpha), 0f);
            //the position of a vertex with a circle centered at Vector3.right*rotateRadius
            Vector3 secondPosition = new Vector3(circleRadius * Mathf.Cos(beta), 0f, circleRadius * Mathf.Sin(beta)) +
                                     Vector3.right * revolveRadius;

            //mapping of rotation
            Vector3 result = firstPosition + Quaternion.FromToRotation(Vector3.right, firstPosition) * secondPosition;
            return result;
        }

        /// <summary>
        /// Finds the vertex indices for the kth triangle
        /// returns the mth vertex index of the kth triangle
        /// Triangles indexed for clockwise front face
        /// </summary>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private int triangle(int k, int m)
        {
            if (k < n * n)
            {
                //lower half triangle
                switch (m)
                {
                    case 0:
                        return k;
                    case 2:
                        return Mathf.FloorToInt((k) / n) * n + ((k + 1) % n);
                    case 1:
                        return ((Mathf.FloorToInt((k) / n) + 1) % n) * n + ((k + 1) % n);
                }
            }
            else
            {
                k = k - n * n;

                switch (m)
                {

                    case 0:
                        return k;
                    case 2:
                        return ((Mathf.FloorToInt((k) / n) + 1) % n) * n + ((k + 1) % n);
                    case 1:
                        return ((Mathf.FloorToInt((k) / n) + 1) % n) * n + (k % n);



                }
            }

            Debug.LogError("Invalid parameter.");



            return 0;
        }


    }
    
}
