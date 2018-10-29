#region Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using PathologicalGames;
using System;
using System.Linq;
using IMRE.HandWaver.Solver;
#endregion

namespace IMRE.HandWaver.FourthDimension {

	/// <summary>
	/// @Nathan please network this.
	/// 
	/// We may need to network the physics values (Rigidbody) in addition to the transform.
	/// </summary>
	[RequireComponent(typeof(InteractionBehaviour))]
	public class HyperBall : MonoBehaviour {

		private float scaleOfBox = 2f;
		private Vector3 worldSpaceOrigin = Vector3.up * 1.8f;

		private void Start()
		{
			this.GetComponent<MeshRenderer>().materials[0].color = UnityEngine.Random.ColorHSV(0,1,1,1,1,1);
		}

		void Update() {
			this.transform.position = positionMap()+worldSpaceOrigin;
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