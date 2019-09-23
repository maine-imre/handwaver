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

           Vector3 a = vertices[0];
           Vector3 b = vertices[1];
           Vector3 c = vertices[2];
           
           Vector3 ab_hat = (b - a) / 


       }



    }
}
