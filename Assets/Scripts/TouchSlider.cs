using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver;
using Leap;
using Unity.Burst;
using UnityEngine;
using Unity.Mathematics;

namespace IMRE.EmbodiedInput{
public class TouchSlider : MonoBehaviour
{
    /// <summary>
    /// The point for visual representation on the line
    /// </summary>
    public Transform point;
    
    /// <summary>
    /// Max distance for finger to point interaction to trigger
    /// </summary>
    public float tolerance = 0.2f;
    
    /// <summary>
    /// Slider line renderer
    /// </summary>
    private LineRenderer tSlider;
	public float3 tSliderEndA;
	public float3 tSldierEndB;

	private EmbodiedInputClassifier[] classifiers;


    /// <summary>
    /// internal percentage of slider
    /// </summary>
    [Range(0,1)]
    private float _sliderValue;
    
    /// <summary>
    /// Field for value to be used to determine what percentage the slider currently represents.
    /// </summary>
    public float SliderValue
    {
        get => _sliderValue;
        set
{
point.position = value * (tSliderEndB - tSliderEndA) + tSliderEndA;
_value = value;
}
    }
    
    
    void Start()
    {
        //make our instance of line renderer equal to the component line renderer on the same Unity
        //game object as the script
        tSlider = GetComponent<LineRenderer>();
	tSlider.SetPosition(0, tSliderEndA);
	tSlider.SetPosition(0, tSliderEndB);
	tSlider.startWidth = .05f;
	tSlider.endWidth = .05f;
	tSlider.useWorldSpace = true;
        //make sure the point aligns with the current position of the line

	//TODO identify and set classifiers
	//classifiers = static reference to the classifier authoring system goes here...
    }

	private void Update()
{
	classifiers.ToList().Where(classifier => classifier.isEligible).ForEach(checkClassifier);
}

    //these are kinda like function calls. They do the math and grab the data at the frame which they are
    //used due to the lambda operator (=>)
    //In this case we look at the Magnitude of a finger position with the position of the line subtracted
    //This will let us know the distance from the finger tip to the line
    //public float leftFingerMag => Vector3.Magnitude((Vector3) LeftFingerPos - transform.position);
    //public float rightFingerMag => Vector3.Magnitude((Vector3) RightFingerPos - transform.position);
    
    private void checkClassifier(EmbodiedInputClassifier classifier)
    {
	//Notice that this code is totally generic and can be used for any classifier - doesn't need to be pinch.
	//think lego blocks.   
        float3 tSliderProjection = Vector3.Project(classifier.position - tSliderEndA, tsliderEndB - tSliderEndA);
	if(Vector3.magnitude(tsliderProjection - classifier.position) < tolerance)
	{
		SliderValue = Vector3.magnitude(tSliderProjection - tSliderEndA)/Vector3.magnitude(tsliderEndB - tSliderEndA);
	}

    }
}
}
