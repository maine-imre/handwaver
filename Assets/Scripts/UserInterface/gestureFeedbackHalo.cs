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
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class gestureFeedbackHalo : MonoBehaviour
    {
        public Color doublePinchColor;
        public Color pinchColor;
        public Color nearColor;

        public List<Transform> haloList;

        private void Start()
        {
            setAllColor(nearColor);
        }

        public void setAllColor(Color assignColor)
        {
           foreach (Transform haloT in haloList)
            {
                setColor(haloT, assignColor);
            }
        }

        public void setDoublePinch()
        {
            setAllColor(doublePinchColor);
        }

        public void setPinch(Transform haloTransform)
        {
            setColor(haloTransform, pinchColor);
        }


        public void setUnPinch(Transform haloTransform)
        {
            setColor(haloTransform, nearColor);
        }

        public void setColor(Transform haloTransform, Color newColor)
        {
            haloTransform.GetComponent<Light>().color = newColor;
        }

    }
}