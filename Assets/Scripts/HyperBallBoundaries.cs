using IMRE.HandWaver.Networking;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.FourthDimension
{
	/// <summary>
	/// Graphics for boundaries of hyperball.
	/// Part of a networking proof of concept.
	/// </summary>
	public class HyperBallBoundaries : 
#if PHOTON_UNITY_NETWORKING
		MonoBehaviourPunCallbacks {
#else
		MonoBehaviour {
#endif
	#if PHOTON_UNITY_NETWORKING
		public enum GeometeryType {ThreeTorus,ThreeSphere,KleinBottle,TrippleTwist};
		public static GeometeryType myGeometery;


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

			rectangularWalls = new List<Transform>
			{
				makeWall(Vector3.forward * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.forward)),
				makeWall(Vector3.back * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.back)),
				makeWall(Vector3.up * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.up)),
				makeWall(Vector3.down * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.down)),
				makeWall(Vector3.right * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.right)),
				makeWall(Vector3.left * scaleOfBox + HyperBall.origin, Quaternion.FromToRotation(Vector3.up, Vector3.left))
			};
			rectangularWalls.ForEach(wall => HandWaver_GameManager.ins.walls.Add(wall.GetComponent<MeshRenderer>()));


			sphereWall.localScale = Vector3.one * 2*scaleOfBox;
			sphereWall.transform.position = HyperBall.origin;
			Color transparentBlack = Color.black;
			transparentBlack.a = 0.6f; ;
			sphereWall.GetComponent<MeshRenderer>().materials[0].color = transparentBlack;
			
			photonView.RPC("setSpaceType", RpcTarget.All, 0);
		}

		[PunRPC]
		public void setSpaceType(int s)
		{
			if (s == 0)
			{
				myGeometery = GeometeryType.ThreeTorus;
			}
			else if (s == 1)
			{
				myGeometery = GeometeryType.ThreeSphere;
			}
			else if (s == 2)
			{
				myGeometery = GeometeryType.KleinBottle;
			}else
			{
				myGeometery = GeometeryType.TrippleTwist;
			}
			switch (myGeometery)
			{
				case GeometeryType.ThreeTorus:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = true);
					sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				case GeometeryType.ThreeSphere:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = false);
					sphereWall.GetComponent<MeshRenderer>().enabled = true;
					break;
				case GeometeryType.KleinBottle:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = true);
					sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				case GeometeryType.TrippleTwist:
					rectangularWalls.ForEach(wall => wall.GetComponent<MeshRenderer>().enabled = true);
					sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				default:
					break;
			}
		}

		internal void advanceGeometeryType()
		{
			switch (myGeometery)
			{
				case GeometeryType.ThreeTorus:
					photonView.RPC("setSpaceType", RpcTarget.All, 1);
					HandWaver_GameManager.ins.photonView.RPC("setWallState", RpcTarget.All, 0);
					break;
				case GeometeryType.ThreeSphere:
					photonView.RPC("setSpaceType", RpcTarget.All, 2);
					HandWaver_GameManager.ins.photonView.RPC("setWallState", RpcTarget.All, 0);
					break;
				case GeometeryType.KleinBottle:
					photonView.RPC("setSpaceType", RpcTarget.All, 3);
					HandWaver_GameManager.ins.photonView.RPC("setWallState", RpcTarget.All, 0);
					break;
				case GeometeryType.TrippleTwist:
					photonView.RPC("setSpaceType", RpcTarget.All, 0);
					HandWaver_GameManager.ins.photonView.RPC("setWallState", RpcTarget.All, 0);
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
#endif
	}
}
