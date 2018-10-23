using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System;

namespace IMRE.HandWaver
{
	/// <summary>
	/// Please note we need a mechanic in here that sets the hand color back after it is assigned.
	/// </summary>
	public class handColourManager : MonoBehaviour
	{
		public static handColourManager ins;
		public List<PolyHand> handsList;

		public static PolyHand left;
		public static PolyHand right;

		public enum handModes {select, grasp, paint, snappingPalm, none};
		public static Color startingColor;

		private void Start()
		{
			ins = this;
			left = handsList[0];
			right = handsList[1];
			startingColor = left.fingers[0].GetComponent<MeshRenderer>().materials[0].color;
		}

		public List<Leap.Unity.Interaction.InteractionHand> iHands;

		private void Update()
		{
			foreach (Leap.Unity.Interaction.InteractionHand hand in iHands)
			{
				Chirality chirality = Chirality.Right;
				if (hand.isLeft)
				{
					chirality = Chirality.Left;
				}
				if (hand.isGraspingObject)
				{
					setHandColorMode(chirality, handModes.grasp);
				}
			}
		}

		public static void setHandColorMode(Leap.Unity.Chirality chirality, handModes mode)
		{
			switch (mode)
			{
				case handModes.select:
					changeFingerIDX(chirality, 1, Color.magenta);
					break;
				case handModes.grasp:
					changeFingerIDX(chirality, 1, Color.green);
					changeFingerIDX(chirality, 2, Color.green);
					changeFingerIDX(chirality, 0, Color.green);
					break;
				case handModes.paint:
					changeFingerIDX(chirality, 1, Color.white);
					break;
				case handModes.snappingPalm:
					for (int i = 0; i < 5; i++)
					{
						changeFingerIDX(chirality, i, Color.cyan);
					}
					break;
				default:
					for (int i = 0; i < 5; i++)
					{
						changeFingerIDX(chirality, i, startingColor);
					}
					break;
			}
		}

		internal static void setHandColorMode()
		{
			throw new NotImplementedException();
		}

		public static void setHandColorMode(Leap.Unity.Chirality chirality, handModes mode, Color color)
		{
			switch (mode)
			{
				case handModes.select:
					changeFingerIDX(chirality, 1, color);
					break;
				case handModes.grasp:
					changeFingerIDX(chirality, 1, color);
					changeFingerIDX(chirality, 2, color);
					changeFingerIDX(chirality, 0, color);
					break;
				case handModes.paint:
					changeFingerIDX(chirality, 1, color);
					break;
				default:
					for (int i = 0; i < 5; i++)
					{
						changeFingerIDX(chirality, i, color);
					}
					break;
			}
		}

		public static void changeFingerIDX(Leap.Unity.Chirality chirality, int fingerIDX, Color color)
		{
			PolyHand hand = right;
			if(chirality == Chirality.Left)
			{
				hand = left;
			}
			FingerModel finger = hand.fingers[fingerIDX];
			finger.GetComponent<MeshRenderer>().materials[0].color = color;
		}

	}
}