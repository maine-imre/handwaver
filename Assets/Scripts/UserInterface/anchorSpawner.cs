/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class anchorSpawner : MonoBehaviour
	{
		[Range(0f, 1f)]

		public float anchorScale;

		public GameObject item;

		public bool itemPooled;
		public string poolName;
		public string objName;

		public GameObject prevItem;

		private Anchor thisAnchor;

		void Start()
		{
			thisAnchor = GetComponent<Anchor>();
			thisAnchor.OnNoAnchorablesAttached += detached;
			spawnItem(this.transform);
		}

		private void OnEnable()
		{
			if(prevItem!= null)
				prevItem.SetActive(true);	
		}


		private void OnDisable()
		{
			if (prevItem != null)
				prevItem.SetActive(false);

		}

		private void detached()
		{
			spawnItem(this.transform);
		}

		public void respawn()
		{
			if(item == null)
			{
				detached();
			}
		}

		public void despawn()
		{
			if (item != null)
			{
				PoolManager.Pools[poolName].Despawn(item.transform);
				item = null;
			}
		}

		internal virtual void spawnItem(Transform spawnPoint)
		{
			if(prevItem != null)
			{
				prevItem.transform.localScale /= anchorScale;
			}
			Transform newObj;
			if (!itemPooled)
			{
				if (item != null)
				{
					newObj = Instantiate(item, spawnPoint.transform.position, spawnPoint.rotation).transform;
					newObj.transform.localScale *= anchorScale;
					newObj.GetComponent<AnchorableBehaviour>().TryAttachToNearestAnchor();
					prevItem = newObj.gameObject;
				}
				else
				{
					Debug.Log("item is not set. please set item or fix pool. Object: "+gameObject.name);
				}

			}
			else
			{
				newObj = PoolManager.Pools[poolName].Spawn(objName).transform;

				newObj.transform.position = gameObject.transform.position;
				newObj.transform.localScale *= anchorScale;

				newObj.GetComponent<AnchorableBehaviour>().useTrajectory = false;
				newObj.GetComponent<AnchorableBehaviour>().TryAttachToNearestAnchor();
				newObj.GetComponent<AnchorableBehaviour>().useTrajectory = true;

				prevItem = newObj.gameObject;
				//Debug.Log(newObj.name + " properly spawned with prevItem set to " + prevItem.name + " with a poolmanager!");

			}

		}
	}
}
