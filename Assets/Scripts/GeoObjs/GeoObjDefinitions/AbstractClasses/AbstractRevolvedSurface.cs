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
/// Revolved surface from line segment
/// Will be refactored in new geometery kernel.
/// </summary>
	abstract class AbstractRevolvedSurface : MasterGeoObj
    {

		public Vector3 endpoint1;
        public Vector3 endpoint2;
        public Vector3 centerPoint;
        public Vector3 normalDirection;

        public int n = 500;

        private float radius1;
        private float radius2;
        public float phaseshiftAdjust = 0f;

		public override Vector3 ClosestSystemPosition(Vector3 abstractPosition)
		{
			throw new NotImplementedException();
		}

		public override void InitializeFigure()
		{
			base.InitializeFigure();
			this.figType = GeoObjType.revolvedsurface;

			this.Position3 = centerPoint;

            Vector3 norm1 = Vector3.right;
            Vector3 norm2 = Vector3.forward;
            Vector3.OrthoNormalize(ref normalDirection, ref norm1, ref norm2);

            float radius1 = Vector3.Magnitude(Vector3.ProjectOnPlane(endpoint1 - centerPoint, normalDirection));
            float radius2 = Vector3.Magnitude(Vector3.ProjectOnPlane(endpoint2 - centerPoint, normalDirection));


            Vector3 ps1a = Vector3.ProjectOnPlane(endpoint1 - centerPoint, normalDirection);
            Vector3 ps2a = Vector3.ProjectOnPlane(endpoint2 - centerPoint, normalDirection);

            //Find the phase shift using the angle between the projection of radius on the plane normal to the circle, 
            //with the sign determined by the cross product of one of the basis vecs for the plane and the projected radius.

            float PhaseShift1 = Mathf.Deg2Rad * (Vector3.Angle(norm1, ps1a)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(norm1, ps1a), normalDirection)) + phaseshiftAdjust;
            //Debug.Log(PhaseShift1);
            float PhaseShift2 = Mathf.Deg2Rad * (Vector3.Angle(norm1, ps2a)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(norm1, ps2a), normalDirection)) + phaseshiftAdjust;


            MeshFilter mf = GetComponent<MeshFilter>();
            Mesh mesh = mf.mesh;

            int numvertices = 2 * n;
            Vector3[] vertices = new Vector3[numvertices];

            int numTriangles = 6 * n;
            int[] triangles = new int[numTriangles];

            float tauOverN = Mathf.PI * 2 / n;

            for (int i = 0; i < n; i++)
            {
                vertices[i] = LocalPosition(radius1 * (Mathf.Sin(PhaseShift1 + (i * tauOverN)) * norm2) + radius1 * (Mathf.Cos(PhaseShift1 + (i * tauOverN)) * norm1) + Vector3.Project(endpoint1 - centerPoint, normalDirection));
                vertices[i + n] = LocalPosition(radius2 * (Mathf.Sin(PhaseShift2 + (i * tauOverN)) * norm2) + radius2 * (Mathf.Cos(PhaseShift2 + (i * tauOverN)) * norm1) + Vector3.Project(endpoint2 - centerPoint, normalDirection));

                triangles[i * 6] = i;
                triangles[i * 6 + 1] = i + n;
                triangles[i * 6 + 2] = i + 1;
                triangles[i * 6 + 3] = i + 1;
                triangles[i * 6 + 4] = i + n;
                triangles[i * 6 + 5] = n;
                if (i != (n - 1))
                {
                    triangles[i * 6 + 5] = i + n + 1;
                }
                else
                {
                    //i == n-1 is true
                    triangles[i * 6 + 2] = 0;
                    triangles[i * 6 + 3] = 0;
                }
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            thisSelectStatus = thisSelectStatus;
        }

        public override void updateFigure()
        {
            //update logic
            Vector3 norm1 = Vector3.right;
            Vector3 norm2 = Vector3.forward;
            Vector3.OrthoNormalize(ref normalDirection, ref norm1, ref norm2);


            float radius1 = Vector3.Magnitude(Vector3.ProjectOnPlane(endpoint1 - centerPoint, normalDirection));
            float radius2 = Vector3.Magnitude(Vector3.ProjectOnPlane(endpoint2 - centerPoint, normalDirection));



            Vector3 ps1a = Vector3.ProjectOnPlane(endpoint1 - centerPoint, normalDirection);
            Vector3 ps2a = Vector3.ProjectOnPlane(endpoint2 - centerPoint, normalDirection);

            //Find the phase shift using the angle between the projection of radius on the plane normal to the circle, 
            //with the sign determined by the cross product of one of the basis vecs for the plane and the projected radius.

            float PhaseShift1 = Mathf.Deg2Rad * (Vector3.Angle(norm1, ps1a)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(norm1, ps1a), normalDirection)) + phaseshiftAdjust;
            //Debug.Log(PhaseShift1);
            float PhaseShift2 = Mathf.Deg2Rad * (Vector3.Angle(norm1, ps2a)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(norm1, ps2a), normalDirection)) + phaseshiftAdjust;


            //find phase shift by converting from cartesian to 
            //float PhaseShift1 = Mathf.Atan2(Vector3.Magnitude(Vector3.Project(ps1a, norm1)), Vector3.Magnitude(Vector3.Project(ps1a, norm2))) - Mathf.PI / 2;
            //float PhaseShift2 = Mathf.Atan2(Vector3.Magnitude(Vector3.Project(ps2a, norm1)), Vector3.Magnitude(Vector3.Project(ps2a, norm2))) - Mathf.PI / 2;

            MeshFilter mf = GetComponent<MeshFilter>();
            Mesh mesh = mf.mesh;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < n; i++)
            {
                vertices[i] = LocalPosition(radius1 * (Mathf.Sin(PhaseShift1 + (i * Mathf.PI * 2 / n)) * norm1) + radius1 * (Mathf.Cos(PhaseShift1 + (i * Mathf.PI * 2 / n)) * norm2) + Vector3.Project(endpoint1 - centerPoint, normalDirection));
                vertices[i + n] =LocalPosition(radius2 * (Mathf.Sin(PhaseShift2 + (i * Mathf.PI * 2 / n)) * norm1) + radius2 * (Mathf.Cos(PhaseShift2 + (i * Mathf.PI * 2 / n)) * norm2) + Vector3.Project(endpoint2 - centerPoint, normalDirection));
            }

            mesh.vertices = vertices;

        }
    }
}