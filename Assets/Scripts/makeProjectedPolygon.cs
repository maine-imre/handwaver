using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Solver
{
	/// <summary>
	/// This constructs projected polygons for the linear programing test scene.
	/// May be depreciated in the future.
	/// </summary>
	public class makeProjectedPolygon : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
			InteractablePolygon iPoly = GeoObjConstruction.rPoly(5, .2f, Vector3.up * 1.8f,Vector3.up);
			ProjectedPolygon pPoly = iPoly.gameObject.AddComponent<ProjectedPolygon>();
			pPoly.Initialize();
		}

	}
}
