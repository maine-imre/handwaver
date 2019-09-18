using System;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.ActionSystem;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Kernel.GGBFunctions;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.ActionSystem
{
    public class MoveGeoElement : GeoElementFunction
    {
        public override void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier)
        {
            //TODO add rotations to this script.  As written, the script only handles translations.
            float3 diff = (classifier.origin - GeoElementProximityLib.ClosestPosition(geo, classifier.origin));

            float3 pointA;
            float3 pointB;

            switch (geo.Type)
            {
                case ElementType.point:
                    MovePoint(geo.ElementName.ToString(), classifier.origin);
                    Debug.Log("I'm here");
                    break;

                case ElementType.plane:
                    pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                    //Change the location of the point used to define the plane.
                    MovePoint(GeoElementDataBase.GetElement(geo.Deps[0]).ElementName.ToString(), pointA);

                    break;
                case ElementType.sphere:
                case ElementType.line:
                case ElementType.circle:
                    pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                    pointB = GeoElementDataBase.GeoElements[geo.Deps[1]].F0 + diff;
                    
                    MovePoint(GeoElementDataBase.GetElement(geo.Deps[0]).ElementName.ToString(), pointA);
                    MovePoint(GeoElementDataBase.GetElement(geo.Deps[1]).ElementName.ToString(), pointA);
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
        public void MovePoint(string elementName, float3 newPos)
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