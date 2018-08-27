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
//using Leap.Unity.DetectionExamples;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class ColorChooser : MonoBehaviour
	{
		public InteractionSlider colorSlider;
		public InteractionSlider alphaSlider;

		public MeshRenderer[] backdrops;

		public Color thisColor;
		public static ColorChooser ins;

		private void Start()
		{
			ins = this;
			setColor(0f);
			colorSlider.HorizontalSlideEvent += (setColor);
			colorSlider.VerticalSlideEvent += (setColor);
			alphaSlider.VerticalSlideEvent += (setColor);
			alphaSlider.HorizontalSlideEvent += (setColor);
			setColor(0.0f);
		}

		internal void setColor(float value)
		{
			///Hue
			float hue = colorSlider.HorizontalSliderPercent;
			//Sat
			float sat = colorSlider.VerticalSliderPercent;

			//lumonsity
			float lum = alphaSlider.HorizontalSliderPercent;

			//alpha
			float alpha = alphaSlider.VerticalSliderPercent;

			thisColor = Color.HSVToRGB(hue, sat, lum);
			thisColor.a = alpha;

			foreach(MeshRenderer mr in backdrops)
			{
				mr.material.color = thisColor;
			}

            foreach(paintbucketBehave bucket in GameObject.FindObjectsOfType<paintbucketBehave>())
            {
                bucket.newColor = thisColor;
            }

			//PinchDraw.ins.DrawColor = thisColor;
		}

	}
}
