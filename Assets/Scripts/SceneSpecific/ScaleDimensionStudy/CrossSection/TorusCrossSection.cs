using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class TorusCrossSection : MonoBehaviour
    {
        private int n;

        //radius of 2d circle for torus
        const float circleRadius = 0;

        //distance from axis that circle will be rotated on
        const float rotateRadius = 0;

        //resulting plane's distance from center
        const float planeDistance = 0;

        private MeshRenderer torusRenderer => GetComponent<MeshRenderer>();
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
        /// Function to calculate cross section of a torus
        /// </summary>
        /// <param name="height"></param>
        public void crossSectTorus(float height)
        {
            float innerRadius = 0.20f;
            float outerRadius = 1f;


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

            }

            //height is not within torus
            else if (Math.Abs(height) > outerRadius)
            {
                Debug.Log("Height is out of range of object.");
                //TODO update rendering

            }

            //cross section results in spiric shape
            else
            {
                //TODO update rendering this loop is complicated

                for (float i = 0; i < n; i++)
                {
                }

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
        private float3x2 spiricMath(float theta, float circleRadius, float rotateRadius, float planeDistance)
        {
            //convert values to variables for equation
            float d = 2f * (float) (Math.Pow(circleRadius, 2) + Math.Pow(rotateRadius, 2) - Math.Pow(planeDistance, 2));
            float e = 2f * (float) (Math.Pow(circleRadius, 2) - Math.Pow(rotateRadius, 2) - Math.Pow(planeDistance, 2));
            float f = -(circleRadius + rotateRadius + planeDistance) * (circleRadius + rotateRadius - planeDistance) *
                      (circleRadius - rotateRadius + planeDistance) * (circleRadius - rotateRadius - planeDistance);

            //distance results 
            float r0;
            float r1;

            r0 = Mathf.Sqrt(
                     -Mathf.Sqrt(
                         Mathf.Pow(-d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta),
                             2) +
                         4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) + e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
                 Mathf.Sqrt(2);

            r1 = Mathf.Sqrt(
                     Mathf.Sqrt(
                         Mathf.Pow(-d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta),
                             2) +
                         4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) + e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
                 Mathf.Sqrt(2);
            ;

            float3x2 result = new float3x2();

            //distance results converted to theta
            result.c0 = r0 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward);
            result.c1 = r1 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward);
            return result;
        }
    }
}
