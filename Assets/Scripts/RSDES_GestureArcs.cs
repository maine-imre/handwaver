using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Gestures;
using Leap;
using System;
using IMRE.HandWaver.Interface;
using Leap.Unity;

namespace IMRE.HandWaver.Space
{
	[RequireComponent(typeof(AudioSource))]
	/// <summary>
	/// This script allows for arcs to be created between RSDESpushPinPreFabs with hand gestures in the RSDES scene.
	/// The main contributor(s) to this script is  CB, NG
	/// Status: WORKING
	/// </summary>
	public class RSDES_GestureArcs : OneHandedGesture
	{
		public bool forgiving = false;
		public float distanceTolerance = .01f;
		public bool forgiveAngleTolerance = false;
		public float angleTolerance = Mathf.Infinity;

		public RSDESPin pinA = null;
		public RSDESPin pinB = null;

		private AudioSource _myAudioSource;

		internal AudioSource myAudioSource
		{
			get
			{
				if (_myAudioSource == null)
					_myAudioSource = GetComponent<AudioSource>();

				return _myAudioSource;

			}
		}

		[Header("Audio Clips")]
		public AudioClip errorClip;
		public AudioClip positiveClip;
		public AudioClip completeClip;

		public Vector3 fingerTip(Hand hand)
		{
				return hand.Fingers[1].TipPosition.ToVector3();
		}
		public Vector3 fingerDirection(Hand hand)
		{
				return hand.Fingers[1].Direction.ToVector3();
		}



		protected override bool ShouldGestureActivate(Hand hand)
		{
			return hand.Fingers[1].IsExtended && !hand.Fingers[2].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended;
		}

		protected override bool ShouldGestureDeactivate(Hand hand, out DeactivationReason? deactivationReason)
		{
			//We may need to add something to indicate when the user has actually finished.

			deactivationReason = DeactivationReason.FinishedGesture;
			return !(hand.Fingers[1].IsExtended && !hand.Fingers[2].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended);
		}

		protected override void WhileGestureActive(Hand hand)
		{
			if (RSDESManager.ins != null)
			{
				RSDESPin bestPin = null;
				float dist = Mathf.Infinity;
				foreach (pinData currPinData in RSDESManager.ins.PinnedPoints)
				{
					if ((currPinData.pin.pinHead.transform.position - fingerTip(hand)).magnitude < dist && isPointingAt(currPinData.pin.pinHead, hand))
					{
						dist = (currPinData.pin.pinHead.transform.position - fingerTip(hand)).magnitude;
						bestPin = currPinData.pin;
					}
				}

				if (dist < distanceTolerance && bestPin != null)
				{
					if(pinA == null && pinB == null)
					{
						pinA = bestPin;
						playPositiveSound();
						pinA.playParticleEffect();

					}
					else if (pinA != null && pinB == null && bestPin != pinA)
					{


						pinB = bestPin;
						pinB.playParticleEffect();


						RSDESManager.ins.instantiateGreatArc(pinA.dbPinData, pinB.dbPinData);
						playCompleteSound();

						RSDESPin tmp;
						tmp = pinA;
						pinA = pinB;
						pinB = tmp;
					}
					else //pinA & pinB are set
					{
						if (bestPin == pinB)
						{

							RSDESManager.ins.instantiateGreatCircle(pinA.dbPinData, pinB.dbPinData);
							playCompleteSound();
							pinA.playParticleEffect();

							pinA = pinB;
							pinB = null;
						}


					}


					#region Old Stuff
					//	if (pinA != null && (bestPin != null && bestPin != pinB))
					//		pinB = pinA;

					//	if (pinB != null && bestPin == pinB)
					//	{
					//		//if(RSDESManager.verboseLogging)
					//		Debug.Log("DRAW CIRCLE");
					//		//draw circle
					//		RSDESManager.ins.instantiateGreatCircle(bestPin.dbPinData, pinA.dbPinData);
					//		pinB = null;
					//	}
					//	else if (pinB != null)
					//	{
					//		//if (RSDESManager.verboseLogging)
					//		Debug.Log("DRAW ARC");
					//		//draw arc
					//		RSDESManager.ins.instantiateGreatArc(bestPin.dbPinData, pinA.dbPinData);
					//	}
					//	else
					//	{
					//		Debug.LogError("Pin B is null within " + name);
					//	}
					//	pinA = bestPin;
					//	pinA.playParticleEffect();
					//	gestureComplete = true;
					//}
					//else if (dist < distanceTolerance)
					//{
					//	pinA = bestPin;
					//}
					#endregion

				}
			}
		}

		protected override void WhenGestureDeactivated(Hand maybeNullHand, DeactivationReason reason)
		{
			if((reason == DeactivationReason.CancelledGesture && !forgiving) || reason == DeactivationReason.FinishedGesture )		
			{
				if(pinA != null || pinB != null)
				{
					playErrorSound();
				}
				pinB = null;
				pinA = null;
			}
		}

		private void playErrorSound()
		{
			if (errorClip == null)
				Debug.LogWarning("Set error clip");
			else
			{
				myAudioSource.clip = errorClip;
				myAudioSource.Play();
			}
		}

		private void playPositiveSound()
		{
			if (positiveClip == null)
				Debug.LogWarning("Set positive clip");
			else
			{
				myAudioSource.clip = positiveClip;
				myAudioSource.Play();
			}
		}

		private void playCompleteSound ()
		{
			if (completeClip == null)
				Debug.LogWarning("Set complete clip");
			else
			{
				myAudioSource.clip = completeClip;
				myAudioSource.Play();
			}
		}

		private bool isPointingAt(MeshRenderer target, Hand hand)
		{
			return (forgiveAngleTolerance || (Vector3.Angle(target.transform.position - fingerTip(hand), fingerDirection(hand)) < angleTolerance));
		}
	}
}
