using Operations = IMRE.Math.Operations;

namespace IMRE.HandWaver.Rendering
{
    /// <summary>
    ///     Contains functional code that would render out a given element based on passed in data.
    /// </summary>
    public static class GeoElementRenderLib
    {
        private const float Width = .0025f;
        private const float VertexCount = 3000f;
        private const float AngleRadius = 0.5f;

        private static Unity.Mathematics.float3 HeadPosition => UnityEngine.Camera.main.transform.position;

        private static Unity.Mathematics.float3 HeadDirection => Unity.Mathematics.float3.zero;

        // TODO: Group by element type.
        //    -- Point
        //    -- Path
        //    -- Surface
        //    -- Region
        //    -- NonGeometeric
        // TODO: Determine input vars.
        // TODO: Implement functionality for base version of HW Rendering.

        /// <summary>
        ///     Function that renders a line based on a point and direction
        ///     Currently being rendered as a very long segment-revise this
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        public static UnityEngine.Mesh Line(Unity.Mathematics.float3 point, Unity.Mathematics.float3 direction)
        {
            direction = Unity.Mathematics.math.normalize(direction);
            return Segment(point + 100f * direction, point - 100f * direction);
        }

        /// <summary>
        ///     Function that renders a point at a given location
        ///     Currently being rendered as a very tiny line segment-revise this
        /// </summary>
        /// <param name="location"></param>
        public static UnityEngine.Mesh Point(Unity.Mathematics.float3 location)
        {
            //TODO a better visualization of a point
            Unity.Mathematics.float3 dir = Width / 2f * new Unity.Mathematics.float3(1, 1, 1);
            return Segment(location - dir, location + dir);
        }

        /// <summary>
        ///     Function to render a line segment based on two endpoints
        ///     Using the two endpoints, calculate corners of the segment with the location as endpointA/B +/- lineWidth
        /// </summary>
        /// <param name="endpointA"></param>
        /// <param name="endpointB"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh Segment(Unity.Mathematics.float3 endpointA, Unity.Mathematics.float3 endpointB)
        {
            //TODO: check why this does not work with endpointA at the origin
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            Unity.Mathematics.float3 lineWidth = Unity.Mathematics.math.cross(
                endpointA - endpointB,
                (endpointA + endpointB) / 2f - HeadDirection);

            //If segment is not visible, debug out a warning as to why and return empty mesh
            if (Operations.Magnitude(lineWidth) == 0f)
            {
                UnityEngine.Debug.LogWarning("Line Segment in Direction of Vision");
                lineWidth = new Unity.Mathematics.float3(1f,0f,0f);
            }

            lineWidth = Width * Unity.Mathematics.math.normalize(lineWidth);

            //verts
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[4];
            verts[0] = endpointA + lineWidth;
            verts[1] = endpointA - lineWidth;
            verts[2] = endpointB - lineWidth;
            verts[3] = endpointB + lineWidth;

            foreach (UnityEngine.Vector3 vector3 in verts) UnityEngine.Debug.Log(vector3);

            mesh.vertices = verts;

            //triangles-counterclockwise
            int[] tris = {1, 3, 0, 1, 2, 3};
            mesh.triangles = tris;

            //uvs
            UnityEngine.Vector2[] uvs =
            {
                new UnityEngine.Vector2(0, 1), new UnityEngine.Vector2(0, 0),
                new UnityEngine.Vector2(1, 0), new UnityEngine.Vector2(1, 1)
            };
            mesh.uv = uvs;

            mesh.RecalculateNormals();

            //normals
            UnityEngine.Vector3[] normals = mesh.normals;
            for (int i = 0; i < verts.Length; i++)
                normals[i] = Unity.Mathematics.math.normalize(HeadDirection);
            mesh.normals = normals;

            return mesh;
        }

        public static UnityEngine.Mesh Angle(UnityEngine.Vector3 center, UnityEngine.Vector3 segmentStart,
            UnityEngine.Vector3 segmentAEndpoint,
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
            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[(int) VertexCount];

            //math for rendering circle
            for (int i = 0; i < VertexCount; i++)
            {
                vertices[i] = AngleRadius *
                              (UnityEngine.Mathf.Sin(i * UnityEngine.Vector3.Angle(segmentStart - segmentAEndpoint,
                                                         segmentStart - segmentBEndpoint) / (VertexCount - 1)) * norm1 +
                               UnityEngine.Mathf.Cos(i * UnityEngine.Vector3.Angle(segmentStart - segmentAEndpoint,
                                                         segmentStart - segmentBEndpoint) / (VertexCount - 1)) *
                               norm2) +
                              center;
            }

            semiCircleMesh.vertices = vertices;
            //semiCircleMesh.SetPositions(vertices);

            #endregion

            return semiCircleMesh;
        }

        public static UnityEngine.Mesh Axis( /*Data needed to render*/)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Function that renders a circle based on centerpoint, point on the edge, and normal direction
        /// </summary>
        /// <param name="center"></param>
        /// <param name="edgePoint"></param>
        /// <param name="normalDirection"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh Circle(Unity.Mathematics.float3 center, Unity.Mathematics.float3 edgePoint,
            Unity.Mathematics.float3 normalDirection)
        {
            const int n = 50;

            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            //normal vectors
            Unity.Mathematics.float3 basis1 = Unity.Mathematics.math.normalize(edgePoint - center);
            Unity.Mathematics.float3 basis2 = Unity.Mathematics.math.cross(basis1, normalDirection);
            float radius = Operations.Magnitude((edgePoint - center));

            //array of vector3s for vertices
            Unity.Mathematics.float3[] positions = new Unity.Mathematics.float3[n];
            Unity.Mathematics.float3[] vertices = new Unity.Mathematics.float3[2 * n];
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[vertices.Length];

            //math for rendering circle
            for (int i = 0; i < n; i++)
            {
                positions[i] = radius * (UnityEngine.Mathf.Sin(i * UnityEngine.Mathf.PI * 2 / (n - 1)) * basis1 +
                                         UnityEngine.Mathf.Cos(i * UnityEngine.Mathf.PI * 2 / (n - 1)) * basis2);
            }

            for (int i = 0; i < n; i++)
            {
                Unity.Mathematics.float3 lineWidth = Width * Unity.Mathematics.math.normalize(
                                                         Unity.Mathematics.math.cross(
                                                             positions[(i - 1 + n) % n] - positions[(i + 1) % n],
                                                             positions[i] / 2f - HeadDirection));
                vertices[2 * i] = positions[i] + lineWidth;
                vertices[2 * i + 1] = positions[i] - lineWidth;
            }

            for (int i = 0; i < vertices.Length; i++)
                verts[i] = new UnityEngine.Vector3(vertices[i].x, vertices[i].y, vertices[i].z);

            mesh.vertices = verts;

            //triangles
            //double check tris length
            int[] tris = new int[6 * n];

            for (int i = 0; i < n; i++)
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
            UnityEngine.Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = Unity.Mathematics.math.normalize(HeadDirection);

            mesh.normals = normals;

            //uvs
            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[2 * n];

            for (int i = 0; i < n; i++)
            {
                //even indices
                uvs[2 * i] = new UnityEngine.Vector2(i / (n - 1f), 0f);

                //odd indices
                uvs[2 * i + 1] = new UnityEngine.Vector2(i / (n - 1f), 1f);
            }

            mesh.uv = uvs;

            return mesh;
        }

        /// <summary>
        ///     Function that renders a plane (very large rectangle) based on an origin and direction
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh Plane(Unity.Mathematics.float3 origin, Unity.Mathematics.float3 direction)
        {
            const float planeSize = 5f;

            //check if normal direction causes error
            if (Operations.Magnitude(direction) == 0f)
            {
                UnityEngine.Debug.LogError("Normal Direction Must Be Non Zero");
                direction = new Unity.Mathematics.float3(1f,0f,0f);            
            }

            direction = Unity.Mathematics.math.normalize(direction);
            Unity.Mathematics.float3 basis0 =
                Operations.Magnitude(Unity.Mathematics.math.cross(direction,
                    new Unity.Mathematics.float3(1f, 0f, 0f))) == 0f
                    ? Unity.Mathematics.math.cross(direction, new Unity.Mathematics.float3(0f, 1f, 0f))
                    : Unity.Mathematics.math.cross(
                        direction, new Unity.Mathematics.float3(1f, 0f, 0f));
            basis0 = Unity.Mathematics.math.normalize(basis0);
            Unity.Mathematics.float3 basis1 = Unity.Mathematics.math.cross(direction, basis0);
            basis1 = Unity.Mathematics.math.normalize(basis1);

            UnityEngine.Debug.Log(Operations.Magnitude(basis0) + " : " + Operations.Magnitude(basis1));


            //TODO: check why this does not work with endpointA at the origin
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            //verts
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[4];
            verts[0] = origin + (basis0 + basis1) * planeSize;
            verts[1] = origin + (basis0 - basis1) * planeSize;
            verts[2] = origin + (-basis0 - basis1) * planeSize;
            verts[3] = origin + (-basis0 + basis1) * planeSize;

            mesh.vertices = verts;

            //triangles-counterclockwise
            int[] tris = {1, 3, 0, 1, 2, 3};
            mesh.triangles = tris;

            //uvs
            UnityEngine.Vector2[] uvs =
            {
                new UnityEngine.Vector2(0, 1), new UnityEngine.Vector2(0, 0),
                new UnityEngine.Vector2(1, 0), new UnityEngine.Vector2(1, 1)
            };
            mesh.uv = uvs;

            mesh.RecalculateNormals();

            //normals
            /*var normals = mesh.normals;
            for (var i = 0; i < verts.Length; i++)
                normals[i] = math.normalize(HeadDirection);
            mesh.normals = normals;*/

            return mesh;
        }

        /// <summary>
        /// </summary>
        /// <param name="pointFloat3Array"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh Polygon(Unity.Mathematics.float3[] pointFloat3Array)
        {
            throw new System.NotImplementedException();
        }

        public static UnityEngine.Mesh Ray( /*Data needed to render*/)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Function that renders a sphere based on a centerpoint and a point on the adge
        /// </summary>
        /// <param name="center"></param>
        /// <param name="edgePoint"></param>
        /// <returns></returns>
        public static UnityEngine.Mesh Sphere(Unity.Mathematics.float3 center, Unity.Mathematics.float3 edgePoint)
        {
            const int n = 50;

            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            float radius = Operations.Magnitude((edgePoint - center));

            int nbLong = n;
            int nbLat = n;

            #region Vertices

            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[(nbLong + 1) * nbLat + 2];
            float pi = UnityEngine.Mathf.PI;
            float _2pi = pi * 2f;

            vertices[0] = UnityEngine.Vector3.up * radius;
            for (int lat = 0; lat < nbLat; lat++)
            {
                float a1 = pi * (lat + 1) / (nbLat + 1);
                float sin1 = UnityEngine.Mathf.Sin(a1);
                float cos1 = UnityEngine.Mathf.Cos(a1);

                for (int lon = 0; lon <= nbLong; lon++)
                {
                    float a2 = _2pi * (lon == nbLong ? 0 : lon) / nbLong;
                    float sin2 = UnityEngine.Mathf.Sin(a2);
                    float cos2 = UnityEngine.Mathf.Cos(a2);

                    vertices[lon + lat * (nbLong + 1) + 1] =
                        new UnityEngine.Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }

            vertices[vertices.Length - 1] = UnityEngine.Vector3.up * -radius;

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
                uvs[lon + lat * (nbLong + 1) + 1] =
                    new UnityEngine.Vector2((float) lon / nbLong, 1f - (float) (lat + 1) / (nbLat + 1));
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
            for (int lat = 0; lat < nbLat - 1; lat++)
            for (int lon = 0; lon < nbLong; lon++)
            {
                int current = lon + lat * (nbLong + 1) + 1;
                int next = current + nbLong + 1;

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
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

            return mesh;
        }
    }
}