using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Leap.Unity.Interaction;
using System;
using Leap.Unity;

namespace IMRE.HandWaver.Lattice {
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class LatticeLandOpenVR : FingerPointLineMaker {

		public InteractionXRController hand;

		private bool lgripDown = false;
		private bool rgripDown = false;
		private bool ltriggerDown = false;
		private bool rtriggerDown = false;

		private void Awake()
		{
			switch (handedness)
			{
				//set to match controller.
				case Chirality.Left:
					indexAttachment = leapHandDataLogger.ins.currHands.LOVR.transform;
					palmAttachment = leapHandDataLogger.ins.currHands.LOVR.transform;
					left = this;
					break;
				case Chirality.Right:
					indexAttachment = leapHandDataLogger.ins.currHands.ROVR.transform;
					palmAttachment = leapHandDataLogger.ins.currHands.ROVR.transform;
					right = this;
					break;
				default:
					break;
			}
		}

		#region  Polling
		private void FixedUpdate()
		{
			switch (handedness)
			{
				case Chirality.Left:
					bool triggerDown = Input.GetKeyDown(KeyCode.JoystickButton14);
					bool touchDown = Input.GetKeyDown(KeyCode.JoystickButton8);

					//bool triggerUp = Input.GetKeyUp(KeyCode.JoystickButton14);
					//bool touchUp = Input.GetKeyUp(KeyCode.JoystickButton8);



					//lh trigger hold and trackpad
					//if (Input.GetKey(KeyCode.JoystickButton14) && Input.GetKey(KeyCode.JoystickButton8))
					//{
					//	triggerPad(hand);
					//}
					////lh trigger hold and trackpad
					//if ((triggerDown && Input.GetKey(KeyCode.JoystickButton8)) || (Input.GetKey(KeyCode.JoystickButton14) && touchDown))
					//{
					//	triggerPadDown(hand);
					//}
					////lh trigger hold and trackpad
					//if ((Input.GetKeyUp(KeyCode.JoystickButton14) && Input.GetKey(KeyCode.JoystickButton8)) || (Input.GetKey(KeyCode.JoystickButton14) && Input.GetKeyUp(KeyCode.JoystickButton8)))
					//{
					//	triggerPadUp(hand);
					//}
					if ((Input.GetKeyDown(KeyCode.JoystickButton8)))
					{
						triggerPadDown(hand);
					}
					if ((Input.GetKeyUp(KeyCode.JoystickButton8)))
					{
						triggerPadUp(hand);
					}
					//lh trigger hold and not trackpad
					if (Input.GetKey(KeyCode.JoystickButton14) && !Input.GetKey(KeyCode.JoystickButton8))
					{
						triggerOnly(hand);
					}

					//lh trigger hold and not trackpad
					if (triggerDown && !Input.GetKey(KeyCode.JoystickButton8))
					{
						triggerOnlyDown(hand);
					}
					//lh trigger hold and not trackpad
					if (Input.GetKeyUp(KeyCode.JoystickButton14) && !Input.GetKey(KeyCode.JoystickButton8))
					{
						triggerOnlyUp(hand);
					}
					//lh trackpad hold not trigger
					if (Input.GetKey(KeyCode.JoystickButton8) && !Input.GetKey(KeyCode.JoystickButton14))
					{
						trackpadOnly(hand);
					}
					//lh trackpad hold not trigger
					if (touchDown && !Input.GetKey(KeyCode.JoystickButton14))
					{
						trackpadOnlyDown(hand);
					}
					//lh trackpad hold not trigger
					if (Input.GetKeyUp(KeyCode.JoystickButton8) && !Input.GetKey(KeyCode.JoystickButton14))
					{
						trackpadOnlyUp(hand);
					}
					//lh grip
					if (Input.GetAxis("LeftVRGripAxis") >= 0.01f)
					{
						if (!lgripDown)
						{
							lgripDown = true;
							gripDown(hand);
						}
						gripHold(hand);
					}
					//lh grip
					if (Input.GetAxis("LeftVRGripAxis") < 0.01f)
					{
						if (lgripDown)
						{
							lgripDown = false;
							gripUp(hand);
						}
					}
					//lh trigger
					if (Input.GetAxis("LeftVRTriggerAxis") >= 0.01f)
					{
						if(!ltriggerDown)
						{
							ltriggerDown = true;
							triggerOnlyDown(hand);
						}
					}
					if (Input.GetAxis("LeftVRTriggerAxis") < 0.01f)
					{
						if(ltriggerDown)
						{
							ltriggerDown = false;
							triggerOnlyUp(hand);
						}
					}
					//lh trigger axis + touchpad
					//if (Input.GetAxis("LeftVRTriggerAxis") >= 0.01f && Input.GetKeyDown(KeyCode.JoystickButton8))
					//{
					//	if (!ltriggerDown)
					//	{
					//		ltriggerDown = true;
					//		triggerPadDown(hand);
					//	}
					//	else if (Input.GetKeyDown(KeyCode.JoystickButton8))
					//	{
					//		triggerPadDown(hand);
					//	}
					//}
					////lh trigger axis + touchpad
					//if (Input.GetAxis("LeftVRTriggerAxis") < 0.01f && Input.GetKeyUp(KeyCode.JoystickButton8))
					//{
					//	//something needs fixing here
					//	if (ltriggerDown)
					//	{
					//		ltriggerDown = false;
					//		triggerPadUp(hand);
					//	}
					//	else if (Input.GetKeyDown(KeyCode.JoystickButton8))
					//	{
					//		triggerPadUp(hand);
					//	}
					//}
					break;
				case Chirality.Right:
					bool triggerDown2 = Input.GetKeyDown(KeyCode.JoystickButton15);
					bool touchDown2 = Input.GetKeyDown(KeyCode.JoystickButton9);

					//bool triggerUp2 = Input.GetKeyUp(KeyCode.JoystickButton15);
					//bool touchUp2 = Input.GetKeyUp(KeyCode.JoystickButton9);


					//rh trigger hold and trackpad
					//if (Input.GetKey(KeyCode.JoystickButton15) && Input.GetKey(KeyCode.JoystickButton9))
					//{
					//	triggerPad(hand);
					//}
					////rh trigger hold and trackpad
					//if ((triggerDown2 && Input.GetKey(KeyCode.JoystickButton9)) || (Input.GetKey(KeyCode.JoystickButton15) && touchDown2))
					//{
					//	triggerPadDown(hand);
					//}
					////rh trigger hold and trackpad
					//if ((Input.GetKeyUp(KeyCode.JoystickButton15) && Input.GetKey(KeyCode.JoystickButton9)) || (Input.GetKey(KeyCode.JoystickButton15) && Input.GetKeyUp(KeyCode.JoystickButton9)))
					//{
					//	triggerPadUp(hand);
					//}
					if ((Input.GetKeyDown(KeyCode.JoystickButton9)))
					{
						triggerPadDown(hand);
					}
					if ((Input.GetKeyUp(KeyCode.JoystickButton9)))
					{
						triggerPadUp(hand);
					}
					//rh trigger hold and not trackpad
					if (Input.GetKey(KeyCode.JoystickButton15) && !Input.GetKey(KeyCode.JoystickButton9))
					{
						triggerOnly(hand);
					}
					//rh trigger hold and not trackpad
					if (triggerDown2 && !Input.GetKey(KeyCode.JoystickButton9))
					{
						triggerOnlyDown(hand);
					}
					//rh trigger hold and not trackpad
					if (Input.GetKeyUp(KeyCode.JoystickButton15) && !Input.GetKey(KeyCode.JoystickButton9))
					{
						triggerOnlyUp(hand);
					}
					//rh trackpad hold not trigger 
					if (Input.GetKey(KeyCode.JoystickButton9) && !Input.GetKey(KeyCode.JoystickButton15))
					{
						trackpadOnly(hand);
					}
					//rh trackpad hold not trigger 
					if (touchDown2 && !Input.GetKey(KeyCode.JoystickButton15))
					{
						trackpadOnlyDown(hand);
					}
					//rh trackpad hold not trigger 
					if (Input.GetKeyUp(KeyCode.JoystickButton9) && !Input.GetKey(KeyCode.JoystickButton15))
					{
						trackpadOnlyUp(hand);
					}
					//rh grip axis
					if (Input.GetAxis("RightVRGripAxis") >= 0.01f)
					{
						if (!rgripDown)
						{
							rgripDown = true;
							gripDown(hand);
						}
						//gripHold(hand);
					}
					//rh grip axis
					if (Input.GetAxis("RightVRGripAxis") < 0.01f)
					{
						if (rgripDown)
						{
							rgripDown = false;
							gripUp(hand);
						}
					}
					//rh trigger axis only
					if (Input.GetAxis("RightVRTriggerAxis") >= 0.01f)
					{
						if (!rtriggerDown)
						{
							rtriggerDown = true;
							triggerOnlyDown(hand);
						}
					}
					//rh trigger axis only
					if (Input.GetAxis("RightVRTriggerAxis") < 0.01f)
					{
						if (rtriggerDown)
						{
							rtriggerDown = false;
							triggerOnlyUp(hand);
						}
					}
					//rh trigger axis + touchpad
					//if (Input.GetAxis("RightVRTriggerAxis") >= 0.01f && Input.GetKeyDown(KeyCode.JoystickButton9))
					//{
					//	if (!rtriggerDown)
					//	{
					//		rtriggerDown = true;
					//		triggerPadDown(hand);
					//	}
					//	else if (Input.GetKeyDown(KeyCode.JoystickButton9))
					//	{
					//		triggerPadDown(hand);
					//	}
					//}
					////rh trigger axis + touchpad
					//if (Input.GetAxis("RightVRTriggerAxis") < 0.01f && Input.GetKeyUp(KeyCode.JoystickButton9))
					//{
					//	if (rtriggerDown)
					//	{
					//		rtriggerDown = false;
					//		triggerPadUp(hand);
					//	}
					//	else if (Input.GetKeyDown(KeyCode.JoystickButton9))
					//	{
					//		triggerPadUp(hand);
					//	}
					//}
					break;
				default:
					break;
			}

		}
		#endregion

		private void gripUp(InteractionController hand)
		{
			thisPinType = pinType.none;
			eraserDetach();
			endInteraction();
		}

		private void gripDown(InteractionController hand)
		{
			//erase
			Debug.Log("You are trying to erase");
			setActiveFourFinger();
		}

		private void trackpadOnlyUp(InteractionController hand)
		{
			thisPinType = pinType.none;
			endInteraction();
		}

		private void trackpadOnlyDown(InteractionController hand)
		{
			throw new NotImplementedException();
		}

		private void triggerOnlyUp(InteractionController hand)
		{
			thisPinType = pinType.none;
			endInteraction();
		}

		private void triggerOnlyDown(InteractionController hand)
		{
			Debug.Log("You are trying to use Wireframe");
			setActiveOneFinger();
		}

		private void triggerPadUp(InteractionController hand)
		{
			thisPinType = pinType.none;
			endInteraction();
		}

		private void triggerPadDown(InteractionController hand)
		{
			Debug.Log("You are trying to make a Polygon");
			setActiveTwoFinger();
		}

		private void gripHold(InteractionController hand)
		{
			throw new NotImplementedException();
		}

		private void trackpadOnly(InteractionController hand)
		{
			throw new NotImplementedException();
		}

		private void triggerOnly(InteractionController hand)
		{
			throw new NotImplementedException();
		}

		private void triggerPad(InteractionController hand)
		{
			throw new NotImplementedException();
		}
	}
}
