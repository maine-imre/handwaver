using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.ScaleStudy

{

    public class TriangleCrossSection : UnityEngine.MonoBehaviour//, IMRE.HandWaver.ScaleStudy.ISliderInput
    {
       /// <summary>
       /// Function to render the intersetion of a plane and a triangle
       /// </summary>
       /// <param name="height"></param>
       /// <param name="vertices"></param>
       /// <param name="crossSectionRenderer"></param>
        public void crossSectTri(float height, Vector3[] vertices, LineRenderer crossSectionRenderer)
        {
            Vector3 segmentEndPoint0, segmentEndPoint1;
            Vector3 triangleTop = vertices[0];
            Vector3 bottomRight = vertices[1];
            Vector3 bottomLeft = vertices[2];

            //TODO: determine what we want to do with points
            if (height == triangleTop.y)
            {
                segmentEndPoint0 = triangleTop;
                crossSectionRenderer.enabled = true;
                crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                crossSectionRenderer.SetPosition(1, segmentEndPoint0);
            }

            if (height < triangleTop.y)
            {
                segmentEndPoint0 = new Vector3(0, triangleTop.y - height, -(height / math.sqrt(3)));
                segmentEndPoint1 = new Vector3(0, triangleTop.y - height, -(height / math.sqrt(3)));
                crossSectionRenderer.enabled = true;
                crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                crossSectionRenderer.SetPosition(1, segmentEndPoint1);
            }
                
               

        }



    }
}
