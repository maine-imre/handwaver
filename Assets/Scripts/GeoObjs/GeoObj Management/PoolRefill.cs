/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using PathologicalGames;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{


    [System.Serializable]
	public struct ManagedPool
	{
		[Tooltip ("name of the pool to manage")]
		public string poolName;
		[Tooltip ("if checked this will despawn oldest instance rather than not spawn anything when the global prefab limit is reached ")]
		public bool useFIFOInsteadofHardLimit;
		[Tooltip ("when the number of unused prefabs in a particualar prefab's pool drops below this number add prefabs to the pool")]
		public int refillThreshold; //when the number of unused prefabs in a particualar prefab's pool drops below this number add prefabs to the pool
		[Tooltip ("when the number of prefabs in each prefab pool reached this number do not add more (0 = unlimited)")]
		public int globalLimit;

	


	}

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class PoolRefill : MonoBehaviour
	{

        [Tooltip("/enables additional logging, tends to flood the console so only enable if debugging this script")]
        public bool verbose = false;
        public List<ManagedPool> PoolList;
        private List<ManagedPool> managedPoolList;

		void Start ()
		{
            this.managedPoolList = new List<ManagedPool>();
            foreach (ManagedPool p in this.PoolList)
            {
                InitializePoolManagement(p);
            }


        }

        /// <summary>
        /// configures the pool manager for p and if p is dynamically sized it adds p to the list of pools managed by this script
        /// </summary>
        /// <param name="p"></param>
        private void InitializePoolManagement(ManagedPool p)
        {
            string name = p.poolName;
            foreach (string prefabName in PoolManager.Pools[name].prefabs.Keys)
            {
                int poolSize = PoolManager.Pools[name].prefabPools[prefabName].preloadAmount;
                int currentlySpawned = PoolManager.Pools[name].prefabPools[prefabName].spawned.Count;
                if (p.globalLimit > 0)
                {//apply settings for non-dynamic pools
                    PoolManager.Pools[name].prefabPools[prefabName].limitInstances = true;
                    PoolManager.Pools[name].prefabPools[prefabName].limitAmount = p.globalLimit;
                    PoolManager.Pools[name].prefabPools[prefabName].limitFIFO = p.useFIFOInsteadofHardLimit;
                }
                if (verbose)
                {
                    Debug.Log("prefab: " + prefabName + " pool size: " + poolSize + " pool max size: " + " number spawned: " + currentlySpawned);
                }
            }
            if (p.globalLimit < 0) { // if p is dynamically sized add the pool to the list of pools we manage
            this.managedPoolList.Add(p);
            }
        }

		void Update ()
		{
			foreach (ManagedPool p in this.managedPoolList) {
				if (p.globalLimit <= 0) { //foreach pool that has a dynamic size
                    
					string name = p.poolName;
					foreach (string prefabName in PoolManager.Pools [name].prefabs.Keys) {
						int poolSize = PoolManager.Pools [name].prefabPools [prefabName].preloadAmount;
						int currentlySpawned = PoolManager.Pools [name].prefabPools [prefabName].spawned.Count;
						if (poolSize - currentlySpawned < p.refillThreshold && currentlySpawned > 0) { 
                            // if there's less prefabs availible than the minumum that we want to have availible at all times and there are > 0 instances spawned
                            if (verbose)
                            {
                                Debug.Log ("prefab: " + prefabName + " (in " + p.poolName + ") pool size: " + poolSize + " number availible: " + (poolSize - currentlySpawned) + " refill threshold: " + p.refillThreshold + " increasing pool size...");
                            }
                            int refill_amount = poolSize - currentlySpawned - p.refillThreshold;
                            PoolManager.Pools [name].prefabPools [prefabName].preloadAmount += refill_amount;
							poolSize = PoolManager.Pools [name].prefabPools [prefabName].preloadAmount;
							if (verbose) {
								Debug.Log ("prefab: " + prefabName + " pool size: " + poolSize + " number availible: " + (poolSize - currentlySpawned));
							}
				
						}
					}
				}
                else
                {
                    Debug.LogError("non-dynamically sized pool " + p.poolName + " is being managed by poolRefill, is something attempting to modify this.poolNames after initialization?");
                    // this script does not support adding additional pools after initialization, if you get this error it probably means something is modifying the list of pools this script manages after the constructor has been called
                }
			}
		}
	}
}
