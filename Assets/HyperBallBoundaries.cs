using IMRE.HandWaver.Networking;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.FourthDimension
{
	public class HyperBallBoundaries : MonoBehaviourPunCallbacks
	{
		public enum SpaceType {ThreeTorus,ThreeSphere,KleinBottle,TrippleTwist};
		public static SpaceType Space;


		private float scaleOfBox
		{
			get
			{
				return HyperBall.scaleOfBox;
			}
		}
		public Transform wallPrefab;
		internal List<Transform> rectangularWalls;
		public Transform sphereWall;


		// Use this for initialization
		void Start()
		{
			this.transform.position = Vector3.zero;

			rectangularWalls.Add(makeWall(Vector3.forward * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.forward)));
			rectangularWalls.Add(makeWall(Vector3.back * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.back)));
			rectangularWalls.Add(makeWall(Vector3.up * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.up)));
			rectangularWalls.Add(makeWall(Vector3.down * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.down)));
			rectangularWalls.Add(makeWall(Vector3.right * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.right)));
			rectangularWalls.Add(makeWall(Vector3.left * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.left)));
			sphereWall.localScale = Vector3.one * scaleOfBox;
			sphereWall.transform.position = HyperBall.origin;
			Color transparentBlack = Color.black;
			transparentBlack.a = 0.6f; ;
			sphereWall.GetComponent<MeshRenderer>().materials[0].color = transparentBlack;
			
			photonView.RPC("setSpaceType", RpcTarget.All, SpaceType.ThreeTorus);
		}

		[PunRPC]
		public void setSpaceType(SpaceType s)
		{
			Space = s;
			switch (Space)
			{
				case SpaceType.ThreeTorus:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = true);
					sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				case SpaceType.ThreeSphere:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = false);
					sphereWall.GetComponent<MeshRenderer>().enabled = true;
					break;
				case SpaceType.KleinBottle:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = true);
					sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				case SpaceType.TrippleTwist:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = true);
					sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				default:
					break;
			}
		}

		private Transform makeWall(Vector3 pos, Quaternion rot)
		{
			Transform t = Instantiate(wallPrefab, pos, rot, transform);
			t.localScale = Vector3.one * scaleOfBox / 5f;
			return t;
		}
	}
}
