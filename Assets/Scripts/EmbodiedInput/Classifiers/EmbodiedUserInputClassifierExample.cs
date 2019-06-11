using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace IMRE.EmbodiedUserInput
{
    /// <summary>
    /// An interface for all classifiers of embodied user input.
    /// </summary>
    /// <typeparam name="BodyInput"></typeparam>
    public interface IEmbodiedClassifier<BodyInput> : IComponentData
    {
        /// <summary>
        /// A function to determine if the classifier should activate when deactivated.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool shouldActivate(BodyInput data);

        /// <summary>
        /// A classifier to determine if the classifier should cancel while active
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool shouldCancel(BodyInput data);

        
        /// <summary>
        /// Whether the classifier should finish due to completed action.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool shouldFinish { get; set; }
        /// <summary>
        /// Whether is classifier is eligible to trigger an action
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
        /// Whether the classifier was cancelled in the last iteration
        /// </summary>
        bool wasActivated { get; set; }
    }
    
    /// <summary>
    /// This is an example class that uses the emobided input classifier in a Job Component System
    /// </summary>
    public class EmbodiedUserInputClassifierExample : JobComponentSystem
    {
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
        
        /// <summary>
        /// A generic example of a classifier.  This should be renamed in each use case.
        /// </summary>
        public struct genericClassifier : IEmbodiedClassifier<BodyInput>
        {
            public Chirality chirality;
            public classifierType type;
            public Vector3 origin;
            public Vector3 direction;
            public bool shouldActivate(BodyInput data)
            {
                Hand hand;
                Vector3 velocity;
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
                        hand = data.GetHand(chirality);
                        direction = hand.Fingers[1].Direction;
                        origin = hand.Fingers[1].Joints[3].Position;
                        return (
                            (hand.Fingers[1].IsExtended) &&
                            !(hand.Fingers[2].IsExtended) &&
                            !(hand.Fingers[3].IsExtended) &&
                            !(hand.Fingers[4].IsExtended) //&&
                            //!(interactionHand.isGraspingObject)
                        );  
                    #endregion 
                    case classifierType.grasp:
                        #region ActivateGrasp
                        hand = data.GetHand(chirality);
                        return (hand.IsPinching);

                        #endregion
                    case classifierType.doubleGrasp:
                        #region ActivateDoubleGrasp
                        return data.LeftHand.IsPinching && data.RightHand.IsPinching;
                        #endregion
                    case classifierType.openPalm:
                        #region ActivateOpenPalm
                        hand = data.GetHand(chirality);
                        direction = hand.Palm.Direction;
                        origin = hand.Palm.Position;
                        return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5);
                        #endregion
                    case classifierType.openPalmPush:
                    #region Activate OpenPalmPush
                        hand = data.GetHand(chirality);
                        //want movement in plane of palm within tolerance.
                        velocity = hand.Palm.Velocity;
                        direction = hand.Palm.Direction;
                        origin = hand.Palm.Position;

                        //we want velocity to be nonzero.
                         speed = hand.Palm.Velocity.magnitude;
                        //we want to have close to zero angle between movement and palm.
                        angle = Mathf.Abs(Vector3.Angle(velocity, direction));
//TODO check the tolerances here.
                    return (hand.Fingers.Count(finger => finger.IsExtended) == 5) && speed > .5f && angle < 30f;
                    #endregion
                    case classifierType.openPalmSwipe:
                        #region
                        hand = data.GetHand(chirality);
                        //want movement in plane of palm within tolerance.
                        velocity = hand.Palm.Velocity;
                        direction = hand.Palm.Direction;
                        origin = hand.Palm.Position;

                        //we want velocity to be nonzero.
                        speed = hand.Palm.Velocity.magnitude;
                        //we want to have close to zero angle between movement and palm.
                        angle = 90 - Mathf.Abs(Vector3.Angle(velocity, direction));
//TODO check the tolerances here.
                        return (hand.Fingers.Count(finger => finger.IsExtended) == 5) && speed > .5f && angle <  30f;
                        #endregion
                    case classifierType.thumbsUp:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public bool shouldCancel(BodyInput data)
            {
                return !shouldActivate(data);
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
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, genericClassifier>
        {

            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref genericClassifier classifier) 
            {
                if (!classifier.isEligible)
                {
                    classifier.isEligible = classifier.shouldActivate(cBodyInput);
                        classifier.wasActivated = classifier.isEligible;
                        classifier.wasCancelled = false;
                        classifier.wasFinished = false;
                }
                else
                {
                    classifier.wasCancelled = classifier.shouldCancel(cBodyInput);
                    classifier.wasFinished = classifier.shouldFinish && !classifier.wasCancelled;
                    classifier.isEligible = !(classifier.wasCancelled || classifier.wasFinished);
                    classifier.wasActivated = false;
                    classifier.shouldFinish = false;
                }
            }
        }
        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new EmbodiedUserInputClassifierJob
            {
                //cBodyInput = 
                //clasifier struct = static
            };

            return job.Schedule(this, inputDependencies);
        }

    }
}