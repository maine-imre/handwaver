/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class IMREEditorLayerLoad : MonoBehaviour
    {

        public String baseScene  = "LeapVive_GeoBase";

        private void Start()
        {

            if (!SceneContains())
            {
#if UNITY_EDITOR
                String cScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadSceneAsync(baseScene, LoadSceneMode.Additive);
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(baseScene));
                Debug.LogWarning("Hey Stupid, you loaded the base scene layer after the additive scene layer. Do not expect things to work well; just saying.");
                //reloadcScenes();
#endif
            }
        }

        //private void reloadcScenes()
        //{
        //    String cScene = SceneManager.GetActiveScene().name;
        //    List<String> activeScenes = new List<String>();
        //    for (int i = 0; i < SceneManager.sceneCount; i++) { 

        //        if (SceneManager.GetSceneAt(i).name != baseScene)
        //            activeScenes.Add(SceneManager.GetSceneAt(i).name);
        //    }
        //    foreach (String sceneName in activeScenes)
        //    {
        //        SceneManager.UnloadSceneAsync(sceneName);
        //    }
        //    foreach (String sceneName in activeScenes)
        //    {
        //        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //    }
        //}

        private bool SceneContains()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == baseScene)
                    return true;
            }
            return false;
        }
    }
}
