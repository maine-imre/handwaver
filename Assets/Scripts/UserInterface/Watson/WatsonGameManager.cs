///**
//HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
//(C) University of Maine
//See license info in readme.md.
//www.imrelab.org
//**/

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PathologicalGames;
//using IBM.Watson.DeveloperCloud.Widgets;
//using IBM.Watson.DeveloperCloud.DataTypes;
//using UnityEngine.SceneManagement;


//namespace IMRE.HandWaver
//{

//    public class WatsonGameManager : MonoBehaviour
//    {
//        [Range(0,1)]
//        public float confLevel = 0.8f;
//        public bool activeListen = false;

//        public AudioClip sorrySound;
//        public AudioClip helloSound;
//        public AudioClip goodbyeSound;

//        public leapControls lHand;
//        public leapControls rHand;

//        public Transform spawnPos;

//        public automatedPyramidControl pyramidController;

//        public void handleWatsonCommand(ClassifyResultData data)
//        {
//            float conf = (float)data.Result.topConfidence;
//            string topClass = data.Result.top_class;

//            if (topClass == "hello")
//            {
//                activeListen = true;
//                PlayClip(helloSound);
//            }
//            else if (topClass == "goodbye")
//            {
//                activeListen = false;
//                PlayClip(goodbyeSound);
//            }

//            else if(data.Result.topConfidence > confLevel)
//            {
//                switch (topClass)
//                {
//                        //Switch scene with voice command "Travel to (scene name)"
//                    case "classicconstructions":
//                        loadScene("ClassicConstructions");
//                        break;
//                    case "conics":
//                        loadScene("ConicsLayer");
//                        break;
//                    case "flatland":
//                        loadScene("FlatlandLayer");
//                        break;
//                    case "globe":
//                        loadScene("GlobeLayer");
//                        break;
//                    case "higherdimensions":
//                        loadScene("HigherDimensionsLayer");
//                        break;
//                    case "menu":
//                        loadScene("MenuLayer");
//                        break;
//                    case "multdiv":
//                        loadScene("MultDivLayer");
//                        break;
//                    case "platonics":
//                        loadScene("Platonics");
//                        break;
//                    case "pyramidsvolume":
//                        loadScene("PyramidLayer");
//                        break;
//                    case "sandbox":
//                        loadScene("Sandbox");
//                        break;
//                    case "spherecylinder":
//                        loadScene("SphereCylinderLayer");
//                        break;
                    
//                        //Create tool or object with voice command "Spawn (tool/object)"
//                    case "arctus":
//                        spawnFromPool("Tools", "Arctus");
//                        break;
//                    case "eraser":
//                        spawnFromPrefab("Tools/eraserPrefab.prefab");
//                        Debug.Log ("Eraser spawned!");
//                        break;
//                    case "flashlight":
//                        spawnFromPrefab("Tools/flashlightPrefab.prefab");
//                        break;
//                    case "paintbucket":
//                        spawnFromPrefab("Tools/paintbucketPrefab.prefab");
//                        break;
//                    case "straightedge":
//                        spawnFromPool("Tools", "Straightedge");
//                        break;
//                    case "spindle":
//                        spawnFromPrefab("prefabs/shipsWheelOffSpindle.prefab");
//                        break;
//                    case "point":
//                        spawnFromPool("GeoObj", "PointPreFab");
//                        break;
//                    case "simplepoint":
//                        spawnFromPool("GeoObj", "SimplePointPrefab");
//                        break;
//                    case "cube":
//                        spawnFromPrefab("Tools/eraserPrefab");
//                        break;
//                    case "sphere":
//                        //spawnFromPrefab("Tools", "SpherePrefab");
//                        break;
//                    case "flatface":
//                        spawnFromPool("Tools", "Flatface");
//                        break;
                    
                   
//                        //Set mode with voice command "Activate (mode name)"
//                    case "delete":
//                        SetMode("delete");
//                        break;
//                    case "draw":
//                        SetMode("draw");
//                        break;
//                    case "revolve":
//                        SetMode("revolve");
//                        break;
//                    case "slice":
//                        SetMode("slice");
//                        break;
//                    case "stretch":
//                        SetMode("stretch");
//                        break;
//                    case "explodePyramid":
//                        pyramidControl(topClass);
//                        break;
//                    default:
//                        PlayClip(sorrySound);
//                        break;
//                }
//            }
//        }

//        public void spawnFromPool(string poolName,string preFabName)
//        {
//            Transform newObj = PoolManager.Pools[poolName].Spawn(preFabName);
//            newObj.transform.position = spawnPos.transform.position;
//        }

//        public void spawnFromPrefab(string path)
//        {
//            Vector3 position = spawnPos.transform.position;
//            Quaternion rotation = spawnPos.transform.rotation;

//            GameObject.Instantiate(Resources.Load(path),position,rotation);
//        }

//        public void pyramidControl(string result)
//        {
//            if (SceneManager.GetActiveScene().name.Contains("Pyramids"))
//            {
//                if(pyramidController == null)
//                {
//                    pyramidController = GameObject.FindObjectOfType<automatedPyramidControl>();
//                }
//                pyramidController.handleInput(result);
//            }
//        }

//        public void PlayClip(AudioClip sound)
//        {
//            Debug.Log("NeedToPlaySound" + sound.ToString());
//        }

//        public void loadScene(string sceneName)
//        {
//            SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
//        }

//        public void SetMode(string value)
//        {
//            //lHand.setMode(value);
//            //rHand.setMode(value);
//        }
//    }
//}