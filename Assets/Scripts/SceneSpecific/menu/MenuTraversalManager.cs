using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IMRE.HandWaver.Menu
{

    public class MenuTraversalManager : MonoBehaviour
    {
        public CanvasRenderer mainPanel;
        public CanvasRenderer optionsPanel;
        public CanvasRenderer closeDialog;
        public Button optionsButton;
        public Button quitButton;

        private bool isSettingsOpen => optionsPanel.gameObject.activeSelf;
        private bool isCloseDialogOpen => closeDialog.gameObject.activeSelf;

        private void Start()
        {
            optionsButton.onClick.AddListener(toggleSettings);
            quitButton.onClick.AddListener(toggleCloseDialog);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isSettingsOpen)
                {
                    toggleSettings();
                }
                else
                {
                    toggleCloseDialog();
                }
            }
            
        }

        private void toggleCloseDialog()
        {
            if (isCloseDialogOpen)
            {
                optionsButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
                closeDialog.gameObject.SetActive(false);
                mainPanel.gameObject.SetActive(true);
            }
            else
            {
                optionsButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);
                closeDialog.gameObject.SetActive(true);
                mainPanel.gameObject.SetActive(false);

            }
        }
        
        private void toggleSettings()
        {
            if (isSettingsOpen)
            {
                optionsButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
                optionsPanel.gameObject.SetActive(false);
                mainPanel.gameObject.SetActive(true);

            }
            else
            {
                optionsButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);
                optionsPanel.gameObject.SetActive(true);
                mainPanel.gameObject.SetActive(false);

            }
        }

    }
}