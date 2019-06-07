using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// A net of a tetrahedron that folds into a tetrahedron.
/// used in study of scale and dimension
/// not integrated with kernel.
/// </summary>
public class tetrahedronNet : MonoBehaviour
{
    public Mesh mesh
    {
        get { return GetComponent<MeshFilter>().mesh; }
    }

    public LineRenderer lineRenderer {
        get { return GetComponent<LineRenderer>(); }
    }

    private float _percentFolded = 0f;

    public float PercentFolded
    {
        get
        {
            return _percentFolded;
        }

        set
        {
            _percentFolded = value;
            lineRenderer.SetPositions(lineRendererVerts(_percentFolded));
            mesh.SetVertices(meshVerts(_percentFolded).ToList());
        }
    }

    private void Start()
    {
        //unfolded shape(degree of fold = 0)
        mesh.vertices = meshVerts(0);
        //triangles for unfolded shape
        mesh.triangles = meshTris();
        //11 vertices on trace of unfolded shape 
        lineRenderer.positionCount = 11;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = .01f;
        lineRenderer.endWidth = .01f;
        lineRenderer.SetPositions(lineRendererVerts(0));
    }
    /// <summary>
    /// fold tetrahedron net up by angle t
    /// </summary>
    /// <param name="percentfolded"></param>
    /// <returns></returns>
    private static Vector3[] meshVerts(float percentfolded)
    {
        float degreefolded = percentfolded * 120f + 180f;
        //6 vertices on tetrahedron
        Vector3[] result = new Vector3[6];

        //inner 3 vertices
        result[0] = Vector3.right * (Mathf.Sqrt(3f) / 2f) + Vector3.forward * .5f;
        result[1] = Vector3.right * (Mathf.Sqrt(3f) / 2f) + Vector3.back * .5f;
        result[2] = Vector3.zero;
        //vertex between 0 and 1
        //use trivert() to fold outer vertices up relative to inner vertices
        result[3] = triVert(result[0], result[1], result[2], degreefolded);
        //result[3] = result[1] + (result[2] - result[0]);
        //result[3] = result[0] + Quaternion.AngleAxis(t, result[0] - result[1])*(result[1]+result[2]);

        //vertex between 1 and 2
        result[4] = triVert(result[1], result[2], result[0], degreefolded);
        //result[4] = result[1] + Quaternion.AngleAxis(t, result[1] - result[2]) * (result[0] + result[2]);

        //vertex between 0 and 2
        result[5] = triVert(result[2], result[0], result[1], degreefolded);
        //result[5] = result[2] + Quaternion.AngleAxis(t, result[2] - result[3]) * (result[0] + result[1]);

        return result;
    }
    /// <summary>
    /// function to calculate outer vertices position relative to inner vertices
    /// </summary>
    /// <param name="nSegmentA"></param>
    /// <param name="nSegmentB"></param>
    /// <param name="oppositePoint"></param>
    /// <param name="degreeFolded"></param>
    /// <returns></returns>
    private static Vector3 triVert(Vector3 nSegmentA, Vector3 nSegmentB, Vector3 oppositePoint, float degreeFolded)
    {
        return Quaternion.AngleAxis(degreeFolded, (nSegmentA - nSegmentB).normalized) * (oppositePoint - (nSegmentA+nSegmentB) / 2f) + (nSegmentA + nSegmentB) / 2f;
    }

    /// <summary>
    /// return array of vertices for the 4 triangles in the unfolded tetrahedron
    /// </summary>
    /// <returns></returns>
    private static int[] meshTris()
    {
        return new int[] {
            0,1,2,
            0,3,1,
            2,1,4,
            2,5,0
        };
    }

    /// <summary>
    /// trace edges of mesh
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private static Vector3[] lineRendererVerts(float t)
    {
        Vector3[] result = new Vector3[11];

        Vector3[] tmp = meshVerts(t);
        //map vertices on line segment(s) to vertices on tetrahedron net
        result[0] = tmp[0];
        result[1] = tmp[3];
        result[2] = tmp[1];
        result[3] = tmp[0];
        result[4] = tmp[5];
        result[5] = tmp[2];
        result[6] = tmp[0];
        result[7] = tmp[1];
        result[8] = tmp[4];
        result[9] = tmp[2];
        result[10] = tmp[1];
        return result;
    }
}
