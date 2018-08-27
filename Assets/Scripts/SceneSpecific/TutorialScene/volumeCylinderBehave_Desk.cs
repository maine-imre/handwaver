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
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class volumeCylinderBehave_Desk : MonoBehaviour
    {
		private void OnCollisionEnter(Collision collision)
        {
			GameObject col = collision.gameObject;

            if (col.transform.CompareTag("desk"))
            {
                Vector3 deskheight = this.transform.localPosition - this.transform.localPosition.y * Vector3.up + this.transform.localScale.y * Vector3.up;
                this.transform.localPosition = deskheight;
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
                this.transform.GetComponent<Rigidbody>().freezeRotation = true;
            }

        }

		private void OnCollisionStay(Collision collision)
		 {
			GameObject col = collision.gameObject;

			if (col.transform.CompareTag("desk"))
            {
                Vector3 deskheight = this.transform.localPosition - this.transform.localPosition.y * Vector3.up + this.transform.localScale.y * Vector3.up;
                if (this.transform.localPosition.y < deskheight.y)
                {
                    this.transform.localPosition = deskheight;
                }
            }
        }

		private void OnTriggerExit(Collider other)
		{
			GameObject col = other.gameObject;

			if (col.transform.CompareTag("desk"))
            {
                this.transform.GetComponent<Rigidbody>().freezeRotation = false;
            }
        }
    }
}