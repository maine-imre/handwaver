using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver.HigherDimensions;
using UnityEditorInternal.Profiling.Memory.Experimental;

#if Photon
using Photon.Pun;
#endif

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// Central control for scale and dimension study.
    /// also includes logic to make a slider out of MasterGeoObjs
    /// </summary>
    public class SpencerStudyControl : MonoBehaviour
    {
	    public static SpencerStudyControl ins;
	    
        /// <summary>
        /// The number of degrees that each vertex is folded by.
        /// Consider changing to percent;
        /// </summary>        
        internal static float percentFolded;
        /// <summary>
        /// An override that automatically animates the slider and the folding process
        /// </summary>
        public bool animateFold;

        public bool animateUp = true;
	
        /// <summary>
        /// The point on the slider that determines the position of the slider.
        /// </summary>
        private InteractablePoint sliderPoint;
        /// <summary>
        /// The bounds of the slider.
        /// </summary>
        private DependentLineSegment slider;

        /// <summary>
        /// A boolean for debugging that allows the fold to be manipulated in the editor at play
        /// </summary>
        public bool foldOverride;
        /// <summary>
        /// The override value with a slider in the editor.
        /// </summary>
        [Range(0, 1)]
        public float foldOverrideValue;

        public static bool debugRendererXC;
        public static float lineRendererWidth = 0.001f;

        public GameObject pointPrefab;
	
	//In editor controls for 4D Projection Perspective.  No rotation projects along the W axis.
	[Range(0, 360)]
	public float xy;
	
	[Range(0, 360)]
	public float xz;
	
	[Range(0, 360)]
	public float xw;
	
	[Range(0, 360)]
	public float yz;
	
	[Range(0, 360)]
	public float yw;
	
	[Range(0, 360)]
	public float zw;
	
	


        private void Start()
        {
	        ins = this;
            if (allFigures == null)
            {
	            allFigures = new List<GameObject>();
	            allFigures.AddRange(nets);
	            allFigures.AddRange(crossSections);
	            allFigures.AddRange(measures);
            }
            //_sliderInputs = allFigures.OfType<ISliderInput>().ToList();
            _sliderInputs = allFigures.Where(go => go.GetComponent(typeof(ISliderInput)) != null).ToList();
            _dPerspectives = allFigures.OfType<I4D_Perspective>().ToList();
        }

        void Update()
        {
	        setActiveObjects();
	        
		//Update Rotation Values for Higher Dim Figures
		_dPerspectives.ForEach(fig => fig.SetRotation(xy,xz,xw,yz,yw,zw));
	
            float percent;
            //if the override bool is set, use in editor override value


            //if the boolean is set to animate the figure
            if (animateFold)
            {
	            
                //increment the degree folded by one degree. 
                percentFolded = animateUp ? (percentFolded + .01f) : (percentFolded - .01f);
                
                              
                if (percentFolded >= 1)
                {
	                percentFolded = 1f; //round to whole
	                animateFold = false;
	                animateUp = false;
                }else if (percentFolded <= 0)
                {
	                percentFolded = 0f;
	                animateFold = false;
	                animateUp = true;
                }
                
                percent = percentFolded;
                
                TouchSlider.ins.SliderValue = percent;
            } else if (foldOverride)
            {
	            percent = foldOverrideValue;
	            
	            TouchSlider.ins.SliderValue = percent;
            }
            // if the participant is directly manipulating the slider
            else
            {
	            percent = TouchSlider.ins.SliderValue;
            }
#if Photon
	   		 photonView.RPC("setPercentFolded", PhotonTargets.All, percent);
#else
            setPercentFolded(percent);
#endif
        }
        
        public List<GameObject> _sliderInputs;
        
#if Photon
        [PunRPC]
#endif
	    private void setPercentFolded(float percent)
	    {    
		    foldOverrideValue = percent;
		    
		    _sliderInputs.ForEach(
			    si =>
			    {
				    if(si.activeSelf)
				    si.GetComponent<ISliderInput>().slider = percent;
			    }
		    );

	    }

	    public List<GameObject> nets;
	    public List<GameObject> crossSections;
	    public List<GameObject> measures;
	    private List<GameObject> allFigures;

	    
	    private int itemId = -1;
	    private List<I4D_Perspective> _dPerspectives;

	    private void setActiveObjects()
	    {
		    if (allFigures == null)
		    {
			    allFigures = new List<GameObject>();
			    allFigures.AddRange(nets);
			    allFigures.AddRange(crossSections);
			    allFigures.AddRange(measures);
		    }
		    if (Input.GetKeyDown(KeyCode.F1))
		    {
			    allFigures.ForEach(net => net.SetActive(false));
		    }

		    if (Input.GetKeyDown(KeyCode.F2))
		    {
			    allFigures.ForEach(net => net.SetActive(false));
			    itemId--;
			    itemId = itemId % allFigures.Count;
			    allFigures[itemId].SetActive(true);
		    }

		    if (Input.GetKeyDown(KeyCode.F3))
		    {
			    allFigures.ForEach(net => net.SetActive(false));
			    itemId++;
			    if (itemId < allFigures.Count)
			    {
				    allFigures[itemId].SetActive(true);
			    }
			    else
			    {
				    Application.Quit();
			    }
		    }
		    if (Input.GetKeyDown(KeyCode.F5))
		    {
			    animateFold = !animateFold;
		    }

    }
    }
}