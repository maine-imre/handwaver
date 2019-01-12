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
				#region Constructors
            public static parallelLines Constructor()
            {
                GameObject go = new GameObject();
				go.AddComponent<LineRenderer>();
				//check if sphere mesh is added.
				go.AddComponent<CapsuleCollider>();
				go.AddComponent<Rigidbody>();
				go.GetComponent<Rigidbody>().useGravity = false;
				go.GetComponent<Rigidbody>().isKinematic = false;
				go.AddComponent<InteractionBehaviour>();
                go.GetComponent<InteractionBehaviour>().ignoreContact = true;
                go.GetComponent<InteractionBehaviour>().ignoreGrasping = true;
				return go.AddComponent<parallelLines>();
            }
        #endregion

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