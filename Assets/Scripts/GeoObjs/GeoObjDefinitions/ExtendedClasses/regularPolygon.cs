/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using PathologicalGames;
using IMRE.HandWaver.Solver;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class regularPolygon : InteractablePolygon
    {
        public int n = 0;
        public Vector3 basis1 = Vector3.right;
        public Vector3 basis2 = Vector3.forward;
        private float apothem;
        private float sideLength
        {
            get
            {
                return 2 * apothem * Mathf.Sin(Mathf.PI / n);
            }
        }
        private float perimeter
        {
            get
            {
                return sideLength * n;
            }
        }

        public float regularPolyArea
        {
            get
            {
                return apothem * perimeter / 2f;
            }
        }

		public void InitRegPoly(int nSides, float a, Vector3 normDir)
		{
            apothem = a;
			float hyp = (apothem) / (Mathf.Cos(Mathf.PI / nSides));

			n = nSides;
			if (normDir != Vector3.zero)
			{
				Vector3.OrthoNormalize(ref normDir, ref basis1);
				Vector3.OrthoNormalize(ref normDir, ref basis1, ref basis2);
			}
            for (int i = 0; i < n; i++)
            {
                pointList.Add(GeoObjConstruction.iPoint(this.Position3 + hyp * (Mathf.Sin(2f * Mathf.PI * i / n) * basis1 + Mathf.Cos(2f * Mathf.PI * i / n) * basis2)));
                if (i!= 0)
                {
                    lineList.Add(GeoObjConstruction.iLineSegment(pointList[i - 1], pointList[i]));
                }
            }

            lineList.Add(GeoObjConstruction.iLineSegment(pointList[0], pointList[n - 1]));

			foreach (AbstractLineSegment line in lineList)
			{
				HW_GeoSolver.ins.addDependence(this.transform, line.transform);
			}

			foreach (AbstractPoint point in pointList)
			{
				HW_GeoSolver.ins.addDependence(this.transform, point.transform);
			}

			this.initializefigure();

            this.addToRManager();
        }

    }
}
