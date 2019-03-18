using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;


namespace IMRE.Gestures
{

    /// <summary>
    /// A class that makes tracking data from different sources generic
    /// </summary>
    public class GestureInput : MonoBehaviour
    {
        
        //consdier switching this to a build flag system.
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
        /// The struct that stores tracking data in a generic form.
        /// This struct could be synced over the network in order to have avatars in a multiplayer context.
        /// This struct is passed to gestures in order to determine if they should be invoked.
        /// </summary>
        internal static BodyInput bodyInput = new BodyInput();

        private Queue<BodyInput> bodyInputQue = new Queue<BodyInput>();

        /// <summary>
        /// An array of all of the gestures in the scene.
        /// Used to lookup eligibility in relation to other gestures.
        /// </summary>
        public static Gesture[] gestures;


        //public event HeadTrackingLost ;
        //public event HandTrackingLost ;
        //public event BodyTrackingLost ;

#if  LeapMotion   
        public static Leap.Unity.LeapXRServiceProvider LeapService;
#endif
        
        /// <summary>
        /// Setup the BodyInput for each source.
        /// </summary>
        public void Awake()
        {
#if  LeapMotion   
            LeapService = FindObjectOfType<Leap.Unity.LeapXRServiceProvider>();
#endif
            
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

            bodyInputQue.Enqueue(bodyInput);
            if (bodyInputQue.Count > 10)
            {
                bodyInputQue.Dequeue();
                calculateVelocities();
            }

            updateAvatar();
            

        }

            private void setPositionsOSVR()
            {
                //bodyInput.Head =
                
                //bodyInput.LeftHand.Palm.Position =
                //bodyInput.LeftHand.Palm.Direction =
                //bodyInput.RightHand.Palm.Position = 
                //bodyInput.RightHand.Palm.Direction =
            }
            private void setPositionsValve()
            {
            }
            
            private void setPositionsLeapMotion()
            {            
                //#if LeapMotion
                bodyInput.LeftHand = leapHandConversion(bodyInput.LeftHand,LeapService.MakeTestHand(true));
                bodyInput.RightHand = leapHandConversion(bodyInput.RightHand,LeapService.MakeTestHand(false));
                //#endif
            }
#if LeapMotion

        private Hand leapHandConversion(Hand myHand, Leap.Hand lmHand)
        {
            myHand.Palm.Position = lmHand.PalmPosition.ToVector3();
            myHand.Palm.Direction = lmHand.PalmNormal.ToVector3();
            myHand.Wrist.Position = lmHand.WristPosition.ToVector3();
            myHand.Wrist.Direction = lmHand.Fingers[2].bones[0].Center.ToVector3() - lmHand.WristPosition.ToVector3();
            myHand.PinchStrength = lmHand.PinchStrength;
            myHand.IsPinching = lmHand.IsPinching();
            
            //count through five fingers
            for (int fIDX = 0; fIDX < 4; fIDX++)
            {
                //count through 4 joints
                for (int jIDX = 0; jIDX < 3; jIDX++)
                {
                    myHand.Fingers[fIDX].Direction = lmHand.Fingers[fIDX].Direction.ToVector3();
                    myHand.Fingers[fIDX].IsExtended = lmHand.Fingers[fIDX].IsExtended;
                    myHand.Fingers[fIDX].Joints[jIDX].Position = lmHand.Fingers[fIDX].bones[jIDX].Center.ToVector3();
                    myHand.Fingers[fIDX].Joints[jIDX].Direction = lmHand.Fingers[fIDX].bones[jIDX].Direction.ToVector3();
                }
            }

            return myHand;
        }
#endif
            
            private void setPositionsRealSense()
            {
            }

            private void setupAvatar()
            {
            }

            private void updateAvatar()
            {
            }

            private void calculateVelocities()
            {
                resetVelocities(bodyInput);
                
                int queueLength = bodyInputQue.Count;
                float frameRate = 90f; //consdier automating this

                BodyInput[] queueArray = bodyInputQue.ToArray();
                
                for (int k = 1; k < queueLength; k++)
                {
                    //take the diff for each frame and divide by framerate
                    BodyInput frame0 = queueArray[k - 1];//note that we start at k=1;
                    BodyInput frame1 = queueArray[k];
                    
                    bodyInput.Head.Velocity +=
                        (frame0.Head.Position-frame1.Head.Position)/frameRate;
                    bodyInput.Neck.Velocity +=
                        (frame0.Neck.Position - frame1.Neck.Position) / frameRate;
                    bodyInput.Chest.Velocity +=
                        (frame0.Chest.Position - frame1.Chest.Position) / frameRate;
                    bodyInput.Waist.Velocity += 
                        (frame0.Waist.Position - frame1.Waist.Position) / frameRate;
                    bodyInput.RightHand.Palm.Velocity += 
                        (frame0.RightHand.Palm.Position - frame1.RightHand.Palm.Position) / frameRate;
                    bodyInput.LeftHand.Palm.Velocity += 
                        (frame0.LeftHand.Palm.Position - frame1.LeftHand.Palm.Position) / frameRate;
                    bodyInput.RightHand.Wrist.Velocity += 
                        (frame0.RightHand.Wrist.Position - frame1.RightHand.Wrist.Position) / frameRate;
                    bodyInput.LeftHand.Wrist.Velocity += 
                        (frame0.LeftHand.Wrist.Position - frame1.LeftHand.Wrist.Position) / frameRate;
                
                    for (int i = 0; i < 3; i++)
                    {
                        bodyInput.LeftArm[i].Velocity += 
                            (frame0.LeftArm[i].Position - frame1.LeftArm[i].Position) / frameRate;
                        bodyInput.RightArm[i].Velocity +=
                            (frame0.RightArm[i].Position - frame1.RightArm[i].Position) / frameRate;
                        bodyInput.LeftLeg[i].Velocity += 
                            (frame0.LeftLeg[i].Position - frame1.LeftLeg[i].Position) / frameRate;
                        bodyInput.RightLeg[i].Velocity += 
                            (frame0.RightLeg[i].Position - frame1.RightLeg[i].Position) / frameRate;

                        for (int j = 0; j < 4; j++)
                        {
                            bodyInput.LeftHand.Fingers[i].Joints[j].Velocity +=
                                (frame0.LeftHand.Fingers[i].Joints[j].Position - frame1.LeftHand.Fingers[i].Joints[j].Position) / frameRate;
                            bodyInput.RightHand.Fingers[i].Joints[j].Velocity +=
                                (frame0.RightHand.Fingers[i].Joints[j].Position - frame1.RightHand.Fingers[i].Joints[j].Position) / frameRate;
                        }
                    }
                }


            }

            private void resetVelocities(BodyInput tmpBodyInput)
            {
                tmpBodyInput.Head.Velocity = Vector3.zero;
                tmpBodyInput.Neck.Velocity = Vector3.zero;
                tmpBodyInput.Chest.Velocity = Vector3.zero;
                tmpBodyInput.Waist.Velocity = Vector3.zero;
                tmpBodyInput.RightHand.Palm.Velocity = Vector3.zero;
                tmpBodyInput.LeftHand.Palm.Velocity = Vector3.zero;
                tmpBodyInput.RightHand.Wrist.Velocity = Vector3.zero;
                tmpBodyInput.LeftHand.Wrist.Velocity = Vector3.zero;
                
                for (int i = 0; i < 3; i++)
                {
                    tmpBodyInput.LeftArm[i].Velocity = Vector3.zero;
                    tmpBodyInput.RightArm[i].Velocity = Vector3.zero;
                    tmpBodyInput.LeftLeg[i].Velocity = Vector3.zero;
                    tmpBodyInput.RightLeg[i].Velocity = Vector3.zero;

                    for (int j = 0; j < 4; j++)
                    {
                        tmpBodyInput.LeftHand.Fingers[i].Joints[j].Velocity = Vector3.zero;
                        tmpBodyInput.RightHand.Fingers[i].Joints[j].Velocity = Vector3.zero;
                    }
                }
            }
    } 

  }
