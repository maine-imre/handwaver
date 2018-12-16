using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.HigherDimensions
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

/// <summary>
/// Higher dimensional solids and their projections automated.
/// Now uses sliers for scale and dimension study, via a dictionary.
/// </summary>
	public abstract class AbstractHigherDimSolid : MonoBehaviour
    {
        internal bool firstDraw = true;
        internal abstract void drawFigure();
        internal List<Vector4> origionalVertices;

        internal MeshFilter meshFilter;
        internal Mesh mesh;

        //public Transform threeAxisRotate;
        //public Transform fourthDimRotate;

        public List<Vector4> originalVerts;
        public List<Vector4> rotatedVerts;

        internal List<Vector3> originalTris;

        internal List<Vector3> verts;
        internal List<int> tris;
        internal List<Vector2> uvs;

        internal List<Axis4D> rotationOrder;
        public Dictionary<Axis4D, float> rotation;

        void Start()
        {
            rotationOrder = new List<Axis4D>();
            rotationOrder.Add(Axis4D.yz);
            rotationOrder.Add(Axis4D.xw);
            rotationOrder.Add(Axis4D.yw);
            rotationOrder.Add(Axis4D.zw);
            rotationOrder.Add(Axis4D.xy);
            rotationOrder.Add(Axis4D.xz);

            rotation = new Dictionary<Axis4D, float>();
            rotation.Add(Axis4D.xy, 0f);
            rotation.Add(Axis4D.xz, 0f);
            rotation.Add(Axis4D.xw, 0f);
            rotation.Add(Axis4D.yz, 0f);
            rotation.Add(Axis4D.yw, 0f);
            rotation.Add(Axis4D.zw, 0f);


            meshFilter = GetComponent<MeshFilter>();
            mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                meshFilter.mesh = new Mesh();
                mesh = meshFilter.sharedMesh;
            }
            //Center and normalize vertices.

            //Vector4 meanVert = Vector4.zero;

            //foreach (Vector4 vert in originalVerts)
            //{
            //    meanVert = meanVert + (vert / originalVerts.Count);
            //}

            //for (int idx = 0; idx < originalVerts.Count; idx++)
            //{
            //    originalVerts[idx] = originalVerts[idx] - meanVert;
            //    originalVerts[idx] = Vector4.Normalize(originalVerts[idx]);
            //}

            ResetVertices();
        }

        Vector3 cameraPos = new Vector3(0, 0, -6);

        //[Range(0,360)]
        //public float xw;
        //[Range(0, 360)]
        //public float yw;
        //[Range(0, 360)]
        //public float zw;

        void Update()
        {
            updateRotate();
            drawFigure();
            //if (firstDraw)
            //{
            //    firstDraw = false;
            //}
        }

        void updateRotate()
        {
            if (GetComponent<Leap.Unity.Interaction.InteractionBehaviour>() != null)
            {
                rotation[Axis4D.xy] = this.transform.rotation.eulerAngles.z;
                rotation[Axis4D.xz] = this.transform.rotation.eulerAngles.y;
                rotation[Axis4D.yz] = this.transform.rotation.eulerAngles.x;
            }
            else
            {
                rotation[Axis4D.xy] = HigherDimControl.xy;
                rotation[Axis4D.xz] = HigherDimControl.xz;
                rotation[Axis4D.yz] = HigherDimControl.yz;
            }

            rotation[Axis4D.xw] = HigherDimControl.xw;
            rotation[Axis4D.yw] = HigherDimControl.yw;
            rotation[Axis4D.zw] = HigherDimControl.zw;

            ApplyRotationToVerts();
        }

        internal void ResetVertices()
        {
            rotatedVerts = new List<Vector4>();
            rotatedVerts.AddRange(originalVerts);
        }

        internal void ApplyRotationToVerts()
        {
            ResetVertices();

            foreach (Axis4D axis in rotationOrder)
            {
                for (int i = 0; i < rotatedVerts.Count; i++)
                {
                    rotatedVerts[i] = rotatedVerts[i].GetRotatedVertex(axis, rotation[axis]);
                }
            }
        }

        internal void CreatePlane(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            Vector2 uv0 = Vector2.zero;
            Vector2 uv1 = Vector2.zero;
            Vector2 uv2 = Vector2.zero;


            List<Vector3> newVerts = new List<Vector3>(){
            p1,p3,p2,
            p1,p2,p4,
            p2,p3,p4,
            p1,p4,p3
        };

            verts.AddRange(newVerts);
            mesh.vertices = verts.ToArray();

            if (firstDraw)
            {
                int t = tris.Count;
                for (int j = 0; j < 12; j++)
                {
                    tris.Add(j + t);
                }

                mesh.SetTriangles(tris.ToArray(), 0);


                uv0 = new Vector2(0, 0);
                uv1 = new Vector2(1, 0);
                uv2 = new Vector2(0.5f, 1);

                uvs.AddRange(
                    new List<Vector2>(){
                uv0,uv1,uv2,
                uv0,uv1,uv2,
                uv0,uv1,uv2,
                uv0,uv1,uv2
                    }
                );
                mesh.uv = uvs.ToArray();
            }

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        internal void CreatePlane(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector2 uv0 = Vector2.zero;
            Vector2 uv1 = Vector2.zero;
            Vector2 uv2 = Vector2.zero;


            List<Vector3> newVerts = new List<Vector3>(){
            p1,p3,p2
        };

            verts.AddRange(newVerts);
            mesh.vertices = verts.ToArray();
            if (firstDraw)
            {
                int t = tris.Count;
                for (int j = 0; j < 3; j++)
                {
                    tris.Add(j + t);
                }

                mesh.SetTriangles(tris.ToArray(), 0);


                uv0 = new Vector2(0, 0);
                uv1 = new Vector2(1, 0);
                uv2 = new Vector2(0.5f, 1);

                uvs.AddRange(
                    new List<Vector2>(){
                uv0,uv1,uv2,
                    }
                );
                mesh.uv = uvs.ToArray();
            }
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

    }
}