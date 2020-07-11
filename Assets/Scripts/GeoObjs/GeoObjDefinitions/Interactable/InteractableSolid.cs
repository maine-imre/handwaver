/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using Leap.Unity.Interaction;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace IMRE.HandWaver
{
/// <summary>
/// Unfinished attempt at interactable solids.
/// Since they can't be rendered fully, it's hard to imagine the interaction.
/// </summary>
	class InteractableSolid : AbstractSolid, InteractiveFigure
    {
        internal override float volume
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal override AbstractPolygon primaryBase { get { throw new NotImplementedException(); } }

		internal override List<AbstractPolygon> allfaces
		{
			get
			{
				throw new NotImplementedException();
			}
		}

        public bool isGraspedLeap()
        {
            throw new NotImplementedException();
        }

        public bool isGraspedVive()
        {
            throw new NotImplementedException();
        }

        public void OnTriggerEnter(Collider col)
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

		public override void updateFigure()
        {
            throw new NotImplementedException();
        }

        internal override void GlueToFigure(AbstractGeoObj toObj)
        {
            throw new NotImplementedException();
        }

        internal override void SnapToFigure(AbstractGeoObj toObj)
		{
			//do nothing
		}
	}
}