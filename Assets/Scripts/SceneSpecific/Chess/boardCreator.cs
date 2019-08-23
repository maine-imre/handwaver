/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;

namespace IMRE.Chess3D
{
    /// <summary>
    ///     Builds an array for  the spatial chess board.
    /// </summary>
    public class boardCreator : MonoBehaviour
    {
        public Material mat;

        private void Awake()
        {
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < 10; j++)
            for (var k = 0; k < 10; k++)
            {
                var name = "Sphere" + i + j + k;
                GenerateSphere(name, .5f * new Vector3(i, j, k));
            }
        }

        private GameObject GenerateSphere(string name, Vector3 spawnPoint)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = name;
            sphere.transform.SetParent(transform);
            sphere.transform.position = spawnPoint;
            sphere.GetComponent<Renderer>().material = mat;
            sphere.transform.localScale = 0.015f * Vector3.one;
            sphere.AddComponent<SphereCollider>().isTrigger = true;

            return sphere;
        }
    }
}