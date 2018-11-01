using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.FourthDimension
{
	public class makeWallsForHyperBalls : MonoBehaviour
	{
		private float scaleOfBox
		{
			get
			{
				return HyperBall.scaleOfBox;
			}
		}
		public Transform wallPrefab;

		// Use this for initialization
		void Start()
		{
			GameObject.Instantiate(wallPrefab, Vector3.forward * scaleOfBox, Quaternion.FromToRotation(Vector3.up, Vector3.forward), this.transform).localScale = Vector3.one * 0.1f * scaleOfBox / 2f;
			GameObject.Instantiate(wallPrefab, Vector3.back * scaleOfBox, Quaternion.FromToRotation(Vector3.up, Vector3.back), this.transform).localScale = Vector3.one * 0.1f * scaleOfBox / 2f;
			GameObject.Instantiate(wallPrefab, Vector3.up * 2 * scaleOfBox, Quaternion.FromToRotation(Vector3.up, Vector3.up), this.transform).localScale = Vector3.one * 0.1f * scaleOfBox / 2f;
			GameObject.Instantiate(wallPrefab, Vector3.down * 0 * scaleOfBox, Quaternion.FromToRotation(Vector3.up, Vector3.down), this.transform).localScale = Vector3.one * 0.1f * scaleOfBox / 2f;
			GameObject.Instantiate(wallPrefab, Vector3.right * scaleOfBox, Quaternion.FromToRotation(Vector3.up, Vector3.right), this.transform).localScale = Vector3.one * 0.1f * scaleOfBox / 2f;
			GameObject.Instantiate(wallPrefab, Vector3.left * scaleOfBox, Quaternion.FromToRotation(Vector3.up, Vector3.left), this.transform).localScale = Vector3.one * 0.1f * scaleOfBox / 2f;
		}
	}
}
