using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace IMRE.HandWaver.Rendering
{
    public class TestScript : MonoBehaviour
    {
        private MeshFilter myMesh;

        // Start is called before the first frame update
        private void Start()
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            myMesh = GetComponent<MeshFilter>();
            myMesh.mesh = GeoElementRenderLib.Segment(new float3(0f, 0f, 0f), new float3(1f, 1f, 0f));
        }
    }
}