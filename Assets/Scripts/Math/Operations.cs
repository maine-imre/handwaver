using Unity.Mathematics;

namespace IMRE.Math
{
    public static class Operations
    {
        public static float Angle(float3 from, float3 to)
        {
            return math.acos(math.dot(math.normalize(from),
                math.normalize(to)));
        }
    }
}