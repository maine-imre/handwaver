using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver.HigherDimensions
{
    /// <summary>
    /// Net of hypercube for scale and dimension study
    /// Projected to 3D
    /// </summary>
    public class HypercubeNet : AbstractHigherDimSolid, IMRE.HandWaver.ScaleStudy.ISliderInput
    {
        //basic vector4 values for computations
        private static readonly Unity.Mathematics.float4 up = new UnityEngine.Vector4(0, 1, 0, 0);
        private static readonly Unity.Mathematics.float4 down = new UnityEngine.Vector4(0, -1, 0, 0);
        private static readonly Unity.Mathematics.float4 right = new UnityEngine.Vector4(1, 0, 0, 0);
        private static readonly Unity.Mathematics.float4 left = new UnityEngine.Vector4(-1, 0, 0, 0);
        private static readonly Unity.Mathematics.float4 forward = new UnityEngine.Vector4(0, 0, 1, 0);
        private static readonly Unity.Mathematics.float4 back = new UnityEngine.Vector4(0, 0, -1, 0);
        private static readonly Unity.Mathematics.float4 wForward = new UnityEngine.Vector4(0, 0, 0, 1);
        private static Unity.Mathematics.float4 wBack = new UnityEngine.Vector4(0, 0, 0, -1);

        private int[] _faces;

        private float _foldPercent;

        private int[] _triangles;
        
        public static Unity.Mathematics.float4 offset = new Unity.Mathematics.float4(2f,2f,2f,2f);

        private UnityEngine.Vector2[] _uvs;
        public bool sliderOverride;

        public float FoldPercent
        {
            get => _foldPercent;
            set
            {
                _foldPercent = value;
                originalVertices = vertices(90f * value);
            }
        }

        public override UnityEngine.Vector2[] uvs
        {
            get
            {
                if (_uvs == null)
                {
                    int numFaces = faces.Length / 4;
                    _uvs = new UnityEngine.Vector2[6 * numFaces];

                    UnityEngine.Vector2 uv0 = new UnityEngine.Vector2(0, 0);
                    UnityEngine.Vector2 uv1 = new UnityEngine.Vector2(1, 0);
                    UnityEngine.Vector2 uv2 = new UnityEngine.Vector2(0.5f, 1);

                    for (int i = 0; i < numFaces; i++)
                    {
                        _uvs[6 * i] = uv0;
                        _uvs[(6 * i) + 1] = uv1;
                        _uvs[(6 * i) + 2] = uv2;
                        _uvs[(6 * i) + 3] = uv0;
                        _uvs[(6 * i) + 4] = uv1;
                        _uvs[(6 * i) + 5] = uv2;
                    }
                }

                return _uvs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int[] triangles
        {
            get
            {
                int numFaces = faces.Length / 4;
                _triangles = new int[6 * numFaces];

                for (int i = 0; i < numFaces; i++)
                {
                    _triangles[6 * i] = faces[4 * i];
                    _triangles[(6 * i) + 1] = faces[(4 * i) + 1];
                    _triangles[(6 * i) + 2] = faces[(4 * i) + 2];
                    _triangles[(6 * i) + 3] = faces[4 * i];
                    _triangles[(6 * i) + 4] = faces[(4 * i) + 2];
                    _triangles[(6 * i) + 5] = faces[(4 * i) + 3];
                }

                return _triangles;
            }
        }

        public override Color[] colors
        {
            get
            {
                Color[] result = new Color[4 * 9];

                //core cube (does not fold)
                result[0] = Color.cyan;
                result[1] = Color.cyan;
                result[2] = Color.cyan;
                result[3] = Color.cyan;

                result[4] = Color.cyan;
                result[5] = Color.cyan;
                result[6] = Color.cyan;
                result[7] = Color.cyan;

                //above up face.            
                result[8] = Color.magenta;
                result[9] = Color.magenta;
                result[10] = Color.magenta;
                result[11] = Color.magenta;

                //below down face
                result[12] = Color.red;
                result[13] = Color.red;
                result[14] = Color.red;
                result[15] = Color.red;

                //right of right face;
                result[16] = Color.yellow;
                result[17] = Color.yellow;
                result[18] = Color.yellow;
                result[19] = Color.yellow;

                //left of left face
                result[20] = Color.green;
                result[21] = Color.green;
                result[22] = Color.green;
                result[23] = Color.green;

                //forward of forward face.
                result[24] = Color.white;
                result[25] = Color.white;
                result[26] = Color.white;
                result[27] = Color.white;

                //back of back face.
                result[28] = Color.blue;
                result[29] = Color.blue;
                result[30] = Color.blue;
                result[31] = Color.blue;

                //down of double down.
                result[32] = Color.grey;
                result[33] = Color.grey;
                result[34] = Color.grey;
                result[35] = Color.grey;

                return result;
            }
        }

        private int[] faces
        {
            get
            {
                //8 cubes for hypercube
                if (_faces == null)
                {
                    int[] result = new int[24 + (20 * 8)];
                    //main cube
                    cubeFaces(0, 1, 2, 3, 4, 5, 6, 7).CopyTo(result, 0); //core
                    //other cubes
                    cubeFacesNoTop(0, 1, 2, 3, 8, 9, 10, 11).CopyTo(result, 24 + (20 * 1)); //up
                    cubeFacesNoTop(0, 1, 2, 3, 12, 13, 14, 15).CopyTo(result, 24 + (20 * 2)); //down
                    cubeFacesNoTop(32, 33, 34, 35, 12, 13, 14, 15).CopyTo(result, 24 + (20 * 3)); //down of down
                    cubeFacesNoTop(0, 3, 7, 4, 16, 17, 18, 19).CopyTo(result, 24 + (20 * 4)); //right
                    cubeFacesNoTop(1, 2, 6, 5, 20, 21, 22, 23).CopyTo(result, 24 + (20 * 5)); //left
                    cubeFacesNoTop(0, 1, 5, 4, 24, 25, 26, 27).CopyTo(result, 24 + (20 * 6)); //forward
                    cubeFacesNoTop(2, 3, 7, 6, 28, 29, 30, 31).CopyTo(result, 24 + (20 * 7)); //back
                    _faces = result;
                }

                return _faces;
            }
        }

        public float slider
        {
            set => FoldPercent = !sliderOverride ? value : 1f;
        }

        private void Awake()
        {
            FoldPercent = 0f;
        }

        /// <summary>
        /// configure vertices of each cube based around core cube
        /// </summary>
        /// <param name="degreeFolded"></param>
        /// <returns></returns>
        private static Unity.Mathematics.float4[] vertices(float degreeFolded)
        {
            Unity.Mathematics.float4[] result = new Unity.Mathematics.float4[4 * 9];

            //core cube (does not fold)
            result[0] = (up + right + forward) / 2f;
            result[1] = (up + left + forward) / 2f;
            result[2] = (up + left + back) / 2f;
            result[3] = (up + right + back) / 2f;

            result[4] = (down + right + forward) / 2f;
            result[5] = (down + left + forward) / 2f;
            result[6] = (down + left + back) / 2f;
            result[7] = (down + right + back) / 2f;

            //above up face.            
            result[8] = result[0] + IMRE.Math.Operations.rotate(up, wForward, degreeFolded);
            result[9] = result[1] + IMRE.Math.Operations.rotate(up, wForward, degreeFolded);
            result[10] = result[2] + IMRE.Math.Operations.rotate(up, wForward, degreeFolded);
            result[11] = result[3] + IMRE.Math.Operations.rotate(up, wForward, degreeFolded);

            //below down face
            result[12] = result[4] + IMRE.Math.Operations.rotate(down, wForward, degreeFolded);
            result[13] = result[5] + IMRE.Math.Operations.rotate(down, wForward, degreeFolded);
            result[14] = result[6] + IMRE.Math.Operations.rotate(down, wForward, degreeFolded);
            result[15] = result[7] + IMRE.Math.Operations.rotate(down, wForward, degreeFolded);

            //right of right face;
            result[16] = result[0] + IMRE.Math.Operations.rotate(right, wForward, degreeFolded);
            result[17] = result[3] + IMRE.Math.Operations.rotate(right, wForward, degreeFolded);
            result[18] = result[7] + IMRE.Math.Operations.rotate(right, wForward, degreeFolded);
            result[19] = result[4] + IMRE.Math.Operations.rotate(right, wForward, degreeFolded);

            //left of left face
            result[20] = result[1] + IMRE.Math.Operations.rotate(left, wForward, degreeFolded);
            result[21] = result[2] + IMRE.Math.Operations.rotate(left, wForward, degreeFolded);
            result[22] = result[6] + IMRE.Math.Operations.rotate(left, wForward, degreeFolded);
            result[23] = result[5] + IMRE.Math.Operations.rotate(left, wForward, degreeFolded);

            //forward of forward face.
            result[24] = result[0] + IMRE.Math.Operations.rotate(forward, wForward, degreeFolded);
            result[25] = result[1] + IMRE.Math.Operations.rotate(forward, wForward, degreeFolded);
            result[26] = result[5] + IMRE.Math.Operations.rotate(forward, wForward, degreeFolded);
            result[27] = result[4] + IMRE.Math.Operations.rotate(forward, wForward, degreeFolded);

            //back of back face.
            result[28] = result[2] + IMRE.Math.Operations.rotate(back, wForward, degreeFolded);
            result[29] = result[3] + IMRE.Math.Operations.rotate(back, wForward, degreeFolded);
            result[30] = result[7] + IMRE.Math.Operations.rotate(back, wForward, degreeFolded);
            result[31] = result[6] + IMRE.Math.Operations.rotate(back, wForward, degreeFolded);

            //down of double down.
            result[32] = result[12] + IMRE.Math.Operations.rotate(down, wForward, 2f * degreeFolded);
            result[33] = result[13] + IMRE.Math.Operations.rotate(down, wForward, 2f * degreeFolded);
            result[34] = result[14] + IMRE.Math.Operations.rotate(down, wForward, 2f * degreeFolded);
            result[35] = result[15] + IMRE.Math.Operations.rotate(down, wForward, 2f * degreeFolded);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] += offset;
            }
            
            return result;
        }

        private int[] cubeFaces(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7)
        {
            return new[]
            {
                //core cube
                a0, a1, a2, a3, //top
                a4, a5, a6, a7, //bottom
                a0, a1, a5, a4, //forward
                a2, a3, a7, a6, //back
                a0, a3, a4, a7, //right
                a1, a2, a6, a5 //left
            };
        }

        private int[] cubeFacesNoTop(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7)
        {
            return new[]
            {
                //core cube
                //a0,a1,a2,a3,  //neglect top
                a4, a5, a6, a7, //bottom
                a0, a1, a5, a4, //forward
                a2, a3, a7, a6, //back
                a0, a3, a4, a7, //right
                a1, a2, a6, a5 //left
            };
        }

    }
}