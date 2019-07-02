using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class HyperTorusCrossSection : MonoBehaviour, ISliderInput
    {
        #region variables/components

        public int n;
        private Mesh crossSectionRenderer => GetComponent<MeshFilter>().mesh;
        public Material sphereMaterial;
        public float radius = 1f;

        #endregion
        
        public enum crossSectionPlane{w,x,y,z}

        private crossSectionPlane plane = crossSectionPlane.z;
        public float R;
        public float P;

        public float slider
        {
            set
            {
                renderToricSection(-1+2*value);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshRenderer>().material = sphereMaterial;
        }


        public float3 HyperToricSectoin(float a, float b, float c)
        {
            float w = (R + (P + math.cos(a)) * math.cos(b)) * math.cos(c);
            float x = (R + (P + math.cos(a)) * math.cos(b)) * math.sin(c);
            float y = (P + math.cos(a)) * math.sin(b);
            float z = math.sin(a);
            
            switch (plane)
            {
                case crossSectionPlane.x:
                    return new float3(w,y,z);
                case crossSectionPlane.y:
                    return new float3(w,x,z);
                case crossSectionPlane.z:
                    return new float3(x,y,w);
                case crossSectionPlane.w:
                    return new float3(x,y,z);
                default:
                    return new float3(0,0,0);
            }
        }
        
        private void renderToricSection(float height)
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
                    //TODO testing with a = arcsin(z), cross secting in z
                    verts[idx] = HyperToricSectoin(math.asin(height), alpha,beta);

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

            crossSectionRenderer.vertices = verts;
            crossSectionRenderer.triangles = triangles;
            crossSectionRenderer.uv = uvs;
            crossSectionRenderer.RecalculateNormals();
            crossSectionRenderer.Optimize();
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