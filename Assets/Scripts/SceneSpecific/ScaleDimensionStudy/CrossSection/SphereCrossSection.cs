using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class SphereCrossSection : MonoBehaviour, ISliderInput
    {
        #region variables/components

        public int n;
        private Mesh sphereRenderer => GetComponent<MeshFilter>().mesh;
        private LineRenderer crossSectionRenderer => transform.GetChild(0).GetComponent<LineRenderer>();

        public Material sphereMaterial;
        public Material crossSectionMaterial;

        public float radius = 1f;
        public Vector3 center = Vector3.zero;
        public Vector3 normal = Vector3.up;

        public bool debugRenderer = SpencerStudyControl.debugRendererXC;
        #endregion

        // Start is called before the first frame update
        void Start()
        {

            #region Render Cross-section

            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshRenderer>().material = sphereMaterial;
            gameObject.GetComponent<MeshRenderer>().enabled = debugRenderer;
            renderSphere();   
            
            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.transform.localPosition = Vector3.zero;
            child.AddComponent<LineRenderer>();
            crossSectionRenderer.material = crossSectionMaterial;
            crossSectionRenderer.useWorldSpace = false;

            crossSectionRenderer.startWidth = SpencerStudyControl.lineRendererWidth;
            crossSectionRenderer.endWidth = SpencerStudyControl.lineRendererWidth;
            crossSectionRenderer.loop = true;

            #endregion
        }

        public float slider
        {
            //scale value from 0 to 1 range to -1 to 1 range.
            set => crossSectSphere(-1+value*2);
        }

        /// <summary>
        /// Function to calculate cross section of circle
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public void crossSectSphere(float height)
        {
            //endpoints for line segment if intersection passes through circle

            //if cross section only hits the edge of the circle
            if (Math.Abs(height) == radius)
            {
                //if top of sphere, create point at intersection
                if (height == radius)
                {
                    Vector3 segmentEndPoint0 = Vector3.up * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.positionCount = 2;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);
                    
                }

                //if bottom of circle, create point at intersection
                else if (height == -radius)
                {
                    Vector3 segmentEndPoint0 = Vector3.down * radius;
                    crossSectionRenderer.enabled = true;
                    crossSectionRenderer.positionCount = 2;
                    crossSectionRenderer.SetPosition(0, segmentEndPoint0);
                    crossSectionRenderer.SetPosition(1, segmentEndPoint0);
                }

            }

            //cross section is a circle
            else if (Math.Abs(height) < radius)
            {
                //horizontal distance from center of circle to point on line segment
               
                renderCircle(Mathf.Sqrt(Mathf.Pow(radius,2) - Mathf.Pow(height, 2)), height*Vector3.up);
            }

            //height for cross section is outside of circle 
            else if (Math.Abs(height) > radius)
            {
                Debug.Log("Height is out of range of object.");
                crossSectionRenderer.enabled = false;
            }

        }

        private void renderSphere()
        {
            sphereRenderer.Clear();
            int nbLong = n;
            int nbLat = n;

            #region Vertices
            Vector3[] vertices = new Vector3[(nbLong + 1) * nbLat + 2];
            float pi = Mathf.PI;
            float _2pi = pi * 2f;

            vertices[0] = Vector3.up * radius;
            for (int lat = 0; lat < nbLat; lat++)
            {
                float a1 = pi * (float)(lat + 1) / (nbLat + 1);
                float sin1 = Mathf.Sin(a1);
                float cos1 = Mathf.Cos(a1);

                for (int lon = 0; lon <= nbLong; lon++)
                {
                    float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                    float sin2 = Mathf.Sin(a2);
                    float cos2 = Mathf.Cos(a2);

                    vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }
            vertices[vertices.Length - 1] = Vector3.up * -radius;
            #endregion

            #region Normales		
            Vector3[] normales = new Vector3[vertices.Length];
            for (int n = 0; n < vertices.Length; n++)
                normales[n] = vertices[n].normalized;
            #endregion

            #region UVs
            Vector2[] uvs = new Vector2[vertices.Length];
            uvs[0] = Vector2.up;
            uvs[uvs.Length - 1] = Vector2.zero;
            for (int lat = 0; lat < nbLat; lat++)
                for (int lon = 0; lon <= nbLong; lon++)
                    uvs[lon + lat * (nbLong + 1) + 1] = new Vector2((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1));
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
            {
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
            }

            //Bottom Cap
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = vertices.Length - 1;
                triangles[i++] = vertices.Length - (lon + 2) - 1;
                triangles[i++] = vertices.Length - (lon + 1) - 1;
            }
            #endregion

            sphereRenderer.vertices = vertices;
            sphereRenderer.normals = normales;
            sphereRenderer.uv = uvs;
            sphereRenderer.triangles = triangles;

            sphereRenderer.RecalculateBounds();
        }

        private void renderCircle (float radius, Vector3 center)
        {
            //worldspace rendering of the circle
            
            //normal vectors
            Vector3 norm1 = Vector3.forward;
            Vector3 norm2 = Vector3.right;

            //array of vector3s for vertices
            Vector3[] vertices = new Vector3[n];

            //math for rendering circle
            for (int i = 0; i < n; i++)
            {
                vertices[i] = radius * ((Mathf.Sin((i * Mathf.PI * 2 / (n - 1))) * norm1) + (Mathf.Cos((i * Mathf.PI * 2 / (n - 1))) * norm2)) + center;
                
            }

            //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            crossSectionRenderer.positionCount = n;
            crossSectionRenderer.SetPositions(vertices);
        }

    }

}