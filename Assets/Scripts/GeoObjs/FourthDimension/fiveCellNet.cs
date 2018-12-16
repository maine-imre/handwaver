using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver.HigherDimensions
{
/// <summary>
/// Net of five cell for scale and dimension study.
/// </summary>
	public class fiveCellNet : AbstractHigherDimSolid
    {

        private static Vector4 right = (new Vector4(0, 0, 1, 0) - new Vector4(Mathf.Sqrt(8f / 9f), 0, -1f / 3f, 0f)).normalized;
        private static Vector4 left = -right;
        private static Vector4 up = (new Vector4(0, 0, 1, 0) - new Vector4(-Mathf.Sqrt(2f / 9f), Mathf.Sqrt(2f / 3f), -1f / 3f, 0f)).normalized;
        private static Vector4 down = -up;
        private static Vector4 forward = (new Vector4(0, 0, 1, 0) - new Vector4(-Mathf.Sqrt(2f / 9f), -Mathf.Sqrt(2f / 3f), -1f / 3f, 0f)).normalized;
        private static Vector4 back = -forward;
        private static Vector4 wForward = new Vector4(0, 0, 0, 1);
        private static Vector4 wBack = -wForward;

        private void Awake()
        {
            Fold = 0f;
        }

        private void FixedUpdate()
        {
            Fold++;
        }

        //readonly static float GoldenRatio = (1f + Mathf.Sqrt(5f)) / 2f;
        private float _fold;
        public float Fold
        {
            get
            {
                return _fold;
            }
            set
            {
                _fold = value;
                originalVerts = vertices(value).ToList();
            }
        }


        private static Vector4[] vertices(float t)
        {
            Vector4[] result = new Vector4[8];
            //core tetrahedron.
            result[0] = Vector4.zero;
            result[1] = right;
            result[2] = up;
            result[3] = forward;

            //apex of tetrahedron for each additional (from fases of first)
            result[4] = (result[0] + result[1] + result[2]) / 3f + forward.rotate(forward, wForward, t);
            result[5] = (result[0] + result[2] + result[3]) / 3f + right.rotate(right, wForward, t);
            result[6] = (result[0] + result[1] + result[3]) / 3f + up.rotate(up, wForward, t);

            Vector4 centerOfFaceOpposite0 = (result[1] + result[2] + result[3]) / 3f;
            result[7] = centerOfFaceOpposite0 + centerOfFaceOpposite0.rotate(centerOfFaceOpposite0, wForward, t);
            return result;
        }

        internal int[] faces = new int[]
        {
            //core tetrahedron
            0,1,2,
            0,1,3,
            2,0,3,
            1,2,3,

            //tetrahedron 1
            0,1,4,
            2,0,4,
            1,2,4,

            //tetrahedron 2
            0,1,5,
            2,0,5,
            1,2,5,

            //tetrahedron 3
            0,1,6,
            2,0,6,
            1,2,6
        };

        internal override void drawFigure()
        {
            mesh.Clear();
            verts = new List<Vector3>();
            tris = new List<int>();
            uvs = new List<Vector2>();

            for (int i = 0; i < 13; i++)
            {
                CreatePlane(rotatedVerts[faces[i * 3]], rotatedVerts[faces[i * 3+1]], rotatedVerts[faces[i * 3+2]]);
            }
        }
    }
}
