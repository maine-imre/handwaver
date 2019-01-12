/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{

	/// <summary>
	/// An anchor spawner unique to our repo, inheriting from LeapMotion.
	/// Consider depreciating.
	/// </summary>
	class tutAnchorSpawner : anchorSpawner
	{

		internal override void spawnItem(Transform spawnPoint)
		{
			if (prevItem != null)
			{
				prevItem.transform.localScale /= anchorScale;
			}
			Transform newObj;

				if (item != null)
				{
					newObj = Instantiate(item, spawnPoint.transform.position, spawnPoint.rotation).transform;
					newObj.transform.localScale *= anchorScale;
					newObj.GetComponent<AnchorableBehaviour>().TryAttachToNearestAnchor();
					prevItem = newObj.gameObject;
				}
				else
				{
					Debug.Log("item is not set. please set item or fix pool. Object: " + gameObject.name);
				}

			
		}
	}
}
