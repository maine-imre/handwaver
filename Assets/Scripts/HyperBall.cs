#region Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

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
	/// <summary>
	/// A networked object that whose bounds are connected to the graphics in hyperballboundaries.
	/// Build as an initial test of networking capacity.
	/// </summary>
	[RequireComponent(typeof(InteractionBehaviour))]
	[RequireComponent(typeof(PhotonView))]
	[RequireComponent(typeof(PhotonTransformView))]
	[RequireComponent(typeof(PhotonRigidbodyView))]
	public class HyperBall : MonoBehaviourPunCallbacks {

		public static float scaleOfBox = 2f;
		private Rigidbody myRB;
		internal static Vector3 origin = Vector3.up*scaleOfBox;
		private PhotonView _photonView;
		private Rigidbody _rigidbody;
		private TrailRenderer _trailRenderer;

		//public Transform wallPrefab;


		private void Start()
		{
			_trailRenderer = GetComponent<TrailRenderer>();
			_rigidbody = GetComponent<Rigidbody>();
			_photonView = GetComponent<PhotonView>();
			if (PhotonNetwork.IsMasterClient)
			{
				float tmp = UnityEngine.Random.Range(0f, 1f);
				photonView.RPC("SetColorOnBall", RpcTarget.AllBuffered, tmp);
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
		void SetColorOnBall(float hue)
		{
			this.GetComponent<MeshRenderer>().materials[0].color = Color.HSVToRGB(hue, 1, 1);
			this.GetComponent<TrailRenderer>().startColor= Color.HSVToRGB(hue, 1, 1);
			this.GetComponent<TrailRenderer>().endColor = Color.HSVToRGB(hue, 1, 1);
			this.GetComponent<TrailRenderer>().enabled = true;
		}

		[PunRPC]
		void ClearTrailRenderer()
		{
			GetComponent<TrailRenderer>().Clear();
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
			GetComponent<PhotonView>().TransferOwnership(0);
		}

		private Vector3 positionMap()
		{
			Vector3 pos = this.transform.position - origin;
			switch (HyperBallBoundaries.myGeometery)
			{
				case HyperBallBoundaries.GeometeryType.ThreeTorus:

					if (pos.x > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.left;
					}

					if (pos.x < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.right;
					}

					if (pos.y > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.down;
					}

					if (pos.y < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.up;
					}

					if (pos.z > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.back;
					}
					if (pos.z < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.forward;
					}
					break;
				case HyperBallBoundaries.GeometeryType.ThreeSphere:
					if(pos.magnitude > scaleOfBox)
					{
						pos *= -1f;
					}
					break;
				case HyperBallBoundaries.GeometeryType.KleinBottle:
					//also need to change velocity direction.
					if (pos.x > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.left;
					}

					if (pos.x < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.right;
					}

					if (pos.y > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.down;
						pos = Quaternion.AngleAxis(180f, Vector3.up)*pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.up) * GetComponent<Rigidbody>().velocity;
					}

					if (pos.y < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.up;
						pos = Quaternion.AngleAxis(180f, Vector3.up)*pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.up) * GetComponent<Rigidbody>().velocity;
					}

					if (pos.z > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.back;
					}
					if (pos.z < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.forward;
					}
					break;
				case HyperBallBoundaries.GeometeryType.TrippleTwist:
					//some sort of a klein bottle in every direction???
					if (pos.x > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.left;
						pos = Quaternion.AngleAxis(180f, Vector3.left) * pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.left) * GetComponent<Rigidbody>().velocity;
					}

					if (pos.x < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.right;
						pos = Quaternion.AngleAxis(180f, Vector3.right) * pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.right) * GetComponent<Rigidbody>().velocity;
					}

					if (pos.y > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.down;
						pos = Quaternion.AngleAxis(180f, Vector3.down) * pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.down) * GetComponent<Rigidbody>().velocity;
					}

					if (pos.y < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.up;
						pos = Quaternion.AngleAxis(180f, Vector3.up) * pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.up) * GetComponent<Rigidbody>().velocity;
					}

					if (pos.z > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.back;
						pos = Quaternion.AngleAxis(180f, Vector3.back) * pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.back) * GetComponent<Rigidbody>().velocity;
					}
					if (pos.z < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.forward;
						pos = Quaternion.AngleAxis(180f, Vector3.forward) * pos;
						GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(180f, Vector3.forward) * GetComponent<Rigidbody>().velocity;
					}
					break;
				default:
					break;
			}

			bool isNew = (pos != this.transform.position - origin);
			GetComponent<TrailRenderer>().emitting = !isNew;
			if (isNew)
			{
				photonView.RPC("ClearTrailRenderer", RpcTarget.All);
			}

			return pos + origin;
		}
	}
}
