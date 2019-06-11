using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public class OpenPalmPushClassifierSystem : JobComponentSystem
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
        public struct OpenPalmPushClassifier : IEmbodiedClassifier<BodyInput>
        {
            public Chirality chirality;
            private const float speedTol = .5f;
            private const float AngleTol = 30f;
            public Vector3 plane;
            public Vector3 origin;

            public bool shouldActivate(BodyInput data)
            {
                var hand = data.GetHand(chirality);
                //want movement in plane of palm within tolerance.
                var move = hand.Palm.Velocity;
                plane = hand.Palm.Direction;
                origin = hand.Palm.Position;

                //we want velocity to be nonzero.
                var speed = move.magnitude;
                //we want to have close to zero angle between movement and palm.
                var angle = Mathf.Abs(Vector3.Angle(move, plane));

                return hand.Fingers.Where(finger => finger.IsExtended).Count() == 5 && speed > speedTol &&
                       angle < AngleTol;
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
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, OpenPalmPushClassifier>
        {
            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref OpenPalmPushClassifier classifier)
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