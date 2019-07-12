using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
    /// <summary>
    /// 
    /// </summary>
	public static class AzmuthalProjectionScript
    {
        
        /// <summary>
        /// This script is used to map a equiangular rectangular projection of the globe onto a azmuthal projection of the globe
        /// For rendering "flat earth disk"
        /// </summary>
        /// <param name="count">the number of verticies per side of equirectangular</param>
        /// <returns>the disk mesh that will accept an equirectuangular image</returns>
        public static Mesh azmuthalUVMesh(int count)
        {
            Mesh myMesh = new Mesh();
            Vector3[] vertices = new Vector3[count*count];
            Vector2[] uvs = new Vector2[count * count];
            int[] triangles = new int[6 * (count-1)*(count-1)];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    vertices[count * i + j] = Vector3.right * i + Vector3.forward * j;

                    //azmuthal projecction UVs.  how to check this?
                    uvs[count * i + j] = (j/count) * (Vector3.right * Mathf.Sin(((1f/2f) - (i/count))*Mathf.PI) + Vector3.forward * Mathf.Cos(((1f / 2f) - (i / count)) * Mathf.PI));

                    if(i!= 0 && j!=0)
                    {
                        //check clockwise/counter for quad.
                        int idx = (count-1) * (i-1) + (j-1);
                        triangles[6 * idx] = count * i + j;
                        triangles[6 * idx+1] = count * (i-1) + j;
                        triangles[6 * idx+2] = count * i + (j-1);

                        triangles[6 * idx+3] = count * (i-1) + (j-1);
                        triangles[6 * idx+4] = count * (i-1) + j;
                        triangles[6 * idx+5] = count * i + (j-1);
                    }
                }
            }

            myMesh.vertices = vertices;
            myMesh.uv = uvs;
            myMesh.triangles = triangles;
            myMesh.RecalculateNormals();

            return myMesh;
        }
    }
}