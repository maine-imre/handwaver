using UnityEngine;

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
        public static Unity.Mathematics.float4 offset = new Unity.Mathematics.float4(2f,2f,2f,2f);

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
                    -Unity.Mathematics.math.sqrt(3f / 2f), 0f, 0f)/2f;
                Unity.Mathematics.float4 center1 = (a + b + c) / 3f;
                Unity.Mathematics.float4 dir1 = center1 - d;
                Unity.Mathematics.float4 apex =
                    new Unity.Mathematics.float4(-2 * Unity.Mathematics.math.sqrt(2f / 5f), 0f, 0f, 0f);
                
                //the calculated result for the dihedral angle is about 91 degrees
                //float dihedralAngle = IMRE.Math.Operations.Angle(dir1, apex - center1);
                //this is a rough guess for the value by an empirical approach.
                float dihedralAngle = 104.4856f;

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

        public override Color[] colors
        {
            get
            {
                Color[] result = new Color[8];
                
                //core tetrahedron
                result[0] = Color.white;
                result[1] = Color.white;
                result[2] = Color.white;
                result[3] = Color.white;

                //apex of tetrahedron for each additional tetrahedron(from faces of first)
                result[4] = Color.red;

                result[5] = Color.green;

                result[6] = Color.yellow;

                result[7] = Color.magenta;

                return result;
            }
        }

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
                -Unity.Mathematics.math.sqrt(3f / 2f), 0f, 0f)/2f;

            //find position of convergent point for other tetrahedrons in the net.
            Unity.Mathematics.float4 apex =
                new Unity.Mathematics.float4(-2 * Unity.Mathematics.math.sqrt(2f / 5f), 0f, 0f, 0f);

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
            
            //Debug.Log(Math.Operations.Angle(dir1, apex-center1) + " : " + Math.Operations.Angle(dir2, apex-center2) + " : " + Math.Operations.Angle(dir3, apex-center3) + " : " + Math.Operations.Angle(dir4, apex-center4));

            for (int i = 0; i < result.Length; i++)
            {
                result[i] += offset;
            }
            
            return result;
        }

        private static void confirmResult(float degreeFolded)
        {
            Unity.Mathematics.float4[] verts = vertices(degreeFolded);
            Unity.Mathematics.float4 apex =
                new Unity.Mathematics.float4(-2 * Unity.Mathematics.math.sqrt(2f / 5f), 0f, 0f, 0f);
            if (!areEqual(apex, verts[4], verts[5], verts[6], verts[7]))
                Debug.Log(apex + " : " + verts[4] + " : " + verts[5] + " : " + verts[6] + " : " + verts[7]);
        }

        private static bool areEqual(Unity.Mathematics.float4 a, Unity.Mathematics.float4 b, Unity.Mathematics.float4 c,
            Unity.Mathematics.float4 d, Unity.Mathematics.float4 e)
        {
            return a.x == b.x && a.x == c.x && a.x == d.x && a.x == e.x &&
                   a.y == b.y && a.y == c.y && a.y == d.y && a.y == e.y &&
                   a.z == b.z && a.z == c.z && a.z == d.z && a.z == e.z &&
                   a.w == b.w && a.w == c.w && a.w == d.w && a.w == e.w;
        }
    }
}
