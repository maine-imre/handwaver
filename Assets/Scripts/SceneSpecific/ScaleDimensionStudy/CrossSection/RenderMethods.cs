namespace IMRE.HandWaver.ScaleDimension
{
    public static class RenderMethods
    {
        /// <summary>
        /// method for rendering a sphere
        /// by taking a centerpoint, radius, mesh, and n vertices
        /// </summary>
        /// <param name="crossSectionRadius"></param>
        /// <param name="center"></param>
        /// <param name="crossSectionRenderer"></param>
        /// <param name="n"></param>
        public static void RenderSphere(float crossSectionRadius, Unity.Mathematics.float3 center,
            UnityEngine.Mesh crossSectionRenderer, int n)
        {
            crossSectionRenderer.Clear();
            int nbLong = n;
            int nbLat = n;

            #region Vertices

            UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[((nbLong + 1) * nbLat) + 2];
            float pi = UnityEngine.Mathf.PI;
            float _2pi = pi * 2f;

            vertices[0] = UnityEngine.Vector3.up * crossSectionRadius;
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
                        new UnityEngine.Vector3(sin1 * cos2, cos1, sin1 * sin2) * crossSectionRadius;
                }
            }

            vertices[vertices.Length - 1] = UnityEngine.Vector3.up * -crossSectionRadius;

            #endregion

            #region Normales		

            UnityEngine.Vector3[] normales = new UnityEngine.Vector3[vertices.Length];
            for (int j = 0; j < vertices.Length; j++)
                normales[j] = vertices[j].normalized;

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

            #endregion

            crossSectionRenderer.vertices = vertices;
            crossSectionRenderer.normals = normales;
            crossSectionRenderer.uv = uvs;
            crossSectionRenderer.triangles = triangles;

            crossSectionRenderer.RecalculateBounds();
        }


        /// <summary>
        /// Finds the vertex indices for the kth triangle
        /// returns the mth vertex index of the kth triangle
        /// Triangles indexed for clockwise front face
        /// </summary>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int ToricTriangle(int k, int m, int n)
        {
            if (k < (n * n))
            {
                //lower half triangle
                switch (m)
                {
                    case 0:
                        return k;
                    case 2:
                        return (UnityEngine.Mathf.FloorToInt(k / n) * n) + ((k + 1) % n);
                    case 1:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + ((k + 1) % n);
                }
            }
            else
            {
                k = k - (n * n);

                switch (m)
                {
                    case 0:
                        return k;
                    case 2:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + ((k + 1) % n);
                    case 1:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + (k % n);
                }
            }

            UnityEngine.Debug.LogError("Invalid parameter.");

            return 0;
        }
    }
}