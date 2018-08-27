/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class VolumeEqualityCheck : MonoBehaviour
    {

		public bool verbose = false;

		private static Dictionary<AbstractSolid, float> solidDict;
        private static Dictionary<AbstractSolid, Color> solidColors;


        // Update is called once per second
        public void CheckEquality()
		{
			//if (Input.GetKey(KeyCode.V))
			//{
				updateVolumeDictionary(FindObjectsOfType<AbstractSolid>());

                gradientScheme();

                foreach(AbstractSolid solid in solidColors.Keys)
                {
                    foreach(AbstractPolygon poly in solid.allfaces)
                    {
                        poly.GetComponent<MeshRenderer>().material.color = solidColors[solid];
                    }
                }
			//}

		}

        private void gradientScheme()
        {
            float minArea = solidDict.Values.Min();
            float maxArea = solidDict.Values.Max();
            solidColors = new Dictionary<AbstractSolid, Color>();

            foreach (AbstractSolid solid in solidDict.Keys)
            {
                float value = (solidDict[solid] - minArea) / (maxArea - minArea);
                solidColors[solid] = Color.HSVToRGB(value, 1, 1);
            }
        }

        private void updateVolumeDictionary(AbstractSolid[] abstractSolids)
		{
			foreach (AbstractSolid solid in abstractSolids)
			{
				if (solidDict.ContainsKey(solid))						 //if already contains
					solidDict[solid] = solid.volume;					 //update value
				else
					solidDict.Add(solid, solid.volume);					 //else add as a new Key Value
			}
			solidDict.OrderBy(k => k.Value);
		}

		private void approxequalVolume(KeyValuePair<AbstractSolid,float> abstractSolid1, KeyValuePair<AbstractSolid, float> abstractSolid2)
		{
			float vol1 = abstractSolid1.Value;
			float vol2 = abstractSolid2.Value;
			if (verbose)
			{
				Debug.Log(abstractSolid1.Key.figName + " and " + abstractSolid2 + " have equal areas.");
			}

			float percentDiff1 = (abstractSolid1.Value - abstractSolid2.Value) / (2f * Mathf.Max(abstractSolid1.Value, abstractSolid2.Value));

			//float percentDiff2 = -percentDiff1; NEVER USED

			foreach (AbstractPolygon face in abstractSolid1.Key.allfaces)
			{
				face.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(33 + percentDiff1, 1, 1);
			}

			foreach (AbstractPolygon face in abstractSolid2.Key.allfaces)
			{
				face.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(33 + percentDiff1, 1, 1);
			}
			//don't change texture.        

		}

		private void equalVolume(AbstractSolid abstractSolid1, AbstractSolid abstractSolid2)
        {
			if (verbose)
			{
				Debug.Log(abstractSolid1.figName + " and " + abstractSolid2 + " have approximately equal areas.");
			}
			foreach (AbstractPolygon face in abstractSolid1.allfaces)
			{
				face.GetComponent<MeshRenderer>().material.color = Color.green;
			}

			foreach (AbstractPolygon face in abstractSolid2.allfaces)
			{
				face.GetComponent<MeshRenderer>().material.color = Color.green;
			}
			//also need to change textures.    
		}
	}
}
