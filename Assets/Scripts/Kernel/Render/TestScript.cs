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
        void Start()
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            myMesh = GetComponent<MeshFilter>();
            myMesh.mesh = GeoElementRenderLib.Circle(new float3(1f, 1f, 1f), new float3(2f, 1f, 1f), 
                new float3(1f, 1f, 1f));           
        }

    }
}