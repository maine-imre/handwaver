using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.ScaleStudy
{
    public class similarFigures : MonoBehaviour
    {

        public Transform tempalte;
        public float scaleFactor = 2f;
        public Vector3 pos1 = Vector3.right;
        public Vector3 pos2 = Vector3.left;
        // Use this for initialization
        void Start()
        {
            pos1 += this.transform.position;
            pos2 += this.transform.position;
            Instantiate(tempalte, pos1, Quaternion.identity, this.transform);
            Transform obj2 = Instantiate(tempalte, pos2, Quaternion.identity, this.transform).transform;
            obj2.transform.localScale *= scaleFactor;
        }
    }
}
