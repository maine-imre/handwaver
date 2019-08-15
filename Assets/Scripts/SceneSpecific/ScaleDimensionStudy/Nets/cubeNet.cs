using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A net of a cube that folds into a cube
    /// The main contributor(s) to this script is __
    /// </summary>
    public class cubeNet : net3D
    {

        private void Start()
        {
            
            
            //uvs
            Vector2[] uvs = new Vector2[14];
            uvs[0] = new Vector2(0.25f, 2f/3f);
            uvs[1] = new Vector2(0.5f, 2f/3f);
            uvs[2] = new Vector2(0.5f, 1f/3f);
            uvs[3] = new Vector2(0.25f, 1f/3f);
            uvs[4] = new Vector2(0f, 2f/3f);
            uvs[5] = new Vector2(0f, 1f/3f);
            uvs[6] = new Vector2(0.25f, 1f);
            uvs[7] = new Vector2(0.5f, 1f);
            uvs[8] = new Vector2(0.5f, 0f);
            uvs[9] = new Vector2(0.25f, 0f);
            uvs[10] = new Vector2(0.75f, 2f/3f);
            uvs[11] = new Vector2(0.75f, 1f/3f);
            uvs[12] = new Vector2(1f, 2f/3);
            uvs[13] = new Vector2(1f, 1f/3f);

            //assign mesh
            mesh.vertices = meshVerts(0);
            mesh.triangles = meshTris();
            mesh.uv = uvs;
            
            //22 vertices on trace of cube net
            lineRenderer.positionCount = 22;
            lineRenderer.useWorldSpace = false;
            lineRenderer.startWidth = .01f;
            lineRenderer.endWidth = .01f;
            lineRenderer.SetPositions(lineRendererVerts(0));
            
        }

        //startTime = DateTime.Now

        /// <summary>
        /// configure vertices of cube around base square
        /// </summary>
        /// <param name="percentFolded"></param>
        /// <returns></returns>
        public override UnityEngine.Vector3[] meshVerts(float percentFolded)
        {
            float degreeFolded = (percentFolded * 90f) + 180f;
            //14 points on cube net
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[14];

            //4 vertices for base of cube
            result[0] = .5f * (UnityEngine.Vector3.forward + UnityEngine.Vector3.right);
            result[1] = .5f * (UnityEngine.Vector3.forward + UnityEngine.Vector3.left);
            result[2] = .5f * (UnityEngine.Vector3.back + UnityEngine.Vector3.left);
            result[3] = .5f * (UnityEngine.Vector3.back + UnityEngine.Vector3.right);

            //use squareVert() to fold outer squares up relative to base square 
            result[4] = squareVert(result[3], result[0], result[1], degreeFolded);
            result[5] = squareVert(result[3], result[0], result[2], degreeFolded);

            result[6] = squareVert(result[0], result[1], result[3], degreeFolded);
            result[7] = squareVert(result[0], result[1], result[2], degreeFolded);

            result[8] = squareVert(result[2], result[3], result[1], degreeFolded);
            result[9] = squareVert(result[2], result[3], result[0], degreeFolded);

            result[10] = squareVert(result[1], result[2], result[0], degreeFolded);
            result[11] = squareVert(result[1], result[2], result[3], degreeFolded);

            result[12] = squareVert(result[10], result[11], result[1], degreeFolded);
            result[13] = squareVert(result[10], result[11], result[2], degreeFolded);

            return result;
        }

        /// <summary>
        /// function to calculate vertices on outer faces
        /// </summary>
        /// <param name="nSegmentA"></param>
        /// <param name="nSegmentB"></param>
        /// <param name="oppositePoint"></param>
        /// <param name="degreeFolded"></param>
        /// <returns></returns>
        private static UnityEngine.Vector3 squareVert(UnityEngine.Vector3 nSegmentA, UnityEngine.Vector3 nSegmentB,
            UnityEngine.Vector3 oppositePoint,
            float degreeFolded)
        {
            //
            return (UnityEngine.Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) *
                    (oppositePoint - ((nSegmentA + nSegmentB) / 2f))) + ((nSegmentA + nSegmentB) / 2f);
        }

        /// <summary>
        /// create an array for each square that divides it into two triangles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static int[] meshQuad(int a, int b, int c, int d)
        {
            return new[] {a, b, d, d, b, c};
        }

        /// <summary>
        /// divide each face of the cube net into two triangles and copy them into a new array of the triangles
        /// </summary>
        /// <returns></returns>
        private static int[] meshTris()
        {
            int[] result = new int[6 * 6];
            meshQuad(0, 4, 5, 3).CopyTo(result, 0);
            meshQuad(0, 3, 2, 1).CopyTo(result, 6);
            meshQuad(3, 9, 8, 2).CopyTo(result, 12);
            meshQuad(0, 1, 7, 6).CopyTo(result, 18);
            meshQuad(1, 2, 11, 10).CopyTo(result, 24);
            meshQuad(10, 11, 13, 12).CopyTo(result, 30);
            return result;
        }

        /// <summary>
        /// mapping of outline of unfold
        /// </summary>
        /// <param name="percentFolded"></param>
        /// <returns></returns>
        public override  UnityEngine.Vector3[] lineRendererVerts(float percentFolded)
        {
            UnityEngine.Vector3[] result = new UnityEngine.Vector3[22];
            //map vertices on line segment to vertices on unfolded cube
            UnityEngine.Vector3[] tmp = meshVerts(percentFolded);
            result[0] = tmp[0];
            result[1] = tmp[4];
            result[2] = tmp[5];
            result[3] = tmp[3];
            result[4] = tmp[9];
            result[5] = tmp[8];
            result[6] = tmp[2];
            result[7] = tmp[11];
            result[8] = tmp[13];
            result[9] = tmp[12];
            result[10] = tmp[10];
            result[11] = tmp[11];
            result[12] = tmp[2];
            result[13] = tmp[3];
            result[14] = tmp[0];
            result[15] = tmp[6];
            result[16] = tmp[7];
            result[17] = tmp[1];
            result[18] = tmp[2];
            result[19] = tmp[3];
            result[20] = tmp[0];
            result[21] = tmp[1];

            return result;
        }
    }
}
