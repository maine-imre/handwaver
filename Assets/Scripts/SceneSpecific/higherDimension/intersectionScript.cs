using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using UnityEngine;

public class intersectionScript : MonoBehaviour
{

    
    void Start()
    {   

    }

    public void crossSectCirc(float radius, float height)
    {
        
        if (height == radius)
        {
            float pointPos = height;
        }
        else if (height < radius)
        {
            Vector2 linePos = new Vector2();
        }

    }

    public void crossSectAnnulus(float innerRadius, float outerRadius, float height)
    {
        if (height == outerRadius)
        {
            float pointPos = height;
        }
        else if (height < innerRadius)
        {
            Vector2 linePos = new Vector2();
        }
        else if (height > innerRadius && height < outerRadius)
        {
            Vector2 linePos = new Vector2();
        }

    }
    
    public void crossSectSphere(float radius, Vector3 center, Vector3 height, Vector3 planeNorm)
    {
        Vector3 circCenter = Vector3.Project(height-center, planeNorm) + center;

        if (radius == (circCenter - center).magnitude)
        {
            Vector3 pointPos = new Vector3();
        }
        else if (radius > (circCenter - center).magnitude)
        {
            Vector3 planePos = new Vector3();
        }

    }

    public void crossSectTorus(float innerRadius, float outerRadius, Vector3 center, Vector3 height, Vector3 plane1Norm, Vector3 plane2Norm)
    {
        
        
    }

}
