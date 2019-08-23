using IMRE.HandWaver.Kernel.Geos;
using IMRE.Math;
using Unity.Mathematics;

namespace IMRE.HandWaver.Kernel
{
    public static class GeoElementProximityLib
    {
        public static float distToGeo(GeoElement geo, float3 pos)
        {
            return Operations.Magnitude(pos - closestPosition(geo, pos));
        }

        public static float3 closestPosition(GeoElement geo, float3 pos)
        {
            //TODO switch by type
            return float3.zero;
        }

        private static float3 closestPositionSphere(float3 sphereCenter, float sphereRadius, float3 pos)
        {
            return math.normalize(pos - sphereCenter) * sphereRadius;
        }

        private static float3 closestPositionPlane(float3 pointOnPlane, float3 planeNormal, float3 pos)
        {
            return pos - Operations.Project(pos - pointOnPlane, planeNormal);
        }

        private static float3 closestPositionLine(float3 pointOnLine, float3 lineDirection, float3 pos)
        {
            return Operations.Project(pos - pointOnLine, lineDirection) + pointOnLine;
        }

        private static float3 closestPositionPoint(float3 point, float3 pos)
        {
            return point;
        }

        private static float3 closestPositionCircle(float3 circleCenter, float circleRadius, float3 circleNormal,
            float3 pos)
        {
            return math.normalize(closestPositionPlane(circleCenter, circleNormal, pos) - circleCenter) * circleRadius +
                   circleCenter;
        }
    }
}