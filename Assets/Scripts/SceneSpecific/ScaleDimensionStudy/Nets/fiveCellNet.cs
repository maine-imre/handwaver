using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IMRE.HandWaver.ScaleStudy;

namespace IMRE.HandWaver.HigherDimensions
{
    /// <summary>
    /// Net of five cell for scale and dimension study.
    /// </summary>
	public class fiveCellNet : AbstractHigherDimSolid, ISliderInput, I4D_Perspective
    {
        //initialize fold
        //read only static float GoldenRatio = (1f + Mathf.Sqrt(5f)) / 2f;
        private float _percentFolded;
        public bool sliderOverride;

        public float PercentFolded
        {
            get
            {
                return _percentFolded;
            }
            set
            {
                _percentFolded = value;
                originalVerts = vertices(value*60f).ToList();
            }
        }
        
        public float slider {
            set => PercentFolded = !sliderOverride ? value : 1f;
        }

        /// <summary>
        /// configure vertices of fivecell around core tetrahedron
        /// </summary>
        /// <param name="degreeFolded"></param>
        /// <returns></returns>
        private static Vector4[] vertices(float degreeFolded)
        {
            //8 points on unfolded fivecell
            Vector4[] result = new Vector4[8];
           
            //core tetrahedron (does not fold)
	    //coordiantes from wikipedia  https://en.wikipedia.org/wiki/5-cell, centered at origin, 
            result[0] = (new float4(1f/math.sqrt(10f), 1f/math.sqrt(6f), 1f/math.sqrt(3f), 1f))/2f;
            result[1] = (new float4(1f/math.sqrt(10f), 1f/math.sqrt(6f), 1f/math.sqrt(3f), -1f))/2f;
            result[2] = (new float4(1f/math.sqrt(10f), 1f/math.sqrt(6f), -2f/math.sqrt(3f), 0f))/2f;
            result[3] = new float4(1f/math.sqrt(10f), -math.sqrt(3f/2f), 0f, 0f);
	    
	    //find position of convergent point for other tetrahedrons in the net.
	    float4 apex = new float4(-2*math.sqrt(2f/5f), 0f, 0f, 0f);
	    //TODO consider making the initial projection onto n
	    
            //apex of tetrahedron for each additional tetrahedron(from fases of first) foldling by degree t
	    float4 center1 =  (result[0] + result[1] + result[2]) / 3f
	    float4 dir1 = center1 - result[3]; //TODO normalize this
            result[4] = center1 + dir1.rotate(dir1, apex - center1, degreeFolded);
	    
	    float4 center2 = (result[0] + result[2] + result[3]) / 3f;
	    float4 dir2 = center2 - result[1];//TODO normalize this
            result[5] = center2 + dir2.rotate(dir2, apex - center2, degreeFolded);
	    
	    float4 center3 = (result[0] + result[1] + result[3]) / 3f;
	    float4 dir3 = center3 - result[2];//TODO normalize this
            result[6] = center3 + dir3.rotate(dir3, apex - center3, degreeFolded);
	    
	    float4 center4 =  (result[1] + result[2] + result[3]) / 3f;
	    float4 dir4 = center4-result[0];//TODO normalize this
	    result[7] = center4 +  dir4.rotate(dir4, apex - center4, degreeFolded);
            
            return result;
        }

        /// <summary>
        /// matrix for vertices to make faces of tetrahedrons for fivecell
        /// </summary>
        internal int[] faces = 
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
	    
	    //tetrahedron 4
	    1,2,7,
	    2,3,7
	    3,1,7
        };
        /// <summary>
        /// override function from abstract class and create figure
        /// </summary>
        internal override void drawFigure()
        {
            //clear mesh,verts, triangles, uvs
            mesh.Clear();
            verts = new List<Vector3>();
            tris = new List<int>();
            uvs = new List<Vector2>();

            //create a triangular plane for each face of the fivecell
            for (int i = 0; i < 13; i++)
            {
                CreatePlane(rotatedVerts[faces[i * 3]], rotatedVerts[faces[i * 3+1]], rotatedVerts[faces[i * 3+2]]);
            }
        }
    }
}
