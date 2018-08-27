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
	public interface UpdatableFigure
    {
        /// <summary>
        /// Called by the ReactionManager on GeoObjMan
        /// Calculates position and position of related objects.
        /// Returns bool: true == has moved, false == has not moved
        /// </summary>
        /// <param name="inputNodeList">
        /// A list of Graphnodes that have moved
        /// </param>
        /// <returns>
        /// Returns bool: true == has moved, false == has not moved
        /// </returns>
        bool reactMotion(NodeList<string> inputNodeList);

        /// <summary>
        /// Updates the mesh or line.
        /// Called on lateupdate by updateManager on GeoObjMan
        /// </summary>
        void updateFigure();

        /// <summary>
        /// Creates first instance of mesh or line for renderer.
        /// Nromall run OnSpawned or at Start
        /// </summary>
        void initializefigure();
    }
}