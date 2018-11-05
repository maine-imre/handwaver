using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	public class ProjectedPolygon : MonoBehaviour
	{
		internal AbstractPolygon myPoly;
		private Mesh mesh0;
		private Mesh mesh1;
		private GameObject child;

		private float functionMap(Vector3 input)
		{
			return .3f*input.x + input.z*-.5f+myPoly.transform.position.y;
		}

		private Vector3 vectorMap(Vector3 input)
		{
			return new Vector3(input.x, functionMap(input), input.z);
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
			child.transform.parent = this.transform;
			mesh1 = child.AddComponent<MeshFilter>().mesh;
			child.AddComponent<MeshRenderer>().material = myPoly.GetComponent<MeshRenderer>().material;
			mesh1.vertices = mesh0.vertices;
			mesh1.triangles = mesh0.triangles;
			mesh1.normals = mesh0.normals;
		}

		private void LateUpdate()
		{
			//this is too low res for polynomials.
			mesh1.vertices = vectorMap(mesh0.vertices);
		}
	}
}
