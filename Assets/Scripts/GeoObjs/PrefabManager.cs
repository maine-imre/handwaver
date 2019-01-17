using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabManager : MonoBehaviour
{
	public static PrefabManager ins;

	public List<GameObject> prefabList;
	private static Dictionary<string, GameObject> prefabDictionary;

	private void Start()
	{
		ins = this;
		prefabList.ForEach(p => prefabDictionary.Add(p.name, p));
	}

	internal static GameObject Spawn(string prefabName)
	{
		if (prefabDictionary.ContainsKey(prefabName))
		{
			return Instantiate(prefabDictionary[prefabName]);
		}
		else
		{
			Debug.LogError("There is no prefab with the name : " + prefabName + " in the prefab dictionary.");
			return new GameObject();
		}
	}

	[ContextMenu("get prefab data")]
	public void gatherPrefabData()
	{
		prefabList = new List<GameObject>();
		prefabList.AddRange(Resources.LoadAll<GameObject>("Prefabs/GeoObj/"));
		prefabList.AddRange(Resources.LoadAll<GameObject>("Prefabs/Tools/"));
	}

}
