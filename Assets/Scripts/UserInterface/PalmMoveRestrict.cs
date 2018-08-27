/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System;
using UnityEngine.Events;

namespace IMRE.HandWaver{
	/// <summary>
	/// Uses finger direction detecters to determine when a palm is open, then restrictionPlane is accessible to restriction movement from the interaction controls.
	/// Will return Vector3.zero for an error.  
	/// ActiveRestriction provides the result of an exclusive or on the finger direction detectors.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class PalmMoveRestrict : MonoBehaviour {
		public ExtendedFingerDetector lHand;
		public ExtendedFingerDetector rHand;

		private bool lHandActive = false;
		private bool rHandActive = false;
		/// <summary>
		/// Allows the user to select wether the palm's direction is snapped to cartesian coordinates.
		/// </summary>
		public bool snapToCartesian = false;
		/// <summary>
		/// ActiveRestriction provides the result of an exclusive or on the finger direction detectors.
		/// </summary>
		internal bool ActiveRestriction
		{
			get { return (lHand ^ rHand); }
		}

		void Start()
		{
			lHand.OnActivate.AddListener(lHandTrue);
			rHand.OnActivate.AddListener(rHandTrue);

			lHand.OnDeactivate.AddListener(lHandFalse);
			rHand.OnDeactivate.AddListener(rHandFalse);
		}

		/// <summary>
		/// This identifies the plane that movement needs to be restricted to.
		/// In the null case, Vector3.zero is returned.
		/// </summary>
		internal Vector3 restrictionPlane
		{
			get
			{
				if (!ActiveRestriction)
				{
					return Vector3.zero;
				}

				if (snapToCartesian)
				{
					return CartesianPlaneProjection;
				}
				else
				{
					return rawRestrictionPlane;
				}
			}
		}

		#region  LotsOfMath
		private Vector3 CartesianPlaneProjection
		{
			get
			{
				Vector3 tmpPlane = rawRestrictionPlane;

				if (tmpPlane == Vector3.zero)
				{
					return Vector3.zero;
				}

				Vector3 value = Vector3.right;
				float error = Vector3.Magnitude(Vector3.Project(tmpPlane, Vector3.right));

				float newError = Vector3.Magnitude(Vector3.Project(tmpPlane, Vector3.up));
				if (newError < error)
				{
					value = Vector3.up;
					error = newError;
				}

				newError = Vector3.Magnitude(Vector3.Project(tmpPlane, Vector3.forward));
				if (newError < error)
				{
					value = Vector3.forward;
				}

				return value;
			}
		}

		private Vector3 rawRestrictionPlane
		{
			get
			{
				if (lHandActive)
				{
					return PalmNormal(lHand);
				}
				else if (rHandActive)
				{
					return PalmNormal(rHand);
				}
				else
				{
					return Vector3.zero;
				}
			}
		}

		private Vector3 PalmNormal(ExtendedFingerDetector value)
		{
			return value.HandModel.GetComponent<RigidHand>().GetPalmNormal();
		}

		private void rHandFalse()
		{
			rHandActive = false;
		}

		private void lHandFalse()
		{
			lHandActive = false;
		}

		private void rHandTrue()
		{
			rHandActive = true;
		}

		private void lHandTrue()
		{
			lHandActive = true;
		}
#endregion
	}
}