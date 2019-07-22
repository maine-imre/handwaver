

/// <summary>
/// This script aligns the Earth model to the correct Latitude/Longitude positioning in the RSDES scene.
/// The main contributor(s) to this script is NG
/// </summary>

namespace IMRE.HandWaver.Space
{
    public class AlignEarthModel : UnityEngine.MonoBehaviour
    {
        // Use this for initialization
        private void Awake()
        {
            UnityEngine.Mesh m = GetComponent<UnityEngine.MeshFilter>().mesh;
            UnityEngine.Vector2[] uvs = m.uv;

            UnityEngine.Vector3[] verts = m.vertices;
            for (int i = 0; i < uvs.Length; i++)
            {
                UnityEngine.Vector2 tmp = verts[i].latlong();
                uvs[i] = new UnityEngine.Vector2((tmp.y + 180f) / 360f, (tmp.x + 90f) / 180f);
                if (uvs[i].x < 0 || uvs[i].x > 1 || uvs[i].y < 0 || uvs[i].y > 1)
                    UnityEngine.Debug.LogWarning("UV OUT OF BOUND " + i + "  " + uvs[i]);
            }

            m.uv = uvs;
        }
    }
}