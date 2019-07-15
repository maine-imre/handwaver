using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using Photon.Realtime;
#endif
using System.Collections.Generic;
using IMRE.HandWaver.FourthDimension;

namespace IMRE.HandWaver.Networking { 
	/// <summary>
	/// The game manager manages the networked instances of players and objects for the hyperball scene.
	/// The name of this scene should be updated to reflect it's relationship to the hyperball scene.
	/// </summary>
	public class HandWaver_GameManager : MonoBehaviourPunCallbacks
	{
		#if PHOTON_UNITY_NETWORKING
		/// <summary>
		/// This is the scene to be loaded after disconnecting
		/// </summary>
		public string lobbyScene;

		[Tooltip("The prefab to use for representing the player")]
		public string playerPrefabPath;

		public List<GameObject> localPlayers;
		public List<MeshRenderer> walls;
		public List<HyperBall> balls;
		public List<Vector3> ballSpawnPos;
		public HyperBallBoundaries makeWallScript;

		public static HandWaver_GameManager ins;

		#region Photon Callbacks

		private void Awake()
		{
			ins = this;
		}

		private void Start()
		{
			if (playerPrefabPath == null)
			{
				Debug.LogError("Missing playerPrefab Reference. Please set it up in Game Manager");
			}
			else
			{
				Debug.LogFormat("We are Instantiating localPlayer");
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefabPath, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
				localPlayers.Add(newPlayer);
				newPlayer.GetComponent<playerHead>().setupPlayer(PhotonNetwork.NickName, localPlayers.IndexOf(newPlayer), UnityEngine.Random.Range(0f,1f));
			}

			photonView.RPC("setWallState", RpcTarget.AllBuffered, 0);

		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				photonView.RPC("setWallState", RpcTarget.All, 0);
			}
			if (Input.GetKeyDown(KeyCode.F2))
			{
				photonView.RPC("setWallState", RpcTarget.All, 1);

			}
			if (Input.GetKeyDown(KeyCode.F3))
			{
				photonView.RPC("setWallState", RpcTarget.All, 2);

			}
			if (Input.GetKeyDown(KeyCode.F4))
			{
				resetBalls();
			}
			if (Input.GetKeyDown(KeyCode.F5))
			{
				makeWallScript.advanceGeometeryType();
			}
		}

		[PunRPC]
		private void setWallState(int v)
		{
			bool boxOn = HyperBallBoundaries.myGeometery != HyperBallBoundaries.GeometeryType.ThreeSphere;

			switch (v)
			{
				case 0:
					walls.ForEach(w => w.enabled = false);
					makeWallScript.sphereWall.GetComponent<MeshRenderer>().enabled = false;
					break;
				case 1:
					walls.ForEach(w => w.enabled = boxOn);
					makeWallScript.sphereWall.GetComponent<MeshRenderer>().enabled = !boxOn;
					Color transparentBlack = Color.black;
					transparentBlack.a = 0.6f;
					walls.ForEach(w => w.material.color = transparentBlack);
					makeWallScript.sphereWall.GetComponent<MeshRenderer>().material.color = transparentBlack;
					break;
				case 2:
					walls.ForEach(w => w.enabled = boxOn);
					makeWallScript.sphereWall.GetComponent<MeshRenderer>().enabled = !boxOn;
					for (int i = 0; i < 6; i++)
					{
						Color newWallColor;
						if (i < 2)
						{
							newWallColor = Color.blue;
							newWallColor.a = 0.6f;
						}
						else if( i < 4)
						{
							newWallColor = Color.red;
							newWallColor.a = 0.6f;
						}
						else
						{
							newWallColor = Color.green;
							newWallColor.a = 0.6f;
						}
						walls[i].material.color = newWallColor;
					}
					makeWallScript.sphereWall.GetComponent<MeshRenderer>().material.color = Color.blue;

					break;
				default:
					Debug.LogError("you coded this wrong. Only accepts 0..2");
					break;
			}
		}

		private void resetBalls()
		{
			for (int i = 0; i < balls.Count; i++)
			{
				balls[i].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
				balls[i].transform.position = ballSpawnPos[i];
				balls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
				balls[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				balls[i].GetComponent<PhotonView>().TransferOwnership(0);

			}
		}



		/// <summary>
		/// Called when the local player left the room.
		/// </summary>
		public override void OnLeftRoom()
		{
			//Application.Quit();
		}


		#endregion


		#region Public Methods


		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}


		#endregion

		#region Private Methods


		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			else
			{
				Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
				PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
			}
		}


		#endregion

		#region Photon Callbacks


		public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", other.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();
			}
		}


		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", other.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();
			}
		}


		#endregion

		[ContextMenu("Get Ball Positions")]
		public void getBallPos()
		{
			ballSpawnPos.Clear();
			foreach (HyperBall ball in FindObjectsOfType<HyperBall>())
			{
				balls.Add(ball);
				ballSpawnPos.Add(ball.transform.position);
			}
		}
		#endif
	}
}