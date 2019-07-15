using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.HigherDimensions
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

/// <summary>
/// Higher dimensional solids and their projections automated.
/// Now uses sliers for scale and dimension study, via a dictionary.
/// </summary>
	public abstract class AbstractHigherDimSolid : MonoBehaviour
    {
        internal float4[] originalVertices;
	private float3[] _projectedVertices;
	internal float3[] ProjectedVertices
		{ 
			get{
				if(_projectedVertices == null)
					_projectedVertices = new float3[originalVertices.Length];
				
				for (int i = 0; i < _projectedVertices.Length; i++)
				{
					_projectedVertices[i] = originalVertices[i].projectDownDimension(viewingBasis, method, viewingAngle, viewingPosition, viewingRadius);
				}
				return _projectedVertices;
			}
		}

	private Vector3[] ProjectedVerticiesV3
	{
		get
		{
			float3[] tmp = ProjectedVertices;
			Vector3[] tmp2 = new Vector3[ProjectedVertices.Length];
			for (int i = 0; i < +tmp.Length; i++)
			{
				tmp2[i] = (Vector3) tmp[i];
			}

			return tmp2;
		}
	}

        internal Mesh mesh;

	public float4 viewingPosition;
	public float4x3 viewingBasis;
	public ProjectionMethod method = ProjectionMethod.projective;
	public float viewingAngle = 0f;
	public float viewingRadius = 0f;

	public abstract Vector2[] uvs { get; }
	public abstract int[] triangles { get; }

	void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;

		mesh.vertices = ProjectedVerticiesV3;
		mesh.uv = uvs;
		mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        void Update()
        {
		mesh.vertices = ProjectedVerticiesV3;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

    }
}
