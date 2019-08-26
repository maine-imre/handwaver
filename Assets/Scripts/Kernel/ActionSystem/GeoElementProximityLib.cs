using System;
using IMRE.HandWaver.Kernel.Geos;
using IMRE.Math;
using Unity.Mathematics;

namespace IMRE.HandWaver.Kernel
{
    public static class GeoElementProximityLib
    {
        public static float DistToGeo(GeoElement geo, float3 pos)
        {
            return (pos - ClosestPosition(geo, pos)).Magnitude();
        }

        public static float3 ClosestPosition(GeoElement geo, float3 pos)
        {
            switch (geo.Type)
            {
                case ElementType.point:
                    return ClosestPositionPoint(geo.F0, pos);
                case ElementType.line:
                    return ClosestPositionLine(GeoElementDataBase.GeoElements[geo.Deps[0]].F0, 
                        (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 - GeoElementDataBase.GeoElements[geo.Deps[0]].F0), pos);
                case ElementType.plane:
                    //TODO Handle case where plane is defined by 3 points.
                    return ClosestPositionPlane(GeoElementDataBase.GeoElements[geo.Deps[0]].F0, geo.F0, pos);
                case ElementType.sphere:
                    return ClosestPositionSphere(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                        (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                         GeoElementDataBase.GeoElements[geo.Deps[0]].F0).Magnitude()
                        , pos);
                case ElementType.circle:
                    return ClosestPositionCircle(GeoElementDataBase.GeoElements[geo.Deps[0]].F0,
                        (GeoElementDataBase.GeoElements[geo.Deps[1]].F0 -
                         GeoElementDataBase.GeoElements[geo.Deps[0]].F0).Magnitude(), geo.F0, pos);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static float3 ClosestPositionSphere(float3 sphereCenter, float sphereRadius, float3 pos)
        {
            return math.normalize(pos - sphereCenter) * sphereRadius;
        }

        private static float3 ClosestPositionPlane(float3 pointOnPlane, float3 planeNormal, float3 pos)
        {
            return pos - Operations.Project(pos - pointOnPlane, planeNormal);
        }

        private static float3 ClosestPositionLine(float3 pointOnLine, float3 lineDirection, float3 pos)
        {
            return Operations.Project(pos - pointOnLine, lineDirection) + pointOnLine;
        }

        private static float3 ClosestPositionPoint(float3 point, float3 pos)
        {
            return point;
        }

        private static float3 ClosestPositionCircle(float3 circleCenter, float circleRadius, float3 circleNormal,
            float3 pos)
        {
            return math.normalize(ClosestPositionPlane(circleCenter, circleNormal, pos) - circleCenter) * circleRadius +
                   circleCenter;
        }
    }
}