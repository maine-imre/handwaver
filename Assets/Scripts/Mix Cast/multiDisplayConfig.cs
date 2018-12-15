/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueprintReality.MixCast;

namespace IMRE.HandWaver
{
/// <summary>
/// Sets up multiple displays to have one display on mixcast and the other for first person.
/// </summary>
	public class multiDisplayConfig : MonoBehaviour
    {
        Canvas can;

        // only works in standalone
        void Start()
        {
            if (Screen.fullScreen)
            {
                can = gameObject.GetComponent<Canvas>();
                Debug.Log("displays connected: " + Display.displays.Length);
                // Display.displays[0] is the primary, default display and is always ON.
                // Check if additional displays are available and activate each.
                if (Display.displays.Length > 1)
                {                                       //If second monitor connected
                    if (!Display.displays[1].active)
                        Display.displays[1].Activate(); //Activate monitor
                    can.targetDisplay = 1;               //render to second monitor
                    MixCast.SetActive(true);
                }
            }
        }

    }
}