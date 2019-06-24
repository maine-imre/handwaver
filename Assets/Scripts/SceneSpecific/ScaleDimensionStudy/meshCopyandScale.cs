
using System;
using System.Linq;
using Photon.Pun.Demo.SlotRacer.Utils;
using UnityEngine;

    /// <summary>
    /// script for copying and scaling mesh and line segments
    /// so that the net folding of shapes can be observed on figures of different sizes
    /// </summary>
    public class meshCopyandScale : MonoBehaviour
    {        
        private MeshFilter childmeshFilt;
        private LineRenderer childlineRend;
        public Vector3 offset;
        public Material mat;
        private bool meshCopied, lineCopied;
        public float scaleFactor = 1f;

        private GameObject childGameObject;

        
        /// <summary>
        /// copies components from parent object
        /// </summary>
        public void Start()
        {
            childGameObject = new GameObject();

            childGameObject.transform.parent = this.transform;
            childGameObject.transform.localPosition = offset;
            childGameObject.transform.localScale = Vector3.one;
            childGameObject.transform.localRotation = Quaternion.identity;
            childlineRend = childGameObject.AddComponent<LineRenderer>();
            childmeshFilt = childGameObject.AddComponent<MeshFilter>();
            
            copyLine();
            copyMesh();

        }

        public void copyLine()
        {
            if (GetComponent<LineRenderer>() != null)
            {
                Vector3[] lineVerts = new Vector3[GetComponent<LineRenderer>().positionCount];

                GetComponent<LineRenderer>().GetPositions(lineVerts);
                childlineRend.SetPositions(lineVerts);
                childlineRend.positionCount = childGameObject.GetComponent<LineRenderer>().positionCount;
                childlineRend.startWidth = GetComponent<LineRenderer>().startWidth;
                childlineRend.endWidth = childGameObject.GetComponent<LineRenderer>().endWidth;
                childlineRend.material = mat;
                lineCopied = true;
            }

        }

        public void copyMesh()
        {            
            if (GetComponent<MeshFilter>() != null){
                childmeshFilt.mesh.SetVertices(GetComponent<MeshFilter>().mesh.vertices.ToList());
                childmeshFilt.mesh.triangles = GetComponent<MeshFilter>().mesh.triangles;

                childGameObject.GetComponentInParent<MeshFilter>().mesh.uv = GetComponent<MeshFilter>().mesh.uv;

                childGameObject.AddComponent<MeshRenderer>();
                childGameObject.GetComponent<MeshRenderer>().material = mat;

                meshCopied = true;
            }
        }

        /// <summary>
        /// scales mesh copied from parent object
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="mf"></param>
        public void scaleMesh(float scaleFactor, MeshFilter mf)
        {
            Vector3[] newmeshVerts = GetComponent<MeshFilter>().mesh.vertices;

            for (int i = 0; i < newmeshVerts.Length; i++)
            {
                newmeshVerts[i] *= scaleFactor;
            }

            mf.mesh.SetVertices(newmeshVerts.ToList());
        }

        /// <summary>
        /// scales linerenderer copied from parent object
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="lr"></param>
        public void scaleLine(float scaleFactor, LineRenderer lr)
        {

            Vector3[] newlineVerts = new Vector3[GetComponent<LineRenderer>().positionCount];
            
            for (int i = 0; i < newlineVerts.Length; i++)
            { 
                newlineVerts[i] *= scaleFactor;
            }
                
            lr.SetPositions(newlineVerts);    
        }
        
        
        private void Update()
        {
            if (meshCopied && lineCopied)
            {
                scaleMesh(scaleFactor, childmeshFilt);
                scaleLine(scaleFactor, childlineRend);
            }
            else if (meshCopied && !lineCopied)
            {
                scaleMesh(scaleFactor, childmeshFilt);
            }
            else if (!meshCopied && lineCopied)
            {
                scaleLine(scaleFactor, childlineRend);
            }
        }
        


        
        
    }
