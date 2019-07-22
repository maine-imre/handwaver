namespace IMRE.HandWaver.VariedPerspective
{
    public class perspectiveManager : UnityEngine.MonoBehaviour
    {
        /// <summary>
        ///     Enumerator that describes the three modes to be used for this scene.
        /// </summary>
        public enum perspectiveMode
        {
            None,
            OnlyMain,
            OnlyOrtho,
            Both
        }

        /// <summary>
        ///     The VR View Camera.
        /// </summary>
        private static UnityEngine.Camera _mainCamera;

        /// <summary>
        ///     This is just a public version of the perspective mode for debug view within the editor.
        ///     Editing this simply changes the default state on load
        /// </summary>
        public perspectiveMode _currentMode;

        /// <summary>
        ///     The list of all main scene cameras. This will include potentially mixcast cameras.
        /// </summary>
        public System.Collections.Generic.List<UnityEngine.Camera> mainCameras;

        /// <summary>
        ///     The orthogonal camera used to get a flat perspective on the shapes within the scene.
        /// </summary>
        public UnityEngine.Camera orthoCamera;

        private int shapeLayer = -1;

        public perspectiveMode currentMode
        {
            get => _currentMode;
            set
            {
                _currentMode = value;
                switch (value)
                {
                    case perspectiveMode.None:
                        // Hide the shape layer from all cameras

                        mainCameras.ForEach(mc => mc.cullingMask &= ~shapeLayer);
                        orthoCamera.cullingMask &= ~shapeLayer;
                        break;
                    case perspectiveMode.OnlyMain:
                        // Hide the shape layer from the ortho camera

                        mainCameras.ForEach(mc => mc.cullingMask &= ~shapeLayer);
                        orthoCamera.cullingMask |= shapeLayer;

                        break;
                    case perspectiveMode.OnlyOrtho:
                        // Hide the shape layer from the main cameras

                        mainCameras.ForEach(mc => mc.cullingMask |= shapeLayer);
                        orthoCamera.cullingMask &= ~shapeLayer;
                        break;
                    case perspectiveMode.Both:
                        //Show shapes to all cameras

                        mainCameras.ForEach(mc => mc.cullingMask |= shapeLayer);
                        orthoCamera.cullingMask |= shapeLayer;

                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Set the perspective mode to none.
        ///     This means that no camera sees the shape layer.
        /// </summary>
        private void SetNone()
        {
            currentMode = perspectiveMode.None;
        }

        /// <summary>
        ///     Sets all cameras able to view shapes. This is for debugging as of right now.
        /// </summary>
        private void SetBoth()
        {
            currentMode = perspectiveMode.Both;
        }

        /// <summary>
        ///     Set the perspective mode to only show shapes on main cameras.
        ///     This disallows the view of projection and allows the view of the source objects.
        /// </summary>
        private void SetOnlyOrtho()
        {
            currentMode = perspectiveMode.OnlyMain;
        }

        /// <summary>
        ///     Set the perspective mode to only show shapes on orthogonal camera.
        ///     This will be effectively only showing the projection and not the source object.
        /// </summary>
        private void SetOnlyMain()
        {
            currentMode = perspectiveMode.OnlyOrtho;
        }

        private void Awake()
        {
            // find reference to main camera
            _mainCamera = UnityEngine.Camera.main;
            // Add the VR view into the main cameras list
            mainCameras.Add(_mainCamera);
            // Find the Shape Layer
            shapeLayer = 1 << UnityEngine.LayerMask.NameToLayer("ShapeLayer");

            if (shapeLayer == -1) //If shape layer is not found
                UnityEngine.Debug.LogError("Shape Layer not found!");

            // This initializes the current mode to match what the user wanted to set it to through unity editor.
            currentMode = _currentMode;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F9))
                SetBoth();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F10))
                SetNone();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F11))
                SetOnlyMain();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F12))
                SetOnlyOrtho();
        }
    }
}