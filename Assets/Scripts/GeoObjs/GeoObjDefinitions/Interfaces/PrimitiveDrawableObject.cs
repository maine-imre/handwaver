/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public interface PrimitiveDrawableObject
	{
		/* check if this instance needs to be updated (redrawn) or specify that a new value will be set and a new value to set */
		/*bool NeedsUpdate(bool set_new_value = false, bool new_value = false);
		/* check if this instance's dependencies need to be updated (redrawn) or specify that a new value will be set and a new value to set */
		/*bool DependenciesNeedUpdate(bool set_new_value = false, bool new_value = false);

        /* update less complex objects this object depends on (e.g line updates points) */
        void updateParents();

        /* update less complex objects this object depends on (e.g point updates line) */
        void updateChildren();


        /* performand work that needs to be done before unity calls Update */
        void StageUpdate();
	}
}

