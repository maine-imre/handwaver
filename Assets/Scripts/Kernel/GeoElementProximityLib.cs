namespace IMRE.HandWaver.Kernel{

	public static class GeoElementProximityLib{
		public static float distToGeo(GeoElement geo, Unity.Mathematics.float3 pos)
		{
			return 0f;
		}
		public static Unity.Mathematics.float3 closestPosition(GeoElement geo, Unity.Mathematics.float3 pos)
		{
			return Unity.Mathematics.float3.zero;
		}
		
		private static Unity.Mathematics.float3 closestPositionSphere(Unity.Mathematics.float3 sphereCenter, float sphereRadius, Unity.Mathematics.float3 pos){
			return Unity.Mathematics.math.normalize(pos-sphereCenter)*sphereRadius;
		}
		
		private static Unity.Mathematics.float3 closestPositionPlane(Unity.Mathematics.float3 pointOnPlane, Unity.Mathematics.float3 planeNormal, Unity.Mathematics.float3 pos)
		{
			return pos - IMRE.Math.Operations.project(pos - pointOnPlane, planeNormal);
		}
		
		private static Unity.Mathematics.float3 closestPositionLine(Unity.Mathematics.float3 pointOnLine, Unity.Mathematics.float3 lineDirection, Unity.Mathematics.float3 pos)
		{
			return IMRE.Math.Operations.project(pos - pointOnLine, lineDirection) + pointOneLine;
		}
		
		private static Unity.Mathematics.float3 closestPositionPoint(Unity.Mathematics.float3 point, Unity.Mathematics.float3 pos){
			return point;
		}
		
		private static Unity.Mathematics.float3 closestPositionCircle(Unity.Mathematics.float3 circleCenter, float circleRadius, Unity.Mathematics.float3 circleNormal, Unity.Mathematics.float3 pos)
		{
			return Unity.Mathematics.math.normalize(closestPositionPlane(circleCenter, circleNormal, pos) - circleCenter)*circleRadius+circleCenter;
		}
	}
}
