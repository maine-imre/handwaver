using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using Leap;
using UnityEngine;

namespace IMRE.HandWaver{
public class spencerIntersections : MonoBehaviour
{
    public float crossSectionHeight = .5f;

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
        crossSectCirc(1f,crossSectionHeight);
        crossSectAnnulus(.5f,1f,crossSectAnnulus);
        crossSectSphere(1f,Vector3.zero,crossSectionHeight,Vector3.up);
        crossSectTorus(.5f, 1f, Vector3.zero, crossSectionHeight, Vector3.forward, Vector3.right)
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
            float segmentLength = (float)Math.Sqrt(1 - Math.Pow(height, 2));
            
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
        float x1, x2;

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
            x1 = (float)(Math.Sqrt(1 - Math.Pow(height, 2)));
           
            //calculations for coordinates of line segment endpoints
            segmentAEndPoint0 = (Vector3.up * height) + (Vector3.right * (x1));
            segmentAEndPoint1 = (Vector3.up * height) + (Vector3.left * (x1));
        }
        //cross section height is less than the inner radius, resulting in two line segments
        else if (Math.Abs(height) < innerRadius )
        {
            //horizontal distance from center to point on outer edge (x1) and inner edge (x2) of annulus
            x1 = (float)(Math.Sqrt(1 - Math.Pow(height, 2)));
            x2 = (float)(Math.Sqrt(0.75 - Math.Pow(height, 2)));
            
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
            circRadius = (float)Math.Sqrt(1 - Math.Pow(height, 2));
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
        float innerRadius = 0.75f;
        float outerRadius = 1f;
        float ellipse1RadiusA, ellipse1RadiusB, ellipse2Radius1, ellipse2Radius2;
        Vector3 ellipse1PointA, ellipse1PointB, ellipse2PointA, ellipse2PointB;
        Vector3 plane1Norm;
        Vector3 plane2Norm;
        

        if (Math.Abs(height) == outerRadius)
        {
            if (height == outerRadius)
            {
                ellipse1PointA = Vector3.up * height;
                ellipse1RadiusA = 0;
                ellipse1RadiusB = 0;
            }
            else if (height == -outerRadius)
            {
                ellipse1PointA = Vector3.down * height;
                ellipse1RadiusA = 0;
                ellipse1RadiusB = 0;
            }
        }
        else if (Math.Abs(height) < outerRadius && Math.Abs(height) >= innerRadius)
        {
            //float fixedRadius =
            float centerPoint = (float)Math.Sqrt(1 - Math.Pow(height, 2)) / 2;
            ellipse1PointA = Vector3.up * height + Vector3.left * centerPoint;
            ellipse1PointB = Vector3.up * height + Vector3.right * centerPoint;
        }
        else if (Math.Abs(height) < innerRadius)
        {
            
        }
        else if (Math.Abs(height) > outerRadius)
        {
                Debug.Log("Height is out of range of object.");
        }
        
    }

    public void crossSectHyperSphere(float height)
    {
        
    }

    public void crossSectHyperTorus(float height)
    {
        
    }

}
}
