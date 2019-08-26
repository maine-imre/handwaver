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

        private static float3 headPosition => Camera.main.transform.position;

        private static float3 headDirection => float3.zero;

        // TODO: Group by element type.
        //    -- Point
        //    -- Path
        //    -- Surface
        //    -- Region
        //    -- NonGeometeric
        // TODO: Determine input vars.
        // TODO: Implement functionality for base version of HW Rendering.

        /// <summary>
        /// Function that renders a line based on a point and direction
        /// Currently being rendered as a very long segment-revise this
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        public static Mesh Line(float3 point, float3 direction)
        {
            direction = math.normalize(direction);
            return Segment(point + 100f * direction, point - 100f * direction);
        }

        /// <summary>
        /// Function that renders a point at a given location
        /// Currently being rendered as a very tiny line segment-revise this
        /// </summary>
        /// <param name="location"></param>
        public static Mesh Point(float3 location)
        {
            //TODO a better visualization of a point
            var dir = width / 2f * new float3(1, 1, 1);
            return Segment(location - dir, location + dir);
        }

        /// <summary>
        /// Function to render a line segment based on two endpoints
        /// Using the two endpoints, calculate corners of the segment with the location as endpointA/B +/- lineWidth
        /// </summary>
        /// <param name="endpointA"></param>
        /// <param name="endpointB"></param>
        /// <returns></returns>
        public static Mesh Segment(float3 endpointA, float3 endpointB)
        {
            //TODO: check why this does not work with endpointA at the origin
            var mesh = new Mesh();

            var lineWidth = math.cross(
                endpointA - endpointB,
                (endpointA + endpointB) / 2f - headDirection);

            //If segment is not visible, debug out a warning as to why and return empty mesh
            if (lineWidth.Magnitude() == 0f)
            {
                Debug.LogWarning("Line Segment in Direction of Vision");
                return new Mesh();
            }

            lineWidth = width * math.normalize(lineWidth);

            //verts
            var verts = new Vector3[4];
            verts[0] = endpointA + lineWidth;
            verts[1] = endpointA - lineWidth;
            verts[2] = endpointB - lineWidth;
            verts[3] = endpointB + lineWidth;

            foreach (var vector3 in verts) Debug.Log(vector3);

            mesh.vertices = verts;

            //triangles-counterclockwise
            int[] tris = {1, 3, 0, 1, 2, 3};
            mesh.triangles = tris;

            //uvs
            Vector2[] uvs =
            {
                new Vector2(0, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1)
            };
            mesh.uv = uvs;

            mesh.RecalculateNormals();

            //normals
            var normals = mesh.normals;
            for (var i = 0; i < verts.Length; i++)
                normals[i] = math.normalize(headDirection);
            mesh.normals = normals;

            return mesh;
        }

        public static Mesh Angle(Vector3 center, Vector3 segmentStart, Vector3 segmentAEndpoint,
            Vector3 segmentBEndpoint)
        {
            Segment(segmentStart, segmentAEndpoint);
            Segment(segmentStart, segmentBEndpoint);

            #region Render Circle

            var semiCircleMesh = new Mesh();

            //semicircle for angle
            //normal vectors
            var norm1 = Vector3.forward;
            var norm2 = Vector3.right;

            //array of vector3s for vertices
            var vertices = new Vector3[(int) vertexCount];

            //math for rendering circle
            for (var i = 0; i < vertexCount; i++)
                vertices[i] = angleRadius *
                              (Mathf.Sin(i * Vector3.Angle(segmentStart - segmentAEndpoint,
                                             segmentStart - segmentBEndpoint) / (vertexCount - 1)) * norm1 +
                               Mathf.Cos(i * Vector3.Angle(segmentStart - segmentAEndpoint,
                                             segmentStart - segmentBEndpoint) / (vertexCount - 1)) * norm2) +
                              center;

            semiCircleMesh.vertices = vertices;
            //semiCircleMesh.SetPositions(vertices);
            #endregion
            return semiCircleMesh;

        }

        public static Mesh Axis( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function that renders a circle based on centerpoint, point on the edge, and normal direction 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="edgePoint"></param>
        /// <param name="normalDirection"></param>
        /// <returns></returns>
        public static Mesh Circle(float3 center, float3 edgePoint, float3 normalDirection)
        {
            const int n = 50;

            var mesh = new Mesh();

            //normal vectors
            var basis1 = math.normalize(edgePoint - center);
            var basis2 = math.cross(basis1, normalDirection);
            var radius = (edgePoint - center).Magnitude();

            //array of vector3s for vertices
            var positions = new float3[n];
            var vertices = new float3[2 * n];
            var verts = new Vector3[vertices.Length];

            //math for rendering circle
            for (var i = 0; i < n; i++)
                positions[i] = radius * (Mathf.Sin(i * Mathf.PI * 2 / (n - 1)) * basis1 +
                                         Mathf.Cos(i * Mathf.PI * 2 / (n - 1)) * basis2);

            for (var i = 0; i < n; i++)
            {
                var lineWidth = width * math.normalize(math.cross(
                                    positions[(i - 1 + n) % n] - positions[(i + 1) % n],
                                    positions[i] / 2f - headDirection));
                vertices[2 * i] = positions[i] + lineWidth;
                vertices[2 * i + 1] = positions[i] - lineWidth;
            }

            for (var i = 0; i < vertices.Length; i++)
                verts[i] = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);

            mesh.vertices = verts;

            //triangles
            //double check tris length
            var tris = new int[6 * n];

            for (var i = 0; i < n; i++)
            {
                //triangle 0
                tris[6 * i] = 2 * i % (2 * n);
                tris[6 * i + 1] = (2 * i + 1) % (2 * n);
                tris[6 * i + 2] = 2 * (i + 1) % (2 * n);

                //triangle 1
                tris[6 * i + 3] = (2 * i + 1) % (2 * n);
                tris[6 * i + 4] = 2 * (i + 1) % (2 * n);
                tris[6 * i + 5] = (2 * (i + 1) + 1) % (2 * n);
            }

            mesh.triangles = tris;

            //normals
            var normals = mesh.normals;
            for (var i = 0; i < normals.Length; i++)
                normals[i] = math.normalize(headDirection);

            mesh.normals = normals;

            //uvs
            var uvs = new Vector2[2 * n];

            for (var i = 0; i < n; i++)
            {
                //even indices
                uvs[2 * i] = new Vector2(i / (n - 1f), 0f);

                //odd indices
                uvs[2 * i + 1] = new Vector2(i / (n - 1f), 1f);
            }

            mesh.uv = uvs;

            return mesh;
        }

        public static Mesh Conic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh ConicPart( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh CurveCartesian( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Function( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh FunctionND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Interval( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Locus( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh LocusND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Number( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function that renders a plane (very large rectangle) based on an origin and direction
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Mesh Plane(float3 origin, float3 direction)
        {
            const float planeSize = 5f;

            //check if normal direction causes error
            if (direction.Magnitude() == 0f)
            {
                Debug.LogError("Normal Direction Must Be Non Zero");
                return new Mesh();
            }

            direction = math.normalize(direction);
            var basis0 =
                math.cross(direction, new float3(1f, 0f, 0f)).Magnitude() == 0f
                    ? math.cross(direction, new float3(0f, 1f, 0f))
                    : math.cross(
                        direction, new float3(1f, 0f, 0f));
            basis0 = math.normalize(basis0);
            var basis1 = math.cross(direction, basis0);
            basis1 = math.normalize(basis1);

            Debug.Log(basis0.Magnitude() + " : " + basis1.Magnitude());


            //TODO: check why this does not work with endpointA at the origin
            var mesh = new Mesh();

            //verts
            var verts = new Vector3[4];
            verts[0] = origin + (basis0 + basis1) * planeSize;
            verts[1] = origin + (basis0 - basis1) * planeSize;
            verts[2] = origin + (-basis0 - basis1) * planeSize;
            verts[3] = origin + (-basis0 + basis1) * planeSize;

            mesh.vertices = verts;

            //triangles-counterclockwise
            int[] tris = {1, 3, 0, 1, 2, 3};
            mesh.triangles = tris;

            //uvs
            Vector2[] uvs =
            {
                new Vector2(0, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1)
            };
            mesh.uv = uvs;

            mesh.RecalculateNormals();

            //normals
            var normals = mesh.normals;
            for (var i = 0; i < verts.Length; i++)
                normals[i] = math.normalize(headDirection);
            mesh.normals = normals;

            return mesh;
        }

        public static Mesh Poly( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh PolyLine( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="pointFloat3Array"></param>
        /// <returns></returns>
        public static Mesh Polygon(float3[] pointFloat3Array)
        {
        }

        public static Mesh Ray( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function that renders a sphere based on a centerpoint and a point on the adge
        /// </summary>
        /// <param name="center"></param>
        /// <param name="edgePoint"></param>
        /// <returns></returns>
        public static Mesh Sphere(float3 center, float3 edgePoint)
        {
            const int n = 50;

            var mesh = new Mesh();

            var radius = (edgePoint - center).Magnitude();

            var nbLong = n;
            var nbLat = n;

            #region Vertices

            var vertices = new Vector3[(nbLong + 1) * nbLat + 2];
            var pi = Mathf.PI;
            var _2pi = pi * 2f;

            vertices[0] = Vector3.up * radius;
            for (var lat = 0; lat < nbLat; lat++)
            {
                var a1 = pi * (lat + 1) / (nbLat + 1);
                var sin1 = Mathf.Sin(a1);
                var cos1 = Mathf.Cos(a1);

                for (var lon = 0; lon <= nbLong; lon++)
                {
                    var a2 = _2pi * (lon == nbLong ? 0 : lon) / nbLong;
                    var sin2 = Mathf.Sin(a2);
                    var cos2 = Mathf.Cos(a2);

                    vertices[lon + lat * (nbLong + 1) + 1] =
                        new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }

            vertices[vertices.Length - 1] = Vector3.up * -radius;

            mesh.vertices = vertices;

            #endregion

            #region Normals		

            var normals = new Vector3[vertices.Length];
            for (var j = 0; j < vertices.Length; j++)
                normals[j] = vertices[j].normalized;

            mesh.normals = normals;

            #endregion

            #region UVs

            var uvs = new Vector2[vertices.Length];
            uvs[0] = Vector2.up;
            uvs[uvs.Length - 1] = Vector2.zero;
            for (var lat = 0; lat < nbLat; lat++)
            for (var lon = 0; lon <= nbLong; lon++)
                uvs[lon + lat * (nbLong + 1) + 1] =
                    new Vector2((float) lon / nbLong, 1f - (float) (lat + 1) / (nbLat + 1));

            mesh.uv = uvs;

            #endregion

            #region Triangles

            var nbFaces = vertices.Length;
            var nbTriangles = nbFaces * 2;
            var nbIndexes = nbTriangles * 3;
            var triangles = new int[nbIndexes];

            //Top Cap
            var i = 0;
            for (var lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = lon + 2;
                triangles[i++] = lon + 1;
                triangles[i++] = 0;
            }

            //Middle
            for (var lat = 0; lat < nbLat - 1; lat++)
            for (var lon = 0; lon < nbLong; lon++)
            {
                var current = lon + lat * (nbLong + 1) + 1;
                var next = current + nbLong + 1;

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
            }

            //Bottom Cap
            for (var lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = vertices.Length - 1;
                triangles[i++] = vertices.Length - (lon + 2) - 1;
                triangles[i++] = vertices.Length - (lon + 1) - 1;
            }

            mesh.triangles = triangles;

            #endregion

            return mesh;
        }

        public static Mesh SurfaceFinite( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Symbolic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Vector2D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Vector3D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Text( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Turtle( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Video( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Image( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public static Mesh Slider( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }
    }
}