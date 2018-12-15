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
    /// Changes the renderQueue on a figure.
    /// </summary>
	public class instanceShader : MonoBehaviour
    {
        public int change = -1;
        void Start()
        {
            gameObject.GetComponent<Renderer>().material.renderQueue += change;                                   //changes render queue to be one less
        }

    }
}
