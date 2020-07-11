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
    /// <summary>
    /// Allows AbstractGeoObjs to snap to straightedgebehaves (lines)
    /// Currently depreciated.
    /// </summary>
	public class straightEdgeSnap : MonoBehaviour
    {

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<AbstractGeoObj>() && other.transform.parent != this.transform.parent && !other.CompareTag("CrossSectionContainer"))
            {
                if (other.transform.position != Vector3.Project(other.transform.position - this.GetComponentInParent<straightEdgeBehave>().center, this.GetComponentInParent<straightEdgeBehave>().normalDir) + this.GetComponentInParent<straightEdgeBehave>().center){
                    other.transform.position = Vector3.Project(other.transform.position - this.GetComponentInParent<straightEdgeBehave>().center, this.GetComponentInParent<straightEdgeBehave>().normalDir) + this.GetComponentInParent<straightEdgeBehave>().center;
                    other.GetComponent<AbstractGeoObj>().AddToRManager();
                }
            }
        }
    }
}