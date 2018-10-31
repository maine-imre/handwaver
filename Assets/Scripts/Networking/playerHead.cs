using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

		[Space]

		[Tooltip("Player Information")]
		public string playerName = "DEFAULTNAME";

		[Range(0, MAXPLAYERCOUNT)]
		public int playerPort;

		#region Mono Funcs
		private void Start()
		{
			if (photonView.IsMine)
			{

				GetComponent<MeshRenderer>().enabled = false;
			}
		}

		private void Update()
		{
			if (photonView.IsMine)
			{
				transform.position = Camera.main.transform.position;
				transform.rotation = Camera.main.transform.rotation;
			}
		}
		#endregion

		public void setupPlayer(int newPlayerNumber, Color newPlayerColor)
		{
			this.playerPort = newPlayerNumber;
			setColor(newPlayerColor);
			this.name = "Player " + newPlayerNumber;
			lHand.name = "Player Hand (" + newPlayerNumber+"L)";
			rHand.name = "Player Hand (" + newPlayerNumber+"R)";


		}

		private void setColor(Color newColor)
		{
			GetComponent<MeshRenderer>().materials[0].color = newColor;
			lHand.GetComponent<MeshRenderer>().materials[0].color = newColor;
			rHand.GetComponent<MeshRenderer>().materials[0].color = newColor;

		}

	}
}