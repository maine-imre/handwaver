using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMRE.Math;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    /// An interface for all classifiers of embodied user input.
    /// </summary>
    /// <typeparam name="BodyInput"></typeparam>
    public interface IEmbodiedClassifier : IComponentData
    {
        /// <summary>
        /// A function to determine if the classifier should activate when deactivated.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool shouldActivate();

        /// <summary>
        /// A classifier to determine if the classifier should cancel while active
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool shouldCancel();

        
        /// <summary>
        /// Whether the classifier should finish due to completed action.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool shouldFinish { get; set; }
        /// <summary>
        /// Whether a classifier is eligible to trigger an action
        /// </summary>
        bool isEligible { get; set; }
        /// <summary>
        /// Whether the classifier was cancelled in the last iteration
        /// </summary>
        bool wasCancelled { get; set; }
        /// <summary>
        /// Whether the classifier was finished in the last iteration
        /// </summary>
        bool wasFinished { get; set; }
        /// <summary>
        /// Whether the classifier was activated in the last iteration
        /// </summary>
        bool wasActivated { get; set; }
    }
    
    /// <summary>
    /// This is an example class that uses the embodied input classifier in a Job Component System
    /// </summary>
    public class EmbodiedUserInputClassifier : JobComponentSystem
    {
        //Classifiers are referring to gestures which can be performed.
        public enum classifierType
        {
            point,
            grasp,
            doubleGrasp,
            openPalm,
            openPalmPush,
            openPalmSwipe,
            thumbsUp
        };
        
        /// <inheritdoc />
        /// <summary>
        /// A generic example of a classifier.  This should be renamed in each use case.
        /// </summary>
        public struct EmbodiedClassifier : IEmbodiedClassifier
        {
            public Chirality chirality;
            public classifierType type;
            
            public float3 origin;
            public float3 direction;
            public bool shouldActivate()
            {
                Hand hand;
                float3 velocity;
                float speed;
                float angle;
                switch (type)
                {
                    case classifierType.point:
                        #region ActivatePoint
                        //Fingers are indexed from thumb to pinky. The thumb is 0, index is 1
                        //middle 2, ring 3, pinky 4. Below, we return true if the pinky,
                        //ring finger, and middle finger are not extended while the index
                        //finger is extended. There is also an additional portion to this 
                        //where the hand should not be pointing if there is a pinch happening.
                        //This prevents false pointing while trying to pinch something
              
                        //The thumb is ignored in whether this gesture will activate or not
             
                        //note that we need a new concept for grasping object.
                        
                        //is the left or right hand being used?
                        hand = BodyInputDataSystem.bodyInput.GetHand(chirality);
                        //where is the index finger pointing?
                        direction = hand.Fingers[1].Direction;
                        origin = hand.Fingers[1].Joints[3].Position;
                        //return true if only the index finger is extended, ignoring what the thumb is doing
                        return (
                            (hand.Fingers[1].IsExtended) &&
                            !(hand.Fingers[2].IsExtended) &&
                            !(hand.Fingers[3].IsExtended) &&
                            !(hand.Fingers[4].IsExtended)
                        );  
                    #endregion 
                    case classifierType.grasp:
                        #region ActivateGrasp
                        //is the left or right hand being used?
                        hand = BodyInputDataSystem.bodyInput.GetHand(chirality);
                        //if the hand is currently performing a pinch then a grasp should be attempted.
                        return (hand.IsPinching);

                    #endregion
                    case classifierType.doubleGrasp:
                        #region ActivateDoubleGrasp
                        //if both hands are performing a pinch, then attempt a double grasp
                        return BodyInputDataSystem.bodyInput.LeftHand.IsPinching && BodyInputDataSystem.bodyInput.RightHand.IsPinching;
                    #endregion
                    case classifierType.openPalm:
                        #region ActivateOpenPalm
                        //is the left or right hand being used?
                        hand = BodyInputDataSystem.bodyInput.GetHand(chirality);
                        //how is the palm rotated?
                        direction = hand.Palm.Direction;
                        //where is the palm?
                        origin = hand.Palm.Position;
                        //if all fingers are extended, the palm is open
                        return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5);
                    #endregion
                    case classifierType.openPalmPush:
                        #region Activate OpenPalmPush
                        //is the left or right hand being used?
                        hand = BodyInputDataSystem.bodyInput.GetHand(chirality);
                        //want movement in plane of palm within tolerance.
                        velocity = hand.Palm.Velocity;
                        direction = hand.Palm.Direction;
                        origin = hand.Palm.Position;

                        //we want velocity to be nonzero.
                        //distance is a workaround for magnitude.
                        speed = math.distance(float3.zero, velocity); 
                        //we want to have close to zero angle between movement and palm.
                        angle = math.abs(Operations.Angle(velocity, direction));
//TODO check the tolerances here.
                        return (hand.Fingers.Count(finger => finger.IsExtended) == 5) && speed > .5f && angle < 30f;
                    #endregion
                    case classifierType.openPalmSwipe:
                        #region
                        hand = BodyInputDataSystem.bodyInput.GetHand(chirality);
                        //want movement in plane of palm within tolerance.
                        velocity = hand.Palm.Velocity;
                        direction = hand.Palm.Direction;
                        origin = hand.Palm.Position;

                        //we want velocity to be nonzero.
                        speed = math.distance(float3.zero,hand.Palm.Velocity);
                        //we want to have close to zero angle between movement and palm.
                        angle = 90 - math.abs(Operations.Angle(velocity, direction));
//TODO check the tolerances here.
                        //if the palm is open and moving, then note the plane it is moving through and activate the
                        //gesture for swiping
                        return (hand.Fingers.Count(finger => finger.IsExtended) == 5) && speed > .5f && angle <  30f;
                    #endregion
                    case classifierType.thumbsUp:
                        #region Thumbs Up Activate
                        //is the left or right hand being used?
                        hand = BodyInputDataSystem.bodyInput.GetHand(chirality);
                        //where is the thumb pointing?
                        direction = hand.Fingers[0].Direction;
                        origin = hand.Fingers[0].Joints[3].Position;
                        //if only the thumb is extended and all other fingers are retracted, thumbs up
                        return (
                            (hand.Fingers[0].IsExtended) &&
                            !(hand.Fingers[1].IsExtended) &&
                            !(hand.Fingers[2].IsExtended) &&
                            !(hand.Fingers[3].IsExtended) &&
                            !(hand.Fingers[4].IsExtended) 
                        );  
                    #endregion
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public bool shouldCancel()
            {
                //currently the check to see if a gesture should stop is the opposite of if it should start
                //this is likely not always going to cut it. There will be more reasons for stopping a gesture
                //than the conditions for activating it no longer being met.
                //BodyInput data = BodyInputDataSystem.bodyInput;

                return !shouldActivate();
            }

            public bool shouldFinish { get; set; }

            public bool isEligible { get; set; }
            public bool wasCancelled { get; set; }
            public bool wasFinished { get; set; }
            public bool wasActivated { get; set; }
        }

        /// <summary>
        /// A thin layer of general abstraction for one-handed and two-handed gestures.
        /// Inspired by LeapPaint https://github.com/leapmotion/Paint
        /// </summary>
        [BurstCompile]
        public struct EmbodiedUserInputClassifierJob : IJobForEach<EmbodiedClassifier>
        {
            
            public void Execute(ref EmbodiedClassifier embodiedClassifier)
            {
                //Gestures are not always going to stop at the conclusion of their function.
                //Therefore this is needed to determine if a gesture is ongoing or cancelled during operation
                //by the user.
                
                //if the gesture is not eligible to activate last frame
                if (!embodiedClassifier.isEligible)
                {
                    //check to see if it should activate and update its eligibility to activate
                    embodiedClassifier.isEligible = embodiedClassifier.shouldActivate();
                    //If this is eligible, and it wasn't eligible before, it is activated
                    embodiedClassifier.wasActivated = embodiedClassifier.isEligible;
                    //therefore it was not cancelled
                    embodiedClassifier.wasCancelled = false;
                    //and it was not finished (since it just started)
                    embodiedClassifier.wasFinished = false;
                }
                else
                {   //if the gesture was eligible last frame
                    
                    //If the gesture should cancel, then cancel it.
                    embodiedClassifier.wasCancelled = embodiedClassifier.shouldCancel();
                    //The gesture wasn't finished if it is ongoing
                    embodiedClassifier.wasFinished = embodiedClassifier.shouldFinish && !embodiedClassifier.wasCancelled;
                    //The gesture is not eligible to start if it is ongoing
                    embodiedClassifier.isEligible = !(embodiedClassifier.wasCancelled || embodiedClassifier.wasFinished);
                    //since it was not activated this frame, set wasActivated to false
                    embodiedClassifier.wasActivated = false;
                    //the gesture should not finish next frame because it is either still ongoing or not continuing 
                    //after this frame.
                    embodiedClassifier.shouldFinish = false;
                }
            }
        }
        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new EmbodiedUserInputClassifierJob();
            
            //schedule all of the jobs to be completed which were set up this frame.
            return job.Schedule(this, inputDependencies);
        }

    }
}