using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Leap.Unity;
using Leap.Unity.Interaction;
using System.Linq;

namespace IMRE.HandWaver.Networking
{
/// <summary>
/// Syncs visual representations of players' hands.
/// Will be expanded to handle InterationHands and Interaction Controllers.
/// </summary>
	public class playerHand : MonoBehaviourPunCallbacks
	{
		public Chirality whichHand;

		public InteractionHand iHand;

		private void Start()
		{
			if (photonView.IsMine)
			{
				//Sets hand based on which hand is needed from whichHand variable.
				switch (whichHand)
				{
					case Chirality.Left:
						iHand = FindObjectsOfType<InteractionHand>().Where(h => h.handDataMode == HandDataMode.PlayerLeft).FirstOrDefault();
						break;
					case Chirality.Right:
						iHand = FindObjectsOfType<InteractionHand>().Where(h => (h.handDataMode == HandDataMode.PlayerRight)).FirstOrDefault();
						break;
					default:
						Debug.LogError(name + " has encountered an error with whichHand variable.");
						break;
				}

				//Asserts that the hand has found something and been assigned.
				if(iHand == null)
				{
					Debug.LogError("Failed to find local player hand. Disabling hand preview on cilent "+PhotonNetwork.NickName);
				}
				else
				{
					Debug.Log("Local Player " + whichHand + " was set to be interaction hand" + iHand.name);
				}
				GetComponent<MeshRenderer>().enabled = false;
			}
		}

		[PunRPC]
		private void showHand(bool show)
		{
			GetComponent<MeshRenderer>().enabled = show;
		}

		private void Update()
		{
			if (photonView.IsMine)
			{
				transform.position = iHand.position;

				photonView.RPC("showHand", RpcTarget.OthersBuffered, iHand.isTracked);
			}
		}
	}
}