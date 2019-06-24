using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class CircleCrossSection : MonoBehaviour
    {
        private int n;

        private LineRenderer circleRenderer => GetComponent<LineRenderer>();
        private LineRenderer crossSectionRenderer => GetComponentInChildren<LineRenderer>();
        // Start is called before the first frame update
        void Start()
        {
            //TODO setup annulus renderer
            //TODO setup cross-section renderer as child object

        }

        // Update is called once per frame
        void Update()
        {
            //TODO dynamic cross section renderer
        }
        
        /// <summary>
        /// Function to calculate cross section of circle
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public void crossSectCirc(float height, float radius)
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
                }
          
                //if bottom of circle, create point at intersection
                else if (height == -radius)
                {
                    segmentEndPoint0 = Vector3.down * radius;
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
                //TODO update rendering

            }
       
            //height for cross section is outside of circle 
            else if (Math.Abs(height) > radius)
            {
                Debug.Log("Height is out of range of object.");
                //TODO update rendering

            }
        
        }
        
    }
}