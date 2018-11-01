using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

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

		#region Photon Callbacks

		private void Start()
		{
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
				newPlayer.GetComponent<playerHead>().setupPlayer(PhotonNetwork.NickName, localPlayers.IndexOf(newPlayer), UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1));
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
	}
}