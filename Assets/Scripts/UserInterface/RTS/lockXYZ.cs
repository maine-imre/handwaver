/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;

namespace IMRE.HandWaver
{

    public class lockXYZ : MonoBehaviour
    {

        public void lockXZ()
        {
            //this.transform.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
            this.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        public void lockY()
        {
            //this.transform.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
            this.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }

        public void freeAll()
        {
            this.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        public void ResetPos()
        {
            this.transform.localPosition = 0.5f * Vector3.up;
        }
    }
}