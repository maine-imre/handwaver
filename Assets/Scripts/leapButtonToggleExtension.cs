using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;
using Leap.Unity.LeapPaint_v3;
using UnityEngine.Events;

namespace IMRE.HandWaver.Interface {
	[RequireComponent(typeof(PressableUI))]
    [RequireComponent(typeof(PressableButtonFeedback))]
    /// <summary>
    /// A script added to a Leap InteractionButton to give it toggle functionality.
    /// </summary>
	public class leapButtonToggleExtension : MonoBehaviour {

		private bool toggleState = false;

		public Color onColor = Color.green;
		public Color offColor = Color.gray;
        public Color disabledColor = Color.red;
        public Color pressedColor = Color.blue;
		private PressableUI _iButton;
        public PressableUI iButton { get
			{
				if (_iButton != null)
					return _iButton;
				else
					_iButton = GetComponent<PressableUI>();

				return GetComponent<PressableUI>();
			}
		}
        private bool _active = true;
		
        public KeyCode keyOverride;
		/// <summary>
		/// Set to false only if this object is still using InteractionButton.cs as the button script
		/// </summary>
		public bool newButtons = true;
		internal UnityEvent buttonPressed;

		/// <summary>
		/// Is the button in an ON state?
		/// </summary>
		public bool ToggleState
		{
			get
			{
				//Debug.Log("checking state " + toggleState);
				return toggleState;
			}

			set
			{
				//Debug.Log("setting state " + value);
				toggleState = value;
			}
		}

        public void setupButton()
		{
			if (GetComponent< PressableUI>() != null)
			{
				iButton.OnPress.AddListener(toggle);
			}
		}

		public void Update()
        {
            if (keyOverride != KeyCode.None && Input.GetKeyDown(keyOverride))
            {
                toggle();
            }
        }

        private void toggle()
		{
			ToggleState = !ToggleState;
			if(buttonPressed != null)
			{
				buttonPressed.Invoke();
			}
		}
	}
}