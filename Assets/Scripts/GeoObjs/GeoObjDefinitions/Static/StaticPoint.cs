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
/// Points that aren't dynamic
/// </summary>
	class StaticPoint : AbstractPoint, StaticFigure
    {
        #region Constructors
            public static StaticPoint Constructor()
            {
                GameObject go = new GameObject();
				go.AddComponent<MeshFilter>();
				go.AddComponent<MeshRenderer>();
				//check if sphere mesh is added.
				go.AddComponent<SphereCollider>();
				go.AddComponent<Rigidbody>();
				go.AddComponent<InteractionBehaviour>();
				go.AddComponent<AnchorableBehaviour>();
				go.AddComponent<StaticPoint>();
				go.GetComponent<SphereCollider>().radius = 0.5f;
				go.GetComponent<Rigidbody>().useGravity = false;
				go.GetComponent<Rigidbody>().isKinematic = true;
				go.GetComponent<InteractionBehaviour>().enabled = false;
                go.GetComponent<AnchorableBehaviour>().maxAnchorRange = 0.3f;
                go.GetComponent<AnchorableBehaviour>().useTrajectory = true;
                go.GetComponent<AnchorableBehaviour>().lockWhenAttached = true;
                go.GetComponent<AnchorableBehaviour>().matchAnchorMotionWhileAttaching = true;
                go.GetComponent<AnchorableBehaviour>().tryAnchorNearestOnGraspEnd = true;
				return go.GetComponent<StaticPoint>();
            }
        #endregion

        public override void initializefigure()
        {
			//do nothing.
		}

		internal override bool RMotion(NodeList<string> inputNodeList)
        {
			return false;
        }

	public override void Stretch(InteractionController obj)
		{
			//do nothing.
		}

		public override void updateFigure()
        {
			//do nothing.
        }

        internal override void GlueToFigure(MasterGeoObj toObj)
        {
			//do nothing.
		}

		internal override void SnapToFigure(MasterGeoObj toObj)
		{
			//do nothing
		}
	}
}
