using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using IMRE.HandWaver.Space;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class AnnulusCrossSection : MonoBehaviour
    {
        private int n;

        
        private MeshRenderer annulusRenderer => GetComponent<MeshRenderer>();
        private LineRenderer crossSectionRenderer => GetComponentInChildren<LineRenderer>();
        
        private float height;
        private float innerRadius = 0.75f;
        private float outerRadius = 1f;
        private Vector3 center;
        private Vector3 normal;

        // Start is called before the first frame update
        void Start()
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            
            //number of vertices within pre-rotated square
            Vector3[] verts = new Vector3[10];
            
            //Array of 2d vectors for uv mapping
            Vector2[] uvs = new Vector2[verts.Length];
            float oneNth = 1f / ((float)n);

            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    //index value computation
                    int idx = i + n * j;

                    //find radian value of length of curve
                    float alpha = i*oneNth;
                    float beta = j * oneNth;

                    //map vertices from 2 dimensions to 3
                    verts[idx] = annulusPosition(alpha,beta);
                
                    //uv mapping 
                    uvs[idx] = new Vector2(j*oneNth, i*oneNth);
                }
            }
            
            //3 vertices x 2 triangles per square x dimension of square
            int[] triangles = new int[6 * n * n];
            
            for (int k = 0; k < 2 * n * n; k++)
            {
                //for each vertex on each triangle
                for (int m = 0; m < 3; m++)
                    triangles[3 * k + m] = triangle(k, m);
            }

            mesh.vertices = verts;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            mesh.Optimize();
            //TODO setup cross-section renderer as child object

        }

        // Update is called once per frame
        void Update()
        {
            //TODO dynamic cross section renderer
            crossSectAnnulus(height, innerRadius, outerRadius, center, normal);
            
        }

        /// <summary>
        /// Function to calculate cross section of an annulus 
        /// </summary>
        /// <param name="height"></param>
        public void crossSectAnnulus(float height, float innerRadius, float outerRadius, Vector3 center, Vector3 plane)
        {

            //point if intersection hits edge, or points for line segment(s) if it passes through annulus
            Vector3 segmentAEndPoint0, segmentAEndPoint1, segmentBEndPoint0, segmentBEndPoint1;

            //horizontal distance from center of annulus to points on line segment(s)
            float x1;
            float x2;

            //cross-section only hits edge of annulus
            if (Math.Abs(height) == outerRadius)
            {
                //if top edge, create point at intersection
                if (height == outerRadius)
                {
                    segmentAEndPoint0 = Vector3.up * outerRadius;
                }
                //if bottom edge, create point at intersection
                else if (height == -outerRadius)
                {
                    segmentAEndPoint0 = Vector3.down * outerRadius;
                }
                //TODO update rendering
            }
            //cross section is a line segment in between the inner circle and outer circle
            else if (Math.Abs(height) < outerRadius && Math.Abs(height) >= innerRadius)
            {
                //horizontal distance from center to point on outer edge of annulus
                x1 = (Mathf.Sqrt(1f - Mathf.Pow(height, 2)));

                //calculations for coordinates of line segment endpoints
                segmentAEndPoint0 = (Vector3.up * height) + (Vector3.right * (x1));
                segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x1));
                
                //TODO update rendering
            }
            //cross section height is less than the inner radius, resulting in two line segments
            else if (Math.Abs(height) < innerRadius)
            {
                //horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
                x1 = (Mathf.Sqrt(1f - Mathf.Pow(height, 2)));
                x2 = (Mathf.Sqrt(0.75f - Mathf.Pow(height, 2)));

                //calculations for inner and outer endpoints for each line segment
                segmentAEndPoint0 = (Vector3.up * height) + (Vector3.left * (x1));
                segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x2));

                segmentBEndPoint0 = (Vector3.up * height) + (Vector3.right * (x2));
                segmentBEndPoint1 = (Vector3.up * height) + (Vector3.right * (x1));
                //TODO update rendering

            }
            //cross section height is out of range of annulus
            else if (Math.Abs(height) > outerRadius)
            {
                Debug.Log("Height is out of range of object.");
                //TODO update rendering

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
        private Vector3 annulusPosition(float alpha, float beta)
        {
            //conversion factor
            float length = (beta * (outerRadius - innerRadius)) + innerRadius;
            //rotation mapping
            Vector3 result = (Vector3.right * Mathf.Cos(alpha * 2 * Mathf.PI) + Vector3.up * Mathf.Sin(alpha * 2 * Mathf.PI)) * length;


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
            if(k < n * n)
            {
                //lower half triangles first 
                switch (m)
                {
                    //simply the index of the triangle
                    case 0:
                        return k;
                    //2nd vertex-same thing
                    case 1:
                        return ((Mathf.FloorToInt((k) / n) + 1)) * n + ((k + 1) % n);
                    //3rd vertex calculation-wrap around horizontally but not vertically
                    case 2:
                        return Mathf.FloorToInt((k) / n) * n + ((k + 1) % n);
                }
            }
            //upper half triangles
            else
            {
                k = k - n * n;

                switch (m)
                {
                    //same logic as with previous cases
                    case 0:
                        return k;
                    case 1:
                        return ((Mathf.FloorToInt((k) / n) + 1)) * n + (k % n);
                    case 2:
                        return ((Mathf.FloorToInt((k) / n) + 1) % n) * n + ((k + 1) % n);



                }
            }

            Debug.LogError("Invalid parameter.");



            return 0;
        }

    }
    
}