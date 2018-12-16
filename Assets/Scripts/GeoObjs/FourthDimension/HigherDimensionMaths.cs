using System.Collections;
using System.Collections.Generic;
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
                Debug.LogWarning("Origional Vector does not lie in the same plane as the first basis vector.");
            }
            return Vector4.Dot(v, basis0) * (Mathf.Cos(theta) * basis0 + basis1 * Mathf.Sin(theta)) + Vector4.Dot(v, basis1) * (Mathf.Cos(theta) * basis1 + Mathf.Sin(theta) * basis0) + remainder;
        }

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
