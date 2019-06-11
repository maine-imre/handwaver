using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

namespace IMRE.EmbodiedUserInput
{
    public class OpenPalmClassifierSystem : JobComponentSystem
    {
        /// <summary>
        /// A generic example of a classifier.  This should be renamed in each use case.
        /// </summary>
        public struct OpenPalmClassifier : IEmbodiedClassifier<BodyInput>
        {
            public Chirality chirality;
            public Vector3 plane;
            public Vector3 origin;

            public bool shouldActivate(BodyInput data)
            {
                Hand hand = data.GetHand(chirality);
                plane = hand.Palm.Direction;
                origin = hand.Palm.Position;
                return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5);
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
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, OpenPalmClassifier>
        {

            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref OpenPalmClassifier classifier)
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