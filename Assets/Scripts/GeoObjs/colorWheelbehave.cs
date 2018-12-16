/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver
{
/// <summary>
/// An attempt at a color selction tool.
/// Not currently funcitonal.
/// </summary>
	public class colorPickerbehave : MonoBehaviour
    {

        public Texture2D colorPicker;
        public int ImageWidth = 100;
        public int ImageHeight = 100;

        void OnGUI()
        {
            if (GUI.RepeatButton(new Rect(10, 10, ImageWidth, ImageHeight), colorPicker))
            {
                Vector2 pickpos = Event.current.mousePosition;
                int aaa = Convert.ToInt32(pickpos.x);
                int bbb = Convert.ToInt32(pickpos.y);
                Color col = colorPicker.GetPixel(aaa, 41 - bbb);

                // "col" is the color value that Unity is returning.
                // Here you would do something with this color value, like
                // set a model's material tint value to this color to have it change
                // colors, etc, etc.
                //
                // Right now we are just printing the RGBA color values to the Console
                Debug.Log(col);
            }
        }

    }
}