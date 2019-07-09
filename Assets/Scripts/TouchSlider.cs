using System;
using System.Collections;
using System.Collections.Generic;
using IMRE.EmbodiedUserInput;
using IMRE.HandWaver;
using Leap;
using Unity.Burst;
using UnityEngine;
using Unity.Mathematics;

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
    /// Position of Left pointer finger
    /// </summary>
    private float3 LeftFingerPos;
    
    /// <summary>
    /// Position of right pointer finger
    /// </summary>
    private float3 RightFingerPos;

    /// <summary>
    /// Slider line renderer
    /// </summary>
    private LineRenderer tSlider;

    
    /// <summary>
    /// The point on the line which the index finger's fingertip is closest to
    /// </summary>
    private Vector3 tSliderInstersectLeft;
    private Vector3 tSliderInstersectRight;

    /// <summary>
    /// internal percentage of slider
    /// </summary>
    [Range(0,1)]
    private float _value;
    
    /// <summary>
    /// Field for value to be used to determine what percentage the slider currently represents.
    /// </summary>
    public float Value
    {
        get => _value;
        set => _value = value;
    }
    
    
    void Start()
    {
        //make our instance of line renderer equal to the component line renderer on the same Unity
        //game object as the script
        tSlider = GetComponent<LineRenderer>();
        //make sure the point aligns with the current position of the line
    }

    //these are kinda like function calls. They do the math and grab the data at the frame which they are
    //used due to the lambda operator (=>)
    //In this case we look at the Magnitude of a finger position with the position of the line subtracted
    //This will let us know the distance from the finger tip to the line
    //public float leftFingerMag => Vector3.Magnitude((Vector3) LeftFingerPos - transform.position);
    //public float rightFingerMag => Vector3.Magnitude((Vector3) RightFingerPos - transform.position);
    
    void Update()
    {   
        //grab the position of the index finger's fingertip for both the left hand and the right hand
        LeftFingerPos = BodyInputDataSystem.bodyInput.LeftHand.Fingers[1].Joints[3].Position;
        RightFingerPos = BodyInputDataSystem.bodyInput.RightHand.Fingers[1].Joints[3].Position;

        tSliderInstersectLeft = tSlider.GetPosition(0) + Vector3.Project((Vector3)LeftFingerPos 
                                                                     - tSlider.GetPosition(0),tSlider.GetPosition(1) 
                                                                                              - tSlider.GetPosition(0));
        //UnityEngine.Debug.Log("Left slider intersect"+tSlider.GetPosition(0));
        tSliderInstersectRight = tSlider.GetPosition(0) + Vector3.Project((Vector3)RightFingerPos 
                                                                     - tSlider.GetPosition(0),tSlider.GetPosition(1) 
                                                                                              - tSlider.GetPosition(0));
        
        //true when the left finger is closer than the right finger
        if(Vector3.Distance((Vector3)LeftFingerPos,transform.InverseTransformPoint(transform.position)) < Vector3.Distance((Vector3)RightFingerPos, transform.InverseTransformPoint(transform.position))) //(leftFingerMag < rightFingerMag)
        {    
            //left finger
            //if (leftFingerMag <= tolerance)
            //{

                if (Vector3.Distance((Vector3)LeftFingerPos, tSliderInstersectLeft) <= tolerance)
                {
                    //enter here is the fingertip is close enough to the closest point on the line to count as
                    //the finger intersecting the line
                    
                    //Divide the distance from where the finger intersects the line to the starting point of the line
                    //by the total distance from start to finish of the line to get a value from 0 to 1 which represents
                    //a percentage of how far from the first position of the line the fingertip is.
                    Value = Vector3.Distance(tSliderInstersectLeft , tSlider.GetPosition(0)
                                                 / Vector3.Distance(tSlider.GetPosition(0) 
                                                     , tSlider.GetPosition(1)));
                    point.transform.localPosition = tSliderInstersectLeft;
                }

            //}
                
        }
        else
        {
             //right finger
             //if (rightFingerMag<= tolerance)
             //{


             if (Vector3.Distance((Vector3)RightFingerPos, tSliderInstersectRight) <= tolerance)
             {
                 //enter here is the fingertip is close enough to the closest point on the line to count as
                 //the finger intersecting the line
                     
                 //Divide the distance from where the finger intersects the line to the starting point of the line
                 //by the total distance from start to finish of the line to get a value from 0 to 1 which represents
                 //a percentage of how far from the first position of the line the fingertip is.
                 Value = Vector3.Distance(tSliderInstersectRight , tSlider.GetPosition(0)
                                                                   / Vector3.Distance(tSlider.GetPosition(0) 
                                                                       , tSlider.GetPosition(1)));
                 point.transform.localPosition = tSliderInstersectRight;
             }
             //}
            
        }
    }
}
