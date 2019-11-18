using IMRE.HandWaver.Kernel;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.Math;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR;

namespace IMRE.EmbodiedAction
{
    public class GraspGeoObj : AbstractOneHandedAction
    {
        public float tolerance = .05f;
        public override void ActionImplementation(SteamVR_Input_Sources fromSource)
        {
            GeoElement geo = new GeoElement();
            //find closest point
            for (var i = 0; i < GeoElementDataBase.GeoElements.Length; i++)
            {
                //TODO address this to the correct location. @Joey
                float3 origin = float3.zero; //float3.zero is a standin for the center of the active grasp.
                
                if (!GeoElementDataBase.HasElement(i)) continue;
                float bestDist = tolerance;

                var closestPoint =
                    GeoElementProximityLib.ClosestPosition(GeoElementDataBase.GeoElements[i], origin);
                if (!((origin - closestPoint).Magnitude() < bestDist)) continue;
                geo = GeoElementDataBase.GeoElements[i];
                bestDist = (origin - closestPoint).Magnitude();
            }

            if (!geo.Equals(default(GeoElement))) GeoElementFunction(geo, fromSource);        
        }
        
        private void GeoElementFunction(GeoElement geo, SteamVR_Input_Sources fromSource)
        {
            //TODO address this to the correct location. @Joey
            float3 origin = float3.zero; //float3.zero is a standin for the center of the active grasp.
            
            //TODO add rotations to this script.  As written, the script only handles translations.
            //TODO Convert script into using a centralized script for object creation/update.
            float3 diff = (origin - GeoElementProximityLib.ClosestPosition(geo, origin));

            float3 pointA;
            float3 pointB;

            switch (geo.Type)
            {
                case ElementType.point:
                    MovePoint(geo.ElementName.ToString(), origin);
                    Debug.Log("I'm here");
                    break;

                case ElementType.plane:
                    pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                    //Change the location of the point used to define the plane.
                    MovePoint(GeoElementDataBase.GetElement(geo.Deps[0]).ElementName.ToString(), pointA);

                    break;
                case ElementType.sphere:
                    //TODO method for sphere
                    break;
                case ElementType.line:
                    //TODO method for line
                    break;
                case ElementType.circle:
                    pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                    pointB = GeoElementDataBase.GeoElements[geo.Deps[1]].F0 + diff;
                    
                    MovePoint(GeoElementDataBase.GetElement(geo.Deps[0]).ElementName.ToString(), pointA);
                    MovePoint(GeoElementDataBase.GetElement(geo.Deps[1]).ElementName.ToString(), pointB);
                    break;
                default:
                    Debug.LogWarning("Type Not Supported");
                    break;
            }
        }

        /// <summary>
        /// This function will take the element name and a new position and assign name to be a point at the location of newPos
        /// </summary>
        /// <param name="elementName">Element to be assigned</param>
        /// <param name="newPos">Location of the point</param>
        private void MovePoint(string elementName, float3 newPos)
        {
            if(GeoElementDataBase.GetElement(elementName).Type!= ElementType.point)
            {
                Debug.LogErrorFormat(
                    "Attempting to move object {0} with MovePoint function. This will cause the element to become a point rather than {1}.",
                    elementName,
                    GeoElementDataBase.GetElement(elementName).Type);
                return;
            }
            StartCoroutine(HandWaverServerTransport.execCommand(
                elementName + " = (" + newPos.x + "," + newPos.y + "," + newPos.z + ")"));
        }
    }
}
