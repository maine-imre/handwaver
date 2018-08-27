/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver
{
    public class unitvectorScript : MonoBehaviour
    {
		private AnchorableBehaviour thisAbehave;
		
		[Range(0f, 1f)]
		public float anchorScale;

		private Vector3 initScale;

		private void Start()
		{
			initScale = transform.localScale;
			thisAbehave = GetComponent<AnchorableBehaviour>();
			thisAbehave.OnAttachedToAnchor += attach;
			thisAbehave.OnDetachedFromAnchor += detach;
		}

		private void detach()
		{
			transform.localScale /= anchorScale;
		}

		private void attach()
		{
			transform.localScale = initScale;

		}

		void Update()
        {
            if (transform.rotation != Quaternion.identity)
                transform.rotation = Quaternion.identity;
        }
    }
}