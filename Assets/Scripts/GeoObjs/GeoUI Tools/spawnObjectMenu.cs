/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	class spawnObjectMenu : HandWaverTools
	{
#pragma warning disable 0649

		public string poolName;
        public string objName;

        public GameObject newObj;

        public Transform spawnPos;
#pragma warning restore 0649

		public void instantiateNewObj()
        {
            GameObject temp  = GameObject.Instantiate(newObj);
            temp.transform.position = spawnPos.transform.position;
        }

        public void spawnNewObj()
        {
            Transform newObj = PoolManager.Pools[poolName].Spawn(objName).transform;
            newObj.transform.position = spawnPos.transform.position;
        }

    }
}
