using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// This script does ___.
/// The main contributor(s) to this script is __
/// Status: ???
/// </summary>
public class cubeNet : MonoBehaviour {
    //public DateTime startTime;
    private Mesh m;
    private LineRenderer lr;

    private float fold = 0f;

    public float Fold
    {
        get
        {
            return fold;
        }

        set
        {
            fold = value;
            lr.SetPositions(lineRendererVerts(fold));
            m.SetVertices(meshVerts(fold).ToList());
        }
    }

    private void Start()
    {
        m = GetComponent<MeshFilter>().mesh;
        m.vertices = meshVerts(0);
        m.triangles = meshTris();

        lr = GetComponent<LineRenderer>();
        lr.positionCount = 22;
        lr.useWorldSpace = false;
        lr.startWidth = .01f;
        lr.endWidth = .01f;
        lr.SetPositions(lineRendererVerts(0));

        //startTime = DateTime.Now;
    }

    private static Vector3[] meshVerts(float t)
    {
        Vector3[] result = new Vector3[14];

        result[0] = .5f * (Vector3.forward + Vector3.right);
        result[1] = .5f * (Vector3.forward + Vector3.left);
        result[2] = .5f * (Vector3.back + Vector3.left);
        result[3] = .5f * (Vector3.back + Vector3.right);

        result[4] = squareVert(result[3], result[0], result[1], t);
        result[5] = squareVert(result[3], result[0], result[2], t);

        result[6] = squareVert(result[0], result[1], result[3], t);
        result[7] = squareVert(result[0], result[1], result[2], t);


        result[8] = squareVert(result[2], result[3], result[1], t);
        result[9] = squareVert(result[2], result[3], result[0], t);

        result[10] = squareVert(result[1], result[2], result[0], t);
        result[11] = squareVert(result[1], result[2], result[3], t);

        result[12] = squareVert(result[10], result[11], result[1], t);
        result[13] = squareVert(result[10], result[11], result[2], t);

        return result;
    }

    private static Vector3 squareVert(Vector3 nSegmentA, Vector3 nSegmentB, Vector3 oppositePoint, float t)
    {
        return Quaternion.AngleAxis(t, (nSegmentA - nSegmentB).normalized) * (oppositePoint - (nSegmentA + nSegmentB) / 2f) + (nSegmentA + nSegmentB) / 2f;
    }

    private static int[] meshQuad(int a, int b, int c, int d)
    {
        return new int[]{a,b,d,d,b,c};
    }

    private static int[] meshTris()
    {
        int[] result = new int[6 * 6];
        meshQuad(0, 4, 5, 3).CopyTo(result, 0);
        meshQuad(0, 3, 2, 1).CopyTo(result, 6);
        meshQuad(3, 9, 8, 2).CopyTo(result, 12);
        meshQuad(0, 1, 7, 6).CopyTo(result, 18);
        meshQuad(1, 2, 11, 10).CopyTo(result, 24);
        meshQuad(10, 11, 13, 12).CopyTo(result, 30);
        return result;
    }

    private static Vector3[] lineRendererVerts(float t)
    {
        Vector3[] result = new Vector3[22];

        Vector3[] tmp = meshVerts(t);
        result[0] = tmp[0];
        result[1] = tmp[4];
        result[2] = tmp[5];
        result[3] = tmp[3];
        result[4] = tmp[9];
        result[5] = tmp[8];
        result[6] = tmp[2];
        result[7] = tmp[11];
        result[8] = tmp[13];
        result[9] = tmp[12];
        result[10] = tmp[10];
        result[11] = tmp[11];
        result[12] = tmp[2];
        result[13] = tmp[3];
        result[14] = tmp[0];
        result[15] = tmp[6];
        result[16] = tmp[7];
        result[17] = tmp[1];
        result[18] = tmp[2];
        result[19] = tmp[3];
        result[20] = tmp[0];
        result[21] = tmp[1];

        return result;
    }
}
