using Enumerable = System.Linq.Enumerable;

#if Photon
using Photon.Pun;
#endif

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    ///     Central control for scale and dimension study.
    ///     also includes logic to make a slider out of MasterGeoObjs
    /// </summary>
    public class SpencerStudyControl : UnityEngine.MonoBehaviour
    {
        public static SpencerStudyControl ins;

        /// <summary>
        ///     The number of degrees that each vertex is folded by.
        ///     Consider changing to percent;
        /// </summary>
        internal static float percentFolded;

        /// <summary>
        ///     An override that automatically animates the slider and the folding process
        /// </summary>
        public bool animateFold;

        public bool animateUp = true;

        /// <summary>
        ///     A boolean for debugging that allows the fold to be manipulated in the editor at play
        /// </summary>
        public bool foldOverride;

        /// <summary>
        ///     The override value with a slider in the editor.
        /// </summary>
        [UnityEngine.RangeAttribute(0, 1)] public float foldOverrideValue;

        public static bool debugRendererXC;
        public static float lineRendererWidth = 0.001f;

        public UnityEngine.GameObject pointPrefab;

        private void Start()
        {
            ins = this;
            if (allFigures == null)
            {
                allFigures = new System.Collections.Generic.List<UnityEngine.GameObject>();
                allFigures.AddRange(nets);
                allFigures.AddRange(crossSections);
                allFigures.AddRange(measures);
            }

            //_sliderInputs = allFigures.OfType<ISliderInput>().ToList();
            _sliderInputs =
                Enumerable.ToList(Enumerable.Where(allFigures, go => go.GetComponent(typeof(ISliderInput)) != null));
            _dPerspectives = Enumerable.ToList(Enumerable.OfType<I4D_Perspective>(allFigures));
        }

        private void Update()
        {
            setActiveObjects();

            float percent;
            //if the override bool is set, use in editor override value

            //if the boolean is set to animate the figure
            if (animateFold)
            {
                //increment the degree folded by one degree. 
                percentFolded = animateUp ? percentFolded + .01f : percentFolded - .01f;

                if (percentFolded >= 1)
                {
                    percentFolded = 1f; //round to whole
                    animateFold = false;
                    animateUp = false;
                }
                else if (percentFolded <= 0)
                {
                    percentFolded = 0f;
                    animateFold = false;
                    animateUp = true;
                }

                percent = percentFolded;

                IMRE.EmbodiedUserInput.TouchSlider.ins.SliderValue = percent;
            }
            else if (foldOverride)
            {
                percent = foldOverrideValue;

                IMRE.EmbodiedUserInput.TouchSlider.ins.SliderValue = percent;
            }
            // if the participant is directly manipulating the slider
            else
            {
                percent = IMRE.EmbodiedUserInput.TouchSlider.ins.SliderValue;
            }
#if Photon
	   		 photonView.RPC("setPercentFolded", PhotonTargets.All, percent);
#else
            setPercentFolded(percent);
#endif
        }

        public System.Collections.Generic.List<UnityEngine.GameObject> _sliderInputs;

#if Photon
        [PunRPC]
#endif
        private void setPercentFolded(float percent)
        {
            foldOverrideValue = percent;

            _sliderInputs.ForEach(
                si =>
                {
                    if (si.activeSelf)
                        si.GetComponent<ISliderInput>().slider = percent;
                }
            );
        }

        public System.Collections.Generic.List<UnityEngine.GameObject> nets;
        public System.Collections.Generic.List<UnityEngine.GameObject> crossSections;
        public System.Collections.Generic.List<UnityEngine.GameObject> measures;
        private System.Collections.Generic.List<UnityEngine.GameObject> allFigures;

        private int itemId = -1;
        private System.Collections.Generic.List<I4D_Perspective> _dPerspectives;

        private void setActiveObjects()
        {
            if (allFigures == null)
            {
                allFigures = new System.Collections.Generic.List<UnityEngine.GameObject>();
                allFigures.AddRange(nets);
                allFigures.AddRange(crossSections);
                allFigures.AddRange(measures);
            }

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F1)) allFigures.ForEach(net => net.SetActive(false));

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F2))
            {
                allFigures.ForEach(net => net.SetActive(false));
                itemId--;
                itemId = itemId % allFigures.Count;
                allFigures[itemId].SetActive(true);
            }

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F3))
            {
                allFigures.ForEach(net => net.SetActive(false));
                itemId++;
                if (itemId < allFigures.Count)
                    allFigures[itemId].SetActive(true);
                else
                    UnityEngine.Application.Quit();
            }

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F5)) animateFold = !animateFold;
        }
    }
}