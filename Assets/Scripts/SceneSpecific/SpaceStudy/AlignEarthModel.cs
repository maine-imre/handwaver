using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script aligns the Earth model to the correct Latitude/Longitude positioning in the RSDES scene.
/// The main contributor(s) to this script is NG
/// </summary>

namespace IMRE.HandWaver.Space {
    public class AlignEarthModel : MonoBehaviour {

        // Use this for initialization
        void Awake() {
            Mesh m = GetComponent<MeshFilter>().mesh;
            Vector2[] uvs = m.uv;

            Vector3[] verts = m.vertices;
            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 tmp = GeoPlanetMaths.latlong(verts[i]);
                uvs[i] = new Vector2((tmp.y+180f)/360f, (tmp.x+90f)/180f);
				if (uvs[i].x < 0 || uvs[i].x > 1 || uvs[i].y < 0 || uvs[i].y > 1)
				{
					Debug.LogWarning("UV OUT OF BOUND " + i + "  " + uvs[i].ToString());
				}
            }
            m.uv = uvs;
        }
    }
}