using System;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.ActionSystem;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.HandWaver.Kernel.GGBFunctions;
using UnityEngine;

namespace IMRE.HandWaver.Kernel.ActionSystem
{
    public class MoveGeoElement : GeoElementFunction
    {
        public override void geoElementFunction(GeoElement geo, EmbodiedClassifier classifier)
        {
        //TODO add rotations to this script.  As written, the script only handles translations.
                switch (geo.Type)
                {
                    float3 diff = (classifier.origin-GeoElementProximityLib.ClosestPosition(geo, classifier.origin));
                    case ElementType.point:
                    StartCoroutine(HandWaverServerTransport.execCommand(
                    "geo.ElementName.ToString() + " = ("+classifier.origin.x+","+classifier.origin.y+","+classifier.origin.z+")"));
                        Debug.Log("I'm here");
                        break;
                    case ElementType.line:
                        float3 pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                        float3 pointB = GeoElementDataBase.GeoElements[geo.Deps[1]].F0 + diff;
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[0].ElementName.ToString() + " = ("+pointA.x+","+pointA.y+","+pointA.z+")"));
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[1].ElementName.ToString() + " = ("+pointB.x+","+pointB.y+","+pointB.z+")"));
                        break;
                    case ElementType.plane:
                        float3 pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[0].ElementName.ToString() + " = ("+pointA.x+","+pointA.y+","+pointA.z+")"));
                        break;
                    case ElementType.sphere:
                        float3 pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                        float3 pointB = GeoElementDataBase.GeoElements[geo.Deps[1]].F0 + diff;
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[0].ElementName.ToString() + " = ("+pointA.x+","+pointA.y+","+pointA.z+")"));
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[1].ElementName.ToString() + " = ("+pointB.x+","+pointB.y+","+pointB.z+")"));
                        break;
                    case ElementType.circle:
                        float3 pointA = GeoElementDataBase.GeoElements[geo.Deps[0]].F0 + diff;
                        float3 pointB = GeoElementDataBase.GeoElements[geo.Deps[1]].F0 + diff;
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[0].ElementName.ToString() + " = ("+pointA.x+","+pointA.y+","+pointA.z+")"));
                        StartCoroutine(HandWaverServerTransport.execCommand(
                        geo.Deps[1].ElementName.ToString() + " = ("+pointB.x+","+pointB.y+","+pointB.z+")"));
                        break;
                    case defaut;
                        debug.LogWarning("Type Not Supported");
                        break;
            }
        }
    }
}
