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
using System.Linq;


namespace IMRE.HandWaver.HigherDimensions
{
/// <summary>
/// Five cell for scale and dimension study.
/// </summary>
	public class fiveCell : AbstractHigherDimSolid
    {
        readonly static float GoldenRatio = (1f + Mathf.Sqrt(5f)) / 2f;

        private void Awake()
        {
            originalVerts = new List<Vector4>(){
            new Vector4(2,0,0,0),
            new Vector4(0,2,0,0),
            new Vector4(0,0,2,0),
            new Vector4(0,0,0,2),
            new Vector4(GoldenRatio,GoldenRatio,GoldenRatio,GoldenRatio),
        };
        }

        internal override void drawFigure()
        {
            mesh.Clear();
            verts = new List<Vector3>();
            tris = new List<int>();
            uvs = new List<Vector2>();

            CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[2]);
            CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[3]);
            CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[4]);

            CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[3]);
            CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[4]);
            CreatePlane(rotatedVerts[0], rotatedVerts[3], rotatedVerts[4]);

            CreatePlane(rotatedVerts[1], rotatedVerts[2], rotatedVerts[3]);
            CreatePlane(rotatedVerts[1], rotatedVerts[3], rotatedVerts[4]);
            CreatePlane(rotatedVerts[1], rotatedVerts[4], rotatedVerts[2]);

            CreatePlane(rotatedVerts[2], rotatedVerts[3], rotatedVerts[4]);
        }
    }
}