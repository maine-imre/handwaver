using System;
using System.Linq;
using IMRE.Math;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
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
    }

    /// <inheritdoc />
    /// <summary>
    ///     A generic example of a classifier.  This should be renamed in each use case.
    /// </summary>
    public struct EmbodiedClassifier : IComponentData
    {
        public Chirality Chirality;
        public classifierType type;

        public float3 origin;
        public float3 direction;

        public bool shouldFinish;
        public bool isEligible;
        public bool wasCancelled;
        public bool wasFinished;
        public bool wasActivated;
    }

    public class EmbodiedUserInputClassifierAuthoring : MonoBehaviour
    {
        public static EmbodiedClassifier[] classifiers;
        public bool printDebug;

        private void Start()
        {
            classifiers = new EmbodiedClassifier[13];
            addClassifierBatch();
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < classifiers.Length; i++) Classify(ref classifiers[i]);

            if (printDebug)
                Enumerable.ToList(Enumerable.Where(Enumerable.ToList(classifiers), c => c.isEligible))
                    .ForEach(c => Debug.Log(c.type + " : " + c.Chirality));
        }

        /// <summary>
        ///     Adds one of each type of classifier to the job list
        /// </summary>
        private void addClassifierBatch()
        {
            //each classifier represents a gesture which can be performed.

            var amount = 13;
            classifiers[0] = new EmbodiedClassifier
            {
                //the point gesture is a one handed gesture. Therefore there must be a classifier for each hand's
                //point gesture.

                //point gesture's classifier for left hand
                Chirality = Chirality.Left,
                type = classifierType.point
            };
            classifiers[1] = new EmbodiedClassifier
            {
                //point gesture's classifier for right hand
                Chirality = Chirality.Right,
                type = classifierType.point
            };
            classifiers[2] = new EmbodiedClassifier
            {
                //grasp gesture's classifier for left hand
                Chirality = Chirality.Left,
                type = classifierType.grasp
            };
            classifiers[3] = new EmbodiedClassifier
            {
                //grasp gesture's classifier for right hand
                Chirality = Chirality.Right,
                type = classifierType.grasp
            };
            classifiers[4] = new EmbodiedClassifier
            {
                //Open Palm gesture's classifier for left hand
                Chirality = Chirality.Left,
                type = classifierType.openPalm
            };
            classifiers[5] = new EmbodiedClassifier
            {
                //open palm gesture's classifier for right hand
                Chirality = Chirality.Right,
                type = classifierType.openPalm
            };
            classifiers[6] = new EmbodiedClassifier
            {
                //open palm push gesture's classifier for left hand
                Chirality = Chirality.Left,
                type = classifierType.openPalmPush
            };
            classifiers[7] = new EmbodiedClassifier
            {
                //open palm push gesture's classifier for right hand
                Chirality = Chirality.Right,
                type = classifierType.openPalmPush
            };
            classifiers[8] = new EmbodiedClassifier
            {
                //open palm swipe gesture's classifier for left hand
                Chirality = Chirality.Left,
                type = classifierType.openPalmSwipe
            };
            classifiers[9] = new EmbodiedClassifier
            {
                //open palm swipe gesture's classifier for right hand
                Chirality = Chirality.Right,
                type = classifierType.openPalmSwipe
            };
            classifiers[10] = new EmbodiedClassifier
            {
                //thumbs up gesture's classifier for left hand
                Chirality = Chirality.Left,
                type = classifierType.thumbsUp
            };
            classifiers[11] = new EmbodiedClassifier
            {
                //thumbs up gesture's classifier for right hand
                Chirality = Chirality.Right,
                type = classifierType.thumbsUp
            };
            classifiers[12] = new EmbodiedClassifier
            {
                //double grasp is a two handed gesture. This means it will only need a single classifier due
                //to it using both hands and therefore not needing separate instances for which hand is being used.
                type = classifierType.doubleGrasp
            };
        }

        private static bool shouldActivate(ref EmbodiedClassifier embodiedClassifier)
        {
            Hand hand;
            float3 velocity;
            float speed;

            float angle;

            switch (embodiedClassifier.type)
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
                    hand = BodyInputDataSystem.bodyInput.GetHand(embodiedClassifier.Chirality);
                    //where is the index finger pointing?
                    embodiedClassifier.direction = hand.Fingers[1].Direction;
                    embodiedClassifier.origin = hand.Fingers[1].Joints[3].Position;
                    //return true if only the index finger is extended, ignoring what the thumb is doing
                    return hand.Fingers[1].IsExtended &&
                           !hand.Fingers[2].IsExtended &&
                           !hand.Fingers[3].IsExtended &&
                           !hand.Fingers[4].IsExtended;

                #endregion

                case classifierType.grasp:

                    #region ActivateGrasp

                    //is the left or right hand being used?
                    hand = BodyInputDataSystem.bodyInput.GetHand(embodiedClassifier.Chirality);
                    //if the hand is currently performing a pinch then a grasp should be attempted.
                    return hand.IsPinching;

                #endregion

                case classifierType.doubleGrasp:

                    #region ActivateDoubleGrasp

                    //if both hands are performing a pinch, then attempt a double grasp
                    return BodyInputDataSystem.bodyInput.LeftHand.IsPinching &&
                           BodyInputDataSystem.bodyInput.RightHand.IsPinching;

                #endregion

                case classifierType.openPalm:

                    #region ActivateOpenPalm

                    //is the left or right hand being used?
                    hand = BodyInputDataSystem.bodyInput.GetHand(embodiedClassifier.Chirality);
                    //how is the palm rotated?
                    embodiedClassifier.direction = hand.Palm.Direction;
                    //where is the palm?
                    embodiedClassifier.origin = hand.Palm.Position;
                    //if all fingers are extended, the palm is open
                    return Enumerable.Count(hand.Fingers, finger => finger.IsExtended) == 5;

                #endregion

                case classifierType.openPalmPush:

                    #region Activate OpenPalmPush

                    //is the left or right hand being used?
                    hand = BodyInputDataSystem.bodyInput.GetHand(embodiedClassifier.Chirality);
                    //want movement in plane of palm within tolerance.
                    velocity = hand.Palm.Velocity;
                    embodiedClassifier.direction = hand.Palm.Direction;
                    embodiedClassifier.direction = hand.Palm.Position;

                    //we want velocity to be nonzero.
                    //distance is a workaround for magnitude.
                    speed = math.distance(float3.zero, velocity);
                    //we want to have close to zero angle between movement and palm.
                    angle = math.abs(Operations.Angle(velocity,
                        embodiedClassifier.direction));
//TODO check the tolerances here.
                    return Enumerable.Count(hand.Fingers, finger => finger.IsExtended) == 5 && speed > .5f &&
                           angle < 30f;

                #endregion

                case classifierType.openPalmSwipe:

                    #region

                    hand = BodyInputDataSystem.bodyInput.GetHand(embodiedClassifier.Chirality);
                    //want movement in plane of palm within tolerance.
                    velocity = hand.Palm.Velocity;
                    embodiedClassifier.direction = hand.Palm.Direction;
                    embodiedClassifier.origin = hand.Palm.Position;

                    //we want velocity to be nonzero.
                    speed = math.distance(float3.zero, hand.Palm.Velocity);
                    //we want to have close to zero angle between movement and palm.
                    angle = 90 - math.abs(Operations.Angle(velocity,
                                embodiedClassifier.direction));
//TODO check the tolerances here.
                    //if the palm is open and moving, then note the plane it is moving through and activate the
                    //gesture for swiping
                    return Enumerable.Count(hand.Fingers, finger => finger.IsExtended) == 5 && speed > .5f &&
                           angle < 30f;

                #endregion

                case classifierType.thumbsUp:

                    #region Thumbs Up Activate

                    //is the left or right hand being used?
                    hand = BodyInputDataSystem.bodyInput.GetHand(embodiedClassifier.Chirality);
                    //where is the thumb pointing?
                    embodiedClassifier.direction = hand.Fingers[0].Direction;
                    embodiedClassifier.origin = hand.Fingers[0].Joints[3].Position;
                    //if only the thumb is extended and all other fingers are retracted, thumbs up
                    return hand.Fingers[0].IsExtended &&
                           !hand.Fingers[1].IsExtended &&
                           !hand.Fingers[2].IsExtended &&
                           !hand.Fingers[3].IsExtended &&
                           !hand.Fingers[4].IsExtended;

                #endregion

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool shouldCancel(ref EmbodiedClassifier embodiedClassifier)
        {
            //currently the check to see if a gesture should stop is the opposite of if it should start
            //this is likely not always going to cut it. There will be more reasons for stopping a gesture
            //than the conditions for activating it no longer being met.
            //BodyInput data = BodyInputDataSystem.bodyInput;

            return !shouldActivate(ref embodiedClassifier);
        }

        private static void Classify(ref EmbodiedClassifier embodiedClassifier)
        {
            //Gestures are not always going to stop at the conclusion of their function.
            //Therefore this is needed to determine if a gesture is ongoing or cancelled during operation
            //by the user.

            //if the gesture is not eligible to activate last frame
            if (!embodiedClassifier.isEligible)
            {
                //check to see if it should activate and update its eligibility to activate
                embodiedClassifier.isEligible = shouldActivate(ref embodiedClassifier);
                //If this is eligible, and it wasn't eligible before, it is activated
                embodiedClassifier.wasActivated = embodiedClassifier.isEligible;
                //therefore it was not cancelled
                embodiedClassifier.wasCancelled = false;
                //and it was not finished (since it just started)
                embodiedClassifier.wasFinished = false;
            }
            else
            {
                //if the gesture was eligible last frame

                //If the gesture should cancel, then cancel it.
                embodiedClassifier.wasCancelled = shouldCancel(ref embodiedClassifier);
                //The gesture wasn't finished if it is ongoing
                embodiedClassifier.wasFinished =
                    embodiedClassifier.shouldFinish && !embodiedClassifier.wasCancelled;
                //The gesture is not eligible to start if it is ongoing
                embodiedClassifier.isEligible =
                    !(embodiedClassifier.wasCancelled || embodiedClassifier.wasFinished);
                //since it was not activated this frame, set wasActivated to false
                embodiedClassifier.wasActivated = false;
                //the gesture should not finish next frame because it is either still ongoing or not continuing 
                //after this frame.
                embodiedClassifier.shouldFinish = false;
            }
        }
    }
}