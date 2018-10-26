using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Interface
{
	[RequireComponent(typeof(Leap.Unity.Interaction.InteractionBehaviour))]
	/// <summary>
	/// Add to an object in a content layer to enable scaling.
	/// </summary>
	public class ScaleMeDynamic : MonoBehaviour
	{
		private void Start()
		{
			this.GetComponent<MeshRenderer>().materials[0].color = Color.HSVToRGB(Random.Range(0,1), 1, 1);
		}
		private void Update()
		{
			this.transform.localScale = Vector3.one * worldScaleModifier.ins.AbsoluteScale;
		}
	}
}