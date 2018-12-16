/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
namespace IMRE.HandWaver
{
    /// <summary>
    /// An exntension of straightedgebehave that makes two lines (straightedges) paralle and stay parallel on update based on isGrasped state.
    /// </summary>
	class parallelLines : straightEdgeBehave
    {
        public straightEdgeBehave otherLine;

        public List<MasterGeoObj> attachedObjs;

        private void Update()
        {

            if (this.GetComponent<InteractionBehaviour>().isGrasped && otherLine.transform.rotation != this.transform.rotation)
            {
                otherLine.transform.rotation = this.transform.rotation;
            }

            foreach (MasterGeoObj obj in attachedObjs)
            {
                Vector3 newpos = Vector3.Project(obj.Position3 - this.Position3, normalDir) + this.Position3;
                if (obj.Position3 != newpos)
                {
                    obj.Position3 = newpos;
                    obj.AddToRManager();
                }
            }
        }
    }
}