using UnityEngine;

namespace IMRE.HandWaver.Space
{
    /// <summary>
    ///     This script aligns the Earth model to the correct Latitude/Longitude positioning in the RSDES scene.
    ///     The main contributor(s) to this script is NG
    /// </summary>
    public class AlignEarthModel : MonoBehaviour
    {
        // Use this for initialization
        private void Awake()
        {
            var m = GetComponent<MeshFilter>().mesh;
            var uvs = m.uv;

            var verts = m.vertices;
            for (var i = 0; i < uvs.Length; i++)
            {
                var tmp = verts[i].latlong();
                uvs[i] = new Vector2((tmp.y + 180f) / 360f, (tmp.x + 90f) / 180f);
                if (uvs[i].x < 0 || uvs[i].x > 1 || uvs[i].y < 0 || uvs[i].y > 1)
                    Debug.LogWarning("UV OUT OF BOUND " + i + "  " + uvs[i]);
            }

            m.uv = uvs;
        }
    }
}