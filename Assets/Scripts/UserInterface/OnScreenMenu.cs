using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.HandWaver.HWIO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IMRE.HandWaver.UserInterface
{
    public class OnScreenMenu : MonoBehaviour
    {
        public TextMeshProUGUI unitText;
        public GameObject menuPanel;
        
        private int _adjustedHeight;
        /// <summary>
        /// Used to set the height variable of the player.
        /// Zero is default height.
        /// This variable is stored in player preferences.
        /// </summary>
        private int AdjustedHeight
        {
            set
            {
                // Set Text to new value
                unitText.text = value.ToString();
                //Set stored player preference of height
                PlayerPrefs.SetInt("height", value);
                //Move Rig accordingly
                playerRig.position += Vector3.up * 0.01f * value;
                //Store int
                _adjustedHeight = value;
            }
            get => _adjustedHeight;
        }
        
        /// <summary>
        /// Finds the player rig by reference of the parent of the main camera.
        /// </summary>
        private Transform playerRig => Camera.main.transform.parent;

        /// <summary>
        /// Called by the reset button to reload the current active scene.
        /// </summary>
        public void ResetButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Called by the quit button to close the application.
        /// </summary>
        public void QuitButton() => Application.Quit();

        /// <summary>
        /// Raise the height of the player by 1 unit.
        /// </summary>
        public void RaiseHeightButton() => AdjustedHeight++;

        /// <summary>
        /// Lower the height of the player by 1 unit.
        /// </summary>
        public void LowerHeightButton() => AdjustedHeight--;

        public void SaveButton()
        {
            string path = Application.dataPath + @"/../Saves/"+Application.productName + Application.version + "_" + DateTime.Now.ToString()+".hw";
            XMLManager.ins.SaveGeoObjs(path);
        }

        public void LoadButton()
        {
            
        }

        private void Awake()
        {
            // Load the players height from file. This is a per computer per build basis.
            AdjustedHeight =  PlayerPrefs.GetInt("height");
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuPanel.SetActive(!menuPanel.activeSelf);
            }
        }
    }  

}

