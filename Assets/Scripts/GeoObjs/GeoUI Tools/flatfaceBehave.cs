/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class flatfaceBehave : MasterGeoObj
	{
        //public bool snap = false;

        //private void OnCollisionStay(Collision collision)
        //{
        //    if (snap)
        //    {
        //        MasterGeoOBj thisGeoObj = collision.transform.GetComponent<MasterGeoOBj>();
        //        if (thisGeoObj != null)
        //        {
        //            thisGeoObj.transform.position = Vector3.ProjectOnPlane(thisGeoObj.transform.position - this.transform.position, normalDir()) + this.transform.position;
        //            thisGeoObj.addToRManager();
        //        }
        //    }
        //}

        public Vector3 normalDir
        {
            get
            {
                return this.transform.up;
            }
        }

        internal override void snapToFigure(MasterGeoObj toObj)
		{
			// do nothing
		}

		internal override void glueToFigure(MasterGeoObj toObj)
		{
			// do nothing
		}

		public override void initializefigure()
		{
			// do nothing
		}

		internal override bool rMotion(NodeList<string> inputNodeList)
		{
			return false;
		}

		public override void updateFigure()
		{
			// do nothing
		}

		public override void Stretch(InteractionController obj)
		{
			// do nothing
		}
	}
}