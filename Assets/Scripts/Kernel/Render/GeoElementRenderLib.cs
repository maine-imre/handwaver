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

        public static void Line(float3 point, float3 direction)
        {
                direction = Unity.Mathematics.math.normalize(direction);
                Segment(point + 100f * direction, point - 100f * direction);
        }

        public static void Point(float3 location)
        {           
            //TODO a better visualization of a point
            float3 dir = (width/2f)*(new float3(1,1,1));
            Segment(location - dir, location + dir);
        }

        public static void Segment(float3 endpointA, float3 endpointB)
        {
                UnityEngine.Mesh mesh = new UnityEngine.Mesh();
                float3 lineWidth = width * Unity.Mathematics.math.normalize(Unity.Mathematics.math.cross(
                                       endpointA - endpointB,
                                       (endpointA + endpointB) / 2f - headDirection));

                //verts
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
        }

        public static void Angle(UnityEngine.Vector3 center, UnityEngine.Vector3 segmentStart, UnityEngine.Vector3 segmentAEndpoint, 
            UnityEngine.Vector3 segmentBEndpoint)
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
        }

        public static void Axis( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Circle(float3 center, float3 edgePoint, float3 normalDirection)
        {
            const int n = 500;
            
            Mesh mesh = new Mesh();
            
            //normal vectors
            Unity.Mathematics.float3 basis1 = Unity.Mathematics.math.normalize(edgePoint-center);
            Unity.Mathematics.float3 basis2 = Unity.Mathematics.math.cross(basis1,normalDirection);
            float radius = IMRE.Math.Operations.magnitude(edgePoint - center);
            //array of vector3s for vertices
            Unity.Mathematics.float3[] positions = new Unity.Mathematics.float3[n];
            Unity.Mathematics.float3[] vertices = new Unity.Mathematics.float3[2*n];
            Vector3[] verts = new Vector3[vertices.Length];

            //math for rendering circle
            for (int i = 0; i < n; i++)
            {
                positions[i] = (radius * ((UnityEngine.Mathf.Sin((i * UnityEngine.Mathf.PI * 2) / (n - 1)) * basis1) +
                                         (UnityEngine.Mathf.Cos((i * UnityEngine.Mathf.PI * 2) / (n - 1)) * basis2))) +
                              Vector3.zero;
            }

            for (int i = 0; i < n; i++)
            {
                float3 lineWidth = width * Unity.Mathematics.math.normalize(Unity.Mathematics.math.cross(
                                       positions[(i-1) % n] - positions[(i+1) % n],
                                       positions[i] / 2f - headDirection));
                vertices[2 * i] = positions[i] + lineWidth;
                vertices[2 * i + 1] = positions[i] - lineWidth;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                verts[i] = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            }

            mesh.vertices = verts;
            
            //triangles
            //double check tris length
            int[] tris = new int[(6) * n];
            
            for(int i = 0; i < n; i++)
            {
                //triangle 0
                tris[6*i] = (2*i) % (2*n);
                tris[6*i + 1] = ((2*i)+1) % (2*n);
                tris[6*i + 2] = (2*(i+1)) % (2*n);
    
                //triangle 1
                tris[6*i + 3] = ((2*i)+1)% (2*n);
                tris[6*i + 4] = (2*(i+1)) % (2*n);
                tris[6*i + 5] = (2*(i+1)+1) % (2*n);
            }

            mesh.triangles = tris;
            
            //normals
            UnityEngine.Vector3[] normals = mesh.normals;
            for (int i = 0; i < verts.Length; i++)
                normals[i] = Unity.Mathematics.math.normalize(headDirection);
            
            mesh.normals = normals;
            
            //uvs
            UnityEngine.Vector2[] uvs = new Vector2[n];

            for (int i = 0; i < n; i++)
            {
                //even indices
                uvs[2 * i] = new Vector2(i / ( n - 1f), 0f);
                    
                //odd indices
                uvs[2 * i + 1] = new Vector2(i / ( n - 1f), 1f);
            }
            
            mesh.uv = uvs;

        }

        public static void Conic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void ConicPart( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void CurveCartesian( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Function( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void FunctionND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Interval( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Locus( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void LocusND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Number( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Plane(float3 origin, float3 direction)
        {
            
        }

        public static void Poly( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void PolyLine( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Render a polygon from an input of an array of points
        /// Function is passed points in clockwise connection of dots
        /// </summary>
        /// <param name="pointFloat3Array"></param>
        /// <returns></returns>
        public static void Polygon(float3[] pointFloat3Array)
        {

        }

        public static void Ray( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }
        
        public static void Sphere(float3 center, float3 edgePoint)
        {
            const int n = 500;
            
            Mesh mesh = new Mesh();

            float radius = IMRE.Math.Operations.magnitude(edgePoint - center);
            
            int nbLong = n;
            int nbLat = n;

            #region Vertices

            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[((nbLong + 1) * nbLat) + 2];
            float pi = UnityEngine.Mathf.PI;
            float _2pi = pi * 2f;

            vertices[0] = UnityEngine.Vector3.up * radius;
            for (int lat = 0; lat < nbLat; lat++)
            {
                float a1 = (pi * (lat + 1)) / (nbLat + 1);
                float sin1 = UnityEngine.Mathf.Sin(a1);
                float cos1 = UnityEngine.Mathf.Cos(a1);

                for (int lon = 0; lon <= nbLong; lon++)
                {
                    float a2 = (_2pi * (lon == nbLong ? 0 : lon)) / nbLong;
                    float sin2 = UnityEngine.Mathf.Sin(a2);
                    float cos2 = UnityEngine.Mathf.Cos(a2);

                    vertices[lon + (lat * (nbLong + 1)) + 1] =
                        new UnityEngine.Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }

            vertices[vertices.Length - 1] = UnityEngine.Vector3.up * - radius;

            mesh.vertices = vertices;

            #endregion

            #region Normals		

            UnityEngine.Vector3[] normals = new UnityEngine.Vector3[vertices.Length];
            for (int j = 0; j < vertices.Length; j++)
                normals[j] = vertices[j].normalized;

            mesh.normals = normals;

            #endregion

            #region UVs

            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[vertices.Length];
            uvs[0] = UnityEngine.Vector2.up;
            uvs[uvs.Length - 1] = UnityEngine.Vector2.zero;
            for (int lat = 0; lat < nbLat; lat++)
            for (int lon = 0; lon <= nbLong; lon++)
            {
                uvs[lon + (lat * (nbLong + 1)) + 1] =
                    new UnityEngine.Vector2((float) lon / nbLong, 1f - ((float) (lat + 1) / (nbLat + 1)));
            }

            mesh.uv = uvs;

            #endregion

            #region Triangles

            int nbFaces = vertices.Length;
            int nbTriangles = nbFaces * 2;
            int nbIndexes = nbTriangles * 3;
            int[] triangles = new int[nbIndexes];

            //Top Cap
            int i = 0;
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = lon + 2;
                triangles[i++] = lon + 1;
                triangles[i++] = 0;
            }

            //Middle
            for (int lat = 0; lat < (nbLat - 1); lat++)
            {
                for (int lon = 0; lon < nbLong; lon++)
                {
                    int current = lon + (lat * (nbLong + 1)) + 1;
                    int next = current + nbLong + 1;

                    triangles[i++] = current;
                    triangles[i++] = current + 1;
                    triangles[i++] = next + 1;

                    triangles[i++] = current;
                    triangles[i++] = next + 1;
                    triangles[i++] = next;
                }
            }

            //Bottom Cap
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = vertices.Length - 1;
                triangles[i++] = vertices.Length - (lon + 2) - 1;
                triangles[i++] = vertices.Length - (lon + 1) - 1;
            }

            mesh.triangles = triangles;

            #endregion
        }
        
        public static void SurfaceFinite( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Symbolic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Vector2D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Vector3D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Text( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Turtle( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Video( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Image( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static void Slider( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

    }
}