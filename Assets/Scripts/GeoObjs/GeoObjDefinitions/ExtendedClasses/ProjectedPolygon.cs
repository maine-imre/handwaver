using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// A polygon projected onto a surface. (no longer a polygon)
	/// for linear programming test scene.
	/// will be depreciated with new geometery kernel.
	/// </summary>
	public class ProjectedPolygon : MonoBehaviour
	{
		internal AbstractPolygon myPoly;
		private Mesh mesh0;
		private Mesh mesh1;
		private GameObject child;

		private float functionMap(Vector3 input)
		{
			return .3f*input.x + input.z*-.5f;
		}

		private Vector3 vectorMap(Vector3 input)
		{
			return new Vector3(input.x + MasterGeoObj.LocalPosition(myPoly.Position3).x, functionMap(input), input.z + MasterGeoObj.LocalPosition(myPoly.Position3).z) +Vector3.up;
		}

		private Vector3[] vectorMap(Vector3[] input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				input[i] = vectorMap(input[i]);
			}
			return input;
		}

		public void Initialize()
		{
			myPoly = GetComponent<AbstractPolygon>();
			mesh0 = GetComponent<MeshFilter>().mesh;
			child = new GameObject();
			child.transform.parent = this.transform.parent;
			mesh1 = child.AddComponent<MeshFilter>().mesh;
			child.AddComponent<MeshRenderer>().material = myPoly.GetComponent<MeshRenderer>().material;
			mesh1.vertices = mesh0.vertices;
			mesh1.triangles = mesh0.triangles;
			mesh1.normals = mesh0.normals;
		}

		private void Update()
		{
			//myPoly.Position3 = Vector3.Project(myPoly.Position3, Vector3.up) + Vector3.up * 1.3f;
			myPoly.normDir = Vector3.up;
		}

		private void LateUpdate()
		{
			//this is too low res for polynomials.
			mesh1.vertices = vectorMap(mesh0.vertices);
		}
	}
}
