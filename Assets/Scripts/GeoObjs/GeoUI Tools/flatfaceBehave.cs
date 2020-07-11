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
/// AbstractGeoObj for planes.
/// </summary>
	class flatfaceBehave : AbstractGeoObj
	{

		#region Constructors
		public static flatfaceBehave Constructor(){
			GameObject go = GameObject.Instantiate(PrefabManager.GetPrefab("Flatface"));
			return go.GetComponent<flatfaceBehave>();
		}
		#endregion
        public Vector3 normalDir
        {
            get
            {
                return this.transform.up;
            }
        }

        internal override void SnapToFigure(AbstractGeoObj toObj)
		{
			// do nothing
		}

		internal override void GlueToFigure(AbstractGeoObj toObj)
		{
			// do nothing
		}

		internal override bool RMotion(NodeList<string> inputNodeList)
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

		internal override Vector3 ClosestSystemPosition(Vector3 abstractPosition)
		{
			return Vector3.Project(abstractPosition - Position3, normalDir) + Position3;
		}
	}
}