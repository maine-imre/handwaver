using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.HandWaver.HigherDimensions
{
	public enum ProjectionMethod {orthographic, projective, parallel, stereographic}
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
	/// <param name="basis"> viewing basis </param>
        /// <returns></returns>
        public static float3 projectDownDimension(this float4 v, float4x3 basis, ProjectionMatheod method, float Vangle = 0f, float4 eyePosition = float4.zero, float viewingRadius = 0f)
        {

	switch(method){
		case ProjectionMethod.orthographic:
			math.normalize(basis.c0);
	            math.normalize(basis.c1);
	            math.normalize(basis.c2);
            
            		return new float3(math.dot(v,basis.c0), math.dot(v,basis.c1), math.dot(v,basis.c2));
		case ProjectionMethod.projective:
			//using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
			float T = 1f/(math.tan(Vangle/2f);
			float4 tmp = v - eyePosition;
			float4x4 basis = calc4Matrix(eyePosition, basis);
			S = T/math.dot(v, basis.c3)
			
			return new float3(S*math.dot(tmp, basis.c0), S*math.dot(tmp, basis.c1), S*math.dot(tmp, basis.c2));

		case ProjectionMethod.parallel:
			//using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
			float S = 1/viewingRadius;
			float4 tmp = v - eyePosition;
			float4x4 basis = calc4Matrix(eyePosition, basis);
			
			return new float3(S*math.dot(tmp, basis.c0), S*math.dot(tmp, basis.c1), S*math.dot(tmp, basis.c2));

		default: return new float3(0f,0f,0f);
        }

	public static float4x4 calc4Matrix(float4 from, float4x3 basis){
		//using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3

		float4 Up = basis.c1;
		float4 Over = basis.c2;
		//Get the normalized Wd column vector.
		float4 Wd = basis.c0;
		norm = math.magnitude(Wd);
		if(norm ==0f)
			Debug.LogError("To point and from point are the same");
		math.normalize(Wd);

		//calculated the normalized Wa column vector.
		float4 Wa = math.cross(Up, Over, Wd);
		norm = math.magnitude(Wa);
		if (norm == 0f)
			Debug.LogError("Invalid Up Vector");
		math.normalize(Wa);
		
		//Calculate the normalized Wb column vector
		float4 Wb = math.cross(Over, Wd, Wa);
		norm = math.magnitude(Wb);
		if (norm == 0f)
			Debug.LogError("Invalid Over Vector");
		math.normalize(Wb);
		
		float4 Wc = math.cross(Wd, Wa, Wb);
		math.normalize(Wc); //theoretically redundant.

		return new float4x3(Wa, Wb, Wc, Wd);		
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
    }
}
