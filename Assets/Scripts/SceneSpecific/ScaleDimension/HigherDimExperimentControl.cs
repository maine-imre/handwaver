using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;

namespace IMRE.HandWaver.HigherDimensions
{
    /// <summary>
    /// Central control for scale and dimension study.
    /// needs to be renamed to reflect this.
    /// </summary>
	public class HigherDimExperimentControl : MonoBehaviour
    {
        public InteractionSlider foldSlider_lm;
        public UnityEngine.UI.Slider foldSlider_onScreen;
        internal static float degreeFolded = 0f;
        public bool animateFold = false;
        public InteractionSlider animateButton_lm;
        public UnityEngine.UI.Button aniamteButton_onScreen;

        public bool foldOverride;
        [Range(0, 360)]
        public float foldOverrideValue = 0f;

        private void Awake()
        {
            foldSlider_onScreen.gameObject.SetActive(!UnityEngine.XR.XRDevice.isPresent);
            foldSlider_lm.gameObject.SetActive(UnityEngine.XR.XRDevice.isPresent);

            aniamteButton_onScreen.gameObject.SetActive(!UnityEngine.XR.XRDevice.isPresent);
            animateButton_lm.gameObject.SetActive(UnityEngine.XR.XRDevice.isPresent);

            if (UnityEngine.XR.XRDevice.isPresent)
            {
                foldSlider_lm.OnPress += updateLMSlider;
                animateButton_lm.OnPress += updateAnimateLM;
            }
            else
            {
                foldSlider_onScreen.onValueChanged.AddListener(updateUSlider);
                aniamteButton_onScreen.onClick.AddListener(updateUAniamteButton);
            }
        }

        private void updateUAniamteButton()
        {
            //help!
            //animateFold  = aniamteButton_onScreen.
        }

        private void updateAnimateLM()
        {
            animateFold = animateButton_lm.isPressed;
        }

        private void updateUSlider(float arg0)
        {
            degreeFolded = foldSlider_onScreen.normalizedValue * 360f;
        }

        private void updateLMSlider()
        {
            degreeFolded = foldSlider_lm.normalizedHorizontalValue * 360f;
        }

        void Update()
        {
            if (foldOverride)
            {
                degreeFolded = foldOverrideValue;
            }
            if (animateFold)
            {
                Debug.Log(degreeFolded);
                degreeFolded++;
                if(foldSlider_lm != null)
                foldSlider_lm.HorizontalSliderValue = degreeFolded / 360f;
                if(foldSlider_onScreen != null)
                foldSlider_onScreen.normalizedValue = degreeFolded / 360f;
            }
        }
    }
}
