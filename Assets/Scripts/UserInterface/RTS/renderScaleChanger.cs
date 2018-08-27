/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.VR;

//namespace IMRE.HandWaver
//{

//    public class renderScaleChanger : MonoBehaviour
//    {

//        public Transform renderScaleSlider;
//        private float RenderScale = 2.0f;

//        void Start()
//        {
//            RenderScale = 2.0f;
//            //RenderScale = VRSettings.renderScale;
//            renderScaleSlider.GetComponent<HoverItemDataSlider>().Value = RenderScale - renderScaleSlider.GetComponent<HoverItemDataSlider>().RangeMin / (renderScaleSlider.GetComponent<HoverItemDataSlider>().RangeMax - renderScaleSlider.GetComponent<HoverItemDataSlider>().RangeMin);
//        }

//        public void changeRenderScale()
//        {
//            RenderScale = renderScaleSlider.GetComponent<HoverItemDataSlider>().RangeValue;

//            VRSettings.renderScale = RenderScale;
//            Debug.Log(VRSettings.renderScale);
//        }
//    }
//}