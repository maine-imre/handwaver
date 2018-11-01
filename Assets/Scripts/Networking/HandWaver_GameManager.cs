using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using IMRE.HandWaver.FourthDimension;

namespace IMRE.HandWaver.Networking { 
	public class HandWaver_GameManager : MonoBehaviourPunCallbacks
	{
		/// <summary>
		/// This is the scene to be loaded after disconnecting
		/// </summary>
		public string lobbyScene;

		[Tooltip("The prefab to use for representing the player")]
		public GameObject playerPrefab;

		public List<GameObject> localPlayers;
		public List<MeshRenderer> walls;
		public List<Transform> balls;
		public List<Vector3> ballSpawnPos;
		public makeWallsForHyperBalls makeWallScript;

		public static HandWaver_GameManager ins;

		#region Photon Callbacks

		private void Start()
		{
			ins = this;
			if (playerPrefab == null)
			{
				Debug.LogError("Missing playerPrefab Reference. Please set it up in Game Manager");
			}
			else
			{
				Debug.LogFormat("We are Instantiating localPlayer");
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				GameObject newPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
				localPlayers.Add(newPlayer);
				newPlayer.GetComponent<playerHead>().setupPlayer(PhotonNetwork.NickName, localPlayers.IndexOf(newPlayer), UnityEngine.Random.Range(0f,1f));
			}
			walls.AddRange(makeWallScript.wallsTMP);

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
				photonView.RPC("resetBall", RpcTarget.All);
			}
		}

		[PunRPC]
		private void setWallState(int v)
		{
			switch (v)
			{
				case 0:
					walls.ForEach(w => w.enabled = false);
					break;
				case 1:
					walls.ForEach(w => w.enabled = true);
					Color transparentBlack = Color.black;
					transparentBlack.a = 0.6f;
					walls.ForEach(w => w.material.color = transparentBlack);

					break;
				case 2:
					walls.ForEach(w => w.enabled = true);

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
					break;
				default:
					Debug.LogError("you coded this wrong. Only accepts 0..2");
					break;
			}
		}

		[PunRPC]
		private void resetBalls()
		{
			for (int i = 0; i < balls.Count; i++)
			{
				balls[i].position = ballSpawnPos[i];
				balls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
				balls[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			}
		}



		/// <summary>
		/// Called when the local player left the room.
		/// </summary>
		public override void OnLeftRoom()
		{
			Application.Quit();
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
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();
			}
		}


		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();
			}
		}


		#endregion

		[ContextMenu("Get Ball Positions")]
		public void getBallPos()
		{
			ballSpawnPos.Clear();
			foreach (Transform ball in balls)
			{
				ballSpawnPos.Add(ball.position);
			}
		}
	}
}