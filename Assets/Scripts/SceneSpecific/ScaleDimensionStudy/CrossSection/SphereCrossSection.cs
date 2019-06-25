using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class SphereCrossSection : MonoBehaviour, ISliderInput
    {
        private int n;
        private MeshRenderer sphereRendere => GetComponent<MeshRenderer>();
        private LineRenderer crossSectionRenderer => GetComponentInChildren<LineRenderer>();

        public float radius = 1f;
        public Vector3 center = Vector3.zero;
        public Vector3 normal = Vector3.up;


        // Start is called before the first frame update
        void Start()
        {
            //TODO setup sphere renderer
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
                
            
            //TODO setup cross-section renderer as child object
            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.AddComponent<MeshRenderer>();
            child.AddComponent<MeshFilter>();

        }

        public float slider
        {
            set => crossSectCirc(value,radius,center,normal);
        }

        /// <summary>
        /// Function to calculate cross section of circle
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public void crossSectCirc(float height, float radius, Vector3 center, Vector3 normal)
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

            }

            //cross section is a line that hits two points on the circle (height smaller than radius of circle)
            else if (Math.Abs(height) < radius)
            {
                //horizontal distance from center of circle to point on line segment
                float segmentLength = Mathf.Sqrt(1f - Mathf.Pow(height, 2));

                //calculations for endpoint coordinates of line segment
                segmentEndPoint0 = (Vector3.up * height) + (Vector3.left * segmentLength);
                segmentEndPoint1 = (Vector3.up * height) + (Vector3.right * segmentLength);

            }

            //height for cross section is outside of circle 
            else if (Math.Abs(height) > radius)
            {
                Debug.Log("Height is out of range of object.");
            }

        }

    }

}