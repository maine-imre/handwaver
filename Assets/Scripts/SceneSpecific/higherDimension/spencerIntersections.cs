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
        Vector3 pointPos;
        
        
        //if cross section only hits the edge of the circle
        if (Math.Abs(height) == radius)
        {
            //if top of circle, create point at intersection
            if (height == radius)
            {
                pointPos = Vector3.up * radius;
            }
            //if bottom of circle, create point at intersection
            else if (height == -radius)
            {
                pointPos = Vector3.down * radius;
            }
            
//TODO @Camden Render Figure
        }

//TODO @Camden Render Figure

        //cross section is a line that hits two points on the circle (smaller than radius of circle)
        else if (Math.Abs(height) < radius)
        {
            float segmentLength = (float)(Math.Sqrt(1 - Math.Pow(height, 2)));
            Vector3 segmentEndPoint0 = (Vector3.up * height) + (Vector3.right * (segmentLength));
            Vector3 segmentEndPoint1 = (Vector3.up * height) + (Vector3.left * (segmentLength));

            //TODO @Camden Render Figure

        }
        else if (Math.Abs(height) > radius)
        {
                //do nothing - stop render of figure
        }
        
    }

    public void crossSectAnnulus(float innerRadius, float outerRadius, float height)
    {
        outerRadius = 1f;
        innerRadius = 0.5f;

        //you might think about this as two different circle intersection problems, and use bits of your previous function

        //
        if (Math.Abs(height) == outerRadius)
        {
            if (height == outerRadius)
            {
                Vector3 pointPos = Vector3.up * outerRadius;
            }
            else if (height == -outerRadius)
            {
                Vector3 pointPos = Vector3.down * outerRadius;
            }
//TODO @Camden Render Figure

        }
        else if (Math.Abs(height) == innerRadius)
        {
            
        }
        else if (Math.Abs(height) < innerRadius )
        {

            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have two line segments.  So you want two pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
            
            //suggest this:
            //Vector3 SegmentAEndpoint0 = 
            //Vector3 SegmentAEndpoint1 = 
            //Vector3 SegmentBEndpoint0 = 
            //Vector3 SegmentBEndpoint1 = 
            //Debug.Log(SegmentAEndpoint0, SegmentAEndpoint1, SegmentBEndpoint0, SegmentBEndpoint1);
//TODO @Camden Render Figure

        }
        else if (height > innerRadius && height < outerRadius)
        {
  //TODO @Rene calculate figure parameters


            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have one line segment.  So you want one pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
                        
            //suggest this:
            //Vector3 SegmentAEndpoint0 = 
            //Vector3 SegmentAEndpoint1 = 
            //Debug.Log(SegmentAEndpoint0, SegmentAEndpoint1);
 //TODO @Camden Render Figure
           
        }
        //here you've missed the case where height == innerRadius.

    }
    
    /// <summary>
    /// Function to calculate cross section of a sphere
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="center"></param>
    /// <param name="height"></param>
    /// <param name="planeNorm"></param>
    public void crossSectSphere(float height, float radius, Vector3 sphereCenter)
    {
        radius = 1f;
        float circRadius;
        Vector3 circCenter;

        //If cross section only hits edge of sphere
        if (Math.Abs(height) == radius)
        {
            if (height == radius)
            {
                circCenter = Vector3.up * height;
                circRadius = 0;
            }

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
        else
        {
            if (Math.Abs(height) > radius)
            {
                //do nothing
            }
        }



    }

    public void crossSectTorus(float innerRadius, float outerRadius, Vector3 center, Vector3 height, Vector3 plane1Norm, Vector3 plane2Norm)
    {
 //TODO @Rene calculate figure parameters
        
 //TODO @Camden Render Figure
       
    }

}
}
