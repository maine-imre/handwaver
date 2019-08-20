using UnityEngine;

namespace IMRE.HandWaver.HigherDimensions
{
    [UnityEngine.RequireComponent(typeof(UnityEngine.MeshFilter))]
    [UnityEngine.RequireComponent(typeof(UnityEngine.MeshRenderer))]
    /// <summary>
    /// Higher dimensional solids and their projections automated.
    /// Now uses sliers for scale and dimension study, via a dictionary.
    /// </summary>
    public abstract class AbstractHigherDimSolid : UnityEngine.MonoBehaviour
    {
        private Unity.Mathematics.float3[] _projectedVertices;

        internal UnityEngine.Mesh mesh;
        public ProjectionMethod method = ProjectionMethod.projective;
        internal Unity.Mathematics.float4[] originalVertices;
        public float viewingAngle;
        public Unity.Mathematics.float4x3 viewingBasis;

        public Unity.Mathematics.float4 viewingPosition;
        public float viewingRadius;

        /// <summary>
        /// 
        /// </summary>
        internal Unity.Mathematics.float3[] ProjectedVertices
        {
            get
            {
                if (_projectedVertices == null)
                    _projectedVertices = new Unity.Mathematics.float3[originalVertices.Length];

                for (int i = 0; i < _projectedVertices.Length; i++)
                {
                    _projectedVertices[i] = originalVertices[i].projectDownDimension(viewingBasis, method, viewingAngle,
                        viewingPosition, viewingRadius);
                }

                return _projectedVertices;
            }
        }

        private UnityEngine.Vector3[] ProjectedVerticiesV3
        {
            get
            {
                Unity.Mathematics.float3[] tmp = ProjectedVertices;
                UnityEngine.Vector3[] tmp2 = new UnityEngine.Vector3[ProjectedVertices.Length];
                for (int i = 0; i < +tmp.Length; i++) tmp2[i] = tmp[i];

                return tmp2;
            }
        }

        public abstract UnityEngine.Vector2[] uvs { get; }
        public abstract int[] triangles { get; }
        public abstract Color[] colors { get; }

        private void Start()
        {
            mesh = GetComponent<UnityEngine.MeshFilter>().mesh;

            mesh.vertices = ProjectedVerticiesV3;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.colors = colors;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private void Update()
        {
            mesh.vertices = ProjectedVerticiesV3;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}
