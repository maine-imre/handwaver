/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{

    public class restrictLocalRotation : MonoBehaviour
    {
        public bool localX = false;
        public bool localY = false;
        public bool localZ = false;


        // Update is called once per frame
        void Update()
        {
            Vector3 eulerRotation = Vector3.zero;
            Vector3 currentEulerRotation = this.transform.localRotation.eulerAngles;

            if (!localX)
            {
                eulerRotation = eulerRotation + currentEulerRotation.x * Vector3.right;
            }
            if (!localY)
            {
                eulerRotation = eulerRotation + currentEulerRotation.y * Vector3.up;
            }
            if (!localZ)
            {
                eulerRotation = eulerRotation + currentEulerRotation.z * Vector3.forward;
            }
            this.transform.localRotation = Quaternion.Euler(eulerRotation);
        }
    }
}