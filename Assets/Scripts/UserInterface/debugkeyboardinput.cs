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
using System.Linq;



namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class debugkeyboardinput : MonoBehaviour//sorry
    {
        private bool unloadBool;
		public bool interalBuild = true;

        public bool loadBackground = true;
        public string backgroundName = "darkPrototype";
		public bool autoLoadPlaintains;
		public Transform Plaintains;

		/// <summary>
		/// Set this in the editor to load a set of scenes on start.
		/// this will disable keyboard input.
		/// </summary>
		public List<string> loadScenesOnStart = new List<string>();

		void Start()
		{

			commandLineArgumentParse.logOverride |= interalBuild;   //or equal the internal build bool so that if its internal it automatically starts logging

			if (loadBackground)
				loadSceneAsyncByName(backgroundName, false);
#if !UNITY_EDITOR
			if (interalBuild)
			{
				Display.displays[0].Activate();
				Display.displays[1].Activate();
				FindObjectOfType<HWMixcastIO>().currMode = mixCastTargetMode.primaryAlt;
			}
#endif
			foreach (string name in loadScenesOnStart)
			{
				loadSceneAsyncByName(name, unloadBool);
			}
			if (autoLoadPlaintains)
			{
				StartCoroutine(enablePlaintains());
			}
		}

        void Update()
        {
			//locks out of other scenes.
			if (loadScenesOnStart.Count > 0)
			{
				unloadBool = Input.GetKey(KeyCode.U);//if you are pressing U it will unload other active scenes excluding base layer

				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					if (Input.GetKeyDown(KeyCode.U))
					{
						removeAllLayers();
					}

					if (Input.GetKeyDown(KeyCode.P))
					{
						loadNewBaseScene("GeometersPlanetariumBase");
					}

					if (Input.GetKeyDown(KeyCode.H))
					{
						loadNewBaseScene("HandWaverBase");
					}

					if (Input.GetKeyDown(KeyCode.L))
					{
						commandLineArgumentParse.logOverride = true;
					}
				}
				else
				{
					if (Input.GetKeyDown(KeyCode.R))
					{
						resetCurrentScenes();
					}

					if (Input.GetKeyDown(KeyCode.B))
					{
						loadNewBaseScene("LittleBeartha");
					}

					if (Input.GetKeyDown(KeyCode.C))
					{
						loadSceneAsyncByName("Chess3DLayer", true);
					}

					//if (Input.GetKeyDown(KeyCode.D))
					//{
					//	loadSceneAsyncByName("HigherDimensionsLayer", unloadBool);
					//}

					if (Input.GetKeyDown(KeyCode.H))
					{
						loadNewBaseScene("HorizonAnalysis");
					}

					if (Input.GetKeyDown(KeyCode.L))
					{
						loadSceneAsyncByName("LatticeLand", true);
					}

					if (Input.GetKeyDown(KeyCode.P))
					{
						Plaintains.gameObject.SetActive(true);
					}

					if (Input.GetKeyDown(KeyCode.T))
					{
						loadSceneAsyncByName("tutorialLayer", unloadBool);
					}

					if (Input.GetKeyDown(KeyCode.V))
					{
						toggleMixCastCamera();
					}

					if (Input.GetKeyDown(KeyCode.F11))
					{
						Interface.worldScaleModifier.advanceFigureType();
					}
					if (Input.GetKeyDown(KeyCode.F10))
					{
						if(Space.RSDESManager.ins != null)
							Space.RSDESManager.ins.toggleNightSky();
					}
					//if (Input.GetKeyDown(KeyCode.S) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
					//{
					//	loadSceneAsyncByName("ShearingLab", true);
					//}
				}
			}
        }

		private void loadSceneByName(string scene)
		{
			SceneManager.LoadScene(scene);
		}

		private void demoToggle()
		{
			playMode.demo = !playMode.demo;
            resetCurrentScenes();
		}


        public static void loadNewBaseScene(string SceneName)
        {
            removeAllLayers();
            loadSceneAsyncByName(SceneName, false);
        }

        public static void loadSceneAsyncByName(string SceneName)
        {
            loadSceneAsyncByName(SceneName, false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SceneName">Name of Scene to Load</param>
        /// <param name="unloadOther">Whether or not to unload other current Scenes</param>
        public static void loadSceneAsyncByName(string SceneName, bool unloadOther)
        {
            //Debug.Log(SceneName +" attempted to load.");
            if (unloadOther)
            {
                List<Scene> layers = new List<Scene>();
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {

					if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
					{
						if (SceneManager.GetSceneAt(i).name.Contains("LatticeLand"))
						{
							FindObjectsOfType<HWSelectGesture>().ToList().ForEach(sg => sg.enabled = false);
						}
						layers.Add(SceneManager.GetSceneAt(i));

					}
				}
                foreach (Scene s in layers)
                {
                    SceneManager.UnloadSceneAsync(s);
                }
            }
			SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

		}

		public static void removeAllLayers()
		{

			List<Scene> layers = new List<Scene>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{

				if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
					layers.Add(SceneManager.GetSceneAt(i));
			}
			foreach (Scene s in layers)
			{
				SceneManager.UnloadSceneAsync(s);
			}
		}


        /// <summary>
        /// Reloads base scene then all scenes that do not contain the keyword base.
        /// </summary>
        public static void resetCurrentScenes() {
            List<Scene> layers = new List<Scene>();
            Scene baseLayer = new Scene();
#region Get Lists of Current Scenes
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
                    layers.Add(SceneManager.GetSceneAt(i));
                else
                    baseLayer = SceneManager.GetSceneAt(i);
            }
#endregion
#region Reload Base & Layers
			
            SceneManager.LoadScene(baseLayer.name, LoadSceneMode.Single);
			foreach (Scene s in layers)
            {
                SceneManager.LoadSceneAsync(s.name, LoadSceneMode.Additive);
            }
#endregion

        }

        public static void reloadCurrentScenes()
        {
            List<Scene> layers = new List<Scene>();
            Scene baseLayer = new Scene();
#region Get Lists of Current Scenes
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
                    layers.Add(SceneManager.GetSceneAt(i));
                else
                    baseLayer = SceneManager.GetSceneAt(i);
            }
#endregion
#region Reload Base & Layers
            SceneManager.LoadScene(baseLayer.name, LoadSceneMode.Single);
            if (layers.Count == 0)
                return;
            foreach (Scene s in layers)
            {
                SceneManager.LoadSceneAsync(s.name, LoadSceneMode.Additive);
            }
#endregion

        }

        private void toggleMixCastCamera()
        {
            mixCastCameraToggle.toggleCameraz();
        }

		IEnumerator enablePlaintains()
		{
			yield return new WaitForSeconds(2f);
			Plaintains.gameObject.SetActive(true);
		}
	}
}
