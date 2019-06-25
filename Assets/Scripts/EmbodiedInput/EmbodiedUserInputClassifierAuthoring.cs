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
        
        public static EntityManager manager;
        public static NativeArray<Entity> entities;
        
        
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
            //each classifier represents a gesture which can be performed.
            
            int amount = 13;
            entities = new NativeArray<Entity>(amount, Allocator.Temp);
            manager.Instantiate(classifierPrefab, entities);
            manager.SetComponentData(entities[0], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //the point gesture is a one handed gesture. Therefore there must be a classifier for each hand's
                //point gesture.
                
                //point gesture's classifier for left hand
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.point
            });
            manager.SetComponentData(entities[1], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //point gesture's classifier for right hand
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.point
            });
            manager.SetComponentData(entities[2], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //grasp gesture's classifier for left hand
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.grasp
            });
            manager.SetComponentData(entities[3], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //grasp gesture's classifier for right hand
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.grasp
            });
            manager.SetComponentData(entities[4], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //Open Palm gesture's classifier for left hand
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.openPalm
            });
            manager.SetComponentData(entities[5], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //open palm gesture's classifier for right hand
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.openPalm
            });
            manager.SetComponentData(entities[6], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //open palm push gesture's classifier for left hand
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.openPalmPush
            });
            manager.SetComponentData(entities[7], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //open palm push gesture's classifier for right hand
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.openPalmPush
            });
            manager.SetComponentData(entities[8], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //open palm swipe gesture's classifier for left hand
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.openPalmSwipe
            });
            manager.SetComponentData(entities[9], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //open palm swipe gesture's classifier for right hand
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.openPalmSwipe
            });
            manager.SetComponentData(entities[10], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //thumbs up gesture's classifier for left hand
                chirality = Chirality.Left,
                type = EmbodiedUserInputClassifier.classifierType.thumbsUp
            });
            manager.SetComponentData(entities[11], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //thumbs up gesture's classifier for right hand
                chirality = Chirality.Right,
                type = EmbodiedUserInputClassifier.classifierType.thumbsUp
            });
            manager.SetComponentData(entities[11], new EmbodiedUserInputClassifier.EmbodiedClassifier
            {
                //double grasp is a two handed gesture. This means it will only need a single classifier due
                //to it using both hands and therefore not needing separate instances for which hand is being used.
                type = EmbodiedUserInputClassifier.classifierType.doubleGrasp
            });
            

        }
    }
}