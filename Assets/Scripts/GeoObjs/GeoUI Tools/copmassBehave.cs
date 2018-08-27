/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class copmassBehave : HandWaverTools
	{
#pragma warning disable 0649

		public Transform controlSwitch;
#pragma warning restore 0649

		// Use this for initialization
		void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void switchToggle()
        {
            this.GetComponent<LineRenderer>().enabled = controlSwitch.GetComponent<Toggle>().isOn;
        }
    }
}