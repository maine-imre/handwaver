/// <summary>
///     A net of a square that folds from a sequence of parallel line segments.
///     Not integrated with kernel.
///     used in study of scale and dimension
/// </summary>
public class squareNet : UnityEngine.MonoBehaviour
{
    private float fold;

    public float Fold
    {
        get => fold;

        set
        {
            fold = value;
            GetComponent<UnityEngine.LineRenderer>().SetPositions(verts(fold));
        }
    }

    private void Start()
    {
        GetComponent<UnityEngine.LineRenderer>().positionCount = 5;
        GetComponent<UnityEngine.LineRenderer>().useWorldSpace = false;
        GetComponent<UnityEngine.LineRenderer>().startWidth = .01f;
        GetComponent<UnityEngine.LineRenderer>().endWidth = .01f;
    }

    private UnityEngine.Vector3[] verts(float t)
    {
        UnityEngine.Vector3[] result = new UnityEngine.Vector3[5];
        result[2] = UnityEngine.Vector3.zero;
        result[1] = UnityEngine.Vector3.right;
        result[0] = result[1] +
                    UnityEngine.Quaternion.AngleAxis(t, UnityEngine.Vector3.up) * UnityEngine.Vector3.right;
        result[3] = result[2] +
                    UnityEngine.Quaternion.AngleAxis(-t, UnityEngine.Vector3.up) * UnityEngine.Vector3.left;
        result[4] = result[3] + UnityEngine.Quaternion.AngleAxis(-2 * t, UnityEngine.Vector3.up) *
                    UnityEngine.Vector3.left;
        return result;
    }
}