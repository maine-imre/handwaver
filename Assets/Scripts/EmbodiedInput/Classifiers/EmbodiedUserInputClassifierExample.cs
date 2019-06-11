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
        /// <summary>
        /// A generic example of a classifier.  This should be renamed in each use case.
        /// </summary>
        public struct genericClassifier : IEmbodiedClassifier<BodyInput>
        {
            public bool shouldActivate(BodyInput data)
            {
                throw new NotImplementedException();
            }

            public bool shouldCancel(BodyInput data)
            {
                throw new NotImplementedException();
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