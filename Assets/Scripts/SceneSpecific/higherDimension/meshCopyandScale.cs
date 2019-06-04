
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
        private bool meshCopied = false, lineCopied = false;

        
        /// <summary>
        /// copies components from parent object
        /// </summary>
        public GameObject copyObject()
        {
            GameObject copy = new GameObject();

            copy.transform.parent = this.transform;
            copy.transform.localPosition = offset;
            copy.transform.localScale = Vector3.one;
            copy.transform.localRotation = Quaternion.identity;

            return copy;
        }

        public void copyLine()
        {
            GameObject copy = copyObject().gameObject;
            
            Vector3[] lineVerts = new Vector3[GetComponent<LineRenderer>().positionCount];

            childlineRend = copy.AddComponent<LineRenderer>();
            GetComponent<LineRenderer>().GetPositions(lineVerts);
            childlineRend.SetPositions(lineVerts);
            childlineRend.positionCount = copy.GetComponent<LineRenderer>().positionCount;
            childlineRend.startWidth = GetComponent<LineRenderer>().startWidth;
            childlineRend.endWidth = copy.GetComponent<LineRenderer>().endWidth;

            lineCopied = true;
        }

        public void copyMesh()
        {
            GameObject copy = copyObject().gameObject;
            
            childmeshFilt = copy.AddComponent<MeshFilter>();
            childmeshFilt.mesh.SetVertices(GetComponent<MeshFilter>().mesh.vertices.ToList());
            childmeshFilt.mesh.triangles = GetComponent<MeshFilter>().mesh.triangles;
            
            copy.GetComponentInParent<MeshFilter>().mesh.uv = GetComponent<MeshFilter>().mesh.uv;
            
            copy.AddComponent<MeshRenderer>();
            copy.GetComponent<MeshRenderer>().material = mat;

            meshCopied = true;
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
                scaleMesh(5, childmeshFilt);
                scaleLine(5, childlineRend);
            }
            else if (meshCopied && !lineCopied)
            {
                scaleMesh(5, childmeshFilt);
            }
            else if (!meshCopied && lineCopied)
            {
                scaleLine(5, childlineRend);
            }
        }
        


        
        
    }
