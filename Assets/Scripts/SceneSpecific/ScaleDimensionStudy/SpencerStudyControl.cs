using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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
        /// <summary>
        /// The number of degrees that each vertex is folded by.
        /// Consider changing to percent;
        /// </summary>        
        internal static float percentFolded = 0f;
        /// <summary>
        /// An override that automatically animates the slider and the folding process
        /// </summary>
        public bool animateFold = false;
	
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
        public float foldOverrideValue = 0f;
	
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
            //construct a slider as a dependent line segment, with points at Vector3.zero and Vector3.right.  
            //Add Vector3.up for height
            slider = GeoObjConstruction.dLineSegment(GeoObjConstruction.dPoint(Vector3.zero+Vector3.up),GeoObjConstruction.dPoint(Vector3.right*.1f+Vector3.up));
            //construct a point on the slider (in the middle)
            //this point will be bound to the slider on update.
            sliderPoint = GeoObjConstruction.iPoint(Vector3.right*.05f);
            if (allFigures == null)
            {
	            allFigures = new List<GameObject>();
	            allFigures.AddRange(nets);
	            allFigures.AddRange(crossSections);
	            allFigures.AddRange(measures);
            }
            _sliderInputs = allFigures.OfType<ISliderInput>().ToList();
            _dPerspectives = allFigures.OfType<I4D_Perspective>().ToList();
        }

        void Update()
        {
	        setActiveObjects();
	        
		//Update Rotation Values for Higher Dim Figures
		_dPerspectives.ForEach(fig => fig.SetRotation(xy,xz,xw,yz,yw,zw));
	
            float percent = 0f;
            //if the override bool is set, use in editor override value


            //if the boolean is set to animate the figure
            if (animateFold)
            {
	            
                //increment the degree folded by one degree. 
                percentFolded = (percentFolded + .01f);
                
                
                if (percentFolded == 1)
                {
	                percentFolded = 1f; //round to whole
	                animateFold = false;
                }
                
                if (percentFolded > 1)
                {
	                percentFolded = Mathf.FloorToInt(percentFolded);
                }
                
                //update the slider's position to reflect the override value
                sliderPoint.Position3 = (percentFolded)*(slider.point2.Position3 - slider.point1.Position3) + slider.point1.Position3;
                percent = percentFolded;
            } else if (foldOverride)
            {
	            percent = foldOverrideValue;
	            //update the slider's position to reflect the override value
	            sliderPoint.Position3 = (percentFolded)*(slider.point2.Position3 - slider.point1.Position3) + slider.point1.Position3;

            }
            // if the participant is directly manipulating the slider
            else
            {
                sliderPoint.Position3 = Vector3.Project(sliderPoint.Position3 - slider.point1.Position3,slider.point1.Position3 - slider.point2.Position3) + slider.point1.Position3;
                percent =(sliderPoint.Position3 - slider.point1.Position3).magnitude/(slider.point1.Position3 - slider.point2.Position3).magnitude;
            }
#if Photon
	   		 photonView.RPC("setPercentFolded", PhotonTargets.All, percent);
#else
            setPercentFolded(percent);
#endif
        }
#if Photon
        [PunRPC]
#endif

	    private List<ISliderInput> _sliderInputs;
	    private void setPercentFolded(float percent)
	    {    
		    percentFolded = percent;
		    
		    _sliderInputs.ForEach(si => si.slider = percentFolded);
		    
		    //update slider point on all users.
		    sliderPoint.Position3 = (percentFolded / 360f) * (slider.point2.Position3 - slider.point1.Position3) +
		                            slider.point1.Position3;

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