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

		private float functionMap(Vector3 input)
		{
			return input.x*input.x + input.z*input.z;
		}

		private Vector3 vectorMap(Vector3 input)
		{
			return new Vector3(input.x, functionMap(input), input.z);
		}

		private Vector3[] vectorMap(Vector3[] input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				input = vectorMap(input);
			}
			return input;
		}

		private void Start()
		{
			myPoly = GetComponent<AbstractPolygon>();
			mesh0 = GetComponent<MeshFilter>().mesh;
			mesh1 = myPoly.gameObject.AddComponent<MeshFilter>().mesh;
			mesh1.vertices = mesh0.vertices;
			mesh1.triangles = mesh0.triangles;
			mesh1.normals = mesh0.normals;
		}

		private void LateUpdate()
		{
			mesh1.vertices = vectorMap(mesh0.vertices);
		}
	}
}
