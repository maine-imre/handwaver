using Unity.Mathematics;
using UnityEngine;

namespace IMRE.Math
{
    public static class Operations
    {
        private const float TOLERANCE = 0.00001f;

        public static float Angle(float3 from, float3 to)
        {
            return math.acos(math.dot(math.normalize(from),
                       math.normalize(to))) * Mathf.Rad2Deg;
        }

        public static float Angle(float4 from, float4 to)
        {
            return math.acos(math.dot(math.normalize(from),
                       math.normalize(to))) * Mathf.Rad2Deg;
        }

        public static float Magnitude(this float3 v)
        {
            return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static float Magnitude(this float4 v)
        {
            return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w);
        }

        public static float4 Cross(float4 v, float4 w, float4 u)
        {
            //from https://github.com/hollasch/ray4/blob/master/wire4/v4cross.c

            float A, B, C, D, E, F; /* Intermediate Values */

            A = v.x * w.y - v.y * w.x;
            B = v.x * w.z - v.z * w.x;
            C = v.x * w.w - v.w * w.x;
            D = v.y * w.z - v.z * w.y;
            E = v.y * w.w - v.w * w.y;
            F = v.z * w.w - v.w * w.z;

            return new float4(
                u[1] * F - u[2] * E + u[3] * D,
                -(u[0] * F) + u[2] * C - u[3] * B,
                u[0] * E - u[1] * C + u[3] * A,
                -(u[0] * D) + u[1] * B - u[2] * A
            );
        }

        public static float4 Rotate(float4 from, float4 to,
            float theta)
        {
            var basis0 = math.normalize(from);
            var basis1 = math.normalize(to - Project(to, math.normalize(from)));
            return Rotate(from, basis0, basis1, theta);
        }

        public static float4 Rotate(float4 v, float4 basis0,
            float4 basis1, float theta)
        {
            math.normalize(basis0);
            math.normalize(basis1);

            var remainder = v - (Project(v, basis0) + Project(v, basis1));
            theta *= Mathf.Deg2Rad;

            var v2 = v;
            math.normalize(v2);

            if (System.Math.Abs(math.dot(basis0, basis1)) > TOLERANCE)
                Debug.LogWarning("Basis is not orthogonal : " + math.dot(basis0, basis1));
            else if (System.Math.Abs(math.dot(v2, basis0) - 1f) > TOLERANCE ||
                     System.Math.Abs(Vector4.Dot(v, basis1)) > TOLERANCE)
                Debug.LogWarning("Original Vector does not lie in the same plane as the first basis vector.");

            return Vector4.Dot(v, basis0) *
                   (math.cos(theta) * basis0 + basis1 * math.sin(theta)) +
                   math.dot(v, basis1) *
                   (math.cos(theta) * basis1 + math.sin(theta) * basis0) +
                   remainder;
        }

        public static float4 Project(float4 v, float4 dir)
        {
            return math.dot(v, dir) * math.normalize(dir);
        }

        public static float3 Project(float3 v, float3 dir)
        {
            return math.dot(v, dir) * math.normalize(dir);
        }
    }
}