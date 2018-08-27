/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver
{
    class boneBehave : HandWaverTools
    {

#pragma warning disable 0649, 0414

		public GameObject toolbox;
        public int time_to_live;
        private InteractionBehaviour iBehave;

        private bool gravityEnable = false;
        private bool allowdespawn;

#pragma warning disable 0649, 0414

		void Start()
		{
			GetComponent<AnchorableBehaviour>().OnDetachedFromAnchor += init;

		}

		private void init()
		{
			iBehave = gameObject.GetComponent<InteractionBehaviour>();
			toolbox = GameObject.Find("BearToolBox[Bannannas]");
			toolbox.GetComponent<toolboxfollower>().bone = gameObject;
			StartCoroutine(allowDelete());
		}
		

        public void despawn()
        {
            if (allowdespawn && !iBehave.isGrasped)
                Destroy(gameObject);
            
        }

        
        IEnumerator allowDelete()
        {
            yield return new WaitForSeconds(time_to_live);
            allowdespawn = true;
            if (toolbox.GetComponent<toolboxfollower>().bone != this.gameObject)
            {
                Destroy(this.gameObject);
            }
            yield break;
        }
    }
}

