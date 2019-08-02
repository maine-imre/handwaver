using System;
using Unity.Mathematics;
using UnityEngine;
using IMRE.Math;

namespace IMRE.HandWaver.Rendering
{
    /// <summary>
    /// Contains functional code that would render out a given element based on passed in data.
    /// </summary>
    public static class GeoElementRenderLib
    {
        private const float width = .0025f;
        private const float vertexCount = 3000f;
        private const float angleRadius = 0.5f;

        private static float3 headPosition
        {
            get
            {
                //TODO attach to emboided input
                return float3.zero;
                
            }
        }

        private static float3 headDirection
        {
            get
            {
                //TODO attach to emboided input
                return float3.zero;
            }
        }
        
        private static void render(UnityEngine.Mesh mesh)
        {
            //TODO @Nathan - how do we do this with ECS?
            throw new NotImplementedException();
        }
        
        // TODO: Group by element type.
        //    -- Point
        //    -- Path
        //    -- Surface
        //    -- Region
        //    -- NonGeometeric
        // TODO: Determine input vars.
        // TODO: Implement functionality for base version of HW Rendering.

        public static bool Line(float3 point, float3 direction)
        {
                direction = Unity.Mathematics.math.normalize(direction);
                return Segment(point + 100f * direction, point - 100f * direction);
        }

        public static bool Point(float3 location)
        {
            throw new NotImplementedException();
        }

        public static bool Segment(float3 endpointA, float3 endpointB)
        {
            //TODO @Nathan review try catch
            try
            {

                UnityEngine.Mesh mesh = new UnityEngine.Mesh();
                float3 lineWidth = width * Unity.Mathematics.math.normalize(Unity.Mathematics.math.cross(
                                       endpointA - endpointB,
                                       (endpointA + endpointB) / 2f - headDirection));

                UnityEngine.Vector3[] verts = new UnityEngine.Vector3[4];
                verts[0] = endpointA + lineWidth;
                verts[1] = endpointA - lineWidth;
                verts[2] = endpointB - lineWidth;
                verts[3] = endpointB + lineWidth;

                mesh.vertices = verts;

                //triangles
                //TODO check clockwise vs counterclockwise
                int[] tris = {1, 3, 0, 1, 2, 3};
                mesh.triangles = tris;

                //uvs
                UnityEngine.Vector2[] uvs = {new UnityEngine.Vector2(0,1), new UnityEngine.Vector2(0,0), 
                    new UnityEngine.Vector2(1,0), new UnityEngine.Vector2(1,1) };
                mesh.uv = uvs;

                mesh.RecalculateNormals();

                //normals
                UnityEngine.Vector3[] normals = mesh.normals;
                for (int i = 0; i < verts.Length; i++)
                    normals[i] = Unity.Mathematics.math.normalize(headDirection);
                mesh.normals = normals;

                render(mesh);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Angle(UnityEngine.Vector3 center, UnityEngine.Vector3 segmentStart, UnityEngine.Vector3 segmentAEndpoint, 
            UnityEngine.Vector3 segmentBEndpoint)
        {
            try
            {
                
                Segment(segmentStart, segmentAEndpoint);
                Segment(segmentStart, segmentBEndpoint);
                
                #region Render Circle
                UnityEngine.Mesh semiCircleMesh = new UnityEngine.Mesh();

                //semicircle for angle
                //normal vectors
                UnityEngine.Vector3 norm1 = UnityEngine.Vector3.forward;
                UnityEngine.Vector3 norm2 = UnityEngine.Vector3.right;

                //array of vector3s for vertices
                UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[(int) vertexCount];

                //math for rendering circle
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices[i] = (angleRadius *
                                   ((UnityEngine.Mathf.Sin((i * UnityEngine.Vector3.Angle(segmentStart - segmentAEndpoint, 
                                                                segmentStart - segmentBEndpoint)) / (vertexCount - 1)) * norm1) +
                                    (UnityEngine.Mathf.Cos((i * UnityEngine.Vector3.Angle(segmentStart - segmentAEndpoint, 
                                                                segmentStart - segmentBEndpoint)) / (vertexCount - 1)) *norm2))) +
                                                                center;
                }

                semiCircleMesh.vertices = vertices;
                //semiCircleMesh.SetPositions(vertices);
                render(semiCircleMesh);
                #endregion           

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Axis( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Conic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool ConicPart( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool CurveCartesian( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Function( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool FunctionND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Interval( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Locus( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool LocusND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Number( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Poly( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool PolyLine( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }
        
        public static bool Polygon(float3[] pointFloat3Array)
        {

            try
            {
                Vector3[] pointVectorArray = new Vector3[pointFloat3Array.Length];

                UnityEngine.Mesh mesh = new UnityEngine.Mesh();
                for (int i = 0; i < pointFloat3Array.Length; i++)
                {
                    pointVectorArray[i] = pointFloat3Array[i].ToVector3();
                }

                mesh.vertices = pointVectorArray;

                //TODO make triangles between groups of three points, starting with smallest x value and increasing
                foreach (Vector3 point in pointVectorArray)
                {
                    
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool Ray( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool SurfaceFinite( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Symbolic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Vector2D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Vector3D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Text( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Turtle( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Video( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Image( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static bool Slider( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

    }
}