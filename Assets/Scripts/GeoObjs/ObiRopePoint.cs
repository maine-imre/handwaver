//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Obi;

//namespace IMRE.HandWaver
//{
/// <summary>
/// This script does ___.
/// The main contributor(s) to this script is __
/// Status: ???
/// </summary>
//	public class ObiRopePoint : MonoBehaviour
//	{
//		public ObiRope rope;
//		AbstractPoint thisPointend, thisPointBegining;
//		public float maxLength = 0;
//		// Use this for initialization
//		void Start()
//		{
//			AbstractPoint thisPointend = GeoObjConstruction.iPoint(rope.GetParticlePosition(rope.TotalParticles-11));
//			AbstractPoint thisPointBegining = GeoObjConstruction.iPoint(rope.GetParticlePosition(0));
//			thisPointend.stretchEnabled = false;
//			thisPointBegining.stretchEnabled = false;

//			ObiPinConstraintBatch batch = new ObiPinConstraintBatch(false, false);
//			batch.AddConstraint(rope.TotalParticles-11, thisPointend.GetComponent<SphereCollider>(), new Vector3(0, 0, 0), 1);
//			batch.AddConstraint(0, thisPointBegining.GetComponent<SphereCollider>(), new Vector3(0, 0, 0), 1);
//			rope.PinConstraints.AddBatch(batch);

//		}
//		void Update()
//		{
//			float len = rope.CalculateLength();
//			if(len > maxLength)
//			{
//				Debug.Log(len);
//				maxLength = len;
//			}

//		}

//	}
//}
