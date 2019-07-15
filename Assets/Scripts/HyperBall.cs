#region Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

using System;
using System.Linq;
using IMRE.HandWaver.Solver;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif
#endregion

namespace IMRE.HandWaver.FourthDimension {

	#if PHOTON_UNITY_NETWORKING
	/// <summary>
	/// A networked object that whose bounds are connected to the graphics in hyperballboundaries.
	/// Build as an initial test of networking capacity.
	/// </summary>
	[RequireComponent(typeof(InteractionBehaviour))]
	[RequireComponent(typeof(PhotonView))]
	[RequireComponent(typeof(PhotonTransformView))]
	[RequireComponent(typeof(PhotonRigidbodyView))]
	#endif
	public class HyperBall : MonoBehaviourPunCallbacks {

		#if PHOTON_UNITY_NETWORKING
		public static float scaleOfBox = 2f;
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
			GetComponent<InteractionBehaviour>().OnContactBegin += startTakeOver;
			GetComponent<InteractionBehaviour>().OnContactEnd += EndTakeOver;
		}

		/// <summary>
		/// This should sync the colors between clients... IDK tho
		/// </summary>
		/// <param name="hue">color to set for this ball</param>
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
			if (_photonView.IsMine)
			{
				this.transform.position = PositionMap();
			}
		}

		private void startTakeOver()
		{
			//take ownership for me, to keep while I hit with my hands.
			GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
		}

		private void EndTakeOver()
		{
			//return ownership to master.
			GetComponent<PhotonView>().TransferOwnership(0);
		}

		private Vector3 PositionMap()
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
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.up) * _rigidbody.velocity;
					}

					if (pos.y < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.up;
						pos = Quaternion.AngleAxis(180f, Vector3.up)*pos;
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.up) * _rigidbody.velocity;
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
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.left) * _rigidbody.velocity;
					}

					if (pos.x < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.right;
						pos = Quaternion.AngleAxis(180f, Vector3.right) * pos;
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.right) * _rigidbody.velocity;
					}

					if (pos.y > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.down;
						pos = Quaternion.AngleAxis(180f, Vector3.down) * pos;
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.down) * _rigidbody.velocity;
					}

					if (pos.y < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.up;
						pos = Quaternion.AngleAxis(180f, Vector3.up) * pos;
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.up) * _rigidbody.velocity;
					}

					if (pos.z > scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.back;
						pos = Quaternion.AngleAxis(180f, Vector3.back) * pos;
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.back) * _rigidbody.velocity;
					}
					if (pos.z < -scaleOfBox)
					{
						pos += 2 * scaleOfBox * Vector3.forward;
						pos = Quaternion.AngleAxis(180f, Vector3.forward) * pos;
						_rigidbody.velocity = Quaternion.AngleAxis(180f, Vector3.forward) * _rigidbody.velocity;
					}
					break;
				default:
					break;
			}

			bool isNew = (pos != this.transform.position - origin);
			_trailRenderer.emitting = !isNew;
			if (isNew)
			{
				photonView.RPC("ClearTrailRenderer", RpcTarget.All);
			}

			return pos + origin;
		}
		#endif
	}
}