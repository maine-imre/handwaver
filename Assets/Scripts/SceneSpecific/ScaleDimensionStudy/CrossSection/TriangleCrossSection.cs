using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.ScaleStudy

{

    public class TriangleCrossSection : UnityEngine.MonoBehaviour
    {
        /// <summary>
        /// Function to render the intersection of a plane and a triangle
        /// </summary>
        /// <param name="height"></param>
        /// <param name="vertices"></param>
        /// <param name="crossSectionRenderer"></param>
        public void crossSectTri(float3 point, float3 direction, Vector3[] vertices, LineRenderer crossSectionRenderer)
        {
            //Vertices are organized in clockwise manner starting from top
            float3 a = vertices[0];
            float3 b = vertices[1];
            float3 c = vertices[2];

            float3 lineDirection = direction - point;

            float3 ac_hat = (c - a) / Vector3.Magnitude(c - a);
            float3 ab_hat = (b - a) / Vector3.Magnitude(b - a);
            float3 bc_hat = (c - b) / Vector3.Magnitude(c - b);

            float3 ac_star = intersectLines(point, lineDirection, a, ac_hat);
            float3 ab_star = intersectLines(point, lineDirection, a, ab_hat);
            float3 bc_star = intersectLines(point, lineDirection, b, bc_hat);

            bool ac_star_isEndpoint;
            ac_star_isEndpoint = isEqual(ac_star, a) || isEqual(ac_star, c);
            bool ab_star_isEndpoint;
            ab_star_isEndpoint = isEqual(ab_star, a) || isEqual(ab_star, b);
            bool bc_star_isEndpoint;
            bc_star_isEndpoint = isEqual(bc_star, b) || isEqual(bc_star, c);

            //correct Vec3 arithmetic to float3
            bool ac_star_onSegment = (Vector3.Magnitude(ac_star - a) > Vector3.Magnitude(c - a) ||
                                      Vector3.Magnitude(ac_star - c) > Vector3.Magnitude(c - a));
            bool ab_star_onSegment = (Vector3.Magnitude(ab_star - a) > Vector3.Magnitude(b - a) ||
                                      Vector3.Magnitude(ab_star - c) > Vector3.Magnitude(b - a));
            bool bc_star_onSegment = (Vector3.Magnitude(bc_star - b) > Vector3.Magnitude(c - b) ||
                                      Vector3.Magnitude(bc_star - c) > Vector3.Magnitude(c - b));

            int endpointCount = 0;
            if (ac_star_isEndpoint)
                endpointCount++;
            if (ab_star_isEndpoint)
                endpointCount++;
            if (bc_star_isEndpoint)
                endpointCount++;

            //If plane does not hit triangle
            if (!(ab_star_onSegment || ac_star_onSegment || bc_star_onSegment))
            {
                crossSectionRenderer.enabled = false;
                Debug.Log("Line does not intersect with any of triangle sides.");
            }
            
            else if (endpointCount >= 2 && (!isEqual(ab_star, ac_star) || !isEqual(ab_star, bc_star) || !isEqual(ac_star, bc_star)))
            {
                crossSectionRenderer.enabled = true;

                if (!isEqual(ab_star, ac_star))
                {
                    if (isEqual(intersectLines(point, direction, a, ab_star), point) || isEqual(intersectLines(point, direction, a, ac_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, b);
                        crossSectionRenderer.SetPosition(1, c);
                    }
                    else if (isEqual(intersectLines(point, direction, b, ab_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, c);
                    }
                    else if (isEqual(intersectLines(point, direction, c, ac_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, b);
                    }
                    
                }
                
                else if (!isEqual(ab_star, bc_star))
                {
                    if (isEqual(intersectLines(point, direction, a, ab_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, b);
                        crossSectionRenderer.SetPosition(1, c);
                    }
                    else if (isEqual(intersectLines(point, direction, b, ab_star), point) || isEqual(intersectLines(point, direction, b, bc_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, c);
                    }
                    else if (isEqual(intersectLines(point, direction, c, bc_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, b);
                    }
                }

                if (!isEqual(ac_star, bc_star))
                {
                    if (isEqual(intersectLines(point, direction, a, ac_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, b);
                        crossSectionRenderer.SetPosition(1, c);
                    }
                    else if (isEqual(intersectLines(point, direction, c, ac_star), point) || isEqual(intersectLines(point, direction, c, bc_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, b);
                    }
                    else if (isEqual(intersectLines(point, direction, b, bc_star), point))
                    {
                        crossSectionRenderer.SetPosition(0, a);
                        crossSectionRenderer.SetPosition(1, c);
                    }                
                }
                
            }
            else if (endpointCount == 2 && (isEqual(ab_star, ac_star) || isEqual(ab_star, bc_star) || isEqual(ac_star, bc_star)))
            {
                crossSectionRenderer.enabled = true;

                if (isEqual(ab_star, ac_star))
                {
                    crossSectionRenderer.SetPosition(0, a);
                    crossSectionRenderer.SetPosition(1, bc_star);
                }
                else if (isEqual(ab_star, bc_star))
                {
                    crossSectionRenderer.SetPosition(0, b);
                    crossSectionRenderer.SetPosition(1, ac_star);
                }
                else if (isEqual(ac_star, bc_star))
                {
                    crossSectionRenderer.SetPosition(0, c);
                    crossSectionRenderer.SetPosition(1, ab_star);
                }
                
            }
            else
            {
                crossSectionRenderer.enabled = true;

                if (ac_star_onSegment && ab_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, ac_star);
                    crossSectionRenderer.SetPosition(1, ab_star);
                }
                else if (ac_star_onSegment && bc_star_onSegment)
                {
                    crossSectionRenderer.SetPosition(0, ac_star);
                    crossSectionRenderer.SetPosition(1, bc_star);
                }
                else
                {
                    crossSectionRenderer.SetPosition(0, ab_star);
                    crossSectionRenderer.SetPosition(1, bc_star);
                }
            }
        }

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



    }
}
