using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver{
	/// <summary>
	/// a manager of managers for object Pooling.
	/// we will either develop an open source solution for object pooling here, or just use instancing when nececssary.
	/// Replaccement for PoolManager by Pathalogical games to depreciate closed source dependency.
	/// </summary>
public static  class PoolManager {
	public static Dictionary<string, PoolManagerPool> Pools = new Dictionary<string, PoolManagerPool>();
}

/// <summary>
/// A pool that exists on a gameobject in a scene.
/// Named with GameObject.name.
/// </summary>
public class PoolManagerPool : MonoBehaviour {

	public Transform[] prefabs;
	public Dictionary<string, Transform> prefabDictionary = new Dictionary<string, Transform>();

	public Dictionary<string, Transform> instanceDictionary = new Dictionary<string, Transform>();
	void Start(){
		PoolManager.Pools.Add(this.name,this);
		foreach(Transform T in prefabs){
			prefabDictionary.Add(T.name,T);
		}		
	}

/// <summary>
/// This function needs to spawn objects from a prefab dictionary.
/// Also needs to be able to deal with "ONSPAWNED" calls.
/// Also should switch to an enum system eventually.
/// </summary>
/// <param name="s"></param>
/// <returns></returns>
	public Transform Spawn(string s)
	{
		if(prefabDictionary.Keys.Contains(s))
		{
		Transform t = GameObject.Instantiate(prefabDictionary[s]);
		instanceDictionary.Add(t.name,t);
		return t;
		}
		else
		{
			Debug.LogWarning("Prefab not found in Pool");
			return null;
		}
	}

	public Transform Spawn(string s, Vector3 position, Quaternion rotation){
		Transform t = Spawn(s);
		t.position = position;
		t.rotation = rotation;
		return t;
	}

	public Transform Spawn(string s, Vector3 position, Quaternion rotation, Transform parent){
		Transform t = Spawn(s,position,rotation);
		t.parent = parent;
		return t;
	}

	public Transform Spawn(string s, Transform parent){
				Transform t = Spawn(s);
		t.parent = parent;
		return t;
	}

	public void Despawn(Transform t){
		GameObject.Destroy(t);
		instanceDictionary.Remove(t.name);
	}
}
}