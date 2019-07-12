using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver{
///<Summary>
/// A way to instantiate prefabs without poolmanaging, while providing static references to other scripts.
///</Summary>
public class PrefabManager : MonoBehaviour
{
	public static PrefabManager ins;

	public List<GameObject> prefabList;
	private static Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

	private void Start()
	{
		ins = this;
		prefabList.ForEach(p =>
		{
			if(!prefabDictionary.ContainsKey(p.name))
				prefabDictionary.Add(p.name, p);
		});
	}

	/// <summary>
	/// Finds a prefab from predetermined Resources folders
	/// To be uesd with a GameObject.Instantiate call.
	/// </summary>
	/// <param name="prefabName">Prefab to be found</param>
	/// <returns>Instance of Prefab</returns>
	internal static GameObject GetPrefab(string prefabName)
	{
		//note that the prefab manager is not a spawner.
		if (prefabDictionary.ContainsKey(prefabName))
		{
			return prefabDictionary[prefabName];
		}
		else
		{
			Debug.LogError("There is no prefab with the name : " + prefabName + " in the prefab dictionary.");
			return null;
		}
	}

	/// <summary>
	/// Finds a prefab from predetermined Resources folders
	/// Instantiates that prefab
	/// </summary>
	/// <param name="prefabName">PrefabToSpawn</param>
	/// <returns>SpawnedPrefab</returns>
	internal static GameObject Spawn(string prefabName)
	{
		return Instantiate(GetPrefab(prefabName));
	}

	/// <summary>
	/// Uses predetermined paths to populate the list of prefab references in the editor.
	/// </summary>
	[ContextMenu("get prefab data")]
	public void gatherPrefabData()
	{
		prefabList = new List<GameObject>();
		prefabList.AddRange(Resources.LoadAll<GameObject>("Prefabs/GeoObj/"));
		prefabList.AddRange(Resources.LoadAll<GameObject>("Prefabs/Tools/"));
	}

}
}
