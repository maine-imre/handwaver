using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.ScaleStudy
{
    public class SquareCrossSection : UnityEngine.MonoBehaviour
    {

        /// <summary>
        /// Function to render the intersection of a plane and a square
        /// </summary>
        /// <param name="height"></param>
        /// <param name="vertices"></param>
        /// <param name="crossSectionRenderer"></param>
        public void crossSectSquare(float3 point, float3 direction, Vector3[] vertices, LineRenderer crossSectionRenderer)
        {
            //Vertices are organized in clockwise manner starting from top left
            //top left
            float3 a = vertices[0];
            //top right
            float3 b = vertices[1];
            //bottom right
            float3 c = vertices[2];
            //bottom left
            float3 d = vertices[3];

            float3 lineDirection = direction - point;
            
            float3 ac_hat = (c - a) / Vector3.Magnitude(c - a);
            float3 ab_hat = (b - a) / Vector3.Magnitude(b - a);
            float3 ad_hat = (d - a) / Vector3.Magnitude(d - a);
            float3 bc_hat = (c - b) / Vector3.Magnitude(c - b);
            float3 bd_hat = (d - b) / Vector3.Magnitude(d - b);
            float3 cd_hat = (d - c) / Vector3.Magnitude(d - c);


        }

        //these are simply copy-pasted for now
        #region functions

        /// <summary>
        /// Returns the point of intersection of two lines
        /// </summary>
        /// <param name="p"></param>
        /// <param name="u"></param>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private float3 intersectLines(float3 p, float3 u, float3 q, float3 v)
        {
            //using method described here: http://geomalgorithms.com/a05-_intersect-1.html
            float3 w = q - p;
            float3 v_perp =
                math.normalize(math.cross(math.cross(u, v), v));
            float3 u_perp =
                math.normalize(math.cross(math.cross(u, v), u));
            float s = Unity.Mathematics.math.dot(-1 * v_perp, w) / math.dot(-1 * v_perp, u);
           
            //note if s == 0, lines are parallel
            float3 solution = p + s * u;

            //the next couple of lines don't calculate a solution but can validate our solution.
            float t = math.dot(-1 * u_perp, w) / math.dot(-1 * u_perp, v);
           
            //note that if t == 0, lines are parallel
            float3 solution_alt = q + t * v;

            if (isEqual(solution, solution_alt))
            {
                return solution;
            }
            else
            {
                Debug.Log("Intersection failed");
                return new float3(0, 0, 0);
            }
        }

        /// <summary>
        /// Function to determine if two float3's have equal components
        /// Created to deal with errors generated from bool3/bool logic
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private bool isEqual(float3 first, float3 second)
        {
            return (first.x == second.x && first.y == second.y && first.x == second.z);
        }
        
        #endregion
    }
}
    
