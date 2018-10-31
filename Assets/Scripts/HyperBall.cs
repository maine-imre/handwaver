#region Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using PathologicalGames;
using System;
using System.Linq;
using IMRE.HandWaver.Solver;
using Photon.Pun;
#endregion

namespace IMRE.HandWaver.FourthDimension {


	[RequireComponent(typeof(InteractionBehaviour))]
	[RequireComponent(typeof(PhotonView))]
	[RequireComponent(typeof(PhotonTransformView))]
	[RequireComponent(typeof(PhotonRigidbodyView))]
	public class HyperBall : MonoBehaviourPunCallbacks {

		private float scaleOfBox = 2f;
		private Vector3 worldSpaceOrigin = Vector3.up * 1.8f;

		private void Start()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				float tmp = UnityEngine.Random.Range(0, 255);
				photonView.RPC("setColorOnBall", RpcTarget.All, tmp);
			}

			GetComponent<InteractionBehaviour>().OnContactBegin += startTakeOver;
			GetComponent<InteractionBehaviour>().OnContactEnd += endTakeOver;
		}

		/// <summary>
		/// This should sync the colors between clients... IDK tho
		/// </summary>
		/// <param name="tmpColor">color to set for this ball</param>
		[PunRPC]
		void setColorOnBall(float hue)
		{
			this.GetComponent<MeshRenderer>().materials[0].color = Color.HSVToRGB(hue, 1, 1);
		}

		void Update() {
			if (GetComponent<PhotonView>().IsMine)
			{
				this.transform.position = positionMap() + worldSpaceOrigin;
			}
			
		}

		private void startTakeOver()
		{
			//take ownserhip for me, to keep while I hit with my hands.
			GetComponent<PhotonView>().RequestOwnership();
		}

		private void endTakeOver()
		{
			//return ownership to master.
			GetComponent<PhotonView>().TransferOwnership(0);
		}

		private Vector3 positionMap()
		{
			Vector3 pos = this.transform.position-worldSpaceOrigin;

			if (pos.x > scaleOfBox) {
				pos += scaleOfBox*Vector3.left;
			}

			if (pos.x < -scaleOfBox) {
				pos += scaleOfBox*2*Vector3.right;
			}

			if (pos.y > scaleOfBox) {
				pos += 0*Vector3.down;
			}

			if (pos.y < -scaleOfBox) {
				pos += scaleOfBox*Vector3.forward;
			}

			if (pos.z > scaleOfBox) {
				pos += scaleOfBox*Vector3.back;
			}
			return pos;
		}
	}
}