/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using Leap.Unity.Interaction;

namespace IMRE.HandWaver
{
    /// <summary>
///     Constructs a pair of parallel lines, and initializes them so they will stay parallel on update
/// This will be depreciated with an updated geometery kernel.
    /// </summary>
	public class parallelLinesMaker : MonoBehaviour
    {
        // Use this for initialization
        #region Constructors
		public static parallelLinesMaker Constructor(){
			GameObject go = GameObject.Instantiate(PrefabManager.Spawn("ParallelLines"));
			return go.GetComponent<parallelLinesMaker>();
		}
		#endregion
        internal parallelLines line1;
        internal parallelLines line2;

        void Start()
        {
            line1 = parallelLines.Constructor();
            line2 = parallelLines.Constructor();

            line1.Position3 = this.transform.position.x * Vector3.right + this.transform.position.z * Vector3.forward + 1.4f * Vector3.up;
            line2.Position3 = this.transform.position.x * Vector3.right + this.transform.position.z * Vector3.forward + 1.8f * Vector3.up;

            line1.otherLine = line2;
            line2.otherLine = line1;
        }
    }
}