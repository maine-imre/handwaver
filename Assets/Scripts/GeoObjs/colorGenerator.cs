/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public static class colorGenerator
    {
        private static bool experimental = false;

        /// <summary>
        /// for Research Experiment, make everything grey.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Color randomColorTransparent(Material mat)
        {
            int tempRand = Random.Range(0, 3);
            //if (experimental)
            //{
            //    //for research experiment;
            //    tempRand = 4;
            //}
            Color tempColor = mat.color;
            switch (tempRand)
            {
                case 0:
                    tempColor = new Color(133/255f, 130/255f, 225/255f, 0.43137254902f);        //blue
                    break;
                case 1:
                    tempColor = new Color(1f, 0.11764705882f, 0.19607843137f, 0.43137254902f);  //red
                    break;
                case 2:
                    tempColor = new Color(1f, 1f, 0.19607843137f, 0.43137254902f);              //yellow
                    break;
                //case 3:
                //    tempColor = new Color(101 / 255f, 225 / 255f, 85 / 255f, 0.43137254902f);   //green
                //    break;
                case 4:
                    tempColor = Color.grey;
                    break;
            }
            return tempColor;
        }

		public static Color randomColorTransparent()
		{
			int tempRand = Random.Range(0, 3);
			//if (experimental)
			//{
			//    //for research experiment;
			//    tempRand = 4;
			//}
			Color tempColor = new Color();
			switch (tempRand)
			{
				case 0:
					tempColor = new Color(133 / 255f, 130 / 255f, 225 / 255f, 0.43137254902f);        //blue
					break;
				case 1:
					tempColor = new Color(1f, 0.11764705882f, 0.19607843137f, 0.43137254902f);  //red
					break;
				case 2:
					tempColor = new Color(1f, 1f, 0.19607843137f, 0.43137254902f);              //yellow
					break;
				//case 3:
				//    tempColor = new Color(101 / 255f, 225 / 255f, 85 / 255f, 0.43137254902f);   //green
				//    break;
				case 4:
					tempColor = Color.grey;
					break;
			}
			return tempColor;
		}

		public static Color randomColorSolid(Material mat)
        {
            //for research experiment;
            int tempRand = 4;
            //int tempRand = Random.Range(0, 3);
            Color tempColor = mat.color;
            switch (tempRand)
            {
                case 0:
                    tempColor = new Color(133/ 255f, 130 / 255f, 225 / 255f, 1f);   //blue
                    break;
                case 1:
                    tempColor = new Color(1f, 0.11764705882f, 0.19607843137f, 1f);  //red
                    break;
                case 2:
                    tempColor = new Color(1f, 1f, 0.19607843137f, 1f);              //yellow
                    break;
                case 4:
                    tempColor = Color.grey;
                    break;
                    //case 3:
                    //    tempColor = new Color(101 / 255f, 225 / 255f, 85 / 255f, 1f);   //green
                    //    break;

            }
            return tempColor;
        }
    }
}