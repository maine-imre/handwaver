/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class pointReset : MonoBehaviour
    {

        private Vector3 initPosPoint;
        // Use this for initialization
        void Start()
        {
            initPosPoint = transform.position;
        }


        public void resetPositionPoints()
        {
            this.transform.position = initPosPoint;
        }
    }
}
