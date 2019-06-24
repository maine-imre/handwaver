using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace IMRE.EmbodiedUserInput
{
    public class EmbodiedUserInputClassifierAuthoring : MonoBehaviour
    {
        public Entity classifierPrefab;
        
        internal EntityManager manager;
        internal NativeArray<Entity> entities;
        
        
        void Start()
        {
            manager = World.Active.GetOrCreateSystem<EntityManager>();
            addClassifierBatch();
        }

        /// <summary>
        /// Adds one of each type of classifier to the job list
        /// </summary>
        void addClassifierBatch()
        {
            int amount = 13;
            entities = new NativeArray<Entity>(amount, Allocator.Temp);
            manager.Instantiate(classifierPrefab, entities);
            manager.SetComponentData(entities[0], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.point
            });
            manager.SetComponentData(entities[1], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.point
            });
            manager.SetComponentData(entities[2], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.grasp
            });
            manager.SetComponentData(entities[3], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.grasp
            });
            manager.SetComponentData(entities[4], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.openPalm
            });
            manager.SetComponentData(entities[5], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.openPalm
            });
            manager.SetComponentData(entities[6], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.openPalmPush
            });
            manager.SetComponentData(entities[7], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.openPalmPush
            });
            manager.SetComponentData(entities[8], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.openPalmSwipe
            });
            manager.SetComponentData(entities[9], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.openPalmSwipe
            });
            manager.SetComponentData(entities[10], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.thumbsUp
            });
            manager.SetComponentData(entities[11], new EmbodiedUserInputClassifier.ClassifierJob
            {
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.thumbsUp
            });
            manager.SetComponentData(entities[11], new EmbodiedUserInputClassifier.ClassifierJob
            {
                type = EmbodiedUserInputClassifier.classifierType.doubleGrasp
            });
            

        }
    }
}