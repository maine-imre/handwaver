/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IMRE.HandWaver
{
    [RequireComponent(typeof(TextMeshProUGUI))]

    //This class displays FPS using TMPro, also colors text if the framerate drops below 60 FPS.
    //This class is derived from the UnityStandardASsets.Utility.FPSCounter class.
    class FPS_Warning : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        public int target = 60;
        public int minFPS = 30;
        const string display = "{0} FPS";
        private TextMeshProUGUI m_Text;


        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            m_Text = this.transform.GetComponent<TextMeshProUGUI>();
        }


        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
                if(m_CurrentFps < target)
                {
                    m_Text.faceColor = Color.yellow;
                }else if (m_CurrentFps < minFPS)
                {
                    m_Text.faceColor = Color.red;
                }
                else
                {
                    m_Text.faceColor = Color.white;
                }
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_Text.text = string.Format(display, m_CurrentFps);
            }
        }
    }
}
