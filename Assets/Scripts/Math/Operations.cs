namespace IMRE.Math
{
    public static class Operations
    {
        public static float Angle(Unity.Mathematics.float3 from, Unity.Mathematics.float3 to)
        {
            return Unity.Mathematics.math.acos(Unity.Mathematics.math.dot(Unity.Mathematics.math.normalize(from),
                       Unity.Mathematics.math.normalize(to))) * UnityEngine.Mathf.Rad2Deg;
        }

        public static float Angle(Unity.Mathematics.float4 from, Unity.Mathematics.float4 to)
        {
            return Unity.Mathematics.math.acos(Unity.Mathematics.math.dot(Unity.Mathematics.math.normalize(from),
                       Unity.Mathematics.math.normalize(to))) * UnityEngine.Mathf.Rad2Deg;
        }

        public static float magnitude(Unity.Mathematics.float3 v)
        {
            return Unity.Mathematics.math.sqrt((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
        }

        public static float magnitude(Unity.Mathematics.float4 v)
        {
            return Unity.Mathematics.math.sqrt((v.x * v.x) + (v.y * v.y) + (v.z * v.z) + (v.w * v.w));
        }

        public static Unity.Mathematics.float4 cross(Unity.Mathematics.float4 v, Unity.Mathematics.float4 w,
            Unity.Mathematics.float4 u)
        {
            //from https://github.com/hollasch/ray4/blob/master/wire4/v4cross.c

            float A, B, C, D, E, F; /* Intermediate Values */

            A = (v.x * w.y) - (v.y * w.x);
            B = (v.x * w.z) - (v.z * w.x);
            C = (v.x * w.w) - (v.w * w.x);
            D = (v.y * w.z) - (v.z * w.y);
            E = (v.y * w.w) - (v.w * w.y);
            F = (v.z * w.w) - (v.w * w.z);

            return new Unity.Mathematics.float4(
                ((u[1] * F) - (u[2] * E)) + (u[3] * D),
                (-(u[0] * F) + (u[2] * C)) - (u[3] * B),
                ((u[0] * E) - (u[1] * C)) + (u[3] * A),
                (-(u[0] * D) + (u[1] * B)) - (u[2] * A)
            );
        }

        public static Unity.Mathematics.float4 rotate(Unity.Mathematics.float4 from, Unity.Mathematics.float4 to,
            float theta)
        {
            Unity.Mathematics.float4 basis0 = Unity.Mathematics.math.normalize(from);
            Unity.Mathematics.float4 basis1 = Unity.Mathematics.math.normalize(to - project(to, Unity.Mathematics.math.normalize(from)));
            return rotate(from, basis0, basis1, theta);
        }

        public static Unity.Mathematics.float4 rotate(Unity.Mathematics.float4 v, Unity.Mathematics.float4 basis0,
            Unity.Mathematics.float4 basis1, float theta)
        {
            Unity.Mathematics.math.normalize(basis0);
            Unity.Mathematics.math.normalize(basis1);

            Unity.Mathematics.float4 remainder = v - (project(v, basis0) + project(v, basis1));
            theta *= UnityEngine.Mathf.Deg2Rad;

            Unity.Mathematics.float4 v2 = v;
            Unity.Mathematics.math.normalize(v2);

            if (Unity.Mathematics.math.dot(basis0, basis1) != 0f)
                UnityEngine.Debug.LogWarning("Basis is not orthagonal : " + Unity.Mathematics.math.dot(basis0, basis1));
            else if ((Unity.Mathematics.math.dot(v2, basis0) != 1f) || (UnityEngine.Vector4.Dot(v, basis1) != 0f))
                UnityEngine.Debug.LogWarning(
                    "Original Vector does not lie in the same plane as the first basis vector.");

            return (UnityEngine.Vector4.Dot(v, basis0) *
                    ((Unity.Mathematics.math.cos(theta) * basis0) + (basis1 * Unity.Mathematics.math.sin(theta)))) +
                   (Unity.Mathematics.math.dot(v, basis1) *
                    ((Unity.Mathematics.math.cos(theta) * basis1) + (Unity.Mathematics.math.sin(theta) * basis0))) +
                   remainder;
        }

        public static Unity.Mathematics.float4 project(Unity.Mathematics.float4 v, Unity.Mathematics.float4 dir)
        {
            return Unity.Mathematics.math.dot(v, dir) * Unity.Mathematics.math.normalize(dir);
        }
    }
}