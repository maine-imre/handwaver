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
            float3 da_hat = (a - d) / Vector3.Magnitude(a - d);


            float3 ab_star = intersectLines(point, lineDirection, a, ab_hat);
            float3 bc_star = intersectLines(point, lineDirection, b, bc_hat);
            float3 cd_star = intersectLines(point, lineDirection, c, cd_hat);
            float3 da_star = intersectLines(point, lineDirection, d, da_hat);

            bool ab_star_isEndpoint = ab_star.Equals(a) || ab_star.Equals(b);
            bool bc_star_isEndpoint = bc_star.Equals(b) || bc_star.Equals(c);
            bool cd_star_isEndpoint = cd_star.Equals(c) || cd_star.Equals(d);
            bool ad_star_isEndpoint = da_star.Equals(d) || da_star.Equals(a);

            bool ab_star_onSegment = Vector3.Magnitude(ab_star - a) > Vector3.Magnitude(b - a) ||
                                     Vector3.Magnitude(ab_star - b) > Vector3.Magnitude(b - a);
            bool bc_star_onSegment = Vector3.Magnitude(bc_star - b) > Vector3.Magnitude(c - b) ||
                                     Vector3.Magnitude(bc_star - c) > Vector3.Magnitude(c - b);
            bool cd_star_onSegment = Vector3.Magnitude(cd_star - c) > Vector3.Magnitude(d - c) ||
                                     Vector3.Magnitude(cd_star - d) > Vector3.Magnitude(d - c);
            bool da_star_onSegment = Vector3.Magnitude(da_star - d) > Vector3.Magnitude(a - d) ||
                                     Vector3.Magnitude(da_star - a) > Vector3.Magnitude(a - d);
            
            int endpointCount = 0;
            if (ab_star_isEndpoint)
                endpointCount++;
            if (bc_star_isEndpoint)
                endpointCount++;
            if (cd_star_isEndpoint)
                endpointCount++;
            if (ad_star_isEndpoint)
                endpointCount++;
            
            if (!(ab_star_onSegment || bc_star_onSegment || cd_star_onSegment || da_star_onSegment))
            {
                crossSectionRenderer.enabled = false;
                Debug.Log("Line does not intersect with any of triangle sides.");
            }
            
            else if (endpointCount >= 2 &&
                     (!ab_star.Equals(bc_star) || !ab_star.Equals(da_star) || !bc_star.Equals(cd_star) || !cd_star.Equals(da_star)))
            {
                crossSectionRenderer.enabled = true;
                
                if(!ab_star.Equals(bc_star))
                {
                    if ()
                    {
                        
                    }
                }
            }

            if (endpointCount == 2 && (ab_star.Equals(bc_star) || bc_star.Equals(cd_star) || cd_star.Equals(da_star)))
            {
                if (ab_star.Equals(bc_star))
                {
                    if (cd_star_onSegment)
                    {
                        crossSectionRenderer.SetPosition(0, b);
                        crossSectionRenderer.SetPosition(1, cd_star);
                    }
                    else
                    {
                        crossSectionRenderer.SetPosition(0, b);
                        crossSectionRenderer.SetPosition(1, da_star);
                    }
                }
                else if (bc_star.Equals(cd_star))
                {
                    if (ab_star_onSegment)
                    {
                        crossSectionRenderer.SetPosition(0, c);
                        crossSectionRenderer.SetPosition(1, ab_star);
                    }
                    else
                    {
                        crossSectionRenderer.SetPosition(0, c);
                        crossSectionRenderer.SetPosition(1, da_star);
                    }
                }
                else if (cd_star.Equals(da_star))
                {
                    if (ab_star_onSegment)
                    {
                        crossSectionRenderer.SetPosition(0, d);
                        crossSectionRenderer.SetPosition(1, ab_star);
                    }
                    else
                    {
                        crossSectionRenderer.SetPosition(0, c);
                        crossSectionRenderer.SetPosition(1, bc_star);
                    }
                }
                else
                {
                    if (cd_star_onSegment)
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, cd_star);
                    }

                    if (bc_star_onSegment)
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, bc_star);
                    }
                }
            }
            //intersection hits two segments of the square
            else
            {
                crossSectionRenderer.enabled = true;

                if (ab_star_onSegment && bc_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, ab_star);
                    crossSectionRenderer.SetPosition(1, bc_star);
                }
                else if (ab_star_onSegment && cd_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, ab_star);
                    crossSectionRenderer.SetPosition(1, cd_star);
                }
                else if (ab_star_onSegment && da_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, ab_star);
                    crossSectionRenderer.SetPosition(1, da_star);
                }
                else if (bc_star_onSegment && da_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, bc_star);
                    crossSectionRenderer.SetPosition(1, da_star);
                }
                else if (bc_star_onSegment && cd_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, bc_star);
                    crossSectionRenderer.SetPosition(1, cd_star);
                }
                else
                {
                    crossSectionRenderer.SetPosition(0, cd_star);
                    crossSectionRenderer.SetPosition(1, da_star);
                }
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

            if (solution.Equals(solution_alt))
            {
                return solution;
            }
            else
            {
                Debug.Log("Intersection failed");
                return new float3(0, 0, 0);
            }
        }


        #endregion
    }
}
    
