/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System;
using UnityEngine;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class InteractableCircle : AbstractCircle, InteractiveFigure
    {
        public void glueToCollider(Collider col)
        {
            throw new NotImplementedException();
        }

        public bool isGraspedLeap()
        {
            throw new NotImplementedException();
        }

        public bool isGraspedVive()
        {
            throw new NotImplementedException();
        }

		public new void OnTriggerStay(Collider col)
		{
			base.OnTriggerStay(col);
			throw new NotImplementedException();
		}

		internal override bool RMotion(NodeList<string> inputNodeList)
        {
            throw new NotImplementedException();
        }

        public void snapToCollider(Collider col)
        {
            throw new NotImplementedException();
        }

 public override void Stretch(InteractionController obj)
		{
			throw new NotImplementedException();
		}

        internal override void GlueToFigure(MasterGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void SnapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}
	}
}