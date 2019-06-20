using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using IMRE.HandWaver.HWIO;
using IMRE.HandWaver.Space;
using Leap;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Unity.Mathematics;


namespace IMRE.HandWaver{
public class spencerIntersections : MonoBehaviour
{
    public float crossSectionHeight = .5f;
   
    [Range(0, 2f * Mathf.PI)]
    public float theta;

    void Start()
    {
 //TODO @Camden render the basic figures
    //TODO render circle
    //TODO render annulus
    //TODO render sphere
    //TODO render torus
    //TODO render hypersphere
    //TODO render hypertorus
    }
    
    void Update()
    {   
//TODO @Camden make these functions dynamically accept the paramters from the figures
        crossSectCirc(crossSectionHeight);
        crossSectAnnulus(crossSectionHeight);
        crossSectSphere(crossSectionHeight);
        crossSectTorus(crossSectionHeight);
    }

    /// <summary>
    /// Function to calculate cross section of circle
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="height"></param>
    public void crossSectCirc(float height)
    {
        
        float radius = 1f;
               
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

    /// <summary>
    /// Function to calculate cross section of an annulus 
    /// </summary>
    /// <param name="height"></param>
    public void crossSectAnnulus(float height)
    {
        float outerRadius = 1f;
        float innerRadius = 0.75f;
       
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
        }
        //cross section is a line segment in between the inner circle and outer circle
        else if (Math.Abs(height) < outerRadius && Math.Abs(height) >= innerRadius)
        {
            //horizontal distance from center to point on outer edge of annulus
            x1 = (Mathf.Sqrt(1f - Mathf.Pow(height, 2)));
           
            //calculations for coordinates of line segment endpoints
            segmentAEndPoint0 = (Vector3.up * height) + (Vector3.right * (x1));
            segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x1));
        }
        //cross section height is less than the inner radius, resulting in two line segments
        else if (Math.Abs(height) < innerRadius )
        {
            //horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
            x1 = (Mathf.Sqrt(1f - Mathf.Pow(height, 2)));
            x2 = (Mathf.Sqrt(0.75f - Mathf.Pow(height, 2)));
            
            //calculations for inner and outer endpoints for each line segment
            segmentAEndPoint0 = (Vector3.up * height) + (Vector3.left * (x1));
            segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x2));
            
            segmentBEndPoint0 = (Vector3.up * height) + (Vector3.right * (x2));
            segmentBEndPoint1 = (Vector3.up * height) + (Vector3.right * (x1));
        }
        //cross section height is out of range of annulus
        else if (Math.Abs(height) > outerRadius)
        {
            Debug.Log("Height is out of range of object.");
        }

    }
    
    /// <summary>
    /// Function to calculate cross section of a sphere
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="center"></param>
    /// <param name="height"></param>
    /// <param name="planeNorm"></param>
    public void crossSectSphere(float height)
    {
        float radius = 1f;
        float circRadius;
        Vector3 circCenter;

        //If cross section only hits edge of sphere
        if (Math.Abs(height) == radius)
        {
            //if top of sphere, create circle with radius 0 at intersection
            if (height == radius)
            {
                circCenter = Vector3.up * height;
                circRadius = 0;
            }
            //if bottom of sphere, create circle with radius 0 at intersection
            if (height == -radius)
            {
                circCenter = Vector3.down * height;
                circRadius = 0;
            }
        }
        //if cross section cuts through sphere, create *circle*
        else if (Math.Abs(height) < radius)
        {
            circCenter = Vector3.up * height;
            circRadius = Mathf.Sqrt(1 - Mathf.Pow(height, 2));
            Vector3 normPlane = Vector3.up;
            
        }
        else if (Math.Abs(height) > radius)
        {
                Debug.Log("Height is out of range of object.");
        }
        



    }
    /// <summary>
    /// Function to calculate cross section of a torus
    /// </summary>
    /// <param name="height"></param>
    public void crossSectTorus(float height)
    {
        float innerRadius = 0.20f;
        float outerRadius = 1f;
        
        //radius of 2d circle for torus
        const float circleRadius = 0;
        
        //distance from axis that circle will be rotated on
        const float rotateRadius = 0;
        
        //resulting plane's distance from center
        const float planeDistance = 0;
        Vector3 torusCenter = Vector3.zero;
        
        Vector3 pointPos;

        //convert values to variables for equation
        float d = 2f * (float)(Math.Pow(circleRadius, 2) + Math.Pow(rotateRadius, 2) - Math.Pow(planeDistance, 2));
        float e = 2f * (float) (Math.Pow(circleRadius, 2) - Math.Pow(rotateRadius, 2) - Math.Pow(planeDistance, 2));
        float f = -(circleRadius + rotateRadius + planeDistance) * (circleRadius + rotateRadius - planeDistance) * (circleRadius - rotateRadius + planeDistance) * (circleRadius - rotateRadius - planeDistance);

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
        }
        
        //height is not within torus
        else if (Math.Abs(height) > outerRadius)
        {
            Debug.Log("Height is out of range of object.");
        }
        
        //cross section results in spiric shape
        else
        {
            for (float i = 0; i < Mathf.PI * 2f; i++)
            {
                spiricMath(theta, d, e, f);
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
    private float3x2 spiricMath(float theta, float d, float e, float f)
    {
        //distance results 
        float r0;
        float r1;

        r0 = Mathf.Sqrt(
                 -Mathf.Sqrt(
                     Mathf.Pow(-d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta), 2) +
                     4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) + e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
             Mathf.Sqrt(2);
        
        r1 = Mathf.Sqrt(
                 Mathf.Sqrt(
                     Mathf.Pow(-d * Mathf.Cos(theta) * Mathf.Cos(theta) - e * Mathf.Sin(theta) * Mathf.Sin(theta), 2) +
                     4 * f) + d * Mathf.Cos(theta) * Mathf.Cos(theta) + e * Mathf.Sin(theta) * Mathf.Sin(theta)) /
             Mathf.Sqrt(2);;
        
        float3x2 result = new float3x2();
        
        //distance results converted to theta
        result.c0 = r0 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward);
        result.c1 = r1 * (Mathf.Cos(theta) * Vector3.right + Mathf.Sin(theta) * Vector3.forward);
        return result;
    }

    public void crossSectHyperSphere(float height)
    {
        
    }

    public void crossSectHyperTorus(float height)
    {
        
    }

}
}
