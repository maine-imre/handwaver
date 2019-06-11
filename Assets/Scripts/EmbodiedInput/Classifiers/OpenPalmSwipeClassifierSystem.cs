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
    public class OpenPalmSwipeClassifierSystem : JobComponentSystem
    {
        /// <summary>
        /// A generic example of a classifier.  This should be renamed in each use case.
        /// </summary>
        public struct OpenPalmSwipeClassifier : IEmbodiedClassifier<BodyInput>
        {
            public Chirality chirality;
            private const float speedTol = .5f;
            private const float AngleTol = 30f;
            public Vector3 plane;
            public Vector3 origin;

            public bool shouldActivate(BodyInput data)
            {
                Hand hand = data.GetHand(chirality);
                //want movement in plane of palm within tolerance.
                Vector3 move = hand.Palm.Velocity;
                plane = hand.Palm.Direction;
                origin = hand.Palm.Position;

                //we want velocity to be nonzero.
                float speed = move.magnitude;
                //we want to have close to zero angle between movement and palm.
                float angle = 90 - Mathf.Abs(Vector3.Angle(move, plane));

                return (hand.Fingers.Where(finger => finger.IsExtended).Count() == 5) && speed > speedTol && angle < AngleTol;
                
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
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, OpenPalmSwipeClassifier>
        {

            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref OpenPalmSwipeClassifier classifier)
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
