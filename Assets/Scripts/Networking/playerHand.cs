using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Leap.Unity;
using Leap.Unity.Interaction;
using System.Linq;
using UnityEngine.Serialization;

namespace IMRE.HandWaver.Networking
{
/// <summary>
/// Syncs visual representations of players' hands.
/// Will be expanded to handle InterationHands and Interaction Controllers.
/// </summary>
	public class playerHand : MonoBehaviourPunCallbacks
	{
		public Chirality whichHand;

		[FormerlySerializedAs("iHand")] public InteractionController iController;

		private void Start()
		{
			if (photonView.IsMine)
			{
				//Sets hand based on which hand is needed from whichHand variable.
				switch (whichHand)
				{
					case Chirality.Left:
						iController = FindObjectsOfType<InteractionController>().Where(h => h.isLeft).FirstOrDefault();
						break;
					case Chirality.Right:
						iController = FindObjectsOfType<InteractionController>().Where(h => (!h.isLeft)).FirstOrDefault();
						break;
					default:
						Debug.LogError(name + " has encountered an error with whichHand variable.");
						break;
				}

				//Asserts that the hand has found something and been assigned.
				if(iController == null)
				{
					Debug.LogError("Failed to find local player hand. Disabling hand preview on cilent "+PhotonNetwork.NickName);
				}
				else
				{
					Debug.Log("Local Player " + whichHand + " was set to be interaction hand" + iController.name);
				}
				GetComponent<MeshRenderer>().enabled = false;
			}
		}

		[PunRPC]
		private void showHand(bool show)
		{
			GetComponent<MeshRenderer>().enabled = show;
		}

		private bool isTracked = false;

		private void Update()
		{
			if (photonView.IsMine && iController != null)
			{
				transform.position = iController.position;
				transform.rotation = iController.rotation;

				if (isTracked != iController.isTracked)
					photonView.RPC("showHand", RpcTarget.AllBuffered, iController.isTracked);

				isTracked = iController.isTracked;
			}
		}
	}
}