/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IMRE.HandWaver.HigherDimensions
{
/// <summary>
/// Implementation of a tesseract, projected into 3D
/// </summary>
	public class Tesseract : AbstractHigherDimSolid
    {
        void Awake()
        {
            originalVerts = new List<Vector4>(){
            new Vector4(1,1,1,1),
            new Vector4(1,1,1,-1),
            new Vector4(1,1,-1,1),
            new Vector4(1,1,-1,-1),
            new Vector4(1,-1,1,1),
            new Vector4(1,-1,1,-1),
            new Vector4(1,-1,-1,1),
            new Vector4(1,-1,-1,-1),
            new Vector4(-1,1,1,1),
            new Vector4(-1,1,1,-1),
            new Vector4(-1,1,-1,1),
            new Vector4(-1,1,-1,-1),
            new Vector4(-1,-1,1,1),
            new Vector4(-1,-1,1,-1),
            new Vector4(-1,-1,-1,1),
            new Vector4(-1,-1,-1,-1)
        };
        }

        internal override void drawFigure() { 

            mesh.Clear();
            verts = new List<Vector3>();
            tris = new List<int>();
            uvs = new List<Vector2>();

            CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[5], rotatedVerts[4]);
            CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[6], rotatedVerts[4]);
            CreatePlane(rotatedVerts[0], rotatedVerts[8], rotatedVerts[12], rotatedVerts[4]);
            CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[3], rotatedVerts[1]);
            CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[9], rotatedVerts[8]);
            CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[10], rotatedVerts[8]);

            CreatePlane(rotatedVerts[1], rotatedVerts[3], rotatedVerts[7], rotatedVerts[5]);
            CreatePlane(rotatedVerts[1], rotatedVerts[9], rotatedVerts[13], rotatedVerts[5]);
            CreatePlane(rotatedVerts[1], rotatedVerts[3], rotatedVerts[9], rotatedVerts[11]);

            CreatePlane(rotatedVerts[2], rotatedVerts[3], rotatedVerts[7], rotatedVerts[6]);
            CreatePlane(rotatedVerts[2], rotatedVerts[3], rotatedVerts[10], rotatedVerts[11]);
            CreatePlane(rotatedVerts[2], rotatedVerts[10], rotatedVerts[14], rotatedVerts[6]);

            CreatePlane(rotatedVerts[3], rotatedVerts[11], rotatedVerts[15], rotatedVerts[7]);

            CreatePlane(rotatedVerts[4], rotatedVerts[12], rotatedVerts[13], rotatedVerts[5]);
            CreatePlane(rotatedVerts[4], rotatedVerts[6], rotatedVerts[14], rotatedVerts[12]);
            CreatePlane(rotatedVerts[4], rotatedVerts[6], rotatedVerts[7], rotatedVerts[5]);

            CreatePlane(rotatedVerts[5], rotatedVerts[7], rotatedVerts[15], rotatedVerts[13]);

            CreatePlane(rotatedVerts[6], rotatedVerts[7], rotatedVerts[14], rotatedVerts[15]);

            CreatePlane(rotatedVerts[8], rotatedVerts[10], rotatedVerts[14], rotatedVerts[12]);
            CreatePlane(rotatedVerts[8], rotatedVerts[9], rotatedVerts[13], rotatedVerts[12]);
            CreatePlane(rotatedVerts[8], rotatedVerts[9], rotatedVerts[10], rotatedVerts[11]);

            CreatePlane(rotatedVerts[9], rotatedVerts[11], rotatedVerts[15], rotatedVerts[13]);

            CreatePlane(rotatedVerts[10], rotatedVerts[11], rotatedVerts[15], rotatedVerts[14]);

        }
    }
}