/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;



namespace IMRE.HandWaver.Solver
{
    /// <summary>
    /// This instantiates and updates a polygon that constantly updates to match the crossSection of the polygon and the plane.
    /// Be careful of how this scales and deals with singularities.
    /// </summary>

    class IntersectionPolygon : MonoBehaviour
    {
        #region Constructors
		public static IntersectionPolygon Constructor(){
			GameObject go = GameObject.Instantiate(PrefabManager.GetPrefab("IntersectionPolygon"));
			return go.GetComponent<IntersectionPolygon>();
		}
		#endregion

        
		public AbstractSolid parentSolid = null;
        //public CrossSectionBehave crossSectionPlane = null;
        public MeshFilter mf;
        public LineRenderer lr;
        private Vector3[] meshVertices;
        private Vector3[] lineRendererVertices;
        private int[] meshTriangles;
        private Mesh mesh;

   //     private bool checkForChanges
   //     {
   //         get
   //         {
   //             return mesh.vertices != meshVertices;
   //         }
   //     }

   //     private void Start()
   //     {
   //         mesh = mf.mesh;

   //         //testing
   //         parentSolid = GeoObjConstruction.iPrism(GeoObjConstruction.rPoly(4, 1, Vector3.zero), Vector3.up);
   //         //crossSectionPlane = GameObject.FindObjectOfType<CrossSectionBehave>();
   //         //crossSectionPlane.Position3 = Vector3.up * .5f;
   //         //crossSectionPlane.transform.rotation = Quaternion.AngleAxis(30,Vector3.right);
   //     }

   //     private void Update()
   //     {
   //         solveIntersection();

   //         //if (checkForChanges)
   //         //{
   //             mesh.vertices = meshVertices;
   //             mesh.triangles = meshTriangles;
   //         lr.positionCount = lineRendererVertices.Length;
   //         lr.SetPositions(lineRendererVertices);
   //         //}
   //     }

   //     private void solveIntersection()
   //     {
   //         //find the intersections beween each of the faces and the solid.
   //         //works for convex polygons.
   //         Dictionary<AbstractLineSegment, Vector3> vertexCanidates = new Dictionary<AbstractLineSegment, Vector3>();
   //         Graph<AbstractLineSegment> lineGraph = new Graph<AbstractLineSegment>();

   //         //Debug.Log(face.figName + " We Have A Canidate Line! : " + figData.vectordata[0].ToString() + " and " + figData.vectordata[1].ToString());
   //         foreach (AbstractLineSegment line in parentSolid.allEdges)
   //         {
   //             lineGraph.AddNode(line);
   //             //intersectionFigData figData2 = IntersectionMath.LinePlaneIntersection(line.vertex0, line.vertex0 - line.vertex1, crossSectionPlane.Position3, crossSectionPlane.transform.up);
   //  //           if (figData2.figtype == GeoObjType.point)
			//		////&& pointOnLineSegment(figData2.vectordata[0], line.vertex0, line.vertex1))
   //  //           {
   //  //               vertexCanidates[line] = (figData2.vectordata[0]);
   //  //           }
   //         }

   //         foreach (AbstractPolygon face in parentSolid.allfaces)
   //         {
   //             foreach (AbstractLineSegment line0 in face.lineList)
   //             {
   //                 foreach (AbstractLineSegment line1 in face.lineList)
   //                 {
   //                     if (line0 != line1)
   //                     {
   //                         lineGraph.AddDirectedEdge(line0, line1, 1);
   //                         lineGraph.AddDirectedEdge(line1, line0, 1);
   //                     }
   //                 }
   //             }
   //         }

   //         if (vertexCanidates.Count < 3)
			//{
			//	Debug.LogWarning("Looks like there isn't a plane-solid intersection");
   //         }

   //         //find a path through the vertices.

   //         NodeList<AbstractLineSegment> EulerPath = lineGraph.findEulerPath();

   //         meshVertices = new Vector3[EulerPath.Count];
   //         lineRendererVertices = new Vector3[EulerPath.Count + 1];
   //         meshTriangles = new int[EulerPath.Count * 3];

   //         for (int i = 0; i < EulerPath.Count; i++)
   //         {
   //             meshVertices[i] = vertexCanidates[EulerPath[i].Value];
   //             lineRendererVertices[i] = vertexCanidates[EulerPath[i].Value];
   //             //the polygon is convex so the triangles are nice. 
   //             if (i > 1)
   //             {
   //                 meshTriangles[3 * i] = 0;
   //                 meshTriangles[3 * i + 1] = i - 1;
   //                 meshTriangles[3 * i + 2] = i;
   //             }
   //         }
   //         lineRendererVertices[EulerPath.Count] = vertexCanidates[EulerPath[0].Value];
   //     }

   //     private AbstractPolygon FindNextFace(AbstractLineSegment currentLine, AbstractPolygon currentFace, Dictionary<AbstractLineSegment, List<AbstractPolygon>> linePolyMap, Dictionary<AbstractPolygon, List<AbstractLineSegment>> polyLineMap)
   //     {
   //         AbstractPolygon result = (linePolyMap[currentLine])[0];
   //         if (result == currentFace)
   //         {
   //             result = (linePolyMap[currentLine])[1];
   //         }
   //         return result;
   //     }

   //     private AbstractLineSegment FindNextEdge(AbstractLineSegment currentLine, AbstractPolygon currentFace, Dictionary<AbstractLineSegment, List<AbstractPolygon>> linePolyMap, Dictionary<AbstractPolygon, List<AbstractLineSegment>> polyLineMap)
   //     {
   //         AbstractLineSegment result = (polyLineMap[currentFace])[0];
   //         if (result == currentLine)
   //         {
   //             result = (polyLineMap[currentFace])[1];
   //         }
   //         return result;
   //     }

   //     private bool pointOnLineSegment(Vector3 vector3, Vector3 vertex0, Vector3 vertex1)
   //     {
   //         return (vector3 - vertex0).magnitude + (vector3 - vertex1).magnitude == (vertex1 - vertex0).magnitude;
   //     }

   //     private Vector2 FindNextEdge(int currentVertex,  List<Vector2> edgebank)
   //     {
   //         foreach(Vector2 edge in edgebank)
   //         {
   //             if (edge.x == currentVertex)
   //             {
   //                 return edge;
   //             }
   //         }
   //         return Vector2.one*-1;
   //     }

   //     private Vector2 updateLineVertexIDX(Vector2 idxStart, int idx)
   //     {
   //         if (idxStart.x != -1)
   //         {
   //             idxStart .x= idx;
   //         }else if (idxStart.y != -1)
   //         {
   //             idxStart.y = idx;
   //         }
   //         return idxStart;
   //     }
        
   }
}