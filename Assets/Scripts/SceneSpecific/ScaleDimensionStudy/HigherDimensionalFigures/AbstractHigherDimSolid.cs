using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        internal float4[] origionalVertices;
	private float3[] _projectedVerticies;
	internal float3[] projectedVerticies
		{ 
			get{
				if(_projectedVerticies == null)
					_projectedVerticies = new float3[origionalVerticies.length];
				
				for (i = 0; i < _projectedVerticies.length; i++)
				{
					_projectedVerticies[i] = origionalVerticies[i].projectDownDimension(viewingBasis, method, viewingAngle, viewingPosition, viewingRadius);
				}
				return _projectedVerticies;
			}
		}

        internal Mesh mesh;

	public float4 viewingPosition;
	public float4x3 viewingBasis;
	public ProjectionMethod method = ProjectionMethod.projective;
	public float viewingAngle = 0f;
	public float viewingRadius = 0f;

	internal abstract Vector2[] uvs;
	internal abstract int[] triangles;

        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;

		mesh.Verticies = projectedVerticies;
		mesh.uvs = uvs;
		mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        void Update()
        {
		mesh.Verticies = projectedVerticies;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

    }
}
