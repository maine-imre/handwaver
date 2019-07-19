using IMRE.HandWaver;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.Math
{
    public static class Operations
    {

       public static float Angle(float3 from, float3 to)
        {
            return math.acos(math.dot(math.normalize(from), math.normalize(to)));
        }

        public static float magnitude(float3 v)
        {
            return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static float magnitude(float4 v)
        {
            return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w);
        }

        public static float4 cross(float4 v, float4 w, float4 u)
        {
            //from https://github.com/hollasch/ray4/blob/master/wire4/v4cross.c

            float A, B, C, D, E, F; /* Intermediate Values */

            A = (v.x * w.y) - (v.y * w.x);
            B = (v.x * w.z) - (v.z * w.x);
            C = (v.x * w.w) - (v.w * w.x);
            D = (v.y * w.z) - (v.z * w.y);
            E = (v.y * w.w) - (v.w * w.y);
            F = (v.z * w.w) - (v.w * w.z);

            return new float4(
                u[1] * F - u[2] * E + u[3] * D,
                -(u[0] * F) + u[2] * C - u[3] * B,
                u[0] * E - u[1] * C + u[3] * A,
                -(u[0] * D) + u[1] * B - u[2] * A
            );
        }

        public static float4 rotate(float4 from, float4 to, float theta)
        {
            float4 basis0 = math.normalize(from);
            float4 basis1 = math.normalize(to - project(to,from));
            return rotate(from, basis0, basis1, theta);
        }
        
        public static float4 rotate(float4 v, float4 basis0, float4 basis1, float theta)
        {
            math.normalize(basis0);
            math.normalize(basis1);

            float4 remainder = v - (project(v, basis0) + project(v, basis1));
            theta *= Mathf.Deg2Rad;

            float4 v2 = v;
            math.normalize(v2);

            if (math.dot(basis0, basis1) != 0f)
            {
                Debug.LogWarning("Basis is not orthagonal");
            }
            else if (math.dot(v2, basis0) != 1f || Vector4.Dot(v, basis1) != 0f)
            {
                Debug.LogWarning("Original Vector does not lie in the same plane as the first basis vector.");
            }

            return Vector4.Dot(v, basis0) * (math.cos(theta) * basis0 + basis1 * math.sin(theta)) +
                   math.dot(v, basis1) * (math.cos(theta) * basis1 + math.sin(theta) * basis0) + remainder;
        }

        public static float4 project(float4 v, float4 dir)
        {
            return math.dot(v, dir) * math.normalize(dir);
        } 

    }
}

