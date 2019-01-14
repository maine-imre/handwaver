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
/// will be depreciated with new geometery kernel.
/// </summary>
	class DependentPoint : AbstractPoint, DependentFigure
    {
		#region Constructors
		public static DependentPoint Constructor(){
				GameObject go = new GameObject();
				go.AddComponent<MeshFilter>();
				go.AddComponent<MeshRenderer>();
				//check if sphere mesh is added.
				go.AddComponent<SphereCollider>();
				go.GetComponent<SphereCollider>().radius = 0.5f;
				go.AddComponent<Rigidbody>();
				go.GetComponent<Rigidbody>().useGravity = false;
				go.GetComponent<Rigidbody>().isKinematic = false;
				go.AddComponent<InteractionBehaviour>();
				return go.AddComponent<DependentPoint>();
		}
		#endregion
		private Vector3 oldPos;

		public override void initializefigure()
        {
			//   do nothing    
		}

        internal override bool RMotion(NodeList<string> inputNodeList)
        {
			return hasMoved();
		}

		public bool hasMoved()
		{
			bool result = (this.Position3 == oldPos);
			oldPos = this.Position3;
			return result;
		}

		public override void Stretch(InteractionController obj)
		{
			//   do nothing    
		}

		public override void updateFigure()
        {
			//   do nothing    
		}

		internal override void GlueToFigure(MasterGeoObj toObj)
        {
			//   do nothing    
		}

		internal override void SnapToFigure(MasterGeoObj toObj)
        {
			//   do nothing    
		}
	}

}
