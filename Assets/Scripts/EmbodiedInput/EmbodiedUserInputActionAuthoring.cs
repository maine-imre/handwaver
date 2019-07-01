using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public class EmbodiedUserInputActionAuthoring : MonoBehaviour
    {
        public Entity actionPrefab;
        
        internal EntityManager manager;
        internal static NativeArray<Entity> entities;


        private void Start()
        {
            manager = World.Active.GetOrCreateSystem<EntityManager>();
            addClassifierBatch();
        }

        /// <summary>
        /// Adds one of each type of classifier to the job list
        /// </summary>
        void addClassifierBatch()
        {
            int amount = EmbodiedUserInputClassifierAuthoring.entities.Length;
            entities = new NativeArray<Entity>(amount, Allocator.Temp);
            manager.Instantiate(actionPrefab, entities);
            
            //SETUP DEBUG ENTITIES;
            for (int i = 0; i < amount; i++)
            {
                manager.SetComponentData(entities[i], new EmbodiedUserInputAction.ActionData
                {
                    priority = 1000,
                    actionType = EmbodiedUserInputAction.ActionType.debug
                    //todo associate classifier
                });
                manager.SetComponentData(entities[0], EmbodiedUserInputClassifierAuthoring.manager.GetComponentData<EmbodiedUserInputClassifier.EmbodiedClassifier>(EmbodiedUserInputClassifierAuthoring.entities[i]));
            }
        }

        private void LateUpdate()
        {
            //READ DEBUG ENTITIES
            int amount = EmbodiedUserInputClassifierAuthoring.entities.Length;
            for (int i = 0; i < amount; i++)
            {
                Debug.Log("I am not really sure what to put through this");
                //manager.GetComponentData<EmbodiedUserInputClassifier.EmbodiedClassifier>(
                //EmbodiedUserInputClassifierAuthoring.entities[i]).type + " " + 
                //manager.GetComponentData<EmbodiedUserInputAction.ActionData>(entities[i]).command);
            }
            
        }
    }
}