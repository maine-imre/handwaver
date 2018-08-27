/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class ParallelMarker : AbstractMarker
    {
        private void Start()
        {
            myType = MarkerType.parallel;
        }

        internal override bool Comply(AbstractMarker marker)
        {
            throw new NotImplementedException();
        }

    }

}