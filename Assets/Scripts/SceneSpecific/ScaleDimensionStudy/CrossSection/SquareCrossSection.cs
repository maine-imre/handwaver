using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.ScaleStudy
{
    public class SquareCrossSection : UnityEngine.MonoBehaviour
    {

        /// <summary>
        /// Function to render the intersection of a plane and a square
        /// </summary>
        /// <param name="height"></param>
        /// <param name="vertices"></param>
        /// <param name="crossSectionRenderer"></param>
        public void crossSectSquare(float3 height, float3 direction, Vector3[] vertices, LineRenderer crossSectionRenderer)
        {
            Vector3 topLeft = vertices[0];
            Vector3 topRight = vertices[1];
            Vector3 bottomRight = vertices[2];
            Vector3 bottomLeft = vertices[3];

            Vector3 segmentEndPoint0;
            Vector3 segmentEndPoint1;

            //TODO: Determine if we want to include points for end of segment
            if (height == topLeft.y && height == topRight.y)
            {
                segmentEndPoint0 = new Vector3(topLeft.x, topLeft.y, topLeft.z);
                segmentEndPoint1 = new Vector3(topRight.x, topRight.y, topRight.z);

                crossSectionRenderer.enabled = true;
                crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                crossSectionRenderer.SetPosition(1, segmentEndPoint1);
            }
            
            else if (height > topLeft.y || height < bottomRight.y)
            {
                
            }
            
            else if ((height < topLeft.y && height >= bottomLeft.y) || (height < topRight.y && height <= bottomLeft.y))
            {
                segmentEndPoint0 = new Vector3(0, topLeft.y - height, topLeft.z);
                segmentEndPoint1 = new Vector3(0, topRight.y - height, topRight.z);

                crossSectionRenderer.enabled = true;
                crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                crossSectionRenderer.SetPosition(1, segmentEndPoint1);
            }
        }
    }
}
    
