/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using Leap.Unity;
using System.Collections;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class LeapPinchPickup : MonoBehaviour
    {

        GameObject _target;

        public void setTarget(GameObject target)
        {
            if (_target == null)
            {
                _target = target;
            }
        }

        public void pickupTarget()
        {
            if (_target)
            {
                StartCoroutine(changeParent());
                Rigidbody rb = _target.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }

        //Avoids object jumping when passing from hand to hand.
        IEnumerator changeParent()
        {
            yield return null;
            if (_target != null)
                _target.transform.parent = transform;
        }

        public void releaseTarget()
        {
            if (_target && _target.activeInHierarchy)
            {
                if (_target.transform.parent == transform)
                { //Only reset if we are still the parent
                    Rigidbody rb = _target.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    _target.transform.parent = null;
                }
                _target = null;
            }
        }

        public void clearTarget()
        {
            _target = null;
        }
    }
}