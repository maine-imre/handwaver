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

namespace IMRE.HandWaver
{
/// <summary>
/// An abstract class for handwaver tools.
/// This was created to give the eraser functionality.
/// Will be depreciated with UX overhaul.
/// </summary>
	public class HandWaverTools : MonoBehaviour
	{
        public enum ToolType {arctus,bluePin,redPin,greenPin,crossSection,dogBone,eraser,extrude,flashlight,flatface,hoistWheel,revolveWheel,rotationWheel,straightedge,unitVector,none};
        public ToolType toolType = ToolType.none;
    }
}
