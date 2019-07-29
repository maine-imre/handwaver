using System;
using Unity.Mathematics;

namespace IMRE.HandWaver.Rendering
{
    /// <summary>
    /// Contains functional code that would render out a given element based on passed in data.
    /// </summary>
    public class GeoElementRenderLib
    {
        public static float width = .0025f;

        private static float3 headPosition
        {
            get
            {
                //TODO attach to emboided input
                return float3.zero;
                
            }
        }

        private static float3 headDirection
        {
            get
            {
                //TODO attach to emboided input
                return float3.zero;
            }
        }
        
        // TODO: Group by element type.
        //    -- Point
        //    -- Path
        //    -- Surface
        //    -- Region
        //    -- NonGeometeric
        // TODO: Determine input vars.
        // TODO: Implement functionality for base version of HW Rendering.

        public bool Line(float3 point, float3 direction)
        {
            direction = Unity.Mathematics.math.normalize(direction);
            return Segment(point + 100f * direction, point - 100f * direction);
        }

        public bool Point(float3 location)
        {
            throw new NotImplementedException();
        }

        public bool Segment(float3 endpointA, float3 endpointB)
        {
            int n = 100;
            
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();
            float3 lineWidth = width*Unity.Mathematics.math.normalize(Unity.Mathematics.math.cross(endpointA - endpointB,
                (endpointA + endpointB) / 2f - headDirection));
            
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[4];
            verts[0] = endpointA + lineWidth;
            verts[1] = endpointA - lineWidth;
            verts[2] = endpointB - lineWidth;
            verts[3] = endpointB + lineWidth;

            mesh.vertices = verts;
            
            //triangles
            int[] tris = {1, 3, 0, 1, 2, 3};            
            mesh.triangles = tris;
                                                           
            //uvs
            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[verts.Length];
            for (int j = 0; j < uvs.Length; j++)
                uvs[j] = new UnityEngine.Vector2(verts[j].x, verts[j].y);

            //normals
            UnityEngine.Vector3[] normals = new UnityEngine.Vector3[verts.Length];
            for (int i = 0; i < verts.Length; i++)
                normals[i] = verts[i].normalized;

            return true;
        }

        #region Unimplemented

        public bool Angle( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Axis( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Conic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool ConicPart( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool CurveCartesian( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Function( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool FunctionND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Interval( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Locus( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool LocusND( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Number( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Poly( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool PolyLine( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Ray( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool SurfaceFinite( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Symbolic( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Vector2D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Vector3D( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Text( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Turtle( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Video( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Image( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        public bool Slider( /*Data needed to render*/)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}