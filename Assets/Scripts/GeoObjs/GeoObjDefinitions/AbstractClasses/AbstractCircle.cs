/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
/// <summary>
/// Circle from radius
/// Will be refactored in new geometery kernel.s
/// </summary>
	abstract class AbstractCircle : MasterGeoObj
    {
        public Vector3 centerPos;
        public Vector3 normalDir;
        public Vector3 edgePos;

        public int numvertices = 300;
        private float radius;
        //public float apothem = .05f;

        public float Radius
        {
            get
            {
                return radius;
            }
        }

		internal override Vector3 ClosestSystemPosition(Vector3 abstractPosition)
		{
			Debug.LogWarning("This FIG TYPE DOESN'T SUPPORT CLOSEST SYS POS : " + figType);

			throw new NotImplementedException();
		}

		public override void InitializeFigure()
		{
			base.InitializeFigure();
			this.figType = GeoObjType.circle;

			this.Position3 = centerPos;

            Vector3 norm1 = Vector3.up;
            Vector3 norm2 = Vector3.right;

            Vector3.OrthoNormalize(ref normalDir, ref norm1, ref norm2);

            Vector3 ps1a = edgePos - centerPos;

            float PhaseShift1 = Mathf.Atan2(ps1a.z, ps1a.x) - Mathf.PI / 2;

            Vector3[] vertices = new Vector3[numvertices];

            Vector3 currentCenter = Vector3.zero;
            radius = Vector3.Distance(edgePos, centerPos);

            for (int i = 0; i < numvertices; i++)
            {
                currentCenter = Radius * (Mathf.Sin(PhaseShift1 + (i * Mathf.PI * 2 / (numvertices - 1))) * norm1) + Radius * (Mathf.Cos(PhaseShift1 + (i * Mathf.PI * 2 / (numvertices - 1))) * norm2);
                vertices[i] = currentCenter + this.Position3;
            }

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            //lineRenderer.startWidth = apothem;
            //lineRenderer.endWidth = apothem;
            lineRenderer.positionCount = numvertices;
            lineRenderer.SetPositions(vertices);
        }

        public override void updateFigure()
        {
            if (this.Position3 != centerPos)
            {
                this.Position3 = centerPos;
            }

            Vector3 norm1 = Vector3.up;
            Vector3 norm2 = Vector3.right;

            Vector3.OrthoNormalize(ref normalDir, ref norm1, ref norm2);

            Vector3 ps1a = edgePos - centerPos;

            float PhaseShift1 = Mathf.Atan2(ps1a.z, ps1a.x) - Mathf.PI / 2;

            Vector3[] vertices = new Vector3[numvertices];
            radius = Vector3.Distance(edgePos, centerPos);

            Vector3 currentCenter = Vector3.zero;

            for (int i = 0; i < numvertices; i++)
            {
                currentCenter = Radius * (Mathf.Sin(PhaseShift1 + (i * Mathf.PI * 2 / (numvertices - 1))) * norm1) + Radius * (Mathf.Cos(PhaseShift1 + (i * Mathf.PI * 2 / (numvertices - 1))) * norm2);
                vertices[i] = LocalPosition(currentCenter + this.Position3);
            }

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(vertices);
        }
    }
}