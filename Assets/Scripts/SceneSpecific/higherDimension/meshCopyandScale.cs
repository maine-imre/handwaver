
using System;
using System.Linq;
using UnityEngine;

    /// <summary>
    /// script for copying and scaling 
    /// </summary>
    public class meshCopyandScale : MonoBehaviour
    {        
        private MeshFilter childmeshFilt;
        private LineRenderer childlineRend;
        public Vector3 offset;
        public Material mat;
        private bool copyBool = false;
                
        public void copyMesh()
        {
            GameObject copy = new GameObject();
            
            copy.transform.parent = this.transform;
            copy.transform.localPosition = offset;
            copy.transform.localScale = Vector3.one;
            copy.transform.localRotation = Quaternion.identity;

            childlineRend = copy.AddComponent<LineRenderer>();
            //childlineRend.SetPositions(GetComponent<LineRenderer>());
            childlineRend.positionCount = copy.GetComponentInParent<LineRenderer>().positionCount;
            childlineRend.startWidth = copy.GetComponentInParent<LineRenderer>().startWidth;
            childlineRend.endWidth = copy.GetComponentInParent<LineRenderer>().endWidth;

            
            childmeshFilt = copy.AddComponent<MeshFilter>();
            childmeshFilt.mesh.SetVertices(GetComponent<MeshFilter>().mesh.vertices.ToList());
            childmeshFilt.mesh.triangles = GetComponent<MeshFilter>().mesh.triangles;
            
            copy.GetComponentInParent<MeshFilter>().mesh.uv = GetComponent<MeshFilter>().mesh.uv;
            
            copy.AddComponent<MeshRenderer>();
            copy.GetComponent<MeshRenderer>().material = mat;

            copyBool = true;
        }


        public void scaleMesh(float scaleFactor, MeshFilter mf)
        {
            Vector3[] newVerts = GetComponent<MeshFilter>().mesh.vertices;

            for (int i = 0; i < newVerts.Length; i++)
            {
                newVerts[i] *= scaleFactor;
            }

            mf.mesh.SetVertices(newVerts.ToList());
        }
        private void Update()
        {
            if (copyBool)
            {
                scaleMesh(5, childmeshFilt);
            }
        }
        


        
        
    }
