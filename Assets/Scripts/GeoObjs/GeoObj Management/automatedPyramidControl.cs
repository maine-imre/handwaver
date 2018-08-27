/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
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
	public class automatedPyramidControl : MonoBehaviour
    {
        public Toggle showBlue;

        public void handleInput(string result)
        {
            switch (result)
            {
                case "toggleBlue":
                    showBlue.isOn = !showBlue.isOn;
                    break;
                case "showBlueOn":
                    showBlue.isOn = true;
                    break;
                case "showBlueOff":
                    showBlue.isOn = false;
                    break;
                default:
                    break;
            }
        }
    }
}
