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
    public class startUpTester : MonoBehaviour
	{
		/// <summary>
		/// This script does ___.
		/// The main contributor(s) to this script is __
		/// Status: ???
		/// </summary>
		[ContextMenu("Make GeoObjs For Testing")]
	    public void  makeSpheresOnSegment() {
			AbstractPoint p1 = GeoObjConstruction.dPoint((Vector3.right*0.3f + Vector3.up*2.2f+Vector3.forward*-0.2f));
			AbstractPoint p2 = GeoObjConstruction.dPoint((Vector3.right * 0.5f + Vector3.up * 2.0f + Vector3.forward * 0.4f));
			/*AbstractSphere s1 = */GeoObjConstruction.dSphere(p1, p2);
			/*AbstractSphere s2 = */GeoObjConstruction.dSphere(p2, p1);
		}
    }
}