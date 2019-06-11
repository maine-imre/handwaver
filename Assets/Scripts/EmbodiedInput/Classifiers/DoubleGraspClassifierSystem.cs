using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public class DoubleGraspClassifierSystem : JobComponentSystem
    {
        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new EmbodiedUserInputClassifierJob();

            return job.Schedule(this, inputDependencies);
        }

        /// <summary>
        ///     A generic example of a classifier.  This should be renamed in each use case.
        /// </summary>
        public struct DoubleGraspClassifier : IEmbodiedClassifier<BodyInput>
        {
            public float distBetweenHands;

            public bool shouldActivate(BodyInput data)
            {
                distBetweenHands = Vector3.Distance(data.LeftHand.Fingers[1].Joints[4].Position,
                    data.RightHand.Fingers[1].Joints[4].Position);
                return data.LeftHand.IsPinching && data.RightHand.IsPinching;
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
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, DoubleGraspClassifier>
        {
            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref DoubleGraspClassifier classifier)
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