using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using IMRE.HandWaver.Space;
using Unity.Mathematics;
using IMRE.HandWaver.HigherDimensions;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver.ScaleStudy
{
    public class AnnulusCrossSection : MonoBehaviour, ISliderInput
    {
        public int n = 5;

        #region Components/Variables
      
        private Mesh annulusRenderer => GetComponent<MeshFilter>().mesh;
        private LineRenderer[] crossSectionRenderer => GetComponentsInChildren<LineRenderer>();
        
        public float innerRadius = 0.75f;
        public float outerRadius = 1f;

        public Material annulusMaterial;
        public Material crossSectionMaterial;
        public bool debugRenderer => SpencerStudyControl.debugRendererXC;
        #endregion
       

        // Start is called before the first frame update
        void Start()
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            gameObject.GetComponent<MeshRenderer>().material = annulusMaterial;
            gameObject.GetComponent<MeshRenderer>().enabled = debugRenderer;

            #region Vertices/Triangles

            //number of vertices within pre-rotated square
            Vector3[] verts = new Vector3[(n + 1) * n];
            
            //Array of 2d vectors for uv mapping
            Vector2[] uvs = new Vector2[verts.Length];
            float oneNth = 1f / n;
            float oneNMinusTwoth = 1f / (n - 2);

            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n-1; j++)
                {
                    //index value computation
                    int idx = i + n * j;

                    //find radian value of length of curve
                    float alpha = i*oneNth;
                    float beta = j * oneNMinusTwoth;

                    //map vertices from 2 dimensions to 3
                    verts[idx] = annulusPosition(beta, alpha);
                   
                    

                    //uv mapping 
                    uvs[idx] = new Vector2(alpha, beta);
                }
            }
            
            //3 vertices x 2 triangles per square x dimension of square
            int[] triangles = new int[6 * n * n];
            
            for (int k = 0; k < 2 * (n)*n; k++)
            {
                //for each vertex on each triangle
                for (int m = 0; m < 3; m++)
                    triangles[3 * k + m] = triangle(k, m);
            }
            annulusRenderer.vertices = verts;
            annulusRenderer.triangles = triangles;
            annulusRenderer.uv = uvs;
            annulusRenderer.RecalculateNormals();
            annulusRenderer.Optimize();
            #endregion

            #region CrossSections' GameObjects
            
            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.transform.localPosition = Vector3.zero;
            child.AddComponent<LineRenderer>();
            
            GameObject child2 = new GameObject();
            child2.transform.parent = transform;
            child2.transform.localPosition = Vector3.zero;
            child2.AddComponent<LineRenderer>();

            child.GetComponent<LineRenderer>().endWidth = SpencerStudyControl.lineRendererWidth;
            child.GetComponent<LineRenderer>().startWidth = SpencerStudyControl.lineRendererWidth;
            child.GetComponent<LineRenderer>().enabled = false;
            child2.GetComponent<LineRenderer>().endWidth = SpencerStudyControl.lineRendererWidth;
            child2.GetComponent<LineRenderer>().startWidth = SpencerStudyControl.lineRendererWidth;
            child2.GetComponent<LineRenderer>().enabled = false;

            
            child2.GetComponent<LineRenderer>().useWorldSpace = false;
            child.GetComponent<LineRenderer>().useWorldSpace = false;
            
            crossSectionRenderer.ToList().ForEach( r => r.material = crossSectionMaterial);
            #endregion

        }
        
        //slider to control cross section
        public float slider
        {
            //value ranges from 0 to 1, scale to -1 to 1
            set => crossSectAnnulus(-1+value*2);
        }
        

        /// <summary>
        /// Function to calculate cross section of an annulus 
        /// </summary>
        /// <param name="height"></param>
        public void crossSectAnnulus(float height)
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
                else
                {
                    segmentAEndPoint0 = Vector3.down * outerRadius;
                }

                crossSectionRenderer[0].enabled = true;
                crossSectionRenderer[0].SetPosition(0,segmentAEndPoint0);
                crossSectionRenderer[0].SetPosition(1,segmentAEndPoint0);
                crossSectionRenderer[1].enabled = false;
            }
            //cross section is a line segment in between the inner circle and outer circle
            else if (Math.Abs(height) < outerRadius && Math.Abs(height) >= innerRadius)
            {
                //horizontal distance from center to point on outer edge of annulus
                x1 = (Mathf.Sqrt(Mathf.Pow(outerRadius, 2) - Mathf.Pow(height, 2)));

                //calculations for coordinates of line segment endpoints
                segmentAEndPoint0 = (Vector3.up * height) + (Vector3.right * (x1));
                segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x1));
                
                crossSectionRenderer[0].enabled = true;
                crossSectionRenderer[0].SetPosition(0,segmentAEndPoint0);
                crossSectionRenderer[0].SetPosition(1,segmentAEndPoint1);
                crossSectionRenderer[1].enabled = false;

            }
            //cross section height is less than the inner radius, resulting in two line segments
            else if (Math.Abs(height) < innerRadius)
            {
                //horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
                x1 = (Mathf.Sqrt(Mathf.Pow(outerRadius, 2) - Mathf.Pow(height, 2)));
                x2 = (Mathf.Sqrt(Mathf.Pow(innerRadius, 2) - Mathf.Pow(height, 2)));

                //calculations for inner and outer endpoints for each line segment
                segmentAEndPoint0 = (Vector3.up * height) + (Vector3.left * (x1));
                segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x2));

                segmentBEndPoint0 = (Vector3.up * height) + (Vector3.right * (x2));
                segmentBEndPoint1 = (Vector3.up * height) + (Vector3.right * (x1));
                crossSectionRenderer[0].enabled = true;
                crossSectionRenderer[1].enabled = true;

                crossSectionRenderer[0].SetPosition(0,segmentAEndPoint0);
                crossSectionRenderer[0].SetPosition(1,segmentAEndPoint1);
                crossSectionRenderer[1].SetPosition(0,segmentBEndPoint0);
                crossSectionRenderer[1].SetPosition(1,segmentBEndPoint1);

            }
            //cross section height is out of range of annulus
            else if (Math.Abs(height) > outerRadius)
            {
                Debug.Log("Height is out of range of object.");
                //TODO update rendering
                crossSectionRenderer[0].enabled = false;
                crossSectionRenderer[1].enabled = false;

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