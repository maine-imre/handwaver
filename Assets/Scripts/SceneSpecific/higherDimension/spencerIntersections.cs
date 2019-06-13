using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using UnityEngine;

public class intersectionScript : MonoBehaviour
{
    public float crossSectionHeight = .5f;

    void Start()
    {   
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
                        //comment here to explain what this condition case is geometerically

            //the positing of the point is a Vector3.  
            //(the point is a 0 dimensional figure, but it exists in a three-dimensional space
            float pointPos = height;
            
            //suggest this:
            //Vector3 pointPos = 
            //Debug.Log(pointPos);
        }
        else if (height < radius)
        {
                        //comment here to explain what this condition case is geometerically

            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have one line segment.  So you want one pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
            
            Vector2 linePos = new Vector2();
            
            //suggest this:
            //Vector3 SegmentAEndpoint0 = 
            //Vector3 SegmentAEndpoint1 = 
            //Debug.Log(SegmentAEndpoint0, SegmentAEndpoint1);
         }
         //what if height > radius

    }

    public void crossSectAnnulus(float innerRadius, float outerRadius, float height)
    {
        //you might think about this as two different circle intersection problems, and use your previous function
    
        //assuming annulus is centered at zero.
        //assuming annulus is in which plane?
        
                //if the center is at zero, consider that the height could be positive or negative.


        if (height == outerRadius)
        {
                        //comment here to explain what this condition case is geometerically

            //the positing of the point is a Vector3.  
            //(the point is a 0 dimensional figure, but it exists in a three-dimensional space
            float pointPos = height;
            
            //suggest this:
            //Vector3 pointPos = 
            //Debug.Log(pointPos);
        }
        else if (height < innerRadius )
        {
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

        }
        else if (height > innerRadius && height < outerRadius)
        {
                //comment here to explain what this condition case is geometerically

            //if the height is smaller than the inner radius, then each line segment is defined by 2 Vector3s (one for each endpoint).
            //and you have one line segment.  So you want one pairs of Vector3's.
            //even though the figure is 2D, it still exists in a 3D space.
            
            Vector2 linePos = new Vector2();
            
            //suggest this:
            //Vector3 SegmentAEndpoint0 = 
            //Vector3 SegmentAEndpoint1 = 
            //Debug.Log(SegmentAEndpoint0, SegmentAEndpoint1);
            
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
        //comment here to explain what this case is geometerically
            Vector3 pointPos = new Vector3();
        }
        else if (radius > (circCenter - center).magnitude)
        {
         //comment here to explain what this case is geometerically
            Vector3 planePos = new Vector3();
            //in this case, we also want a radius and a center of the circle.
        }
        //what if radius < 9circCenter-center).magnitude?

    }

    public void crossSectTorus(float innerRadius, float outerRadius, Vector3 center, Vector3 height, Vector3 plane1Norm, Vector3 plane2Norm)
    {
        
        
    }

}
