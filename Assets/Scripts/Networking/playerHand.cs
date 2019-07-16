using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using Photon.Realtime;
#endif
using Leap.Unity;
using Leap.Unity.Interaction;
using System.Linq;

namespace IMRE.HandWaver.Networking
{
	/// <summary>
	/// Syncs visual representations of players' hands.
	/// Will be expanded to handle InterationHands and Interaction Controllers.
	/// </summary>
	public class playerHand : 
		
#if PHOTON_UNITY_NETWORKING
		MonoBehaviourPunCallbacks {
#else
		MonoBehaviour {
#endif
		public Chirality whichHand;
		
		public Mesh handModel;
		public Mesh controllerModel;

		public static int LeapStatusCheckInterval = 1;

		internal bool isLeapMotionActive
		{
			//check to see if a leap motion device is present
			get
			{
				//thankfully leap motion has a function for this.
				bool current = FindObjectOfType<LeapServiceProvider>().IsConnected();
				//to double check, see if an XR controller exists.  This should always be true.
				if (!FindObjectOfType<LeapXRServiceProvider>().IsConnected() && !current)
				{
					Debug.LogWarning("No HandTracking is Available!  Please attach a controller.");
				}

				return current;
			}
		}

		private bool wasLeapMotionActive;

		internal InteractionHand iHand
		{
			get
			{
				if (_iHand == null)
				{
					//find the object in the scene with the appropriate chirality.
					_iHand = FindObjectsOfType<InteractionHand>().FirstOrDefault(h => ((h.handDataMode == HandDataMode.PlayerLeft) == (whichHand == Chirality.Left)));
				}

				return _iHand;
			}
		}

		private InteractionHand _iHand;


		internal InteractionXRController iXR
		{
			get
			{
				if (_iXR == null)
				{
					//find the object in the scene with the appropriate chirality.
					_iXR = FindObjectsOfType<InteractionXRController>().FirstOrDefault(h => (h.isLeft) == (whichHand == Chirality.Left));
				}

				return _iXR;
			}
		}

		private InteractionXRController _iXR;

		private InteractionController iController
		{
			get
			{
				//chose the object in the scene that is active.  Prefer LeapMotion.
				return  isLeapMotionActive ? (InteractionController) iHand : (InteractionController) iXR;
			}
		}
		
		private IEnumerator leapStatus;
#if PHOTON_UNITY_NETWORKING

		private void Start()
		{
			if (photonView.IsMine)
			{
				//Asserts that the hand has found something and been assigned.
				if(iHand == null)
				{
					Debug.LogError("Failed to find local player hand. Disabling hand preview on cilent "+PhotonNetwork.NickName);
				}
				else
				{
					Debug.Log("Local Player " + whichHand + " was set to be interaction hand" + iHand.name);
				}
				//turn off the renderer locally.
				GetComponent<MeshRenderer>().enabled = false;
				
				//sync the "showHand variable".  We don't want to sync to self becasue we don't want to see the approximation mesh.
				photonView.RPC("showHand", RpcTarget.OthersBuffered, iController.isTracked);
				
				//initial LM check and sync to others.  We don't want to sync to self because we don't care what controller type 
				//is used for a mesh we don't see.
				photonView.RPC("handMode", RpcTarget.OthersBuffered, isLeapMotionActive);

				//start a co-routine to allow LM to be plugged or unpluged at regular intervals.
				//this will run the above commands as necessary.
				leapStatus = checkLeapStatus(LeapStatusCheckInterval);

				StartCoroutine(leapStatus);
			}
		}

		
		private void Update()
		{
			if (photonView.IsMine)
			{
				//update the position and rotation of the controller.  PUN handels syncing this data.
				transform.position = iController.position;
				transform.rotation = iController.transform.rotation;
			}
		}

		[PunRPC]
		private void showHand(bool show)
		{
			GetComponent<MeshRenderer>().enabled = show;
		}

		[PunRPC]
		private void handMode(bool isHand)
		{
			GetComponent<MeshFilter>().mesh = isHand ? handModel : controllerModel;
		}

		private IEnumerator checkLeapStatus(int waitTime)
		{
			while (true)
			{
				//sync the "showHand variable".  We don't want to sync to self becasue we don't want to see the approximation mesh.
				photonView.RPC("showHand", RpcTarget.OthersBuffered, iController.isTracked);
				
				//initial LM check and sync to others.  We don't want to sync to self because we don't care what controller type 
				//is used for a mesh we don't see.
				photonView.RPC("handMode", RpcTarget.OthersBuffered, isLeapMotionActive);
				yield return new WaitForSeconds(waitTime);
			}
		}
		#endif 
		
	}
}