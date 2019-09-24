using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.ScaleStudy

{

    public class TriangleCrossSection : UnityEngine.MonoBehaviour
    {
       /// <summary>
       /// Function to render the intersection of a plane and a triangle
       /// </summary>
       /// <param name="height"></param>
       /// <param name="vertices"></param>
       /// <param name="crossSectionRenderer"></param>
        public void crossSectTri(float3 point, float3 direction, Vector3[] vertices, LineRenderer crossSectionRenderer)
       {

           crossSectionRenderer = new LineRenderer();
           
           //Vertices are organized in clockwise manner
           float3 a = vertices[0];
           float3 b = vertices[1];
           float3 c = vertices[2];

           float3 segmentab = b - a;
           float3 segmentac = c - a;
           float3 segmentbc = c - b;
           float3 intersectionLine = direction - point;
           
           float3 ac_hat = (c - a) / Vector3.Magnitude(c - a);
           float3 ab_hat = (b - a) / Vector3.Magnitude(b - a);
           float3 bc_hat = (c - b) / Vector3.Magnitude(c - b);

           float3 ac_star = Vector3.Project(intersectionLine - segmentac, ac_hat) + new Vector3(a.x, a.y, a.z);
           float3 ab_star = Vector3.Project(intersectionLine - segmentab, ab_hat) + new Vector3(a.x, a.y, a.z);
           float3 bc_star = Vector3.Project(intersectionLine - segmentbc, bc_hat) + new Vector3(b.x, b.y, b.z);

           //If plane does not hit triangle
           if (Vector3.Magnitude(ac_star - a) > Vector3.Magnitude(c - a) ||
               Vector3.Magnitude(ac_star - c) > Vector3.Magnitude(c - a))
           {
               Debug.Log("Plane does not intersect with triangle.");
           }

           //Point of intersection is a vertex (a or c)
           else if (Vector3.Magnitude(ac_star - a) == 0 || Vector3.Magnitude(ac_star - c) == 0)
           {
               crossSectionRenderer.enabled = true;

           }
           //Point of intersection is a or b
           else if (Vector3.Magnitude(ab_star - a) == 0 || Vector3.Magnitude(ab_star - b) == 0)
           {
               crossSectionRenderer.enabled = true;

           }
           
           else if (Vector3.Magnitude(bc_star - b) == 0 || Vector3.Magnitude(bc_star - c) == 0)
           {
               crossSectionRenderer.enabled = true;
               
              
           }
           
           //c_star is on segmentac
           else
           {
               
           }
       }



    }
}
