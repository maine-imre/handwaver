#if LeapMotion
using System.Collections.Generic;
using Leap.Unity;
using Unity.Mathematics;
using UnityEngine;
#endif
#if ViveSense
using ViveHandTracking;

#endif

#if ValveSkeletal
    using Valve.VR;
#endif

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    ///     A class that makes tracking data from different sources generic
    /// </summary>
    public class BodyInputDataSystem : MonoBehaviour
    {
        public bool enableLeapMotion = true;
        public bool enableViveSense = true;
        public bool enableOSVR;
        public bool enableRealSense;
        public bool enableValveSkeletal;

        /// <summary>
        ///     The struct that stores tracking data in a generic form.
        ///     This struct could be synced over the network in order to have avatars in a multiplayer context.
        ///     This struct is passed to gestures in order to determine if they should be invoked.
        /// </summary>
        public static BodyInput bodyInput;

        private readonly Queue<BodyInput> bodyInputQue =
            new Queue<BodyInput>();

        private Camera mainCamera;

        //public event HeadTrackingLost ;
        //public event HandTrackingLost ;
        //public event BodyTrackingLost ;

#if LeapMotion
        private static LeapXRServiceProvider LeapService;
#endif

        /// <summary>
        ///     Setup the BodyInput for each source.
        /// </summary>
        public void Awake()
        {
            mainCamera = Camera.main;
            bodyInput = BodyInput.newInput();
            bodyInput.FullBodyTracking = false;
            bodyInput.HandTracking = false;
            bodyInput.SimulatedHandTracking = false;
            bodyInput.HeadTracking = false;
            bodyInput.SimulatedHeadTracking = false;
            if (enableOSVR) bodyInput.HeadTracking = true;

#if ViveSense
            if (enableViveSense)
            {
                bodyInput.FullBodyTracking = true;
                bodyInput.HandTracking = true;
                bodyInput.SimulatedHandTracking = true;
                bodyInput.HeadTracking = true;
            }
#endif

#if ValveSkeletal
            if (enableValveSkeletal)
            {
                bodyInput.FullBodyTracking = true;
                bodyInput.HandTracking = true;
                bodyInput.SimulatedHandTracking = true;
                bodyInput.HeadTracking = true;            
            }
#endif

#if RealSense
            if (enableRealSense)
            {
                bodyInput.FullBodyTracking = true;
                bodyInput.HandTracking = true;
                bodyInput.SimulatedHandTracking = true;
                bodyInput.HeadTracking = true;            
            }
#endif

#if LeapMotion
            LeapService = FindObjectOfType<LeapXRServiceProvider>();
            if (enableLeapMotion)
            {
                bodyInput.HandTracking = true;
                bodyInput.SimulatedHandTracking = true;
                bodyInput.HeadTracking = true;
                bodyInput.SimulatedHeadTracking = false;
            }
#endif

#if RealSense
            if (enableRealSense)
            {
                bodyInput.HandTracking = true;
                bodyInput.HeadTracking = true;
            }
#endif
            setupAvatar();
        }

        /// <summary>
        ///     Every 1/90th of a second, update the Body Input
        /// </summary>
        public void FixedUpdate()
        {
#if ValveSkeletal
            if (enableOSVR)
            {
                setPositionsOSVR();
            }
#endif

#if ViveSense
            if (enableViveSense)
            {
                if (GestureProvider.UpdatedInThisFrame)
                {
                    //this will enter here only when there is at least one hand with no new data from the vive pro tracking
                    //this means that we do not know which hand or if it was both hands that were not seen by the sensor this frame

                    //this is a dummy value which will be used to indicate that hands were not tracked
                    var empty = new float3(-10f, -10f, -10f);

                    //These two ifs  will check to see which hand was not tracked. Then gives dummy values for all fields
                    if (GestureProvider.LeftHand == null)
                    {
                        bodyInput.LeftHand.Palm.Position = empty;
                        bodyInput.LeftHand.Palm.Direction = empty;
                        bodyInput.LeftHand.PinchStrength = 0;

                        //count through five fingers
                        for (var fIDX = 0; fIDX < 5; fIDX++)
                        {
                            bodyInput.LeftHand.Fingers[fIDX].Direction = empty;

                            //count through 4 joints
                            for (var jIDX = 0; jIDX < 4; jIDX++)
                            {
                                bodyInput.LeftHand.Fingers[fIDX].Joints[jIDX].Position = empty;
                                bodyInput.LeftHand.Fingers[fIDX].Joints[jIDX].Direction = empty;
                            }

                            //this could cause problems later if we set some sort of meaning to be derived from making a fist
                            bodyInput.LeftHand.Fingers[fIDX].IsExtended = false;
                        }
                    }

                    if (GestureProvider.RightHand == null)
                    {
                        bodyInput.RightHand.Palm.Position = empty;
                        bodyInput.RightHand.Palm.Direction = empty;
                        bodyInput.RightHand.PinchStrength = 0;

                        for (var fIDX = 0; fIDX < 5; fIDX++)
                        {
                            bodyInput.RightHand.Fingers[fIDX].Direction = empty;

                            //count through 4 joints
                            for (var jIDX = 0; jIDX < 4; jIDX++)
                            {
                                bodyInput.RightHand.Fingers[fIDX].Joints[jIDX].Position = empty;
                                bodyInput.RightHand.Fingers[fIDX].Joints[jIDX].Direction = empty;
                            }

                            //this could cause problems later if we set some sort of meaning to be derived from making a fist
                            bodyInput.RightHand.Fingers[fIDX].IsExtended = false;
                        }
                    }
                }

                setPositionsViveSense();
            }
#endif

#if RealSense
            if (enableRealSense)
            {
                    setPositionsRealSense();         
            }
#endif

#if LeapMotion
            if (enableLeapMotion)
            {
                setPositionsLeapMotion();

                bodyInput.Head.Position = mainCamera.transform.position;
                bodyInput.Head.Direction = mainCamera.transform.forward;
            }
#endif

#if RealSense
            if (enableRealSense)
            {
                setPositionsRealSense();
            }
#endif

#if ValveSkeletal
            if (enableValveSkeletal)
                {  
                  setPositionsValve();
                }
#endif

            bodyInputQue.Enqueue(bodyInput);
            if (bodyInputQue.Count > 10)
            {
                bodyInputQue.Dequeue();
                calculateVelocities();
            }

            updateAvatar();

            //TODO pass body input to other players
        }

#if ValveSkeletal
            private void setPositionsOSVR()
            {
                //bodyInput.Head = SteamVR_Input_Sources.Head
                
                //bodyInput.LeftHand.Palm.Position =
                //bodyInput.LeftHand.Palm.Direction =
                //bodyInput.RightHand.Palm.Position = 
                //bodyInput.RightHand.Palm.Direction =
            }
            
            private void setPositionsValve()
            {
                //SteamVR_Input_Sources.LeftHand
            }
#endif

#if LeapMotion
        private void setPositionsLeapMotion()
        {
            if (Hands.Left != null)
            {
                bodyInput.LeftHand = leapHandConversion(bodyInput.LeftHand, Hands.Left);
            }
            else
            {
                var empty = new float3(-10f, -10f, -10f);

                bodyInput.LeftHand.Palm.Position = empty;
                bodyInput.LeftHand.Palm.Direction = empty;
                bodyInput.LeftHand.PinchStrength = 0;

                //count through five fingers
                for (var fIDX = 0; fIDX < 5; fIDX++)
                {
                    bodyInput.LeftHand.Fingers[fIDX].Direction = empty;

                    //count through 4 joints
                    for (var jIDX = 0; jIDX < 4; jIDX++)
                    {
                        bodyInput.LeftHand.Fingers[fIDX].Joints[jIDX].Position = empty;
                        bodyInput.LeftHand.Fingers[fIDX].Joints[jIDX].Direction = empty;
                    }

                    //this could cause problems later if we set some sort of meaning to be derived from making a fist
                    bodyInput.LeftHand.Fingers[fIDX].IsExtended = false;
                }
            }

            if (Hands.Right != null)
            {
                bodyInput.RightHand = leapHandConversion(bodyInput.RightHand, Hands.Right);
            }
            else
            {
                var empty = new float3(-10f, -10f, -10f);
                bodyInput.RightHand.Palm.Position = empty;
                bodyInput.RightHand.Palm.Direction = empty;
                bodyInput.RightHand.PinchStrength = 0;

                for (var fIDX = 0; fIDX < 5; fIDX++)
                {
                    bodyInput.RightHand.Fingers[fIDX].Direction = empty;

                    //count through 4 joints
                    for (var jIDX = 0; jIDX < 4; jIDX++)
                    {
                        bodyInput.RightHand.Fingers[fIDX].Joints[jIDX].Position = empty;
                        bodyInput.RightHand.Fingers[fIDX].Joints[jIDX].Direction = empty;
                    }

                    //this could cause problems later if we set some sort of meaning to be derived from making a fist
                    bodyInput.RightHand.Fingers[fIDX].IsExtended = false;
                }
            }
        }

        private Hand leapHandConversion(Hand myHand, Leap.Hand lmHand)
        {
            myHand.Palm.Position = lmHand.PalmPosition.ToVector3();
            myHand.Palm.Direction = lmHand.PalmNormal.ToVector3();
            myHand.Wrist.Position = lmHand.WristPosition.ToVector3();
            myHand.Wrist.Direction = lmHand.Fingers[2].bones[0].Center.ToVector3() - lmHand.WristPosition.ToVector3();
            myHand.PinchStrength = lmHand.PinchStrength;

            //count through five fingers
            for (var fIDX = 0; fIDX < 5; fIDX++)
            {
                myHand.Fingers[fIDX].Direction = lmHand.Fingers[fIDX].Direction.ToVector3();
                myHand.Fingers[fIDX].IsExtended = lmHand.Fingers[fIDX].IsExtended;

                //count through 4 joints
                for (var jIDX = 0; jIDX < 4; jIDX++)
                {
                    myHand.Fingers[fIDX].Joints[jIDX].Position = lmHand.Fingers[fIDX].bones[jIDX].Center.ToVector3();
                    myHand.Fingers[fIDX].Joints[jIDX].Direction =
                        lmHand.Fingers[fIDX].bones[jIDX].Direction.ToVector3();
                }
            }

            return myHand;
        }
#endif

#if ViveSense
        private void setPositionsViveSense()
        {
            if (GestureProvider.LeftHand != null)
                bodyInput.LeftHand = viveHandConversion(bodyInput.LeftHand, GestureProvider.LeftHand);
            if (GestureProvider.RightHand != null)
                bodyInput.RightHand = viveHandConversion(bodyInput.RightHand, GestureProvider.RightHand);
        }

        private Hand viveHandConversion(Hand myHand, GestureResult viveHand)
        {
            myHand.Palm.Position = viveHand.position;
            //TODO make this better

            myHand.Palm.Direction = Vector3.Cross(viveHand.points[0] - viveHand.points[5],
                viveHand.points[0] - viveHand.points[17]).normalized;
            myHand.Wrist.Position = viveHand.points[0];
            myHand.Wrist.Direction = viveHand.points[9] - viveHand.points[1];

            myHand.PinchStrength = pinchStrength(myHand);

            //count through five fingers
            for (var fIDX = 0; fIDX < 5; fIDX++)
            {
                //average dir across last two joints
                myHand.Fingers[fIDX].Direction = viveHand.points[4 * fIDX + 4] - viveHand.points[4 * fIDX + 2];

                //count through 4 joints
                for (var jIDX = 0; jIDX < 4; jIDX++)
                {
                    myHand.Fingers[fIDX].Joints[jIDX].Position = viveHand.points[4 * fIDX + jIDX + 1];
                    if (jIDX == 0)
                        myHand.Fingers[fIDX].Joints[jIDX].Direction =
                            viveHand.points[4 * fIDX + jIDX + 1] - viveHand.points[0];
                    else
                        myHand.Fingers[fIDX].Joints[jIDX].Direction =
                            viveHand.points[4 * fIDX + jIDX + 1] - viveHand.points[4 * fIDX + jIDX];
                }

                myHand.Fingers[fIDX].IsExtended = isFingerExtended(myHand, fIDX);
            }

            return myHand;
        }
#endif

        private static bool isFingerExtended(Hand currHand, int fingerIndex)
        {
            //TODO make this better, consider curl
            return Mathf.Abs(Vector3.Angle(currHand.Fingers[fingerIndex].Joints[3].Direction,
                       currHand.Fingers[fingerIndex].Joints[0].Direction)) < 30f;
            //return Vector3.Dot(currHand.Fingers[fingerIndex].Direction, currHand.Palm.Direction) < .1f;
        }

        private static float pinchStrength(Hand currHand)
        {
            return 1 - Vector3.Magnitude(currHand.Fingers[0].Joints[3].Position -
                                         currHand.Fingers[1].Joints[3].Position) * 10f;
        }

        private void setupAvatar()
        {
            //throw new NotImplementedException();
        }

        private void updateAvatar()
        {
            //throw new NotImplementedException();
        }

        private void calculateVelocities()
        {
            resetVelocities(bodyInput);

            var queueLength = bodyInputQue.Count;

            var frameRate = 90f; // consider automating this

            var queueArray = bodyInputQue.ToArray();

            // This should loop from the most recent to the least recent pairs of frames.
            // For example an array of length 5, the loop would do 5/4, 4/3, 3/2, 2/1, 1/0. 
            // This should have the behaviour stopping at K = 1 such that k-1 is 0.
            for (var k = queueLength; k > 2; k--)
            {
                //take the diff for each frame and divide by framerate
                var frame0 = queueArray[k - 1];
                var frame1 = queueArray[k - 2];

                bodyInput.Head.Velocity +=
                    (frame0.Head.Position - frame1.Head.Position) / frameRate;
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

                for (var i = 0; i < 3; i++)
                {
                    bodyInput.LeftArm[i].Velocity +=
                        (frame0.LeftArm[i].Position - frame1.LeftArm[i].Position) / frameRate;
                    bodyInput.RightArm[i].Velocity +=
                        (frame0.RightArm[i].Position - frame1.RightArm[i].Position) / frameRate;
                    bodyInput.LeftLeg[i].Velocity +=
                        (frame0.LeftLeg[i].Position - frame1.LeftLeg[i].Position) / frameRate;
                    bodyInput.RightLeg[i].Velocity +=
                        (frame0.RightLeg[i].Position - frame1.RightLeg[i].Position) / frameRate;
                }

                for (var i = 0; i < 5; i++)
                for (var j = 0; j < 4; j++)
                {
                    bodyInput.LeftHand.Fingers[i].Joints[j].Velocity +=
                        (frame0.LeftHand.Fingers[i].Joints[j].Position -
                         frame1.LeftHand.Fingers[i].Joints[j].Position) / frameRate;
                    bodyInput.RightHand.Fingers[i].Joints[j].Velocity +=
                        (frame0.RightHand.Fingers[i].Joints[j].Position -
                         frame1.RightHand.Fingers[i].Joints[j].Position) / frameRate;
                }
            }
        }

        private void resetVelocities(BodyInput currBodyInput)
        {
            currBodyInput.Head.Velocity = Vector3.zero;
            currBodyInput.Neck.Velocity = Vector3.zero;
            currBodyInput.Chest.Velocity = Vector3.zero;
            currBodyInput.Waist.Velocity = Vector3.zero;
            currBodyInput.RightHand.Palm.Velocity = Vector3.zero;
            currBodyInput.LeftHand.Palm.Velocity = Vector3.zero;
            currBodyInput.RightHand.Wrist.Velocity = Vector3.zero;
            currBodyInput.LeftHand.Wrist.Velocity = Vector3.zero;

            for (var i = 0; i < 3; i++)
            {
                currBodyInput.LeftArm[i].Velocity = Vector3.zero;
                currBodyInput.RightArm[i].Velocity = Vector3.zero;
                currBodyInput.LeftLeg[i].Velocity = Vector3.zero;
                currBodyInput.RightLeg[i].Velocity = Vector3.zero;
            }

            for (var i = 0; i < 5; i++)
            for (var j = 0; j < 4; j++)
            {
                currBodyInput.LeftHand.Fingers[i].Joints[j].Velocity = Vector3.zero;
                currBodyInput.RightHand.Fingers[i].Joints[j].Velocity = Vector3.zero;
            }
        }
    }
}