using Enumerable = System.Linq.Enumerable;

/// <summary>
///     script for copying and scaling mesh and line segments
///     so that the net folding of shapes can be observed on figures of different sizes
/// </summary>
public class meshCopyandScale : UnityEngine.MonoBehaviour
{
    private UnityEngine.GameObject childGameObject;
    private UnityEngine.LineRenderer childlineRend;
    private UnityEngine.MeshFilter childmeshFilt;
    public UnityEngine.Material mat;
    private bool meshCopied, lineCopied;
    public UnityEngine.Vector3 offset;
    public float scaleFactor = 1f;

    /// <summary>
    ///     copies components from parent object
    /// </summary>
    public void Start()
    {
        childGameObject = new UnityEngine.GameObject();

        childGameObject.transform.parent = transform;
        childGameObject.transform.localPosition = offset;
        childGameObject.transform.localScale = UnityEngine.Vector3.one;
        childGameObject.transform.localRotation = UnityEngine.Quaternion.identity;
        childlineRend = childGameObject.AddComponent<UnityEngine.LineRenderer>();
        childmeshFilt = childGameObject.AddComponent<UnityEngine.MeshFilter>();

        copyLine();
        copyMesh();
    }

    public void copyLine()
    {
        if (GetComponent<UnityEngine.LineRenderer>() != null)
        {
            UnityEngine.Vector3[] lineVerts =
                new UnityEngine.Vector3[GetComponent<UnityEngine.LineRenderer>().positionCount];

            GetComponent<UnityEngine.LineRenderer>().GetPositions(lineVerts);
            childlineRend.SetPositions(lineVerts);
            childlineRend.positionCount = childGameObject.GetComponent<UnityEngine.LineRenderer>().positionCount;
            childlineRend.startWidth = GetComponent<UnityEngine.LineRenderer>().startWidth;
            childlineRend.endWidth = childGameObject.GetComponent<UnityEngine.LineRenderer>().endWidth;
            childlineRend.material = mat;
            lineCopied = true;
        }
    }

    public void copyMesh()
    {
        if (GetComponent<UnityEngine.MeshFilter>() != null)
        {
            childmeshFilt.mesh.SetVertices(Enumerable.ToList(GetComponent<UnityEngine.MeshFilter>().mesh.vertices));
            childmeshFilt.mesh.triangles = GetComponent<UnityEngine.MeshFilter>().mesh.triangles;

            childGameObject.GetComponentInParent<UnityEngine.MeshFilter>().mesh.uv =
                GetComponent<UnityEngine.MeshFilter>().mesh.uv;

            childGameObject.AddComponent<UnityEngine.MeshRenderer>();
            childGameObject.GetComponent<UnityEngine.MeshRenderer>().material = mat;

            meshCopied = true;
        }
    }

    /// <summary>
    ///     scales mesh copied from parent object
    /// </summary>
    /// <param name="scaleFactor"></param>
    /// <param name="mf"></param>
    public void scaleMesh(float scaleFactor, UnityEngine.MeshFilter mf)
    {
        UnityEngine.Vector3[] newmeshVerts = GetComponent<UnityEngine.MeshFilter>().mesh.vertices;

        for (int i = 0; i < newmeshVerts.Length; i++) newmeshVerts[i] *= scaleFactor;

        mf.mesh.SetVertices(Enumerable.ToList(newmeshVerts));
    }

    /// <summary>
    ///     scales linerenderer copied from parent object
    /// </summary>
    /// <param name="scaleFactor"></param>
    /// <param name="lr"></param>
    public void scaleLine(float scaleFactor, UnityEngine.LineRenderer lr)
    {
        UnityEngine.Vector3[] newlineVerts =
            new UnityEngine.Vector3[GetComponent<UnityEngine.LineRenderer>().positionCount];

        for (int i = 0; i < newlineVerts.Length; i++) newlineVerts[i] *= scaleFactor;

        lr.SetPositions(newlineVerts);
    }

    private void Update()
    {
        if (meshCopied && lineCopied)
        {
            scaleMesh(scaleFactor, childmeshFilt);
            scaleLine(scaleFactor, childlineRend);
        }
        else if (meshCopied && !lineCopied)
        {
            scaleMesh(scaleFactor, childmeshFilt);
        }
        else if (!meshCopied && lineCopied)
        {
            scaleLine(scaleFactor, childlineRend);
        }
    }
}