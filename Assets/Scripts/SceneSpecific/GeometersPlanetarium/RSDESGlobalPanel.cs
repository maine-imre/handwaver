using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IMRE.HandWaver.Space
{
	/// <summary>
	/// This script controls the functionality of the Global Menu panel in the RSDES scene.
	/// The main contributor(s) to this script is NG
	/// Will be replaced in UX overhaul.
	/// </summary>
	public class RSDESGlobalPanel : MonoBehaviour
	{
		[System.Serializable]
		public struct buttonFunction
		{
			public Leap.Unity.LeapPaint_v3.PressableUI button;
			public UnityEvent action;

			internal buttonFunction(Leap.Unity.LeapPaint_v3.PressableUI newbutton)
			{
				this.button = newbutton;
				this.action = new UnityEvent();
			}
		};

		public List<buttonFunction> globalPanelButtons;

		private void Start()
		{
			globalPanelButtons.ForEach(bf => setButtonAction(bf));
		}

		private void setButtonAction(buttonFunction bf)
		{
			if (bf.action != null)
			{
				if (bf.button.GetComponent<IMRE.HandWaver.Interface.leapButtonToggleExtension>() != null)
				{
					bf.button.GetComponent<IMRE.HandWaver.Interface.leapButtonToggleExtension>().buttonPressed = bf.action;
					bf.button.GetComponent<IMRE.HandWaver.Interface.leapButtonToggleExtension>().setupButton();
				}

			}
			else
			{
				Debug.Log("UNSET ACTION FOR BUTTON "+bf.button.name);
			}
		}

		[ContextMenu("Get Child Buttons")]
		public void getChildButtons()
		{
			globalPanelButtons.Clear();
			foreach (Leap.Unity.LeapPaint_v3.PressableUI childedButton in GetComponentsInChildren<Leap.Unity.LeapPaint_v3.PressableUI>())
			{
				globalPanelButtons.Add(new buttonFunction(childedButton));
			}
		}
	}
}