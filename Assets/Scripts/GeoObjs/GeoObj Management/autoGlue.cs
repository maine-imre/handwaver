/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Solver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class autoGlue : MonoBehaviour
    {
        private HW_GeoSolver objGraph;
        private NodeList<string> nodes;

        private void Start()
        {
            objGraph = HW_GeoSolver.ins;
            refreshGraph();
        }

        void refreshGraph()
        {
            nodes = objGraph.geomanager.Nodes;
        }

        public void SetGlue(bool state)
        {
            refreshGraph();
            foreach (Node<string> node in nodes)
            {
                if (node.Value.Contains("Point"))
                {
                    GameObject.Find(node.Value).GetComponent<InteractablePoint>().glueBool = state;
                }
            }
        }
    }
}
