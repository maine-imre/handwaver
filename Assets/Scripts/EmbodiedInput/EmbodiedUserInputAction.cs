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
    /// This is an example class that uses the embodied input classifier in a Job Component System
    /// </summary>
    public class EmbodiedUserInputAction : JobComponentSystem
    {
        public enum ActionType
        {
            debug 
        };

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public struct ActionData : IComponentData
        {
            //Todo the data that is needed to do the job
            public int priority;
            public ActionType actionType;
            public bool isActive;
            public bool activatable;
            
            /// <summary>
            /// The GGB command string to send to the server
            /// </summary>
            //public char command;
        }

        /// <summary>
        /// A thin layer of general abstraction for one-handed and two-handed gestures.
        /// Inspired by LeapPaint https://github.com/leapmotion/Paint
        /// </summary>
        [BurstCompile]
        public struct EmbodiedUserInputClassifierJob : IJobForEach<ActionData, EmbodiedUserInputClassifier.EmbodiedClassifier>
        {
            
            public void Execute(ref ActionData actionData, ref EmbodiedUserInputClassifier.EmbodiedClassifier embodiedClassifier)
            {
                if (embodiedClassifier.isEligible)
                {
                    actionData.activatable = true;
                    if (embodiedClassifier.wasActivated)
                    {
                        //was turned on last cycle
                        switch (actionData.actionType)  
                        {
                            case ActionType.debug :
                                //actionData.command = 's';
                                break;
                        }
                    }
                    else
                    {
                        //continuing to run
                        switch (actionData.actionType)  
                        {
                            case ActionType.debug :
                                //actionData.command = 'o';
                                break;
                        }
                        
                    }
                }
                else
                {
                    actionData.isActive = false;
                    actionData.activatable = false;
                    if (embodiedClassifier.wasCancelled)
                    {
                        //Todo  provide feedback gesutre is 
                        switch (actionData.actionType)  
                        {
                            case ActionType.debug :
                                //actionData.command = 'c';
                                break;
                        }
                    }else if (embodiedClassifier.wasFinished)
                    {
                        //todo provide feedback gesture is finished
                        switch (actionData.actionType)  
                        {
                            case ActionType.debug :
                                //actionData.command = 'f';
                                break;
                        }
                    }
                    else
                    {
                        //todo remove feedback
                        switch (actionData.actionType)  
                        {
                            case ActionType.debug :
                                //actionData.command = 'f';
                                break;
                        }
                    }
                }
                
                //todo turn on and off feedback
            }
        }
        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new EmbodiedUserInputClassifierJob();
            
            return job.Schedule(this, inputDependencies);
        }

    }
}