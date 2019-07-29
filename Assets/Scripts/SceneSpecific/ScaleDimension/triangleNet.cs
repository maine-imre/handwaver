using UnityEngine;

/// <summary>
///     A net of a triangle that folds into a triangle.
///     Used in study of scale and dimension
///     not integrated with kernel.
/// </summary>
public class triangleNet : MonoBehaviour
{
    private float fold;

    public float Fold
    {
        get => fold;

        set
        {
            fold = value;
            GetComponent<LineRenderer>().SetPositions(verts(fold));
        }
    }

    private void Start()
    {
        GetComponent<LineRenderer>().positionCount = 4;
        GetComponent<LineRenderer>().useWorldSpace = false;
        GetComponent<LineRenderer>().startWidth = .01f;
        GetComponent<LineRenderer>().endWidth = .01f;
    }

    private Vector3[] verts(float t)
    {
        var result = new Vector3[4];
        result[2] = Vector3.zero;
        result[1] = Vector3.right;
        result[0] = result[1] +
                    Quaternion.AngleAxis(t, Vector3.up) * Vector3.right;
        result[3] = result[2] +
                    Quaternion.AngleAxis(-t, Vector3.up) * Vector3.left;
        return result;
    }
}