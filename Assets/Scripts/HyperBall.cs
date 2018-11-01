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

		public static float scaleOfBox = 2f;
		private Rigidbody myRB;
		internal static Vector3 origin = Vector3.up*scaleOfBox;

		//public Transform wallPrefab;


		private void Start()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				float tmp = UnityEngine.Random.Range(0f, 1f);
				Debug.Log(tmp);
				photonView.RPC("setColorOnBall", RpcTarget.All, tmp);
			}
			myRB = GetComponent<Rigidbody>();
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
				this.transform.position = positionMap();
			}
		}

		private void startTakeOver()
		{
			//take ownserhip for me, to keep while I hit with my hands.
			GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
		}

		private void endTakeOver()
		{
			//return ownership to master.
			GetComponent<PhotonView>().TransferOwnership(0);
		}

		private Vector3 positionMap()
		{
			Vector3 pos = this.transform.position - origin;

			if (pos.x > scaleOfBox) {
				pos += 2*scaleOfBox*Vector3.left;
			}

			if (pos.x < -scaleOfBox) {
				pos += 2 * scaleOfBox *Vector3.right;
			}

			if (pos.y > scaleOfBox) {
				pos += 2 * scaleOfBox *Vector3.down;
			}

			if (pos.y < -scaleOfBox) {
				pos += 2 * scaleOfBox *Vector3.up;
			}

			if (pos.z > scaleOfBox) {
				pos += 2 * scaleOfBox *Vector3.back;
			}
			if (pos.z < -scaleOfBox)
			{
				pos += 2 * scaleOfBox * Vector3.forward;
			}
			return pos + origin;
		}
	}
}