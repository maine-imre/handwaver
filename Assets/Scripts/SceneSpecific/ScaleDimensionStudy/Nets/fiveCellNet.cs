namespace IMRE.HandWaver.HigherDimensions
{
    /// <summary>
    /// Net of five cell for scale and dimension study.
    /// </summary>
    public class fiveCellNet : AbstractHigherDimSolid, IMRE.HandWaver.ScaleStudy.ISliderInput
    {
        //initialize fold
        //read only static float GoldenRatio = (1f + Mathf.Sqrt(5f)) / 2f;
        private float _percentFolded;

        private UnityEngine.Vector2[] _uvs;
        public bool sliderOverride;

        public float PercentFolded
        {
            get => _percentFolded;
            set
            {
                Unity.Mathematics.float4 a = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                                                 1f / Unity.Mathematics.math.sqrt(6f),
                                                 1f / Unity.Mathematics.math.sqrt(3f), 1f) / 2f;
                Unity.Mathematics.float4 b = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                                                 1f / Unity.Mathematics.math.sqrt(6f),
                                                 1f / Unity.Mathematics.math.sqrt(3f), -1f) / 2f;
                Unity.Mathematics.float4 c = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                                                 1f / Unity.Mathematics.math.sqrt(6f),
                                                 -2f / Unity.Mathematics.math.sqrt(3f), 0f) / 2f;
                Unity.Mathematics.float4 d = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                    -Unity.Mathematics.math.sqrt(3f / 2f), 0f, 0f);
                Unity.Mathematics.float4 center1 = (a + b + c) / 3f;
                Unity.Mathematics.float4 dir1 = center1 - d;
                Unity.Mathematics.float4 apex =
                    new Unity.Mathematics.float4(-2 * Unity.Mathematics.math.sqrt(2f / 5f), 0f, 0f, 0f);
                Unity.Mathematics.float4 dihedralAngle = IMRE.Math.Operations.Angle(dir1, apex - center1);
                if(apex != e)
                UnityEngine.Debug.Log(apex + " : " + e);
                
                //check that the apex matches the rotated state with the calculated angle
                Unity.Mathematics.float4 center1 = (result[0] + result[1] + result[2]) / 3f;
                Unity.Mathematics.float4 dir1 = center1 - result[3];
                float4 e = center1 + IMRE.Math.Operations.rotate(dir1, apex - center1, degreeFolded);
                
                _percentFolded = value;
                //TODO find this value.			
                originalVertices = vertices(value * dihedralAngle);
            }
        }

        public override UnityEngine.Vector2[] uvs
        {
            get
            {
                _uvs = new UnityEngine.Vector2[originalVertices.Length];
                for (int i = 0; i < originalVertices.Length; i++)
                    //temp uv map.
                    _uvs[i] = UnityEngine.Vector2.right * ((float) i / originalVertices.Length);

                return _uvs;
            }
        }

        /// <summary>
        /// vertex values for triangles of each tetrahedron
        /// </summary>
        public override int[] triangles =>
            new[]
            {
                //core tetrahedron
                0, 1, 2,
                0, 1, 3,
                2, 0, 3,
                1, 2, 3,

                //tetrahedron 1
                //0, 1, 2
                0, 1, 4,
                2, 0, 4,
                1, 2, 4,

                //tetrahedron 2
                //2, 0, 3
                0, 3, 5,
                2, 0, 5,
                3, 2, 5,

                //tetrahedron 3
                //0, 1 ,3
                0, 1, 6,
                3, 0, 6,
                1, 3, 6,

                //tetrahedron 4
                //1, 2, 3
                1, 2, 7,
                2, 3, 7,
                3, 1, 7
            };

        public float slider
        {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

        private void Awake()
        {
            PercentFolded = 0f;
        }

        /// <summary>
        /// configure vertices of fivecell around core tetrahedron
        /// </summary>
        /// <param name="degreeFolded"></param>
        /// <returns></returns>
        private static Unity.Mathematics.float4[] vertices(float degreeFolded)
        {
            //8 points on unfolded fivecell
            Unity.Mathematics.float4[] result = new Unity.Mathematics.float4[8];

            //core tetrahedron (does not fold)
            //coordiantes from wikipedia  https://en.wikipedia.org/wiki/5-cell, centered at origin, 
            result[0] = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                            1f / Unity.Mathematics.math.sqrt(6f), 1f / Unity.Mathematics.math.sqrt(3f), 1f) / 2f;
            result[1] = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                            1f / Unity.Mathematics.math.sqrt(6f), 1f / Unity.Mathematics.math.sqrt(3f), -1f) / 2f;
            result[2] = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                            1f / Unity.Mathematics.math.sqrt(6f), -2f / Unity.Mathematics.math.sqrt(3f), 0f) / 2f;
            result[3] = new Unity.Mathematics.float4(1f / Unity.Mathematics.math.sqrt(10f),
                -Unity.Mathematics.math.sqrt(3f / 2f), 0f, 0f);

            //find position of convergent point for other tetrahedrons in the net.
            Unity.Mathematics.float4 apex =
                new Unity.Mathematics.float4(-2 * Unity.Mathematics.math.sqrt(2f / 5f), 0f, 0f, 0f);
            //TODO consider making the initial projection onto n

            //apex of tetrahedron for each additional tetrahedron(from fases of first) foldling by degree t
            Unity.Mathematics.float4 center1 = (result[0] + result[1] + result[2]) / 3f;
            Unity.Mathematics.float4 dir1 = center1 - result[3];
            result[4] = center1 + IMRE.Math.Operations.rotate(dir1, apex - center1, degreeFolded);

            Unity.Mathematics.float4 center2 = (result[0] + result[2] + result[3]) / 3f;
            Unity.Mathematics.float4 dir2 = center2 - result[1];
            result[5] = center2 + IMRE.Math.Operations.rotate(dir2, apex - center2, degreeFolded);

            Unity.Mathematics.float4 center3 = (result[0] + result[1] + result[3]) / 3f;
            Unity.Mathematics.float4 dir3 = center3 - result[2];
            result[6] = center3 + IMRE.Math.Operations.rotate(dir3, apex - center3, degreeFolded);

            Unity.Mathematics.float4 center4 = (result[1] + result[2] + result[3]) / 3f;
            Unity.Mathematics.float4 dir4 = center4 - result[0];
            result[7] = center4 + IMRE.Math.Operations.rotate(dir4, apex - center4, degreeFolded);

            return result;
        }
    }
}
