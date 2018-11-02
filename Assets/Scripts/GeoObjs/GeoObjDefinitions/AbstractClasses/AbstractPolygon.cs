/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	abstract class AbstractPolygon : MasterGeoObj
    {
		public bool skewable = true;

		public List<AbstractPoint> pointList = new List<AbstractPoint>();
		public List<MasterGeoObj> pointListMGO
		{
			get
			{
				return pointList.Cast<MasterGeoObj>().ToList();
			}
		}
        public List<AbstractLineSegment> lineList = new List<AbstractLineSegment>(); //clockwise

		public Vector3[] vertices;
		public int[] triangles;
        private Color defaultColor = new Color(133 / 255f, 130 / 255f, 225 / 255f, 0.43137254902f);

		internal override Vector3 ClosestSystemPosition(Vector3 abstractPosition)
		{
			result = Vector3.ProjectOnPlane(abstractPosition-Position3,normDir) + Position3;
			
			if(!checkInPolygon(result))
			{
				Vector3 best = lineList[0];
				foreach(AbstractLineSegment l in lineList)
				{
					dist = l.ClosestSystemPosition(result) - result;
					l.ClosestSystemPosition(result);
					if (dist < (best - result).magnitude)
					{
						best = l.ClosestSystemPosition(result);
					}
				}
				return best
			}
			else
			{
			return result;
			}
		}

		internal Vector3[] basisSystem
		{
			get
			{
				Vector3 basis1 = Vector3.zero;
				int i = 0;
				while (basis1.magnitude == 0 && i < vertices.Length - 1)
				{
					basis1 = vertices[i] - vertices[i + 1];
					i++;
				}
				Vector3 basis2 = Vector3.zero;
				while ((basis2.magnitude == 0 || Vector3.Cross(basis1, basis2).magnitude == 0) && i < vertices.Length - 1)
				{
					basis2 = vertices[i] - vertices[i + 1];
				}
                if(Vector3.Cross(basis1,basis2).magnitude == 0)
                {
                    basis2 = new Vector3(basis1.y, - basis1.x, 0);
                }
				basis2 = Vector3.Cross(basis1, basis2);
				basis2 = Vector3.Cross(basis1, basis2);
				Vector3[] result = new Vector3[2];
				result[0] = basis1.normalized;
				result[1] = basis2.normalized;
				return result;
			}
		}

        internal float CrossProduct(Vector2 v1, Vector2 v2)
        {
            Vector3 v31 = new Vector3(v1.x, v1.y, 0);
            Vector3 v32 = new Vector3(v2.x, v2.y, 0);
            //the cross product should be in the z direction.
            return Vector3.Cross(v31, v32).z;
        }

		/// <summary>
		/// Project the vertices into the plane.
		/// </summary>
		internal Vector2[] vertices2D
		{
			get
			{
				Vector3[] tempBasis = basisSystem;
				//Debug.Log(tempBasis[0] + "  BASIS   " + tempBasis[1] + figName);
				Vector2[] result = new Vector2[vertices.Length];
				for (int j = 0; j < vertices.Length; j++)
				{
					result[j] = new Vector2(Vector3.Dot(vertices[j], tempBasis[0]), Vector3.Dot(vertices[j], tempBasis[1]));
				}
				//Debug.Log(result[0].ToString() + result[1].ToString() +result[2].ToString() + result[3].ToString() + figName);

				return result;
			}
		}

        /// <summary>
        /// The following method assumes that the points in the polygon are coplanar.
        /// </summary>
        internal float area
        {
            get
            {
                float result = 0f;

				if (pointList.Count < 3)
				{
                    Debug.LogWarning("NOT ENOUGH POINTS");
					return float.NaN;
				}else if (GetComponent<regularPolygon>() != null)
                {
                    //breaks out regular polygons for a more efficient method.
                    return GetComponent<regularPolygon>().regularPolyArea;
                }

				Vector2[] vertices2D_shifted = vertices2D;

                if (pointList.Count == 3)
                {
                    //triangle area
                    return Mathf.Abs(CrossProduct(vertices2D_shifted[0] - vertices2D_shifted[1], vertices2D_shifted[1] - vertices2D_shifted[2])/2f);
                } else if (pointList.Count == 4)
                {
                    //break quad into triangles
                    return Mathf.Abs(CrossProduct(vertices2D_shifted[0] - vertices2D_shifted[1], vertices2D_shifted[1] - vertices2D_shifted[2]) / 2f) + Mathf.Abs(CrossProduct(vertices2D_shifted[1] - vertices2D_shifted[2], vertices2D_shifted[2] - vertices2D_shifted[3]) / 2f);
                }
                else {


                    result += CrossProduct(vertices2D_shifted[vertices2D_shifted.Length - 1], vertices2D_shifted[0]);

                    for (int i = 0; i < vertices2D_shifted.Length - 1; i++)
                    {
                        result += CrossProduct(vertices2D_shifted[i], vertices2D_shifted[i + 1]);
                    }

                    result = Mathf.Abs(result) / 2f;

                    if (result == 0f)
                    {
                        Debug.LogWarning("NET AREA ZERO???");
                        return float.NaN;
                    }

                    return result;
                }
            }
        }

		/// <summary>
		/// finds the center as the center of mass given an constant density distribution.
		/// </summary>
		internal Vector3 center
		{
			get
			{
				if (skewable)
				{
					//approximate the centroid for a skewed polygon
					Vector3 centerSkew = Vector3.zero;
					foreach (AbstractPoint point in pointList)
					{
						centerSkew += point.Position3 / pointList.Count;
					}
					return centerSkew;
				}

				float x_centroid = 0f;
				float y_centroid = 0f;
				float signedArea = 0f;

                //check this math.

				for (int j = 0; j < vertices2D.Length - 2; j++)
				{
                    if (j == vertices2D.Length - 2)
                    {
                        x_centroid += (1f / 6f) * (vertices2D[j].x - vertices2D[0].x) * (vertices2D[j].x * vertices2D[0].y - vertices2D[0].x * vertices2D[j].y);
                        y_centroid += (1f / 6f) * (vertices2D[j].y - vertices2D[0].y) * (vertices2D[j].x * vertices2D[0].y - vertices2D[0].x * vertices2D[j].y);
                        signedArea += (1f / 2f) * (vertices2D[j].x * vertices2D[0].y - vertices2D[0].x * vertices2D[j].y);
                    }
                    else
                    {
                        x_centroid += (1f / 6f) * (vertices2D[j].x - vertices2D[j + 1].x) * (vertices2D[j].x * vertices2D[j + 1].y - vertices2D[j + 1].x * vertices2D[j].y);
                        y_centroid += (1f / 6f) * (vertices2D[j].y - vertices2D[j + 1].y) * (vertices2D[j].x * vertices2D[j + 1].y - vertices2D[j + 1].x * vertices2D[j].y);
                        signedArea += (1f / 2f) * (vertices2D[j].x * vertices2D[j + 1].y - vertices2D[j + 1].x * vertices2D[j].y);
                    }
				}
				x_centroid = x_centroid / signedArea;
				y_centroid = y_centroid / signedArea;

				return x_centroid * basisSystem[0] + y_centroid * basisSystem[1];

			}
		}

		public override void initializefigure()
        {
            this.figType = GeoObjType.polygon;

            MeshFilter mf = GetComponent<MeshFilter>();
            Mesh mesh = mf.mesh;

            //Init ();
            Renderer rend = gameObject.GetComponent<Renderer>();
            Material mat = rend.material;
            mat.color = new Color(133 / 255f, 130 / 255f, 225 / 255f, 0.43137254902f); //colorGenerator.randomColorTransparent(mat);

			int pointNum = pointList.Count;

            if (pointNum > 2)
			{
				vertices = new Vector3[pointNum + 1];
				int numTriangles = 3 * pointNum;
				triangles = new int[numTriangles];
				Vector3 center = Vector3.zero;
				int i = 0;

				foreach (AbstractPoint point in pointList)
				{
					center = center + point.Position3 / pointNum;
				}
				this.transform.localPosition = center;

				foreach (AbstractPoint point in pointList)
				{
					vertices[i] = LocalPosition(point.Position3 - center);
					triangles[3 * i + 2] = i;
					if (i + 1 < pointNum)
					{
						triangles[3 * i + 1] = i + 1;
					}
					else
					{
						triangles[3 * i + 1] = 0;
					}
					triangles[3 * i] = pointNum;
					i++;
				}
				vertices[pointNum] = LocalPosition(Vector3.zero);

				mesh.Clear();
				mesh.vertices = vertices;
				mesh.triangles = triangles;
				mesh.RecalculateNormals();

				//GameObject planeHandle = Instantiate(Resources.Load("HandlePreFab", typeof(GameObject))) as GameObject;
				//planeHandle.transform.parent = this.transform;
				//planeHandle.transform.localPosition = Vector3.zero;
				StartCoroutine(recalcMesh(mesh));

			}
		}

		IEnumerator recalcMesh(Mesh mesh)
		{
			yield return new WaitForSeconds(0.2f);
			mesh.RecalculateNormals();
		}

		public override void updateFigure()
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            Mesh mesh = mf.mesh;

            this.transform.rotation = Quaternion.identity;

			mesh.RecalculateNormals();
			mesh.vertices = vertices;
			//thisIBehave.NotifyTeleported();


			//if a polygon is a skew polygon, turn it grey.
			if (/*CheckSkewPolygon()*/ false)
            {
#pragma warning disable CS0162 // Unreachable code detected
				defaultColor = GetComponent<MeshRenderer>().materials[0].color;
				this.GetComponent<MeshRenderer>().materials[0].color = Color.grey;
#pragma warning restore CS0162 // Unreachable code detected

			}
			else
            {
                this.GetComponent<MeshRenderer>().materials[0].color = defaultColor;
            }

        }

        internal Vector3 normDir
        {
            get
            {
                return Vector3.Cross(pointList[0].Position3 - pointList[1].Position3, pointList[1].Position3 - pointList[2].Position3).normalized;
            }
			set
			{
				pointList.ForEach(p => p.Position3 = Vector3.ProjectOnPlane(p.Position3 - Position3, value) + Position3);
				addToRManager();
			}
        }

        internal bool checkInPolygon(Vector3 positionOnPlane)
		{
			Vector3 positionOnPlane = Vector3.project(positionOnPlane - Position3) + Position3;
			Vector3[] basis = basisSystem;
			Vector2 point = (Vector3.Dot(positionOnPlane,basis[0],Vector3.Dot(positionOnPlane,basis[1]));
		}

        internal bool CheckSkewPolygon()
        {
            Vector3[] tempBasis = basisSystem;

            for (int i = 0; i < pointList.Count; i++)
            {
                if (Vector3.Magnitude(Vector3.Project(pointList[i].Position3-this.Position3, Vector3.Cross(tempBasis[0], tempBasis[1]))) > 0)
                {
                    return true;
                }
            }
            return false;
        }
	}
}
