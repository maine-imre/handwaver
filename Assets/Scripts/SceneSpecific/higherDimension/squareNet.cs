using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A net of a square that folds from a sequence of parallel line segments.
/// Not integrated with kernel.
/// used in study of scale and dimension
/// </summary>
public class squareNet : MonoBehaviour
{

    private float fold = 0f;

    public float Fold
    {
        get
        {
            return fold;
        }

        set
        {
            //set vertices using vert function
            fold = value;
            GetComponent<LineRenderer>().SetPositions(verts(fold));
        }
    }

    private void Start()
    {
        //intial line segment with 5 points (2 vertices merge)
		GetComponent<LineRenderer>().positionCount = 5;
		//
		GetComponent<LineRenderer>().useWorldSpace = false;
        //start and end width of line
        GetComponent<LineRenderer>().startWidth = .01f;
        GetComponent<LineRenderer>().endWidth = .01f;
    }

    //generate vertices for square
    private Vector3[] verts(float t)
    {

        //matrix of vertices
        Vector3[] result = new Vector3[5];
        //initial vertices that don't need to move/are pivot points
        result[2] = Vector3.zero;
        result[1] = Vector3.right;
        //rotate vertice by t or -t around (0, 1, 0) 
        result[0] = result[1] + Quaternion.AngleAxis(t, Vector3.up)*Vector3.right;
        result[3] = result[2] + Quaternion.AngleAxis(-t, Vector3.up)*Vector3.left;
        //rotate vertice by -2t around (0, 1, 0)
        result[4] = result[3] + Quaternion.AngleAxis(-2 * t, Vector3.up)*Vector3.left;
        return result;
    }
}
