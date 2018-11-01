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
			this.transform.position = Vector3.zero;

			GameObject.Instantiate(wallPrefab, Vector3.forward * scaleOfBox+Vector3.up*scaleOfBox+HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.forward), this.transform).localScale = Vector3.one * scaleOfBox / 5f;
			GameObject.Instantiate(wallPrefab, Vector3.back * scaleOfBox + Vector3.up * scaleOfBox+HyperBall.origin + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.back), this.transform).localScale = Vector3.one *  scaleOfBox / 5f;
			GameObject.Instantiate(wallPrefab, Vector3.up * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.up), this.transform).localScale = Vector3.one *  scaleOfBox / 5f;
			GameObject.Instantiate(wallPrefab, Vector3.down * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.down), this.transform).localScale = Vector3.one *  scaleOfBox / 5f;
			GameObject.Instantiate(wallPrefab, Vector3.right * scaleOfBox + Vector3.up * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.right), this.transform).localScale = Vector3.one *  scaleOfBox / 5f;
			GameObject.Instantiate(wallPrefab, Vector3.left * scaleOfBox + Vector3.up * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.left), this.transform).localScale = Vector3.one *  scaleOfBox / 5f;
		}
	}
}
