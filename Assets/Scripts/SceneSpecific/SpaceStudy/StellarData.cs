using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// Script Created 8.13.18 by Timothy Bruce
	/// Based on the SkyboxGenerator script, this takes star data and returns the data as cartesian coordinates.
	/// This program uses the HYG database to get its data.
	/// </summary>
	public static class StellarData
	{
		
		public class Star
		{                                                               //This class has all the information for each star. It's one of my more STELLAR ideas.
			public int id;                                              //The database ID of the star.
			public string ProperName;                                   //The proper name of the star. Note that the Sun is marked SOL, Cody!
			public float distance;                                      //The distance from the Sun to the designated star in parsecs.
			public float VisualMagnitude;                               //The star's appearent visual magnitude (from sol I think).
			public float ColorIndex;                                    //The color index of the star blue magnitude - visual magnitude.
			public Vector3 position;                                    //The X Y Z position of the star relative to offset center.
			public Vector3 velocity;                                    //The X Y Z velocity of the star (without offset).
			public float luminousity;                                   //The luminousity of the star as a multiple of solar luminosity.'

			

			public Star(int idi, string ProperNamei, float distancei, float VisualMagnitudei, float ColorIndexi, Vector3 positioni, Vector3 velocityi, float luminousityi)
			{
				id = idi;                                               //This function is a constructor for the Star class.
				ProperName = ProperNamei;
				distance = distancei;
				VisualMagnitude = VisualMagnitudei;
				ColorIndex = ColorIndexi;
				position = positioni;
				velocity = velocityi;
				luminousity = luminousityi;
			}
		}

		public static List<Star> Stars = new List<Star>();                         //A list of stars (this is where you get your output).

		/// <summary>
		/// Function to call to generate the star data.
		/// </summary>
		/// <param name="offset">the distance to coordinate center from the sun at J2000.</param>
		/// <returns>The position of stars where the earth is at Vector3.zero</returns>
		public static void getData(Vector3 offset)
		{
			Stars.Clear();                                                  //J2000 is a hard concept to grasp because it's in TT or "Terrestrial Time" which is a number of seconds off GMT. Good luck.
			TextAsset hyg = Resources.Load<TextAsset>("Databases/hygdata_v3");
			string[] hygLines = hyg.text.Split('\n');

			for (int i = 1; i < hygLines.Length; i ++)
			{
				string[] lv = hygLines[i].Split(',');
				//(below) Add star class to list with data from line.

				if (lv.Length >16 && lv[16] != "")
				{
					Stars.Add(new Star(int.Parse(lv[0]), lv[6], float.Parse(lv[9]), float.Parse(lv[13]), float.Parse(lv[16]), new Vector3(float.Parse(lv[17]), float.Parse(lv[18]), float.Parse(lv[19])) + offset, new Vector3(float.Parse(lv[20]), float.Parse(lv[21]), float.Parse(lv[22])), float.Parse(lv[33])));
				}
			}
		}
	}
}