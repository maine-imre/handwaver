/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class handColorChange : MonoBehaviour
    {
        public Transform hueSlider;
        private Renderer rend;
        private Material mat;

		private Color desired;

		public Color Desired
		{
			get
			{
				return desired;
			}

			set
			{
				desired = value;
				mat.color = value;

			}
		}

		private void Start()
        {
            rend = gameObject.GetComponent<Renderer>();
            mat = rend.material;
        }

    }
}