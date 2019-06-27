using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class CircleCrossSection : MonoBehaviour, ISliderInput
    {
        #region Variables/Components
        public int n = 5;
        public float radius = 1f;

        private LineRenderer circleRenderer => GetComponent<LineRenderer>();
        private LineRenderer crossSectionRenderer => transform.GetChild(0).GetComponent<LineRenderer>();
        public Material circleMaterial;
        public Material crossSectionMaterial;
        
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            #region Render
            gameObject.AddComponent<LineRenderer>();
            circleRenderer.material = circleMaterial;
            circleRenderer.startWidth = .005f;
            circleRenderer.endWidth = .005f;
            renderCircle();

            
            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.AddComponent<LineRenderer>();
            crossSectionRenderer.material = crossSectionMaterial;
            crossSectionRenderer.startWidth = .005f;
            crossSectionRenderer.endWidth = .005f;

            #endregion
        }

        //slider to control the cross section
        public float slider
        {
            set => crossSectCirc(value);
        }
        
        /// <summary>
        /// Function to calculate cross section of circle
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public void crossSectCirc(float height)
        {               
            //endpoints for line segment if intersection passes through circle
            Vector3 segmentEndPoint0;
            Vector3 segmentEndPoint1;
        
            //if cross section only hits the edge of the circle
            if (Math.Abs(height) == radius)
            {
           
                //if top of circle, create point at intersection
                if (height == radius)
                {
                    segmentEndPoint0 = Vector3.up * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);
                }
          
                //if bottom of circle, create point at intersection
                else if (height == -radius)
                {
                    segmentEndPoint0 = Vector3.down * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);
                }
                //TODO update rendering

                

            }
       
            //cross section is a line that hits two points on the circle (height smaller than radius of circle)
            else if (Math.Abs(height) < radius)
            {
                //horizontal distance from center of circle to point on line segment
                float segmentLength = Mathf.Sqrt(1f - Mathf.Pow(height, 2));
            
                //calculations for endpoint coordinates of line segment
                segmentEndPoint0 = (Vector3.up * height) + (Vector3.left * segmentLength);
                segmentEndPoint1 = (Vector3.up * height) + (Vector3.right * segmentLength);

                crossSectionRenderer.enabled = true;
                crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                crossSectionRenderer.SetPosition(1, segmentEndPoint1);

            }
       
            //height for cross section is outside of circle 
            else if (Math.Abs(height) > radius)
            {
                Debug.Log("Height is out of range of object.");
                //TODO update rendering
                crossSectionRenderer.enabled = false;

            }
        
        }

        /// <summary>
        /// Function to outline and render a circle
        /// </summary>
        public void renderCircle()
        {
            //normal vectors
            Vector3 norm1 = Vector3.up;
            Vector3 norm2 = Vector3.right;

            //array of vector3s for vertices
            Vector3[] vertices = new Vector3[n];

            //math for rendering circle
            for (int i = 0; i < n; i++)
            {
                vertices[i] = radius * (Mathf.Sin((i * Mathf.PI * 2 / (n - 1))) * norm1) + radius * (Mathf.Cos((i * Mathf.PI * 2 / (n - 1))) * norm2);
                
            }

            //Render circle
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            lineRenderer.startWidth = .005f;
            lineRenderer.endWidth = .005f;
            lineRenderer.positionCount = n;
            lineRenderer.SetPositions(vertices);
        }

    }
}