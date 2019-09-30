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
            
            float3 ab_hat = (b - a) / Vector3.Magnitude(b - a);
            float3 bc_hat = (c - b) / Vector3.Magnitude(c - b);
            float3 cd_hat = (d - c) / Vector3.Magnitude(d - c);
            float3 ad_hat = (a - d) / Vector3.Magnitude(a - d);


            float3 ab_star = intersectLines(point, direction, a, ab_hat);
            float3 bc_star = intersectLines(point, direction, b, bc_hat);
            float3 cd_star = intersectLines(point, direction, c, cd_hat);
            float3 ad_star = intersectLines(point, direction, d, ad_hat);

            bool ab_star_isEndpoint = isEqual(ab_star, a) || isEqual(ab_star, b);
            bool bc_star_isEndpoint = isEqual(bc_star, b) || isEqual(bc_star, c);
            bool cd_star_isEndpoint = isEqual(cd_star, c) || isEqual(cd_star, d);
            bool ad_star_isEndpoint = isEqual(ad_star, d) || isEqual(ad_star, d);

            bool ab_star_onSegment = Vector3.Magnitude(ab_star - a) > Vector3.Magnitude(b - a) ||
                                     Vector3.Magnitude(ab_star - b) > Vector3.Magnitude(b - a);
            bool bc_star_onSegment = Vector3.Magnitude(bc_star - b) > Vector3.Magnitude(c - b) ||
                                     Vector3.Magnitude(bc_star - c) > Vector3.Magnitude(c - b);
            bool cd_star_onSegment = Vector3.Magnitude(cd_star - c) > Vector3.Magnitude(d - c) ||
                                     Vector3.Magnitude(cd_star - d) > Vector3.Magnitude(d - c);
            bool ad_star_onSegment = Vector3.Magnitude(ad_star - d) > Vector3.Magnitude(a - d) ||
                                     Vector3.Magnitude(ad_star - a) > Vector3.Magnitude(a - d);
            
            int endpointCount = 0;
            if (ab_star_isEndpoint)
                endpointCount++;
            if (bc_star_isEndpoint)
                endpointCount++;
            if (cd_star_isEndpoint)
                endpointCount++;
            if (ad_star_isEndpoint)
                endpointCount++;
            
            if (!(ab_star_onSegment || bc_star_onSegment || cd_star_onSegment || ad_star_onSegment))
            {
                crossSectionRenderer.enabled = false;
                Debug.Log("Line does not intersect with any of triangle sides.");
            }
            


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
    
