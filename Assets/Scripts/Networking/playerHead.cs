using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.XR;

namespace IMRE.HandWaver.Networking
{

	public class playerHead : MonoBehaviourPunCallbacks
	{

		#region Constant Values

		const int MAXPLAYERCOUNT = 4;

		#endregion



		[Tooltip("Player Hands")]
		public playerHand lHand;
		public playerHand rHand;
		public Vector3 rotOffset;
		public Color myColor;


		[Space]

		[Tooltip("Player Information")]
		public string playerName = "JOHN MADDEN";

		[Range(0, MAXPLAYERCOUNT)]
		public int playerPort;

		public TMPro.TextMeshPro text;

		#region Mono Funcs
		private void Start()
		{
			if (photonView.IsMine)
			{

				GetComponent<MeshRenderer>().enabled = false;
				text.renderer.enabled = false;
				UnityEngine.XR.InputTracking.trackingAcquired += notifyTracked;
				UnityEngine.XR.InputTracking.trackingLost += notifyNotTracked;
			}
			else
			{
				photonView.RPC("setHeadName", RpcTarget.AllBuffered);
				photonView.RPC("setColor", RpcTarget.AllBuffered);
			}
		}

		private void notifyNotTracked(XRNodeState obj)
		{
			photonView.RPC("showHead", RpcTarget.OthersBuffered, false);
		}

		private void notifyTracked(XRNodeState obj)
		{
			photonView.RPC("showHead", RpcTarget.OthersBuffered, true);
		}

		private void Update()
		{
			if (photonView.IsMine)
			{
				transform.position = Camera.main.transform.position;
				transform.rotation = Camera.main.transform.rotation*Quaternion.Euler(rotOffset);
			}
		}
		#endregion

		public void setupPlayer(string newNick, int newPlayerNumber, float newPlayerHue)
		{
			this.playerPort = newPlayerNumber;
			myColor = Color.HSVToRGB(newPlayerHue, 1, 1);
			playerName = newNick;
			this.name = "Player " + PhotonNetwork.NickName;
			lHand.name = "Player Hand (" + newPlayerNumber + "L)";
			rHand.name = "Player Hand (" + newPlayerNumber + "R)";
			photonView.RPC("setHeadName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
			photonView.RPC("setColor", RpcTarget.AllBuffered, newPlayerHue);
		}

		[PunRPC]
		private void setHeadName(string newName)
		{
			text.text = newName;
		}

		[PunRPC]
		private void setHeadName()
		{
			playerName = PhotonNetwork.NickName;
			text.text = playerName;
		}



		[PunRPC]
		private void showHead(bool show)
		{
			GetComponent<MeshRenderer>().enabled = show;
		}


		[PunRPC]
		private void setColor(float newPlayerHue)
		{
			myColor = Color.HSVToRGB(newPlayerHue, 1, 1);
			GetComponent<MeshRenderer>().materials[0].color = myColor;
			lHand.GetComponent<MeshRenderer>().materials[0].color = myColor;
			rHand.GetComponent<MeshRenderer>().materials[0].color = myColor;
			photonView.RPC("setColor", RpcTarget.OthersBuffered, newPlayerHue);

		}
		private void setColor()
		{
			GetComponent<MeshRenderer>().materials[0].color = myColor;
			lHand.GetComponent<MeshRenderer>().materials[0].color = myColor;
			rHand.GetComponent<MeshRenderer>().materials[0].color = myColor;

		}

	}
}