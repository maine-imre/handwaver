namespace IMRE.HandWaver.HigherDimensions
{
    public enum ProjectionMethod
    {
        orthographic,
        projective,
        parallel,
        stereographic
    }
    
   	public struct ProjectionData{
		ProjectionMethod method;
		Unity.Mathematics.float4x3 inputBasis;
        float Vangle;
        Unity.Mathematics.float4 eyePosition;
        float viewingRadius;	
    }

    /// <summary>
    ///     Maths for 4D to 3D projection.
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
        public static UnityEngine.Vector4 rotate(this UnityEngine.Vector4 v, UnityEngine.Vector4 basis0,
            UnityEngine.Vector4 basis1, float theta)
        {
            basis0 = basis0.normalized;
            basis1 = basis1.normalized;
            UnityEngine.Vector4 remainder =
                v - (UnityEngine.Vector4.Project(v, basis0) + UnityEngine.Vector4.Project(v, basis1));
            theta *= UnityEngine.Mathf.Deg2Rad;

            if (UnityEngine.Vector3.Dot(basis0, basis1) != 0f)
                UnityEngine.Debug.LogWarning("Basis is not orthagonal");
            else if ((UnityEngine.Vector4.Dot(v.normalized, basis0) != 1f) || (UnityEngine.Vector4.Dot(v, basis1) != 0f)
            ) UnityEngine.Debug.LogWarning("Original Vector does not lie in the same plane as the first basis vector.");

            return (UnityEngine.Vector4.Dot(v, basis0) *
                    ((UnityEngine.Mathf.Cos(theta) * basis0) + (basis1 * UnityEngine.Mathf.Sin(theta)))) +
                   (UnityEngine.Vector4.Dot(v, basis1) *
                    ((UnityEngine.Mathf.Cos(theta) * basis1) + (UnityEngine.Mathf.Sin(theta) * basis0))) + remainder;
        }

        /// <summary>
        /// Projects a vector onto another vector using a dot product.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Unity.Mathematics.float4 project(this Unity.Mathematics.float4 v, Unity.Mathematics.float4 axis)
        {
            Unity.Mathematics.math.normalize(axis);
            return Unity.Mathematics.math.dot(v, axis) * axis;
        }

        /// <summary>
        /// Writes the projection of a float4 onto a new basis for the hyperplane.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="basis"> viewing basis </param>
        /// <returns></returns>
        public static Unity.Mathematics.float3 projectDownDimension(this Unity.Mathematics.float4 v,
            Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
            float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
        {
            float T, S;
            Unity.Mathematics.float4x4 basis;
            Unity.Mathematics.float4 tmp;
            //set defaults
            Vangle = Vangle ?? 0f;
            eyePosition = eyePosition ?? Unity.Mathematics.float4.zero;
            viewingRadius = viewingRadius ?? 1f;

            switch (method)
            {
                case ProjectionMethod.orthographic:
                    Unity.Mathematics.math.normalize(inputBasis.c0);
                    Unity.Mathematics.math.normalize(inputBasis.c1);
                    Unity.Mathematics.math.normalize(inputBasis.c2);

                    return new Unity.Mathematics.float3(Unity.Mathematics.math.dot(v, inputBasis.c0),
                        Unity.Mathematics.math.dot(v, inputBasis.c1),
                        Unity.Mathematics.math.dot(v, inputBasis.c2));
                case ProjectionMethod.projective:
                    //using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
                    T = 1f / Unity.Mathematics.math.tan(Vangle.Value / 2f);
                    tmp = v - eyePosition.Value;
                    basis = calc4Matrix(eyePosition.Value, inputBasis);
                    S = T / Unity.Mathematics.math.dot(v, basis.c3);

                    return new Unity.Mathematics.float3(S * Unity.Mathematics.math.dot(tmp, basis.c0),
                        S * Unity.Mathematics.math.dot(tmp, basis.c1),
                        S * Unity.Mathematics.math.dot(tmp, basis.c2));

                case ProjectionMethod.parallel:
                    //using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3
                    S = 1f / viewingRadius.Value;
                    tmp = v - eyePosition.Value;
                    basis = calc4Matrix(eyePosition.Value, inputBasis);

                    return new Unity.Mathematics.float3(S * Unity.Mathematics.math.dot(tmp, basis.c0),
                        S * Unity.Mathematics.math.dot(tmp, basis.c1),
                        S * Unity.Mathematics.math.dot(tmp, basis.c2));
                case ProjectionMethod.stereographic:
                    float r = Math.Operations.magnitude(v);
                    //assume north pole is at (0,0,0,1);
                    Unity.Mathematics.float4 north = new Unity.Mathematics.float4(0,0,0,1)*r;
                    Unity.Mathematics.float4 vPrime =
                        (north - v) * (Math.Operations.magnitude(north) /
                                       Unity.Mathematics.math.dot((north - v),
                                           Unity.Mathematics.math.normalize(north))) + north;
                    return new Unity.Mathematics.float3 (vPrime.x, vPrime.y, vPrime.z);                                                             
                default: return new Unity.Mathematics.float3(0f, 0f, 0f);
            }
        }
                                                                                 
        public static Unity.Mathematics.float3[] projectSegment(Unity.Mathematics.float4 a, Unity.Mathematics.float4 b,
            int n, Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
            float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
        {
            Unity.Mathematics.float3[] result = new Unity.Mathematics.float3[n];
            for(int i = 0; i < n; i ++)
            {
                Unity.Mathematics.float4 v = ((float)i/((float)n-1f))*(b-a)+a;
                if(method == ProjectionMethod.stereographic)
                {
                    //assume center == Vector4.zero;
                    //assume a and b are on surface of sp
                    v = Unity.Mathematics.math.normalize(v)*Math.Operations.magnitude(a);
                }    
                result[i] = projectDownDimension(v,inputBasis,method,Vangle, eyePosition, viewingRadius);
            }    
            return result;
        }
        public static Unity.Mathematics.float3[] projectQuad  (Unity.Mathematics.float4 a, Unity.Mathematics.float4 b, 
                                                               Unity.Mathematics.float4 c, Unity.Mathematics.float4 d,
                                                               int n, Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
                                                                float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
         {
             Unity.Mathematics.float3[] result = new Unity.Mathematics.float3[n];

             for(int i = 0; i < n; i++){
                 Unity.Mathematics.float4 a1 = ((float)i/((float)n-1f))*(b-a)+a;
                 Unity.Mathematics.float4 b1 = ((float)i/((float)n-1f))*(c-d)+d;
                if(method == ProjectionMethod.stereographic)
                {
                    //assume center == Vector4.zero;
                    //assume a and b are on surface of sp
                    a1 = Unity.Mathematics.math.normalize(a1)*Math.Operations.magnitude(a);
                    b1 = Unity.Mathematics.math.normalize(b1)*Math.Operations.magnitude(b);
                }   
                 Unity.Mathematics.float3[] seg = projectSegment(a1, b1, n, inputBasis, method,
                                                        Vangle, eyePosition, viewingRadius);
                 System.Array.Copy(seg, 0, result, i*n,n);
             }
             return result;
         }
                                                                                 
         public static Unity.Mathematics.float3[] projectTriangle  (Unity.Mathematics.float4 a, Unity.Mathematics.float4 b, 
                                                               Unity.Mathematics.float4 c,
                                                               int n, Unity.Mathematics.float4x3 inputBasis, ProjectionMethod method,
                                                                float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
         {
             return  projectQuad  (a, b,c,c, n, inputBasis, method, Vangle, eyePosition, viewingRadius);
         }
                                                                

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="basis"></param>
        /// <returns></returns>
        public static Unity.Mathematics.float4x4 calc4Matrix(Unity.Mathematics.float4 from,
            Unity.Mathematics.float4x3 basis)
        {
            //using http://hollasch.github.io/ray4/Four-Space_Visualization_of_4D_Objects.html#chapter3

            Unity.Mathematics.float4 Up = basis.c1;
            Unity.Mathematics.float4 Over = basis.c2;
            //Get the normalized Wd column vector.
            Unity.Mathematics.float4 Wd = basis.c0;
            float norm = IMRE.Math.Operations.magnitude(Wd);
            if (norm == 0f)
                UnityEngine.Debug.LogError("To point and from point are the same");
            Unity.Mathematics.math.normalize(Wd);

            //calculated the normalized Wa column vector.
            Unity.Mathematics.float4 Wa = IMRE.Math.Operations.cross(Up, Over, Wd);
            norm = IMRE.Math.Operations.magnitude(Wa);
            if (norm == 0f)
                UnityEngine.Debug.LogError("Invalid Up Vector");
            Unity.Mathematics.math.normalize(Wa);

            //Calculate the normalized Wb column vector
            Unity.Mathematics.float4 Wb = IMRE.Math.Operations.cross(Over, Wd, Wa);
            norm = IMRE.Math.Operations.magnitude(Wb);
            if (norm == 0f)
                UnityEngine.Debug.LogError("Invalid Over Vector");
            Unity.Mathematics.math.normalize(Wb);

            Unity.Mathematics.float4 Wc = IMRE.Math.Operations.cross(Wd, Wa, Wb);
            Unity.Mathematics.math.normalize(Wc); //theoretically redundant.

            return new Unity.Mathematics.float4x4(Wa, Wb, Wc, Wd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="axis"></param>
        /// <param name="method"></param>
        /// <param name="Vangle"></param>
        /// <param name="eyePosition"></param>
        /// <param name="viewingRadius"></param>
        /// <returns></returns>
        public static Unity.Mathematics.float3[] projectDownDimension(this Unity.Mathematics.float4[] vectors,
            Unity.Mathematics.float4 axis, ProjectionMethod method,
            float? Vangle, Unity.Mathematics.float4? eyePosition, float? viewingRadius)
        {
            Unity.Mathematics.float4x3 basis = axis.basisSystem();
            Unity.Mathematics.float3[] result = new Unity.Mathematics.float3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                result[i] = vectors[i].projectDownDimension(basis, method, Vangle, eyePosition, viewingRadius);

            return result;
        }

        /// <summary>
        ///  Return the basis of a hyper plane orthagonal to a given vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Unity.Mathematics.float4x3 basisSystem(this Unity.Mathematics.float4 v)
        {
            Unity.Mathematics.math.normalize(v);
            //use method described here:  https://www.geometrictools.com/Documentation/OrthonormalSets.pdf
            if ((v.x == 0) && (v.y == 0) && (v.z == 0) && (v.w == 0))
                UnityEngine.Debug.LogError("Can't form basis from zero vector");

            //the vector is the first basis vector for the 4-space, orthag to the hyperplane
            Unity.Mathematics.math.normalize(v);
            //establish a second basis vector
            Unity.Mathematics.float4 basis0;
            if ((v.x != 0) || (v.y != 0))
                basis0 = new UnityEngine.Vector4(v.y, v.x, 0, 0);
            else
                basis0 = new UnityEngine.Vector4(0, 0, v.w, v.z);

            Unity.Mathematics.math.normalize(basis0);

            float[] determinants = Determinant2X2(v, basis0);

            //index of largest determinant
            int idx = 0;
            for (int i = 0; i < 6; i++)
                if (determinants[i] > determinants[idx])
                    idx = i;

            if (determinants[idx] == 0) UnityEngine.Debug.LogError("No non-zero determinant");

            //choose bottom row of det matrix to generate next basis vector
            Unity.Mathematics.float4 bottomRow;
            if ((idx == 0) || (idx == 1) || (idx == 3))
                bottomRow = new Unity.Mathematics.float4(0, 0, 0, 1);
            else if ((idx == 2) || (idx == 4))
                bottomRow = new Unity.Mathematics.float4(0, 0, 1, 0);
            else
                bottomRow = new Unity.Mathematics.float4(0, 1, 0, 0);

            Unity.Mathematics.float4 basis1 = determinantCoef(new Unity.Mathematics.float4x3(v, basis0, bottomRow));
            Unity.Mathematics.math.normalize(basis1);

            Unity.Mathematics.float4 basis2 = determinantCoef(new Unity.Mathematics.float4x3(v, basis0, basis1));
            Unity.Mathematics.math.normalize(basis2);

            //returns the basis that spans the hyperplane orthogonal to v
            Unity.Mathematics.float4x3 basis = new Unity.Mathematics.float4x3(basis0, basis1, basis2);
            //check that v is orthogonal.
            v.projectDownDimension(basis, ProjectionMethod.parallel, null, null, null);
            if ((v.x != 0) || (v.y != 0) || (v.z != 0)) UnityEngine.Debug.LogError("Basis is not orthogonal to v");

            return basis;
        }

        private static float[] Determinant2X2(Unity.Mathematics.float4 v0, Unity.Mathematics.float4 v1)
        {
            //find largest determinant of 2x2
            float[] determinants = new float[6];
            determinants[0] = (v0.x * v1.y) - (v0.y * v1.x);
            determinants[1] = (v0.x * v1.z) - (v0.z * v1.x);
            determinants[2] = (v0.x * v1.w) - (v0.w * v1.x);
            determinants[3] = (v0.y * v1.z) - (v0.z * v1.y);
            determinants[4] = (v0.y * v1.w) - (v0.w * v1.y);
            determinants[5] = (v0.z * v1.w) - (v0.w * v1.z);
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
        private static Unity.Mathematics.float4 determinantCoef(Unity.Mathematics.float4x3 matrix)
        {
            Unity.Mathematics.float4 bottomRow = matrix.c2;
            float[] determinants = Determinant2X2(matrix.c0, matrix.c1);
            return new Unity.Mathematics.float4(
                ((bottomRow.y * determinants[5]) - (bottomRow.z * determinants[4])) + (bottomRow.w * determinants[3]),
                -(((bottomRow.x * determinants[5]) - (bottomRow.z * determinants[2])) +
                  (bottomRow.w * determinants[3])),
                ((bottomRow.x * determinants[4]) - (bottomRow.y * determinants[2])) + (bottomRow.w * determinants[0]),
                -(((bottomRow.x * determinants[3]) - (bottomRow.y * determinants[1])) + (bottomRow.z * determinants[0]))
            );
        }
    }
}
