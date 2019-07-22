namespace IMRE.Math
{
    public static class Operations
    {
        public static float Angle(Unity.Mathematics.float3 from, Unity.Mathematics.float3 to)
        {
            return Unity.Mathematics.math.acos(Unity.Mathematics.math.dot(Unity.Mathematics.math.normalize(from),
                Unity.Mathematics.math.normalize(to)));
        }
    }
}