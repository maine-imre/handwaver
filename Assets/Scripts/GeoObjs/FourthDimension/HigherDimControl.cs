using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System;
using UnityEngine.UI;
using System.Linq;

namespace IMRE.HandWaver.HigherDimensions
{
/// <summary>
/// Slider controls for higher dimensions
/// used in scale and dimension study
/// used with sliders to support VR and desktop implementations.
/// </summary>
	public class HigherDimControl : MonoBehaviour
    {
        internal static float xy = 0f;
        internal static float xz = 0f;
        internal static float yz = 0f;
        internal static float xw = 0f;
        internal static float yw = 0f;
        internal static float zw = 0f;

        public InteractionSlider xySlider;
        public InteractionSlider xzSlider;
        public InteractionSlider yzSlider;
        public InteractionSlider xwSlider;
        public InteractionSlider ywSlider;
        public InteractionSlider zwSlider;

        public Slider xyOnScreen;
        public Slider xzOnScreen;
        public Slider yzOnScreen;
        public Slider xwOnScreen;
        public Slider ywOnScreen;
        public Slider zwOnScreen;

        public bool manualOverride = false;
        [Range(0,360)]
        public static float xy_override = 0f;
        [Range(0, 360)]
        public static float xz_override = 0f;
        [Range(0, 360)]
        public static float yz_override = 0f;
        [Range(0, 360)]
        public static float xw_override = 0f;
        [Range(0, 360)]
        public static float yw_override = 0f;
        [Range(0, 360)]
        public static float zw_override = 0f;

        private void Update()
        {
            if (manualOverride)
            {
                xy = xy_override;
                xz = xz_override;
                yz = yz_override;
                xw = xw_override;
                yw = yw_override;
                zw = zw_override;
            }
        }

        private void Awake()
        {
            enableDisableSliders(UnityEngine.XR.XRDevice.isPresent);
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                xySlider.OnPress += updateLeapSliders;
                xzSlider.OnPress += updateLeapSliders;
                yzSlider.OnPress += updateLeapSliders;
                xwSlider.OnPress += updateLeapSliders;
                ywSlider.OnPress += updateLeapSliders;
                zwSlider.OnPress += updateLeapSliders;
            }
            else
            {
                xyOnScreen.onValueChanged.AddListener(updateUnitySlider);
                xzOnScreen.onValueChanged.AddListener(updateUnitySlider);
                yzOnScreen.onValueChanged.AddListener(updateUnitySlider);
                xwOnScreen.onValueChanged.AddListener(updateUnitySlider);
                ywOnScreen.onValueChanged.AddListener(updateUnitySlider);
                zwOnScreen.onValueChanged.AddListener(updateUnitySlider);
            }
        }

        private void enableDisableSliders(bool isPresent)
        {
            List<Slider> onscreen = new List<Slider>()
            {
                xyOnScreen, xzOnScreen, yzOnScreen, xwOnScreen, ywOnScreen,zwOnScreen
            };
            List<InteractionSlider> lm = new List<InteractionSlider>()
            {
                xySlider,xzSlider,yzSlider,xwSlider,ywSlider,zwSlider
            };

            onscreen.ForEach(p => p.gameObject.SetActive(!isPresent));
            lm.ForEach(p => p.gameObject.SetActive(isPresent));
        }

        private void updateUnitySlider(float arg0)
        {
            xy = xyOnScreen.normalizedValue * (360);
            xz = xzOnScreen.normalizedValue * (360);
            yz = yzOnScreen.normalizedValue * (360);
            xw = xwOnScreen.normalizedValue * (360);
            yw = ywOnScreen.normalizedValue * (360);
            zw = zwOnScreen.normalizedValue * (360);
        }

        private void updateLeapSliders()
        {
            xy = xySlider.HorizontalSliderPercent * (360);
            xz = xzSlider.HorizontalSliderPercent * (360);
            yz = yzSlider.HorizontalSliderPercent * (360);
            xw = xwSlider.HorizontalSliderPercent * (360);
            yw = ywSlider.HorizontalSliderPercent * (360);
            zw = zwSlider.HorizontalSliderPercent * (360);
        }
    }
}
