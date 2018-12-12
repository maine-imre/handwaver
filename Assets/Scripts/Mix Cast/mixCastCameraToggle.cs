/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueprintReality.MixCast;

namespace IMRE.HandWaver
{
/// <summary>
/// Depreciated after MixCast integrated into their system.
/// </summary>
	public static class mixCastCameraToggle
    {

         /// <summary>
         /// Toggles all Mix Cast Cameras within the scene
         /// </summary>
        public static void toggleCameraz()
        {

            MixCast.SetActive(!(MixCast.Active));
        }

        /// <summary>
        /// Sets all Mix Cast Cameras within the scene to value passed in.
        /// </summary>
        /// <param name="val">Value you want for cameras within scene</param>
        public static void CamerazSet(bool val)
        {
            MixCast.SetActive(val);
        }

    }
}