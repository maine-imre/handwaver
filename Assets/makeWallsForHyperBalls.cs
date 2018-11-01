using IMRE.HandWaver.Networking;
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
		public List<MeshRenderer> wallsTMP;

		// Use this for initialization
		void Start()
		{
			this.transform.position = Vector3.zero;

			makeWall(Vector3.forward * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.forward));
			makeWall(Vector3.back * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.back));
			makeWall(Vector3.up * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.up));
			makeWall(Vector3.down * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.down));
			makeWall(Vector3.right * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.right));
			makeWall(Vector3.left * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.left));
		}

		private void makeWall(Vector3 pos, Quaternion rot)
		{
			Transform t = Instantiate(wallPrefab, pos, rot, transform);
			t.localScale = Vector3.one * scaleOfBox / 5f;
			wallsTMP.Add(t.GetComponent<MeshRenderer>());
		}
	}
}
