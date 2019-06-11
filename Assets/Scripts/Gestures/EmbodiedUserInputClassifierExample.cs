using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

namespace IMRE.EmbodiedUserInput
{
    public interface IEmbodiedClassifier<BodyInput> : IComponentData
    {
        bool shouldActivate(BodyInput data);
        bool shouldFinish(BodyInput data);
        bool shouldCancel(BodyInput data);

        bool isEligible { get; set; }
        bool wasCancelled { get; set; }
        bool wasFinished { get; set; }
        bool wasActivated { get; set; }
    }
    
    public class EmbodiedUserInputClassifierExample : JobComponentSystem
    {
        public struct genericClassifier : IEmbodiedClassifier<BodyInput>
        {
            public bool shouldActivate(BodyInput data)
            {
                throw new NotImplementedException();
            }

            public bool shouldFinish(BodyInput data)
            {
                throw new NotImplementedException();
            }

            public bool shouldCancel(BodyInput data)
            {
                throw new NotImplementedException();
            }

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
                    classifier.wasFinished = classifier.shouldFinish(cBodyInput) && !classifier.wasCancelled;
                    classifier.isEligible = !(classifier.wasCancelled || classifier.wasFinished);
                    classifier.wasActivated = false;
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