using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public class PointClassifierSystem : JobComponentSystem
    {
        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new EmbodiedUserInputClassifierJob();

            return job.Schedule(this, inputDependencies);
        }

        public struct PointClassifier : IEmbodiedClassifier<BodyInput>
        {
            public Chirality chirality;

            public Vector3 pointingDirection;
            public Vector3 pointingOrigin;

            public bool shouldActivate(BodyInput data)
            {
                //Fingers are indexed from thumb to pinky. The thumb is 0, index is 1
                //middle 2, ring 3, pinky 4. Below, we return true if the pinky,
                //ring finger, and middle finger are not extended while the index
                //finger is extended. There is also an additional portion to this 
                //where the hand should not be pointing if there is a pinch happening.
                //This prevents false pointing while trying to pinch something

                //The thumb is ignored in whether this gesture will activate or not

                //note that we need a new concept for grasping object.
                var hand = data.GetHand(chirality);
                pointingDirection = hand.Fingers[1].Direction;
                pointingOrigin = hand.Fingers[1].Joints[3].Position;
                return hand.Fingers[1].IsExtended &&
                       !hand.Fingers[2].IsExtended &&
                       !hand.Fingers[3].IsExtended &&
                       !hand.Fingers[4].IsExtended;
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
        ///     A thin layer of general abstraction for one-handed and two-handed gestures.
        ///     Inspired by LeapPaint https://github.com/leapmotion/Paint
        /// </summary>
        [BurstCompile]
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, PointClassifier>
        {
            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref PointClassifier classifier)
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
    }
}