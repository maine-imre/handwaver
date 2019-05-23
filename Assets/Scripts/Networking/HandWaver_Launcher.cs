using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Networking
{
/// <summary>
/// This launcher manages connection to PUN, loading of scenes and divion into rooms.
/// A UI will be added to integrate with this script and expand it's functionality.
/// </summary>
	public class HandWaver_Launcher : MonoBehaviourPunCallbacks
	{
		#region Private Serializable Fields
		/// <summary>
		/// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
		/// </summary>
		private readonly RoomOptions options = new RoomOptions {MaxPlayers = 10, PublishUserId = true};
		private readonly TypedLobby lobby = new TypedLobby("IMRE_Lobby", LobbyType.Default);
		private readonly string[] expectedUsers = null;

		#endregion

		#region Private Fields

		/// <summary>
		/// The client's version number.
		/// </summary>
		string gameVersion = "abc";

		public bool isIMRE = false;

		#endregion

		#region MonoBehaviour CallBacks

		private void Awake()
		{
			PhotonNetwork.AutomaticallySyncScene = true;
		}

		private void Start()
		{
			Connect();
		}

		public void Connect()
		{
			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				PhotonNetwork.GameVersion = gameVersion;
				PhotonNetwork.ConnectUsingSettings();
			}
		}
		#endregion

		#region MonoBehaviourPunCallbacks Callbacks


		public override void OnConnectedToMaster()
		{
			Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
			if (isIMRE)
			{
				PhotonNetwork.JoinOrCreateRoom("IMRE_LAB", options, lobby, expectedUsers);
			}
			else
			{
				PhotonNetwork.JoinRandomRoom();
			}
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, options);
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
			PhotonNetwork.NickName = SystemInfo.deviceName;
			PhotonNetwork.LoadLevel("ThreeTorus");
		}
		#endregion
	}
}
