namespace IMRE.HandWaver.ScaleDimension{

	[UnityEngine.RequireComponent(typeof(UnityEngine.MeshFilter))]
	[UnityEngine.RequireComponent(typeof(UnityEngine.MeshRenderer))]

	public abstract class ScaleDimensionFigure{
		public enum scaleDimensionLens
		{
			none, projection, cross-section, net;
		}
		
        public abstract UnityEngine.Vector2[] standard_uvs { get; }
        public abstract int[] standard_triangles { get; }
   		public abstract Unity.Mathematics.float4[] origionalVerticies;
   		public abstract UnityEngine.Mesh net_mesh {get;}
   		public abstract UnityEngine.Mesh crossSection_mesh {get;}

		public scaleDimensionLens lens; 
		public Unity.Mathematics.float4x4 rotationMatrix;
		public Unity.Mathematics.float4 translationVector;
		public IMRE.HandWaver.Higherdimensions.ProjectionData projectionValues;
		
		internal UnityEngine.Mesh mf_mesh
		{
			get{return GetComponent<MeshFilter>().mesh;}
		}


        private Unity.Mathematics.float3[] ProjectedVertices
        {
            get
            {
            //instantiate a temporary variable
                if (_projectedVertices == null)
                    _projectedVertices = new Unity.Mathematics.float3[originalVertices.Length];

			//TODO rotate verticies
			
			    for (int i = 0; i < _projectedVertices.Length; i++)
                {
                	//use a float4x4 matrix to describe the rotation, this canb e generalized down diemsnions as necessary.
                    _projectedVertices[i] = originalVertices[i].rotate(rotationMatrix);
                }
			
			//TODO translate verticies
			
			    for (int i = 0; i < _projectedVertices.Length; i++)
                {
                	//use a float4 vector to shift through space
                    _projectedVertices[i] = _projectedVertices[i].translate(translationVector);
                }
			
			//project based on projection struct
                for (int i = 0; i < _projectedVertices.Length; i++)
                {
                	//use a struct with all necessary projection data, from public variable
                    _projectedVertices[i] = _projectedVertices[i].projectDownDimension(projectionValues);
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
        
        private UnityEngine.Mesh standard_mesh
        {
            mesh.vertices = ProjectedVerticiesV3;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.colors = colors;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private void Update()
        {
        	//TODO move this onto an event-based system that only updates when values change.
			switch(lens){
				case cross-section:
					break;
				case net:
					break;
				default:
					mf_mesh = standard_mesh;
					break;
			}
        } 		
	}
}
