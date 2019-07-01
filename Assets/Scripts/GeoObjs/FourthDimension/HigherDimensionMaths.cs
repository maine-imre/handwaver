using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.HigherDimensions
{
    public enum Axis4D
        {
            xy,
            xz,
            xw,
            yz,
            yw,
            zw,
        }

/// <summary>
/// Maths for 4D to 3D projection.
/// </summary>
	public static class HigherDimensionsMaths
    {
        /// <summary>
        /// Rotate the vector v around a plane for a given angle, in degrees.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="basis0"></param>
        /// <param name="basis1"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static Vector4 rotate(this Vector4 v, Vector4 basis0, Vector4 basis1, float theta)
        {
            basis0 = basis0.normalized;
            basis1 = basis1.normalized;
            Vector4 remainder = v - (Vector4.Project(v, basis0) + Vector4.Project(v, basis1));
            theta *= Mathf.Deg2Rad;

            if (Vector3.Dot(basis0, basis1) != 0f)
            {
                Debug.LogWarning("Basis is not orthagonal");
            }
            else if (Vector4.Dot(v.normalized, basis0) != 1f || Vector4.Dot(v, basis1) != 0f)
            {
                Debug.LogWarning("Original Vector does not lie in the same plane as the first basis vector.");
            }
            return Vector4.Dot(v, basis0) * (Mathf.Cos(theta) * basis0 + basis1 * Mathf.Sin(theta)) + Vector4.Dot(v, basis1) * (Mathf.Cos(theta) * basis1 + Mathf.Sin(theta) * basis0) + remainder;
        }

        /// <summary>
        /// Projects a vector onto another vector using a dot product.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static float4 project(this float4 v, float4 axis)
        {
            math.normalize(axis);
            return math.dot(v, axis) * axis;
        }

        /// <summary>
        /// Writes the projection of a float4 onto a new basis for the hyperplane.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float3 projectDownDimension(this float4 v, float4x3 basis)
        {
            math.normalize(basis.c0);
            math.normalize(basis.c1);
            math.normalize(basis.c2);
            
            return new float3(math.dot(v,basis.c0), math.dot(v,basis.c1), math.dot(v,basis.c2));
        }

        public static float3[] projectDownDimension(this float4[] vectors, float4 axis)
        {
            float4x3 basis = axis.basisSystem();
            float3[] result = new float3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                result[i] = vectors[i].projectDownDimension(basis);
            }

            return result;
        }

        /// <summary>
        /// Return the basis of a hyper plane orthagonal to a given vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float4x3 basisSystem(this float4 v)
        {
            math.normalize(v);
            //use method described here:  https://www.geometrictools.com/Documentation/OrthonormalSets.pdf
            if (v.x == 0 && v.y == 0 && v.z == 0 && v.w ==0)
            {
                Debug.LogError("Can't form basis from zero vector");
            }
            //the vector is the first basis vector for the 4-space, orthag to the hyperplane
            math.normalize(v);
            //establish a second basis vector
            float4 basis0;
            if (v.x != 0 || v.y != 0)
            {
                basis0 = new Vector4(v.y, v.x, 0, 0);
            }
            else
            {
                basis0 = new Vector4(0 ,0,v.w,v.z);
            }

            math.normalize(basis0);

            float[] determinants = Determinant2X2(v, basis0);

            //index of largest determinant
            int idx = 0;
            for (int i = 0; i < 6; i++)
            {
                if (determinants[i] > determinants[idx])
                {
                    idx = i;
                }
            }

            if (determinants[idx] == 0)
            {
                Debug.LogError("No non-zero determinant");
            }
            //choose bottom row of det matrix to generate next basis vector
            float4 bottomRow;
            if (idx == 0 || idx == 1 || idx == 3)
            {
                bottomRow = new float4(0,0,0,1);
            }else if (idx == 2 || idx == 4)
            {
                bottomRow = new float4(0,0,1,0);
            }
            else
            {
                //idx = 5
                bottomRow = new float4(0,1,0,0);
            }

            float4 basis1 = determinantCoef(new float4x3(v, basis0, bottomRow));
            math.normalize(basis1);

            float4 basis2 = determinantCoef(new float4x3(v, basis0, basis1));
            math.normalize(basis2);
            
            //returns the basis that spans the hyperplane orthogonal to v
            float4x3 basis = new float4x3(basis0,basis1,basis2);
            //check that v is orthogonal.
            v.projectDownDimension(basis);
            if (v.x != 0 || v.y != 0 || v.z != 0)
            {
                Debug.LogError("Basis is not orthogonal to v");
            }
            return basis;
        }

        private static float[] Determinant2X2(float4 v0, float4 v1)
        {
            //find largest determinant of 2x2
            float[] determinants = new float[6];
            determinants[0] = v0.x * v1.y - v0.y * v1.x;
            determinants[1] = v0.x * v1.z - v0.z * v1.x;
            determinants[2] = v0.x * v1.w - v0.w * v1.x;
            determinants[3] = v0.y * v1.z - v0.z * v1.y;
            determinants[4] = v0.y * v1.w - v0.w * v1.y;
            determinants[5] = v0.z * v1.w - v0.w * v1.z;
            return determinants;
        }

        /// <summary>
        /// Assume the following structure, return the determinant coeficients for v0, v1, v2, v3
        /// v0 v1 v2 v3
        /// x00 x01 x02 x03
        /// x10 x11 x12 x13
        /// x20 x21 x22 x23
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static float4 determinantCoef(float4x3 matrix)
        {
            float4 bottomRow = matrix.c2;
            float[] determinants = Determinant2X2(matrix.c0, matrix.c1);
            return new float4(
                bottomRow.y*determinants[5]-bottomRow.z*determinants[4]+bottomRow.w*determinants[3],
                -(bottomRow.x*determinants[5]-bottomRow.z*determinants[2]+bottomRow.w*determinants[3]),
                bottomRow.x*determinants[4]-bottomRow.y*determinants[2]+bottomRow.w*determinants[0],
                -(bottomRow.x*determinants[3]-bottomRow.y*determinants[1]+bottomRow.z*determinants[0])
             );
        }

        public static float3 projectDownDimension(this Vector4 v, Vector4 axis) =>
            projectDownDimension((float4) v, (float4) axis);

        /// <summary>
        /// Rotate a vertex v around an axis for an angle theta, in degrees.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="axis"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static Vector4 GetRotatedVertex(this Vector4 v, Axis4D axis, float theta)
        {
            if(theta %360 == 0)
            {
                return v;
            }
            switch (axis)
            {
                case Axis4D.xy:
                    return v.rotate(right, up, theta);
                case Axis4D.xz:
                    return v.rotate(right, forward, theta);
                case Axis4D.xw:
                    return v.rotate(right, wPos, theta);
                case Axis4D.yz:
                    return v.rotate(up, forward, theta);
                case Axis4D.yw:
                    return v.rotate(up, wPos, theta);
                case Axis4D.zw:
                    return v.rotate(forward, wPos, theta);
                default:
                    return Vector4.zero;
            }
        }

        public static readonly Vector4 right = new Vector4(1, 0, 0, 0);
        public static readonly Vector4 up = new Vector4(0, 1, 0, 0);
        public static readonly Vector4 forward = new Vector4(0, 0, 1, 0);
        public static readonly Vector4 wPos = new Vector4(0, 0, 0, 1);
            //switch (axis)
            //{
            //    case Axis4D.xy:
            //        return RotateAroundXY(v, s, c);
            //    case Axis4D.xz:
            //        return RotateAroundXZ(v, s, c);
            //    case Axis4D.xw:
            //        return RotateAroundXW(v, s, c);
            //    case Axis4D.yz:
            //        return RotateAroundYZ(v, s, c);
            //    case Axis4D.yw:
            //        return RotateAroundYW(v, s, c);
            //    case Axis4D.zw:
            //        return RotateAroundZW(v, s, c);
            //    default:
            //        return Vector4.zero;
            //}
        

        //public static Vector4 RotateAroundXY(this Vector4 v, float s, float c)
        //{
        //    float tmpX = c * v.x + s * v.y;
        //    float tmpY = -s * v.x + c * v.y;
        //    return new Vector4(tmpX, tmpY, v.z, v.w);
        //}

        //public static Vector4 RotateAroundXZ(this Vector4 v, float s, float c)
        //{
        //    float tmpX = c * v.x + s * v.z;
        //    float tmpZ = -s * v.x + c * v.z;
        //    return new Vector4(tmpX, v.y, tmpZ, v.w);
        //}

        //public static Vector4 RotateAroundXW(this Vector4 v, float s, float c)
        //{
        //    float tmpX = c * v.x + s * v.w;
        //    float tmpW = -s * v.x + c * v.w;
        //    return new Vector4(tmpX, v.y, v.z, tmpW);
        //}

        //public static Vector4 RotateAroundYZ(this Vector4 v, float s, float c)
        //{
        //    float tmpY = c * v.y + s * v.z;
        //    float tmpZ = -s * v.y + c * v.z;
        //    return new Vector4(v.x, tmpY, tmpZ, v.w);
        //}

        //public static Vector4 RotateAroundYW(this Vector4 v, float s, float c)
        //{
        //    float tmpY = c * v.y - s * v.w;
        //    float tmpW = s * v.y + c * v.w;
        //    return new Vector4(v.x, tmpY, v.z, tmpW);
        //}

        //public static Vector4 RotateAroundZW(this Vector4 v, float s, float c)
        //{
        //    float tmpZ = c * v.z - s * v.w;
        //    float tmpW = s * v.z + c * v.w;
        //    return new Vector4(v.x, v.y, tmpZ, tmpW);
        //}
    }
}
