using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using UnityEngine;

namespace IMRE.HandWaver{
public class intersectionScript : MonoBehaviour
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

    public void crossSectCirc(float radius, float height)
    {
        //assuming circle is centered at zero.
        //assuming circle is in which plane?
        
        //if the circle is at zero, consider that the height could be positive or negative.
        
        if (height == radius)
        {
//TODO @Rene calculate figure parameters
                        //comment here to explain what this condition case is geometerically

            //the positing of the point is a Vector3.  
            //(the point is a 0 dimensional figure, but it exists in a three-dimensional space
            float pointPos = height;
            
            //suggest this:
            //Vector3 pointPos = Vector3.up*radius;
            //Debug.Log(pointPos);
//TODO @Camden Render Figure
        }
        //else if (-height == radius)
        //{
//TODO @Rene calculate figure parameters

                                    //comment here to explain what this condition case is geometerically

            //the positing of the point is a Vector3.  
            //(the point is a 0 dimensional figure, but it exists in a three-dimensional space
            
            //Vector3 pointPos = Vector3.down*radius;
            //Debug.Log(pointPos);
 //TODO @Camden Render Figure

       // }
        else if (height < radius)
        {
 //TODO @Rene calculate figure parameters

                        //comment here to explain what this condition case is geometerically

            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have one line segment.  So you want one pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
            
            Vector2 linePos = new Vector2();
            
            //suggest this:
            //Vector3 SegmentAEndpoint0 = 
            //Vector3 SegmentAEndpoint1 = 
            //Debug.Log(SegmentAEndpoint0, SegmentAEndpoint1);
            
 //TODO @Camden Render Figure

         }
         //what if height > radius

    }

    public void crossSectAnnulus(float innerRadius, float outerRadius, float height)
    {

        //you might think about this as two different circle intersection problems, and use bits of your previous function
    
        //assuming annulus is centered at zero.
        //assuming annulus is in which plane?
        
                //if the center is at zero, consider that the height could be positive or negative.


        if (height == outerRadius)
        {
 //TODO @Rene calculate figure parameters

                        //comment here to explain what this condition case is geometerically

            //the positing of the point is a Vector3.  
            //(the point is a 0 dimensional figure, but it exists in a three-dimensional space
            float pointPos = height;
            
            //suggest this:
            //Vector3 pointPos = 
            //Debug.Log(pointPos);
//TODO @Camden Render Figure

        }
        else if (height < innerRadius )
        {
 //TODO @Rene calculate figure parameters

                        //comment here to explain what this condition case is geometerically

            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have two line segments.  So you want two pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
            Vector2 linePos = new Vector2();
            
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

                //comment here to explain what this condition case is geometerically

            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have one line segment.  So you want one pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
            
            Vector2 linePos = new Vector2();
            
            //suggest this:
            //Vector3 SegmentAEndpoint0 = 
            //Vector3 SegmentAEndpoint1 = 
            //Debug.Log(SegmentAEndpoint0, SegmentAEndpoint1);
 //TODO @Camden Render Figure
           
        }
        //here you've missed the case where height == innerRadius.

    }
    
    public void crossSectSphere(float radius, Vector3 center, Vector3 height, Vector3 planeNorm)
    {
            //comment here to explain what this case is geometerically

                //if the center is at zero, consider that the height could be positive or negative.


        Vector3 circCenter = Vector3.Project(height-center, planeNorm) + center;

        if (radius == (circCenter - center).magnitude)
        {
 //TODO @Rene calculate figure parameters

        //comment here to explain what this case is geometerically
            Vector3 pointPos = new Vector3();
//TODO @Camden Render Figure
            
        }
        else if (radius > (circCenter - center).magnitude)
        {
 //TODO @Rene calculate figure parameters

         //comment here to explain what this case is geometerically
            Vector3 planePos = new Vector3();
            //in this case, we also want a radius and a center of the circle.
 //TODO @Camden Render Figure
           
        }
        //what if radius < 9circCenter-center).magnitude?

    }

    public void crossSectTorus(float innerRadius, float outerRadius, Vector3 center, Vector3 height, Vector3 plane1Norm, Vector3 plane2Norm)
    {
 //TODO @Rene calculate figure parameters
        
 //TODO @Camden Render Figure
       
    }

}
}
