using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace IMRE.HandWaver.HigherDimensions
{
    /// <summary>
    ///     Central control for scale and dimension study.
    ///     needs to be renamed to reflect this.
    /// </summary>
    public class HigherDimExperimentControl : MonoBehaviour
    {
        internal static float degreeFolded;
        public Button aniamteButton_onScreen;
        public InteractionSlider animateButton_lm;
        public bool animateFold;

        public bool foldOverride;

        [Range(0, 360)] public float foldOverrideValue;

        public InteractionSlider foldSlider_lm;
        public Slider foldSlider_onScreen;

        private void Awake()
        {
            foldSlider_onScreen.gameObject.SetActive(!XRDevice.isPresent);
            foldSlider_lm.gameObject.SetActive(XRDevice.isPresent);

            aniamteButton_onScreen.gameObject.SetActive(!XRDevice.isPresent);
            animateButton_lm.gameObject.SetActive(XRDevice.isPresent);

            if (XRDevice.isPresent)
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

        private void Update()
        {
            if (foldOverride) degreeFolded = foldOverrideValue;
            if (animateFold)
            {
                Debug.Log(degreeFolded);
                degreeFolded++;
                if (foldSlider_lm != null)
                    foldSlider_lm.HorizontalSliderValue = degreeFolded / 360f;
                if (foldSlider_onScreen != null)
                    foldSlider_onScreen.normalizedValue = degreeFolded / 360f;
            }
        }
    }
}