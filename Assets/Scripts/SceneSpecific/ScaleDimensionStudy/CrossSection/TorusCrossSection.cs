using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            crossSectionRenderer.ToList().ForEach(r => r.material = crossSectionMaterial);
            crossSectionRenderer.ToList().ForEach(r => r.startWidth = .005f);
            crossSectionRenderer.ToList().ForEach(r => r.endWidth = .005f);
            crossSectionRenderer.ToList().ForEach(r => r.loop = true);
            crossSectionRenderer.ToList().ForEach(r => r.positionCount = n);
            debugIntersection();
            
        }

        public float slider
        {
            //scale value from 0 to 1 to -1 to 1
            //set => crossSectTorus(-1+value*2);
            set => debugIntersection();
        }

        /// <summary>
        /// Function to calculate cross section of a torus
        /// </summary>
        /// <param name="height"></param>
        public void crossSectTorus(float height)
        {
            float innerRadius = revolveRadius - circleRadius;
            float outerRadius = revolveRadius + circleRadius;


            Vector3 torusCenter = Vector3.zero;

            Vector3 pointPos;



            //cross section only hits a point on outer edge of torus
            if (Math.Abs(height) == outerRadius)
            {
                if (height == outerRadius)
                {
                    pointPos = Vector3.up * height;
                }
                else if (height == -outerRadius)
                {
                    pointPos = Vector3.down * height;
                }

                //TODO update rendering
                crossSectionRenderer.ToList().ForEach(r => r.enabled = false);

            }

            //height is not within torus
            else if (Math.Abs(height) > outerRadius)
            {
                Debug.Log("Height is out of range of object.");
                //TODO update rendering
                crossSectionRenderer.ToList().ForEach(r => r.enabled = false);

            }

            //cross section results in spiric shape
            else if (Mathf.Abs(height) < innerRadius)
            {
                //there is only one spiric
                for (int i = 0; i < n; i++)
                {
                    float theta = i * (1 / n) * Mathf.PI * 2;
                    crossSectionRenderer[0].SetPosition(i, spiricMath(theta, height).c0);
                }

                crossSectionRenderer[0].enabled = true;
                crossSectionRenderer[1].enabled = false;
            }
            else
            {
                //there is a double spiric
                //on each render, need to walk renderer forward and back with each solution to cover the spiric.
                //the second renderer is a reflection of the first.

                //todo find thetaMax
                float thetaMax = Mathf.PI / 2f;
                for (int i = 0; i < (n / 2); i++)
                {
                    float theta = i * (1 / (n - 2)) * thetaMax;
                    //walk forward on first
                    crossSectionRenderer[0].SetPosition(i, spiricMath(theta, height).c0);
                    //walk backward on first
                    crossSectionRenderer[0].SetPosition((n - 1) - i, spiricMath(-theta, height).c1);
                    //walk forward on second
                    crossSectionRenderer[1].SetPosition(i, spiricMath(Mathf.PI - theta, height).c0);
                    //walk backward on second
                    crossSectionRenderer[1].SetPosition((n - 1) - i, spiricMath(Mathf.PI + theta, height).c1);
                }

                crossSectionRenderer.ToList().ForEach(r => r.enabled = true);

            }

        }

        /// <summary>
        /// Math for calculating intersection of torus and plane
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private float3x2 spiricMath(float theta, float height)
        {
            //convert values to variables for equation
            float d = 2f * (float) (Math.Pow(circleRadius, 2) + Math.Pow(revolveRadius, 2) -
                                    Math.Pow(height, 2));
            float e = 2f * (float) (Math.Pow(circleRadius, 2) - Math.Pow(revolveRadius, 2) -
                                    Math.Pow(height, 2));
            float f = -(circleRadius + revolveRadius + height) *
                      (circleRadius + revolveRadius - height) *
                      (circleRadius - revolveRadius + height) *
                      (circleRadius - revolveRadius - height);
            
            //distance results 
            float r0;
            float r1;

            r0 = Mathf.Sqrt(
                     -Mathf.Sqrt(
                         Mathf.Pow(
                             -d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta),
                             2) +
                         4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) +
                     e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
                 Mathf.Sqrt(2);

            r1 = Mathf.Sqrt(
                     Mathf.Sqrt(
                         Mathf.Pow(
                             -d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta),
                             2) +
                         4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) +
                     e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
                 Mathf.Sqrt(2);
            ;

            float3x2 result = new float3x2();

            //distance results converted to theta
            result.c0 = r0 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward);
            result.c1 = r1 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward);
            return result;
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

        private void debugIntersection()
        {
            float height = (revolveRadius - .25f);
            //convert values to variables for equation
            float d = 2f * (float) (Math.Pow(circleRadius, 2) + Math.Pow(revolveRadius, 2) -
                                    Math.Pow(height, 2));
            float e = 2f * (float) (Math.Pow(circleRadius, 2) - Math.Pow(revolveRadius, 2) -
                                    Math.Pow(height, 2));
            float f = -(circleRadius + revolveRadius + height) *
                      (circleRadius + revolveRadius - height) *
                      (circleRadius - revolveRadius + height) *
                      (circleRadius - revolveRadius - height);

            float theta1 = Mathf.Acos(Mathf.Sqrt((2 * Mathf.Sqrt(-Mathf.Pow(e - d, 2) * f)) / (Mathf.Pow(d, 2) - 2 * e * d + Mathf.Pow(e, 2)) - ((e * d) / (Mathf.Pow(d, 2) - 2 * e * d + Mathf.Pow(e, 2))) + Mathf.Pow(e, 2) / (Mathf.Pow(d, 2) - 2 * e * d + Mathf.Pow(e, 2))));
            float theta2 = Mathf.Acos(Mathf.Sqrt((-2 * Mathf.Sqrt(-Mathf.Pow(e - d, 2) * f) / (Mathf.Pow(d, 2) - 2 * e * d + Mathf.Pow(e, 2)) - ((e * d) / (Mathf.Pow(d, 2) - 2 * e * d + Mathf.Pow(e, 2))) + Mathf.Pow(e, 2) / (Mathf.Pow(d, 2) - 2 * e * d + Mathf.Pow(e, 2)))));
            Debug.Log(theta1 + "  :  " + theta2);
        }

    }
    
}
