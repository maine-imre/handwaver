using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.Gestures
{

    /// <summary>
    /// A class that makes tracking data from different sources generic
    /// </summary>
    public class GestureInput : MonoBehaviour
    {
        /// <summary>
        /// The hardware that is bieng used to track the body/hands
        /// </summary>
        public enum InputSource
        {
            OSVR,
            ValveSkeletal,
            LeapMotion,
            RealSense
        };

        /// <summary>
        /// The hardware being used to track the body/hands
        /// </summary>
        public InputSource _inputSource;

        /// <summary>
        /// The struct that will store tracking data in a generic form.
        /// </summary>
        internal static BodyInput bodyInput = new BodyInput();


        //public event HeadTrackingLost ;
        //public event HandTrackingLost ;
        //public event BodyTrackingLost ;

        /// <summary>
        /// Setup the BodyInput for each source.
        /// </summary>
        public void Awake()
        {
            switch (_inputSource)
            {
                case InputSource.OSVR:
                    //fill with OSVR tracking of head and hands
                    bodyInput.FullBodyTracking = false;
                    bodyInput.HandTracking = false;
                    bodyInput.SimulatedHandTracking = false;
                    bodyInput.HeadTracking = true;
                    bodyInput.SimulatedHeadTracking = false;
                    break;
                case InputSource.ValveSkeletal:
                    ///fill with OSVR tracking of head and skeletal tracking of hands.
                    bodyInput.FullBodyTracking = true;
                    bodyInput.HandTracking = true;
                    bodyInput.SimulatedHandTracking = true;
                    bodyInput.HeadTracking = true;
                    bodyInput.SimulatedHeadTracking = false;
                    break;
                case InputSource.LeapMotion:
                    //fill with OSVR tracking of head and skeletal tracking of hands.
                    bodyInput.FullBodyTracking = false;
                    bodyInput.HandTracking = true;
                    bodyInput.SimulatedHandTracking = true;
                    bodyInput.HeadTracking = true;
                    bodyInput.SimulatedHeadTracking = false;
                    break;
                case InputSource.RealSense:
                    // fill with RealSense tracking of head and skeletal tracking of hands. 
                    bodyInput.FullBodyTracking = false;
                    bodyInput.HandTracking = true;
                    bodyInput.SimulatedHandTracking = false;
                    bodyInput.HeadTracking = true;
                    bodyInput.SimulatedHeadTracking = false;
                    break;
            }

setupAvatar();
        }

        /// <summary>
        /// Every 1/90th of a second, update the Body Input
        /// </summary>
        public void FixedUpdate()
        {
            switch (_inputSource)
            {
                case InputSource.OSVR:
                    //fill with OSVR tracking of head and hands
setPositionsOSVR();
                    break;
                case InputSource.ValveSkeletal:
                    ///fill with OSVR tracking of head and skeletal tracking of hands.
setPositionsValve();
                    break;
                case InputSource.LeapMotion:
                    //fill with OSVR tracking of head and skeletal tracking of hands.
setPositionsLeapMotion();
                    break;
                case InputSource.RealSense:
                    // fill with RealSense tracking of head and skeletal tracking of hands.
setPositionsRealSense(); 
                    break;
            }
updateAvatar();
}
            
            public void setPositionsOSVR()
            {
            }
            public void setPositionsValve()
            {
            }
            
            public void setPositionsLeapMotion()
            {
            }
            
            public void setPositionsRealSense()
            {
            }

public void setupAvatar(){
}
public void updateAvatar(){
}
} 

  }
