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
	abstract class AbstractSolid : MasterGeoObj
    {
        abstract internal float volume { get; }
        abstract internal AbstractPolygon primaryBase { get; }
		abstract internal List<AbstractPolygon> allfaces { get; }
        internal List<AbstractLineSegment> allEdges {
            get {
                List<AbstractLineSegment> result = new List<AbstractLineSegment>();
                foreach(AbstractPolygon face in allfaces)
                {
                    foreach(AbstractLineSegment line in face.lineList)
                    {
                        if (!result.Contains(line))
                        {
                            result.Add(line);
                        }
                    }
                }
                return result;
            }
        }
    }
}