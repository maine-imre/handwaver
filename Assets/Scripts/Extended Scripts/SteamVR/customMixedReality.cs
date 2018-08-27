/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class customMixedReality : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 2.0f;
            GameObject.Find("External Camera").transform.GetChild(0).gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}