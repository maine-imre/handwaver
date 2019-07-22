/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.Chess3D
{
    /// <summary>
    ///     Builds an array for  the spatial chess board.
    /// </summary>
    public class boardCreator : UnityEngine.MonoBehaviour
    {
        public UnityEngine.Material mat;

        private void Awake()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        string name = "Sphere" + i + j + k;
                        GenerateSphere(name, .5f * new UnityEngine.Vector3(i, j, k));
                    }
                }
            }
        }

        private UnityEngine.GameObject GenerateSphere(string name, UnityEngine.Vector3 spawnPoint)
        {
            UnityEngine.GameObject sphere = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere);
            sphere.name = name;
            sphere.transform.SetParent(transform);
            sphere.transform.position = spawnPoint;
            sphere.GetComponent<UnityEngine.Renderer>().material = mat;
            sphere.transform.localScale = 0.015f * UnityEngine.Vector3.one;
            sphere.AddComponent<UnityEngine.SphereCollider>().isTrigger = true;

            return sphere;
        }
    }
}