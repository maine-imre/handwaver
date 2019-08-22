namespace IMRE.HandWaver.UserInterface
{
    public class OnScreenMenu : UnityEngine.MonoBehaviour
    {
        private int _adjustedHeight;
        public UnityEngine.GameObject menuPanel;
        public TMPro.TextMeshProUGUI unitText;

        /// <summary>
        ///     Used to set the height variable of the player.
        ///     Zero is default height.
        ///     This variable is stored in player preferences.
        /// </summary>
        private int AdjustedHeight
        {
            set
            {
                // Set Text to new value
                unitText.text = value.ToString();
                //Set stored player preference of height
                UnityEngine.PlayerPrefs.SetInt("height", value);
                //Move Rig accordingly
                playerRig.position += UnityEngine.Vector3.up * 0.01f * value;
                //Store int
                _adjustedHeight = value;
            }
            get => _adjustedHeight;
        }

        /// <summary>
        ///     Finds the player rig by reference of the parent of the main camera.
        /// </summary>
        private UnityEngine.Transform playerRig => UnityEngine.Camera.main.transform.parent;

        /// <summary>
        ///     Called by the reset button to reload the current active scene.
        /// </summary>
        public void ResetButton()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                .buildIndex);
        }

        /// <summary>
        ///     Called by the quit button to close the application.
        /// </summary>
        public void QuitButton()
        {
            UnityEngine.Application.Quit();
        }

        /// <summary>
        ///     Raise the height of the player by 1 unit.
        /// </summary>
        public void RaiseHeightButton()
        {
            AdjustedHeight++;
        }

        /// <summary>
        ///     Lower the height of the player by 1 unit.
        /// </summary>
        public void LowerHeightButton()
        {
            AdjustedHeight--;
        }

        private void Awake()
        {
            // Load the players height from file. This is a per computer per build basis.
            AdjustedHeight = UnityEngine.PlayerPrefs.GetInt("height");
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape)) menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }
}